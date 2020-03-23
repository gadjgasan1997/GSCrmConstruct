using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrm.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using GSCrm.Models.Default.TableModels;
using System.ComponentModel.DataAnnotations;
using GSCrm.Models.Default.RequestModels;
using GSCrm.Services.Info;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using GSCrm.Factories.Default.MainFactories;
using Action = GSCrm.Models.Default.TableModels.Action;
using GSCrm.Data.Context;

namespace GSCrm.Controllers.Default.APIControllers.MainControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainAppletController<ParentTableEntity, MainTable, MainBusinessComponent, MainApplet, MainDataBUSFR, MainBUSUIFR> : ControllerBase
        where ParentTableEntity : Models.Default.MainEntities.MainTable
        where MainTable : Models.Default.MainEntities.MainTable, new()
        where MainBusinessComponent : Models.Default.MainEntities.MainBusinessComponent, new()
        where MainApplet : Models.Default.MainEntities.MainApplet, new()
        where MainDataBUSFR : MainDataBUSFR<MainTable, MainBusinessComponent>, new()
        where MainBUSUIFR : MainBUSUIFR<MainBusinessComponent, MainApplet>, new()
    {
        private readonly ApplicationContext context;
        private readonly IWebHostEnvironment environment;
        private readonly DbSet<ParentTableEntity> parentEntityDBSet;
        private readonly DbSet<MainTable> entityDBSet;
        private readonly IScreenInfo screenInfo;
        private readonly IViewInfo viewInfo;
        private readonly IAppletInfo appletInfo;
        private readonly IAppletInfoUI appletInfoUI;
        public MainAppletController(ApplicationContext context,
            IWebHostEnvironment environment,
            DbSet<ParentTableEntity> parentEntityDBSet,
            DbSet<MainTable> entityDBSet,
            IScreenInfo screenInfo,
            IViewInfo viewInfo,
            IAppletInfo appletInfo,
            IAppletInfoUI appletInfoUI)
        {
            this.context = context;
            this.environment = environment;
            this.parentEntityDBSet = parentEntityDBSet; 
            this.entityDBSet = entityDBSet;
            this.screenInfo = screenInfo;
            this.viewInfo = viewInfo;
            this.appletInfo = appletInfo;
            this.appletInfoUI = appletInfoUI;
        }

        #region Requests
        [HttpGet("AppletInfo/{appletName}")]
        public virtual ActionResult<string> AppletInfo(string appletName)
        {
            try
            {
                appletInfo.InitializeAppletInfo(appletName, context);
                return Ok(appletInfoUI.Serialize(context));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetRecord")]
        public virtual ActionResult<object> GetRecord()
        {
            try
            {
                MainBUSUIFR busUIFactory = new MainBUSUIFR();
                MainDataBUSFR dataBUSFactory = new MainDataBUSFR();
                return Ok(new JsonResult(
                    busUIFactory.BusinessToUI(
                        viewInfo.CurrentPopupApplet?.Initflag == true ?
                        busUIFactory.Init() : 
                        dataBUSFactory.DataToBusiness(entityDBSet.FirstOrDefault(i => i.Id.ToString() == viewInfo.CurrentRecord), 
                        context))
                    ).Value);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetRecords")]
        public virtual ActionResult<object> GetRecords([FromBody] RequestAppletModel model)
        {
            try
            {
                // Получение всех необходимых сущностей
                List<BOComponent> boComponents = viewInfo.BOComponents;
                Applet applet = viewInfo.ViewApplets.FirstOrDefault(n => n.Name == model.AppletName);
                BusComp busComp = viewInfo.ViewBCs.FirstOrDefault(i => i.Id == applet.BusCompId);
                BOComponent objectComponent = boComponents.FirstOrDefault(bcId => bcId.BusCompId == busComp.Id);
                ViewItem viewItem = viewInfo.ViewItems
                    .Where(apId => apId.AppletId == applet.Id)
                    .FirstOrDefault();

                // Получение данных
                List<MainTable> dataEntities = entityDBSet.OrderByDescending(c => c.Created).ToList();

                // Среди БКО ищу ту, у которой линка не пустая и где дочерней БК является текущая
                Link link = boComponents
                    .Where(link => link.Link != null)
                    .Select(link => link.Link)
                    .FirstOrDefault(bc => bc.ChildBCId == busComp.Id);

                // Есть линка
                if (link != null)
                {
                    // Родительская БК и филды
                    BusComp parentBusComp = context.BusinessComponents
                        .Include(f => f.Fields)
                        .FirstOrDefault(i => i.Id == link.ParentBCId);
                    Field parentField = parentBusComp.Fields.FirstOrDefault(i => i.Id == link.ParentFieldId);
                    Field childField = busComp.Fields.FirstOrDefault(i => i.Id == link.ChildFieldId);

                    // Родительские апплет и элемент представления
                    Applet parentApplet = context.Applets
                        .Where(bcId => bcId.BusCompId == parentBusComp.Id)
                        .Where(type => type.Type == "Tile")
                        .FirstOrDefault();

                    // Родительская запись
                    var recId = ComponentContext.GetSelectedRecord(parentBusComp.Name);
                    var parentRecord = parentEntityDBSet.FirstOrDefault(i => i.Id.ToString() == recId);

                    // Значение родитеского поля из родительской БКО по которому будет фильтроваться дочерняя(текущая БКО)
                    string parentFieldValue = parentRecord == null ? string.Empty : parentRecord
                        .GetType()
                        .GetProperty(parentField.Name)
                        .GetValue(parentRecord)
                        .ToString();

                    /* Установка search spec-а по дочерней(текущей) БКО
                       Если на родительской бк есть записи, надо фильтровать дочернюю бк */
                    if (parentFieldValue != string.Empty)
                    {
                        string searchSpec = $"{childField.Name} = \"{parentFieldValue}\"";
                        boComponents.FirstOrDefault(i => i.Id == objectComponent.Id).SearchSpecification = searchSpec;

                        dataEntities = dataEntities
                            .AsQueryable()
                            .Where(searchSpec)
                            .ToList();
                    }
                    // Если записей нет, значит надо очистить дочернюю бк
                    else dataEntities = new List<MainTable>();
                }

                // Иначе
                else
                {
                    // Получаю текущую запись из автофокуса элемента представления
                    var selectedRecord = ComponentContext.GetSelectedRecord(busComp.Name);
                    var currentRecord = selectedRecord == null ? entityDBSet
                        .OrderByDescending(c => c.Created)
                        .ToList()
                        .ElementAtOrDefault(viewItem.AutofocusRecord)
                        : entityDBSet
                        .ToList()
                        .FirstOrDefault(i => i.Id.ToString() == selectedRecord);
                }

                // Отбор записей
                List<string> displayRecords = ComponentContext.GetDisplayedRecords(busComp.Name);
                List<string> dataEntitiesId = dataEntities.Select(i => i?.Id.ToString()).ToList();
                List<MainTable> currentRecords = new List<MainTable>();
                string selectedrecordId = ComponentContext.GetSelectedRecord(busComp.Name);
                if (viewInfo.CurrentApplet?.Name == model.AppletName)
                {
                    // Обнуление текущего контрола
                    viewInfo.CurrentControl = null;

                    switch (viewInfo.Action?.Name)
                    {
                        // Навигация вперед по списку
                        case "NextRecords":
                            // Ищу по id последней записи на тайле
                            MainTable lastRecord = dataEntities.FirstOrDefault(i => i.Id.ToString() == displayRecords.LastOrDefault());

                            // Следующие записи
                            dataEntities = dataEntities
                                .SkipWhile(i => i.Id != lastRecord.Id)
                                .Take(applet.DisplayLines)
                                .ToList();

                            // Если id последней записи в базе совпадает с id последней отоброжаемой записи, значит список долистали до конца
                            if (dataEntities.LastOrDefault().Id == lastRecord.Id) return BadRequest();

                            // Проставление текущей записи
                            var nextRecord = dataEntities.ElementAtOrDefault(viewItem.AutofocusRecord);
                            if (nextRecord != null && nextRecord?.Id != Guid.Empty)
                            {
                                viewInfo.CurrentRecord = nextRecord.Id.ToString();
                                ComponentContext.SetSelectedRecord(busComp.Name, nextRecord.Id.ToString());
                            }
                            break;

                        // Навигация назад по списку
                        case "PreviousRecords":
                            // Ищу по id первой записи на тайле
                            MainTable firstRecord = dataEntities.FirstOrDefault(i => i.Id.ToString() == displayRecords.FirstOrDefault());

                            // Предыдущие записи
                            dataEntities = dataEntities
                                .AsEnumerable()
                                .Reverse()
                                .SkipWhile(i => i.Id != firstRecord.Id)
                                .Take(applet.DisplayLines)
                                .Reverse()
                                .ToList();

                            // Если id первой записи в базе совпадает с id первой отоброжаемой записи, значит список долистали до начала
                            if (dataEntities.FirstOrDefault()?.Id == firstRecord.Id) return BadRequest();

                            // Проставление текущей записи
                            var previousRecord = dataEntities.ElementAtOrDefault(viewItem.AutofocusRecord);
                            if (previousRecord != null && previousRecord?.Id != Guid.Empty)
                            {
                                viewInfo.CurrentRecord = previousRecord.Id.ToString();
                                ComponentContext.SetSelectedRecord(busComp.Name, previousRecord.Id.ToString());
                            }
                            break;
                    }

                    if (viewInfo.CurrentPopupApplet != null)
                    {
                        // Обнуление текущего контрола
                        viewInfo.CurrentPopupControl = null;

                        switch (viewInfo.Action?.Name)
                        {
                            case "NewRecord":
                                // Удаление попап апплета из информации о представлении
                                viewInfo.RemovePopupApplet(context);
                                break;

                            case "UpdateRecord":
                                // Удаление попап апплета из информации о представлении
                                viewInfo.RemovePopupApplet(context);
                                break;
                        }
                    }
                }

                // В зависимости от действия, произошедшего в представлении
                switch (viewInfo.Action?.Name)
                {
                    case "InitializeView":
                        /* Промотка до текущей отоброжаемой записи
                         * Проверяется, что в список с текущими отображаемыми записями не пуст
                         * И что они содержат текущую выбранную запись */
                        if (displayRecords?.Count > 0 && dataEntitiesId.IndexOf(selectedrecordId) != -1)
                        {
                            displayRecords.ForEach(id => currentRecords.Add(dataEntities.FirstOrDefault(i => i.Id.ToString() == id)));
                            dataEntities = currentRecords;
                        }

                        else ComponentContext.SetSelectedRecord(busComp.Name, dataEntitiesId.ElementAtOrDefault(viewItem.AutofocusRecord));

                        // Установка текущей выбранной записи
                        if (viewInfo.CurrentApplet?.Name == model.AppletName)
                            viewInfo.CurrentRecord = ComponentContext.GetSelectedRecord(busComp.Name);

                        // Обнуление действия
                        viewInfo.Action = null;
                        break;

                    case "ReloadView":
                    case "UpdateRecord":
                        // Промотка до текущей отоброжаемой записи
                        displayRecords.ForEach(id => currentRecords.Add(dataEntities.FirstOrDefault(i => i.Id.ToString() == id)));
                        dataEntities = currentRecords;
                        break;

                    case "ShowPopup":
                    case "NextRecords":
                    case "PreviousRecords":
                    case "SelectTileItem":
                        if (viewInfo.CurrentApplet?.Name != model.AppletName)
                            ComponentContext.SetSelectedRecord(busComp.Name, dataEntitiesId.ElementAtOrDefault(viewItem.AutofocusRecord));
                        break;
                }

                if (ComponentContext.GetSelectedRecord(busComp.Name) == null)
                    ComponentContext.SetSelectedRecord(busComp.Name, dataEntitiesId.ElementAtOrDefault(viewItem.AutofocusRecord));

                // Ограничение записей по количество отоброжаемых
                dataEntities = dataEntities.Take(applet.DisplayLines).ToList();
                dataEntitiesId = dataEntities.Select(i => i?.Id.ToString()).ToList();
                ComponentContext.SetDisplayedRecords(busComp.Name, dataEntitiesId);

                // Маппинг в UI
                List<MainApplet> UIEntities = new List<MainApplet>();
                MainBUSUIFR busUIFactory = new MainBUSUIFR();
                MainDataBUSFR dataBUSFactory = new MainDataBUSFR();
                dataEntities.ForEach(dataEntity => 
                    UIEntities.Add(
                        busUIFactory.BusinessToUI(dataBUSFactory.DataToBusiness(dataEntity, context))
                    ));

                // Формирую результат, где указываются записи для апплета и текущие выделенные записи
                Dictionary<string, object> result = new Dictionary<string, object>();

                result.Add("SelectedRecords", ComponentContext.GetUIRecords(context, viewInfo));
                result.Add("DisplayedRecords", UIEntities);

                // Если все нормально возвращаю результат
                if (ModelState.IsValid)
                    return Ok(new JsonResult(result).Value);
                else return BadRequest(ModelState);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PickListRecords")]
        public virtual ActionResult<string> PickListRecords([FromBody] RequestAppletModel model)
        {
            PL pickList = context.PickLists.FirstOrDefault();
            return JsonConvert.SerializeObject(pickList, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpPost("NewRecord")]
        public virtual ActionResult NewRecord([FromBody] MainApplet model)
        {
            try
            {
                MainTable dataEntity = new MainTable();
                MainBUSUIFR busUIFactory = new MainBUSUIFR();
                MainDataBUSFR dataBUSFactory = new MainDataBUSFR();
                var businessComponent = busUIFactory.UIToBusiness(model, context, viewInfo, true);
                ValidationContext validationContext = new ValidationContext(businessComponent);
                IEnumerable<ValidationResult> result = busUIFactory.Validate(validationContext, businessComponent, model);
                if (!result.Any())
                {
                    dataEntity = dataBUSFactory.BusinessToData(businessComponent, entityDBSet, true);
                    dataBUSFactory.OnRecordCreate(dataEntity, entityDBSet, environment, context);
                    return Ok(dataEntity.Id);
                }
                
                else return BadRequest(result);
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("UpdateRecord")]
        public virtual ActionResult UpdateRecord([FromBody] MainApplet model)
        {
            try
            {
                MainBUSUIFR busUIFactory = new MainBUSUIFR();
                MainDataBUSFR dataBUSFactory = new MainDataBUSFR();
                var businessEntity = busUIFactory.UIToBusiness(model, context, viewInfo, false);
                ValidationContext validationContext = new ValidationContext(businessEntity);
                IEnumerable<ValidationResult> result = busUIFactory.Validate(validationContext, businessEntity, model);
                if (!result.Any())
                {
                    MainTable dataEntity = dataBUSFactory.BusinessToData(businessEntity, entityDBSet, false);
                    dataBUSFactory.OnRecordUpdate(dataEntity, entityDBSet, context);
                    return Ok(dataEntity.Id);
                }

                else return BadRequest(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("DeleteRecord")]
        public virtual ActionResult DeleteRecord([FromBody] RequestAppletModel model)
        {
            try
            {
                // Получение всех необходимых сущностей
                List<BOComponent> boComponents = viewInfo.BOComponents;
                Applet applet = viewInfo.ViewApplets.FirstOrDefault(n => n.Name == model.AppletName);
                BusComp busComp = viewInfo.ViewBCs.FirstOrDefault(i => i.Id == applet.BusCompId);
                BOComponent objectComponent = boComponents.FirstOrDefault(bcId => bcId.BusCompId == busComp.Id);
                string searchSpec = objectComponent.SearchSpecification;

                // Получение контекста
                IEnumerable<MainTable> dataEntities = entityDBSet
                    .OrderByDescending(c => c.Created)
                    .ToList();

                // Ограничение списка сущностей
                if (searchSpec != null)
                {
                    dataEntities = dataEntities
                        .AsQueryable()
                        .Where(searchSpec)
                        .ToList();
                }
                
                // Получение записи предназначенной для удаления
                MainTable currentEntity = dataEntities.FirstOrDefault(i => i.Id == model.Id);
                if (currentEntity == null) return NotFound();
                else
                {
                    // Удаление записи из списка отоброжаемых
                    List<string> displayRecords = ComponentContext.GetDisplayedRecords(busComp.Name);
                    displayRecords.Remove(currentEntity.Id.ToString());
                    ComponentContext.SetDisplayedRecords(busComp.Name, displayRecords);

                    // Получение следующей за этой записи
                    MainTable nextEntity = dataEntities
                        .SkipWhile(item => item != currentEntity)
                        .Skip(1)
                        .FirstOrDefault();

                    // Если ее не найдено, получение предыдущей
                    if (nextEntity == null)
                        nextEntity = dataEntities
                            .TakeWhile(item => item != currentEntity)
                            .LastOrDefault();

                    // Удаление выбранной записи
                    MainDataBUSFR dataBUSFactory = new MainDataBUSFR();
                    dataBUSFactory.OnRecordDelete(currentEntity, entityDBSet, context);

                    // Обязательно необходимо перекверить данные после удаления
                    dataEntities = entityDBSet
                            .OrderByDescending(c => c.Created)
                            .ToList();

                    // Ограничение списка сущностей
                    if (searchSpec != null)
                    {
                        dataEntities = dataEntities
                            .AsQueryable()
                            .Where(searchSpec)
                            .ToList();
                    }

                    // Если запись для фокуса найдена, то она добавляется в список отоброжаемых и возвращается
                    if (nextEntity != null)
                    {
                        // Пропуск текущих отоброжаемых записей и взятие слеующей
                        var record = dataEntities
                            .SkipWhile(i => i.Id.ToString() != displayRecords.LastOrDefault())
                            .Skip(1)
                            .FirstOrDefault();

                        // Если следущая запись есть, то она добавляется в список отоброжаемых и возвращается
                        if (record != null)
                            displayRecords.Add(record.Id.ToString());

                        ComponentContext.SetDisplayedRecords(busComp.Name, displayRecords);

                        return Ok(nextEntity.Id);
                    }
                    else return Ok(null);
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}