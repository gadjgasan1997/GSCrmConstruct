using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.RequestModels;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.MainEntities;
using static GSCrmLibrary.Data.ComponentsRecordsInfo;

namespace GSCrmLibrary.Controllers.ApiControllers.MainControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainAppletController<TTable, TBusinessComponent, TApplet, TContext, TDataBUSFactory, TBUSFactory, TBUSUIFactory> : ControllerBase
        where TTable : class, IDataEntity, new()
        where TBusinessComponent : IBUSEntity, new()
        where TApplet : IUIEntity, new()
        where TContext : MainContext, new()
        where TDataBUSFactory : IDataBUSFactory<TTable, TBusinessComponent, TContext>, new()
        where TBUSFactory : BUSFactory<TContext>, new()
        where TBUSUIFactory : IBUSUIFactory<TBusinessComponent, TApplet, TContext>, new()
    {
        private readonly TContext context;
        private readonly DbSet<TTable> entityDBSet;
        private readonly IOrderedEnumerable<TTable> orderedEntities;
        private readonly IScreenInfo screenInfo;
        private readonly IViewInfo viewInfo;
        public MainAppletController(TContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            entityDBSet = context.Set<TTable>();
            orderedEntities = entityDBSet.ToList().OrderByDescending(c => c.Created);
            this.screenInfo = screenInfo;
            this.viewInfo = viewInfo;
        }

        [HttpGet("GetRecord/{appletName}")]
        public ActionResult<object> GetRecord(string appletName)
        {
            // Если в представлении не установлена текущая запись
            if (viewInfo.CurrentRecord == null && orderedEntities.FirstOrDefault() != null)
                viewInfo.CurrentRecord = orderedEntities.FirstOrDefault().Id.ToString();

            TBUSUIFactory busUIFactory = new TBUSUIFactory();
            TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
            TBusinessComponent businessEntity;

            ViewItem viewItem = viewInfo.ViewItems.FirstOrDefault(n => n.Applet.Name == appletName);
            BusinessComponent busComp = viewInfo.ViewBCs.FirstOrDefault(i => i.Id == viewItem.Applet.BusCompId);
            BusinessObjectComponent objectComponent = viewInfo.BOComponents.FirstOrDefault(bcId => bcId.BusCompId == busComp.Id);
            string selectedRecordId = GetSelectedRecord(busComp?.Name);

            if (viewInfo.AppletsSortedByLinks.LastOrDefault()?.Id == viewItem.Applet?.Id)
                viewInfo.EndInitialize();

            // Если у апплета установлена кастомная инициализация
            if ((viewInfo.CurrentApplet?.Initflag == true && viewInfo.CurrentApplet.Name == appletName) || 
                (viewInfo.CurrentPopupApplet?.Initflag == true && viewInfo.CurrentPopupApplet.Name == appletName))
            {
                TBusinessComponent initComponent = busUIFactory.Init(context);
                ComponentsContext<TBusinessComponent>.SetComponentContext(busComp.Name, initComponent);
                return JsonConvert.SerializeObject(busUIFactory.BusinessToUI(initComponent), Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }

            // Выделенная запись
            if (selectedRecordId != null)
                businessEntity = dataBUSFactory.DataToBusiness(orderedEntities.FirstOrDefault(i => i.Id.ToString() == selectedRecordId), context);

            // Запись по умолчанию, в случае, если на бизнес компонете апплета нет выделенной записи
            else
            {
                // Если у записи есть ограничение по родительской
                string searchSpecificationByParent = GetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecificationByParent);
                if (searchSpecificationByParent == null)
                {
                    // Если количество отображаемых записей больше нуля
                    if (orderedEntities?.Count() > 0)
                        businessEntity = dataBUSFactory.DataToBusiness(orderedEntities.ElementAtOrDefault(viewItem.AutofocusRecord), context);
                    else
                    {
                        businessEntity = new TBusinessComponent() { };
                        return null;
                    }
                }
                else
                {
                    // Если количество отображаемых записей больше нуля
                    if (orderedEntities.AsQueryable().Where(searchSpecificationByParent)?.Count() != 0)
                        businessEntity = dataBUSFactory.DataToBusiness(orderedEntities.ElementAtOrDefault(viewItem.AutofocusRecord), context);
                    else
                    {
                        businessEntity = new TBusinessComponent() { };
                        return null;
                    }
                }
            }

            ComponentsContext<TBusinessComponent>.SetComponentContext(busComp.Name, businessEntity);
            return Ok(JsonConvert.SerializeObject(busUIFactory.BusinessToUI(businessEntity), Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        [HttpGet("GetRecords/{appletName}")]
        public ActionResult<object> GetRecords(string appletName)
        {
            // Получение всех необходимых сущностей
            List<BusinessObjectComponent> boComponents = viewInfo.BOComponents;
            Applet applet = viewInfo.ViewApplets.FirstOrDefault(n => n.Name == appletName);
            BusinessComponent busComp = context.BusinessComponents
                .AsNoTracking()
                .Select(bc => new
                {
                    id = bc.Id,
                    name = bc.Name,
                    fields = bc.Fields.Select(field => new { id = field.Id, name = field.Name })
                })
                .Select(bc => new BusinessComponent
                {
                    Id = bc.id,
                    Name = bc.name,
                    Fields = bc.fields.Select(field => new Field { Id = field.id, Name = field.name }).ToList()
                })
                .FirstOrDefault(i => i.Id == applet.BusCompId);
            BusinessObjectComponent objectComponent = viewInfo.BOComponents.FirstOrDefault(bcId => bcId.BusCompId == busComp.Id);
            ViewItem viewItem = viewInfo.ViewItems.Where(apId => apId.AppletId == applet.Id).FirstOrDefault();
            TBUSUIFactory busUIFactory = new TBUSUIFactory();
            TDataBUSFactory dataBUSFactory = new TDataBUSFactory();

            // Получение данных
            List<TTable> dataEntities = orderedEntities.ToList();
            string searchSpecification = GetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecification);
            object[] searchSpecArgs = (object[])GetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecArgs);
            if (!string.IsNullOrWhiteSpace(searchSpecification))
                dataEntities = orderedEntities.AsQueryable().Where(searchSpecification, searchSpecArgs).ToList();

            // Среди БКО ищу ту, у которой линка не пустая и где дочерней БК является текущая
            Link link = boComponents
                .Where(link => link.Link != null)
                .Select(link => link.Link)
                .FirstOrDefault(bc => bc.ChildBCId == busComp.Id);

            // Есть линка
            if (link != null)
            {
                // Родительская БК и филды
                BusinessComponent parentBusComp = context.BusinessComponents
                    .AsNoTracking()
                    .Select(bc => new
                    {
                        id = bc.Id,
                        name = bc.Name,
                        table = bc.Table,
                        fields = bc.Fields.Select(field => new { id = field.Id, name = field.Name })
                    })
                    .Select(bc => new BusinessComponent
                    {
                        Id = bc.id,
                        Name = bc.name,
                        Table = bc.table,
                        Fields = bc.fields.Select(field => new Field { Id = field.id, Name = field.name }).ToList()
                    })
                    .FirstOrDefault(i => i.Id == link.ParentBCId);
                Field parentField = parentBusComp.Fields.FirstOrDefault(i => i.Id == link.ParentFieldId);
                Field childField = busComp.Fields.FirstOrDefault(i => i.Id == link.ChildFieldId);

                // Родительская запись
                var recId = GetSelectedRecord(parentBusComp.Name);
                IEnumerable<dynamic> parentRecords = (IEnumerable<dynamic>)(context.GetType().GetProperty(parentBusComp.Table.Name).GetValue(context));
                dynamic parentRecord = parentRecords.FirstOrDefault(i => i.Id.ToString() == recId);

                // Значение родитеского поля из родительской БКО по которому будет фильтроваться дочерняя(текущая БКО)
                string parentFieldValue = parentRecord == null ? string.Empty 
                    : parentRecord.GetType().GetProperty(parentField.Name).GetValue(parentRecord).ToString();

                /* Установка search spec-а по дочерней(текущей) БКО
                   Если на родительской бк есть записи, надо фильтровать дочернюю бк */
                if (parentFieldValue != string.Empty)
                {
                    string searchSpecificationByParent = $"{childField.Name} = \"{parentFieldValue}\"";
                    SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecificationByParent, searchSpecificationByParent);
                    dataEntities = dataEntities.AsQueryable().Where(searchSpecificationByParent).ToList();
                }

                // Если записей нет, значит надо очистить дочернюю бк
                else dataEntities = new List<TTable>();
            }

            // Иначе
            else
            {
                // Получение текущей записи из автофокуса элемента представления
                var selectedRecord = GetSelectedRecord(busComp.Name);
                var currentRecord = selectedRecord == null
                    ? orderedEntities.ElementAtOrDefault(viewItem.AutofocusRecord)
                    : orderedEntities.FirstOrDefault(i => i.Id.ToString() == selectedRecord);
            }

            // Отбор записей
            List<string> displayRecords = GetDisplayedRecords(busComp.Name);
            List<string> dataEntitiesId = dataEntities.Select(i => i?.Id.ToString()).ToList();
            List<TTable> currentRecords = new List<TTable>();
            string selectedRecordId = GetSelectedRecord(busComp.Name);
            if (viewInfo.CurrentApplet?.Name == appletName)
            {
                // Обнуление текущего контрола
                viewInfo.CurrentControl = null;

                switch (viewInfo.ActionType)
                {
                    // Навигация вперед по списку
                    case ActionType.NextRecords:
                        // Ищу по id последней записи на тайле
                        TTable lastRecord = dataEntities.FirstOrDefault(i => i.Id.ToString() == displayRecords.LastOrDefault());

                        // Следующие записи
                        dataEntities = dataEntities.SkipWhile(i => i.Id != lastRecord.Id).Take(applet.DisplayLines).ToList();

                        // Если id последней записи в базе совпадает с id последней отоброжаемой записи, значит список долистали до конца
                        if (dataEntities.LastOrDefault().Id == lastRecord.Id) return BadRequest();

                        // Проставление текущей записи
                        var nextRecord = dataEntities.ElementAtOrDefault(viewItem.AutofocusRecord);
                        if (nextRecord != null && nextRecord?.Id != Guid.Empty)
                        {
                            viewInfo.CurrentRecord = nextRecord.Id.ToString();
                            SetSelectedRecord(busComp.Name, nextRecord.Id.ToString());
                            ComponentsContext<TBusinessComponent>.SetComponentContext(busComp.Name, dataBUSFactory.DataToBusiness(nextRecord, context));
                        }
                        break;

                    // Навигация назад по списку
                    case ActionType.PreviousRecords:
                        // Ищу по id первой записи на тайле
                        TTable firstRecord = dataEntities.FirstOrDefault(i => i.Id.ToString() == displayRecords.FirstOrDefault());

                        // Предыдущие записи
                        List<TTable> previousRecords = dataEntities.AsEnumerable().Reverse().SkipWhile(i => i.Id != firstRecord.Id).ToList();
                        if (previousRecords.Count < applet.DisplayLines && dataEntities.Count > applet.DisplayLines)
                        {
                            TTable firstPreviousRecord = previousRecords.AsEnumerable().Reverse().FirstOrDefault();
                            dataEntities = dataEntities.SkipWhile(i => i.Id != firstPreviousRecord.Id).Take(applet.DisplayLines).ToList();
                        }
                        else
                        {
                            dataEntities = previousRecords.Take(applet.DisplayLines).Reverse().ToList();
                        }

                        // Если id первой записи в базе совпадает с id первой отоброжаемой записи, значит список долистали до начала
                        if (dataEntities.FirstOrDefault()?.Id == firstRecord.Id) return BadRequest();

                        // Проставление текущей записи
                        var previousRecord = dataEntities.ElementAtOrDefault(viewItem.AutofocusRecord);
                        if (previousRecord != null && previousRecord?.Id != Guid.Empty)
                        {
                            viewInfo.CurrentRecord = previousRecord.Id.ToString();
                            SetSelectedRecord(busComp.Name, previousRecord.Id.ToString());
                            ComponentsContext<TBusinessComponent>.SetComponentContext(busComp.Name, dataBUSFactory.DataToBusiness(previousRecord, context));
                        }
                        break;
                }

                if (viewInfo.CurrentPopupApplet != null)
                {
                    // Обнуление текущего контрола
                    viewInfo.CurrentPopupControl = null;

                    switch (viewInfo.ActionType)
                    {
                        case ActionType.NewRecord:
                        case ActionType.UpdateRecord:
                            // Удаление попап апплета из информации о представлении
                            viewInfo.RemovePopupApplet(context);
                            break;
                    }
                }
            }

            // В зависимости от действия, произошедшего в представлении
            TBusinessComponent defaultRecord;
            if (dataEntities.Count > 0)
                defaultRecord = dataBUSFactory.DataToBusiness(dataEntities.ElementAtOrDefault(viewItem.AutofocusRecord), context);
            else defaultRecord = new TBusinessComponent();
            switch (viewInfo.ActionType)
            {
                case ActionType.InitializeView:
                    /* Промотка до текущей отоброжаемой записи
                     * Проверяется, что в список с текущими отображаемыми записями не пуст
                     * И что они содержат текущую выбранную запись */
                    if (displayRecords?.Count > 0 && dataEntitiesId.IndexOf(selectedRecordId) != -1)
                    {
                        displayRecords.ForEach(id => currentRecords.Add(dataEntities.FirstOrDefault(i => i.Id.ToString() == id)));
                        dataEntities = currentRecords;
                    }

                    else
                    {
                        SetSelectedRecord(busComp.Name, dataEntitiesId.ElementAtOrDefault(viewItem.AutofocusRecord));
                        ComponentsContext<TBusinessComponent>.SetComponentContext(busComp.Name, defaultRecord);
                    }

                    // Установка текущей выбранной записи
                    if (viewInfo.CurrentApplet?.Name == appletName)
                        viewInfo.CurrentRecord = GetSelectedRecord(busComp.Name);
                    break;

                case ActionType.Drilldown:
                    // Промотка до текущей отоброжаемой записи
                    if (dataEntities?.Count > 0 && !string.IsNullOrWhiteSpace(selectedRecordId))
                        dataEntities = dataEntities.SkipWhile(i => i.Id.ToString() != selectedRecordId).Take(applet.DisplayLines).ToList();
                    break;

                case ActionType.DeleteRecord:
                    // Промотка до текущей отоброжаемой записи
                    if (displayRecords?.Count > 0 && viewInfo.CurrentApplet?.Name == appletName)
                    {
                        displayRecords.ForEach(id => currentRecords.Add(dataEntities.FirstOrDefault(i => i.Id.ToString() == id)));
                        dataEntities = currentRecords;
                    }
                    else
                    {
                        SetSelectedRecord(busComp.Name, dataEntitiesId.ElementAtOrDefault(viewItem.AutofocusRecord));
                        ComponentsContext<TBusinessComponent>.SetComponentContext(busComp.Name, defaultRecord);
                    }
                    break;

                case ActionType.ReloadView:
                case ActionType.UpdateRecord:
                    // Промотка до текущей отоброжаемой записи
                    if (displayRecords?.Count > 0)
                        displayRecords.ForEach(id => currentRecords.Add(dataEntities.FirstOrDefault(i => i.Id.ToString() == id)));
                    dataEntities = currentRecords;
                    break;

                case ActionType.CancelQuery:
                    // Промотка до текущей отоброжаемой записи
                    if (displayRecords?.Count > 0 && !string.IsNullOrWhiteSpace(selectedRecordId))
                        dataEntities = dataEntities.SkipWhile(i => i.Id.ToString() != selectedRecordId).Take(applet.DisplayLines).ToList();
                    break;

                case ActionType.ExecuteQuery:
                    if (dataEntities.Count > 0)
                    {
                        selectedRecordId = dataEntities.FirstOrDefault().Id.ToString();
                        SetSelectedRecord(busComp.Name, selectedRecordId);
                    }
                    break;

                case ActionType.NewRecord:
                case ActionType.ShowPopup:
                case ActionType.NextRecords:
                case ActionType.PreviousRecords:
                case ActionType.SelectTileItem:
                    if (viewInfo.CurrentApplet?.Name != appletName)
                    {
                        SetSelectedRecord(busComp.Name, dataEntitiesId.ElementAtOrDefault(viewItem.AutofocusRecord));
                        ComponentsContext<TBusinessComponent>.SetComponentContext(busComp.Name, defaultRecord);
                    }
                    break;
            }

            // Установка текущей выбранной записи
            if (GetSelectedRecord(busComp.Name) == null)
                SetSelectedRecord(busComp.Name, dataEntitiesId.ElementAtOrDefault(viewItem.AutofocusRecord));

            if (!ComponentsContext<TBusinessComponent>.TryGetComponentContext(busComp.Name, out TBusinessComponent businessComponent))
                ComponentsContext<TBusinessComponent>.SetComponentContext(busComp.Name, defaultRecord);

            // Установка текущих отоброжаеммых записи
            dataEntities = dataEntities.Take(applet.DisplayLines).ToList();
            dataEntitiesId = dataEntities.Select(i => i?.Id.ToString()).ToList();
            SetDisplayedRecords(busComp.Name, dataEntitiesId);

            // Маппинг в UI
            List<TApplet> UIEntities = new List<TApplet>();
            dataEntities.ForEach(dataEntity =>
                UIEntities.Add(
                    busUIFactory.BusinessToUI(dataBUSFactory.DataToBusiness(dataEntity, context))
                ));

            // Результат с отоброжаемыми и выбранными записями
            Dictionary<string, object> result = new Dictionary<string, object>
            {
                { "SelectedRecords", GetUIRecords(context, viewInfo) },
                { "DisplayedRecords", UIEntities }
            };

            // Обнуление действия после полного обновления представления(обновления последнего апплета)
            if (viewInfo.AppletsSortedByLinks.LastOrDefault()?.Id == applet.Id)
                viewInfo.EndInitialize();

            return Ok(new JsonResult(result).Value);
        }

        [HttpPost("NewRecord")]
        public ActionResult<object> NewRecord([FromBody] TApplet model)
        {
            TBUSUIFactory busUIFactory = new TBUSUIFactory();
            TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
            Dictionary<string, object> result = new Dictionary<string, object>()
            {
                { "ErrorMessages", new object() { } },
                { "NewRecord", new object() { } }
            };
            Dictionary<string, string> errorsList = new Dictionary<string, string>();
            List<ValidationResult> UIValidation = busUIFactory.UIValidate(context, viewInfo, model, true).ToList();
            if (!UIValidation.Any())
            {
                TBusinessComponent businessEntity = busUIFactory.UIToBusiness(model, context, viewInfo, true);
                List<ValidationResult> BUSUIValidation = busUIFactory.BUSUIValidate(context, businessEntity, model).ToList();
                businessEntity.ErrorMessage = string.Empty;
                if (!BUSUIValidation.Any())
                {
                    TTable dataEntity = dataBUSFactory.BusinessToData(null, businessEntity, context, true);
                    try
                    {
                        dataBUSFactory.OnRecordCreate(dataEntity, entityDBSet, context);
                        result["ErrorMessages"] = null;
                        result["NewRecord"] = busUIFactory.BusinessToUI(dataBUSFactory.DataToBusiness(dataEntity, context));
                        ComponentsContext<TBusinessComponent>.SetComponentContext(viewInfo.CurrentApplet.BusComp.Name, businessEntity);
                        return Ok(new JsonResult(result).Value);
                    }
                    catch(Exception ex)
                    {
                        errorsList = Utils.GetErrorsInfo(ex);
                    }
                }
                else
                {
                    BUSUIValidation.ForEach(error => errorsList.Add(error.MemberNames.FirstOrDefault(), error.ErrorMessage));
                }
            }
            else
            {
                UIValidation.ForEach(error => errorsList.Add(error.MemberNames.FirstOrDefault(), error.ErrorMessage));
            }
            result["NewRecord"] = null;
            result["ErrorMessages"] = errorsList;
            return BadRequest(new JsonResult(result).Value);
        }

        [HttpPost("UpdateRecord")]
        public ActionResult<object> UpdateRecord([FromBody] TApplet model)
        {
            TBUSUIFactory busUIFactory = new TBUSUIFactory();
            TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
            TBUSFactory busFactory = new TBUSFactory();
            TTable oldRecord = orderedEntities.FirstOrDefault(i => i.Id == model.Id);
            TApplet oldUIRecord = busUIFactory.BusinessToUI(dataBUSFactory.DataToBusiness(oldRecord, context));
            Dictionary<string, object> result = new Dictionary<string, object>()
            {
                { "ErrorMessages", new object() { } },
                { "ChangedRecord", new object() { } }
            };
            Dictionary<string, string> errorsList = new Dictionary<string, string>();
            List<string> fieldsToUpdate = new List<string>();
            List<ValidationResult> UIValidation = busUIFactory.UIValidate(context, viewInfo, model, false).ToList();
            if (!UIValidation.Any())
            {
                TBusinessComponent businessEntity = busUIFactory.UIToBusiness(model, context, viewInfo, false);
                List<ValidationResult> BUSUIValidation = busUIFactory.BUSUIValidate(context, businessEntity, model).ToList();
                businessEntity.ErrorMessage = string.Empty;
                if (!BUSUIValidation.Any())
                {
                    List<ValidationResult> BUSValidation = dataBUSFactory.DataBUSValidate(oldRecord, businessEntity, viewInfo, context).ToList();
                    businessEntity.ErrorMessage = string.Empty;
                    if (!BUSValidation.Any())
                    {
                        TTable changedRecord = dataBUSFactory.BusinessToData(oldRecord, businessEntity, context, false);
                        try
                        {
                            dataBUSFactory.OnRecordUpdate(oldRecord, changedRecord, entityDBSet, context);
                            result["ErrorMessages"] = null;
                            result["ChangedRecord"] = busUIFactory.BusinessToUI(dataBUSFactory.DataToBusiness(changedRecord, context));
                            ComponentsContext<TBusinessComponent>.SetComponentContext(viewInfo.CurrentApplet.BusComp.Name, businessEntity);
                            return Ok(new JsonResult(result).Value);
                        }
                        catch(Exception ex)
                        {
                            errorsList = Utils.GetErrorsInfo(ex);
                        }
                    }
                    else
                    {
                        BUSValidation.ForEach(error => errorsList.Add(error.MemberNames.FirstOrDefault(), error.ErrorMessage));
                        fieldsToUpdate = errorsList.Select(k => k.Key).ToList();
                    }
                }
                else
                {
                    BUSUIValidation.ForEach(error => errorsList.Add(error.MemberNames.FirstOrDefault(), error.ErrorMessage));
                    fieldsToUpdate = errorsList.Select(k => k.Key).ToList();
                }
            }
            else
            {
                UIValidation.ForEach(error => errorsList.Add(error.MemberNames.FirstOrDefault(), error.ErrorMessage));
                fieldsToUpdate = errorsList.Select(k => k.Key).ToList();
            }
            result["FieldsToUpdate"] = fieldsToUpdate;
            result["ErrorMessages"] = errorsList;
            result["ChangedRecord"] = oldUIRecord;
            return BadRequest(new JsonResult(result).Value);
        }

        [HttpPost("UndoUpdate")]
        public ActionResult<object> UndoUpdate([FromBody] TApplet model)
        {
            TBUSUIFactory busUIFactory = new TBUSUIFactory();
            TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
            TTable dataEntity = orderedEntities.FirstOrDefault(i => i.Id == model.Id);
            TApplet UIEntity = busUIFactory.BusinessToUI(dataBUSFactory.DataToBusiness(dataEntity, context));
            return Ok(new JsonResult(UIEntity).Value);
        }

        [HttpPost("DeleteRecord")]
        public ActionResult<object> DeleteRecord([FromBody] RequestAppletModel model)
        {
            // Получение всех необходимых сущностей
            List<BusinessObjectComponent> boComponents = viewInfo.BOComponents;
            Applet applet = viewInfo.ViewApplets.FirstOrDefault(n => n.Name == model.AppletName);
            BusinessComponent busComp = viewInfo.ViewBCs.FirstOrDefault(i => i.Id == applet.BusCompId);
            BusinessObjectComponent objectComponent = boComponents.FirstOrDefault(bcId => bcId.BusCompId == busComp.Id);
            object[] searchSpecArgs = (object[])GetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecArgs);
            string searchSpecification = GetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecification);
            string searchSpecificationByParent = GetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecificationByParent);

            // Получение контекста
            IEnumerable<TTable> dataEntities = orderedEntities;

            // Ограничение списка сущностей
            if (searchSpecification != null)
                dataEntities = dataEntities.AsQueryable().Where(searchSpecification, searchSpecArgs).ToList();
            if (searchSpecificationByParent != null)
                dataEntities = dataEntities.AsQueryable().Where(searchSpecificationByParent).ToList();

            // Получение записи предназначенной для удаления
            TTable recordToDelete = dataEntities.FirstOrDefault(i => i.Id == model.Id);
            if (recordToDelete == null) return NotFound();
            else
            {
                // Удаление записи из списка отоброжаемых
                List<string> displayRecords = GetDisplayedRecords(busComp.Name);
                displayRecords.Remove(recordToDelete.Id.ToString());
                SetDisplayedRecords(busComp.Name, displayRecords);

                // Получение следующей за этой записи
                TTable nextEntity = dataEntities.SkipWhile(item => !item.Equals(recordToDelete)).Skip(1).FirstOrDefault();

                // Если ее не найдено, получение предыдущей
                if (nextEntity == null)
                    nextEntity = dataEntities.TakeWhile(item => !item.Equals(recordToDelete)).LastOrDefault();

                // Удаление выбранной записи
                TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
                TBUSUIFactory busUIFactory = new TBUSUIFactory();
                dataBUSFactory.OnRecordDelete(recordToDelete, entityDBSet, context);

                // Обязательно необходимо перекверить данные после удаления
                dataEntities = orderedEntities;

                // Ограничение списка сущностей
                if (searchSpecArgs != null)
                    dataEntities = dataEntities.AsQueryable().Where(searchSpecification, searchSpecArgs).ToList();
                if (searchSpecificationByParent != null)
                    dataEntities = dataEntities.AsQueryable().Where(searchSpecificationByParent).ToList();

                // Если запись для фокуса найдена, то она добавляется в список отоброжаемых и возвращается
                if (nextEntity != null)
                {
                    // Пропуск текущих отоброжаемых записей и взятие слеующей
                    var record = dataEntities
                        .SkipWhile(i => i.Id.ToString() != displayRecords.LastOrDefault())
                        .Skip(1).FirstOrDefault();

                    // Если следущая запись есть и она не является удаляемой, то она добавляется в список отоброжаемых и возвращается
                    if (record != null && record.Id != recordToDelete.Id)
                        displayRecords.Add(record.Id.ToString());

                    SetDisplayedRecords(busComp.Name, displayRecords);

                    return Ok(new JsonResult(busUIFactory.BusinessToUI(dataBUSFactory.DataToBusiness(nextEntity, context))).Value);
                }
                else return Ok(new JsonResult(null).Value);
            }
        }

        [HttpPost("ExecuteQuery")]
        public virtual void ExecuteQuery([FromBody] TApplet model)
        {
            viewInfo.ActionType = ActionType.ExecuteQuery;
        }

        [HttpGet("CancelQuery")]
        public virtual void CancelQuery()
        {
            viewInfo.ActionType = ActionType.CancelQuery;
            Applet applet = viewInfo.CurrentPopupApplet ?? viewInfo.CurrentApplet;
            BusinessObjectComponent objectComponent = viewInfo.BOComponents.FirstOrDefault(i => i.BusCompId == applet.BusCompId);
            SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecification, string.Empty);
            SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecArgs, null);
        }
    }
}