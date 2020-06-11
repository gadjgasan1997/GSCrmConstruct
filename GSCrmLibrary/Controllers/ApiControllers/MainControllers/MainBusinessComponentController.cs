using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.RequestModels;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.CodeGeneration;
using static GSCrmLibrary.Configuration.EntitiesConfig;
using Table = GSCrmLibrary.Models.TableModels.Table;
using BusinessComponent = GSCrmLibrary.Models.TableModels.BusinessComponent;
using Applet = GSCrmLibrary.Models.TableModels.Applet;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.BUSUIFactories;
using GSCrmLibrary.Models.AppletModels;
using static GSCrmLibrary.Data.ComponentsRecordsInfo;
using Microsoft.CSharp.RuntimeBinder;

namespace GSCrmLibrary.Controllers.ApiControllers.MainControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainBusinessComponentController<TTable, TBusinessComponent, TContext, TDataBUSFactory, TBUSFactory> : ControllerBase
        where TTable : class, IDataEntity, new()
        where TBusinessComponent : IBUSEntity, new()
        where TContext : MainContext, new()
        where TDataBUSFactory : IDataBUSFactory<TTable, TBusinessComponent, TContext>, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        private readonly TContext context;
        private readonly DbSet<TTable> entityDBSet;
        private readonly IOrderedEnumerable<TTable> orderedEntities;
        private readonly IViewInfo viewInfo;
        public MainBusinessComponentController(TContext context, IViewInfo viewInfo)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            entityDBSet = context.Set<TTable>();
            orderedEntities = entityDBSet.ToList().OrderByDescending(c => c.Created);
            this.viewInfo = viewInfo;
        }

        [HttpGet("GetComponentRecord/{componentName}")]
        public TBusinessComponent GetComponentRecord(string componentName)
        {
            TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
            TBusinessComponent record;
            if (ComponentsContext<TBusinessComponent>.TryGetComponentContext(componentName, out TBusinessComponent currentRecord))
                record = currentRecord;
            else record = dataBUSFactory.DataToBusiness(orderedEntities.FirstOrDefault(i => i.Id.ToString() == viewInfo.CurrentRecord), context);
            return record;
        }

        [HttpGet("GetComponentRecord")]
        public TBusinessComponent GetComponentRecord(string propertyName, string propertyValue)
        {
            TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
            return dataBUSFactory.DataToBusiness(orderedEntities.AsQueryable().FirstOrDefault($"{propertyName} = \"{propertyValue}\""), context);
        }

        [HttpGet("SetComponentRecord/{componentName}")]
        public void SetComponentRecord(string componentName, dynamic record)
            => ComponentsContext<TBusinessComponent>.SetComponentContext(componentName, record);

        [HttpGet("ApplyTable")]
        public ActionResult<object> ApplyTable()
        {
            Table table = context.Tables
                .AsNoTracking()
                .Include(tc => tc.TableColumns)
                .FirstOrDefault(i => i.Id.ToString() == GetSelectedRecord("Table"));
            if (table != null && table.TableColumns.Count != 0 && (table.TableColumns.FirstOrDefault(u => u.NeedCreate == true) != null || table.TableColumns.FirstOrDefault(u => u.NeedUpdate == true) != null))
            {
                string migrationName = table.Name + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                CodGenMigration.CreateMigration(context, table, migrationName);
                table.IsApply = true;
                context.Entry(table).State = EntityState.Modified;
                table.TableColumns.ForEach(tableColumn =>
                {
                    tableColumn.NeedCreate = false;
                    tableColumn.NeedUpdate = false;
                    context.Entry(tableColumn).State = EntityState.Modified;
                });
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("Publish")]
        public ActionResult Publish()
        {
            dynamic currentRecord;
            TBUSFactory busFactory = new TBUSFactory();
            switch (typeof(TTable).Name)
            {
                case "Table":
                    currentRecord = busFactory.GetRecord(null, context, viewInfo, context.BusinessComponents.FirstOrDefault(n => n.Name == "Table"));
                    CodGenModels.GenerateModel(context, typeof(Table), currentRecord.Id);
                    CodGenContext.AddEntityToContext(currentRecord.Name);
                    break;
                case "BusinessComponent":
                    currentRecord = busFactory.GetRecord(null, context, viewInfo, context.BusinessComponents.FirstOrDefault(n => n.Name == "Business Component"));
                    CodGenModels.GenerateModel(context, typeof(BusinessComponent), currentRecord.Id);
                    CodGenFactories.GenerateFactory(context, DataFR, currentRecord.Id);
                    CodGenController.GenerateComponentCotroller(context, currentRecord.Id);
                    break;
                case "Applet":
                    currentRecord = busFactory.GetRecord(null, context, viewInfo, context.BusinessComponents.FirstOrDefault(n => n.Name == "Applet"));
                    CodGenModels.GenerateModel(context, typeof(Applet), currentRecord.Id);
                    CodGenFactories.GenerateFactory(context, UIFR, currentRecord.Id);
                    CodGenController.GenerateAppletCotroller(context, currentRecord.Id);
                    break;
                case "View":
                    currentRecord = busFactory.GetRecord(null, context, viewInfo, context.BusinessComponents.FirstOrDefault(n => n.Name == "View"));
                    CodGenFactories.GenerateLinksForApplets(context, currentRecord.Id);
                    break;
                case "Screen":
                    currentRecord = busFactory.GetRecord(null, context, viewInfo, context.BusinessComponents.FirstOrDefault(n => n.Name == "Screen"));
                    CodGenController.GenerateScreenController(context, currentRecord.Id);
                    break;
                default:
                    break;
            }
            return Ok();
        }

        [HttpGet("PickListRecords")]
        public ActionResult<object> PickListRecords()
        {
            Control control = viewInfo.CurrentPopupControl ?? viewInfo.CurrentControl;
            if (control?.Field != null)
            {
                // Получение необходимых сущностей
                BusinessComponent busComp = context.BusinessComponents
                    .AsNoTracking()
                    .Include(f => f.Fields)
                        .ThenInclude(pl => pl.PickList)
                    .Include(f => f.Fields)
                        .ThenInclude(pl => pl.PickMaps)
                    .FirstOrDefault(i => i.Id == control.Field.BusCompId);
                TBUSFactory busFactory = new TBUSFactory();
                TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
                List<TBusinessComponent> businessEntities = new List<TBusinessComponent>();
                Field field = busComp.Fields.FirstOrDefault(i => i.Id == control.FieldId);
                PickList pickList = field.PickList;
                List<PickMap> pickMaps = field.PickMaps;
                PickMap mapForCurrentField = pickMaps.FirstOrDefault(f => f.BusCompFieldId == field.Id);

                // Текущая запись
                dynamic currentRecord = busFactory.GetRecord(null, context, viewInfo, busComp);
                DataBUSPickMapFR<TContext> dataBUSPickMapFR = new DataBUSPickMapFR<TContext>();
                DataBUSPickListFR<TContext> dataBUSPickListFR = new DataBUSPickListFR<TContext>();
                BUSUIPickListFR<TContext> busUIPickListFR = new BUSUIPickListFR<TContext>();

                // Мапа, на основании которой принимается решение о том, данные какого поля будут отображаться в пиклисте
                if (mapForCurrentField != null)
                {
                    IEnumerable<TTable> dataEntities = orderedEntities.ToList();
                    List<PickMap> constrainPickMaps = pickMaps.Where(c => c.Constrain).ToList();
                    if (currentRecord != null)
                    {
                        // Ограничение отбираемых для пиклсита записей по constrain пик мапам
                        constrainPickMaps.ForEach(constrainMap =>
                        {
                            BUSPickMap businessEntity = dataBUSPickMapFR.DataToBusiness(constrainMap, context);
                            string constrainPickListField = businessEntity.PickListFieldName;
                            string constrainComponentField = businessEntity.BusCompFieldName;
                            dynamic constrainComponentFieldValue = currentRecord.GetType().GetProperty(constrainComponentField).GetValue(currentRecord);
                            if (constrainComponentFieldValue != null)
                            {
                                constrainComponentFieldValue = constrainComponentFieldValue.ToString();
                                dataEntities = dataEntities.AsQueryable().Where($"{constrainPickListField} = \"{constrainComponentFieldValue}\"").ToList();
                            }
                            else dataEntities = new List<TTable>();
                        });
                    }
                    // Ограничение отбираемых для пиклсита записей по search spec на пиклисте
                    if (!string.IsNullOrWhiteSpace(pickList.SearchSpecification))
                    {
                        var conversionSearchSpecification = Utils.SearchSpecificationConversion(pickList.SearchSpecification, currentRecord);
                        dataEntities = dataEntities.AsQueryable().Where($"{conversionSearchSpecification}").ToList();
                    }

                    // Получение названий для филды с бизнес компоненты и пиклиста для отбора отображаемых записей и получение текущей
                    BUSPickMap businessEntity = dataBUSPickMapFR.DataToBusiness(mapForCurrentField, context);
                    string componentField = businessEntity.BusCompFieldName;
                    string pickListField = businessEntity.PickListFieldName;
                    dataEntities.ToList().ForEach(dataEntity => businessEntities.Add(dataBUSFactory.DataToBusiness(dataEntity, context)));
                    ComponentsContext<TBusinessComponent>.SetPickListContext(businessEntities);
                    IQueryable displayedPickListRecords = businessEntities.AsQueryable().Select($"new(Id, {pickListField} as Value)");
                    UIPickList pickListInfo = busUIPickListFR.BusinessToUI(dataBUSPickListFR.DataToBusiness(pickList, context));

                    // Установка текущей выбранной записи в пиклисте
                    if (currentRecord != null)
                    {
                        pickListInfo.CurrentRecordId = businessEntities.AsQueryable()
                            .FirstOrDefault($"{pickListField} = \"{currentRecord.GetType().GetProperty(componentField).GetValue(currentRecord)}\"")?.Id;
                    }

                    return JsonConvert.SerializeObject(
                        new Dictionary<string, object>
                        {
                                { "PickListInfo", pickListInfo },
                                { "DisplayedPickListRecords", displayedPickListRecords }
                        }, Formatting.Indented, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                }
                else return null;
            }
            else return null;
        }

        [HttpPost("Pick")]
        public ActionResult<object> Pick(RequestPickListModel model)
        {
            string newPickedValue = string.Empty;
            Control control = viewInfo.CurrentPopupControl ?? viewInfo.CurrentControl;
            Dictionary<string, object> result = new Dictionary<string, object>()
            {
                { "ErrorMessages", new object() { } },
                { "NewPickedValue", newPickedValue },
                { "Status", string.Empty }
            };
            if (control?.Field != null)
            {
                // Получение необходимых сущностей
                TBUSFactory busFactory = new TBUSFactory();
                BusinessComponent busComp = context.BusinessComponents
                    .AsNoTracking()
                    .Include(f => f.Fields)
                        .ThenInclude(pl => pl.PickList)
                    .Include(f => f.Fields)
                        .ThenInclude(pl => pl.PickMaps)
                    .FirstOrDefault(i => i.Id == control.Field.BusCompId);
                Field field = busComp.Fields.FirstOrDefault(i => i.Id == control.FieldId);
                PickList pickList = field.PickList;
                BusinessComponent pickListBusComp = context.BusinessComponents
                    .AsNoTracking()
                    .Include(f => f.Fields)
                    .FirstOrDefault(i => i.Id == pickList.BusCompId);
                List<PickMap> pickMaps = field.PickMaps;
                PickMap mapForCurrentField = pickMaps.FirstOrDefault(f => f.BusCompFieldId == field.Id);
                List<TBusinessComponent> pickListRecords = ComponentsContext<TBusinessComponent>.GetPickListContext();

                // Пикнутая и текущая запись
                dynamic pickedRecord = model.Value ?? busFactory.GetRecord(null, context, viewInfo, pickListBusComp, "Id", model.PickedRecord);
                dynamic currentRecord = busFactory.GetRecord(null, context, viewInfo, busComp);

                // Если произошел выбор записи из пиклсита
                if (model.IsPicked)
                {
                    // Пик на основе пикмапы
                    pickMaps.Where(c => c.Constrain == false).OrderBy(s => s.Sequence).ToList().ForEach(map =>
                    {
                        string componentField = busComp.Fields.FirstOrDefault(i => i.Id == map.BusCompFieldId).Name;
                        string PLComponentField = pickListBusComp.Fields.FirstOrDefault(i => i.Id == map.PickListFieldId).Name;
                        dynamic PLComponentFieldValue = pickedRecord.GetType().GetProperty(PLComponentField).GetValue(pickedRecord);
                        var PLComponentFieldType = PLComponentFieldValue.GetType();
                        Type componentFieldType = currentRecord.GetType().GetProperty(componentField).GetValue(currentRecord)?.GetType();
                        if (componentFieldType?.BaseType == typeof(Enum))
                        {
                            switch (componentFieldType.Name)
                            {
                                case "ActionType":
                                    PLComponentFieldValue = (ActionType)Enum.Parse(typeof(ActionType), PLComponentFieldValue);
                                    break;
                            }
                        }
                        try
                        {
                            currentRecord.GetType().GetProperty(componentField).SetValue(currentRecord, PLComponentFieldValue);
                        }
                        catch(RuntimeBinderException ex)
                        {
                            result["ErrorMessages"] = Utils.GetErrorsInfo(ex);
                            result["Status"] = "Fail";
                        }
                        if (map.Id == mapForCurrentField.Id)
                            newPickedValue = PLComponentFieldValue.ToString();
                    });
                }

                // Если пользователь ввел значение сам или стер его
                else
                {
                    newPickedValue = string.Empty;
                    string componentField = busComp.Fields.FirstOrDefault(i => i.Id == mapForCurrentField.BusCompFieldId).Name;
                    // Если пиклист не позволяет принять введенное пользователем значение
                    if (pickList.Bounded)
                    {
                        // Если значение не пустое, то выполняется поиск такого значения в пиклисте
                        if (!string.IsNullOrWhiteSpace(model.Value))
                        {
                            // Если значение найдено, то оно пикается
                            if (pickListRecords.AsQueryable().Any($"{pickListBusComp.Fields.FirstOrDefault(i => i.Id == mapForCurrentField.PickListFieldId).Name} = \"{model.Value}\""))
                            {
                                dynamic newValue = model.Value;
                                PropertyInfo property = currentRecord.GetType().GetProperty(componentField);
                                if (property?.Name == "ActionType")
                                {
                                    newValue = ActionType.None;
                                    newPickedValue = "None";
                                }
                                currentRecord.GetType().GetProperty(componentField).SetValue(currentRecord, newValue);
                                newPickedValue = model.Value;
                            }
                            // Иначе текущая запись никак не меняется и на фронт возвращаетя старое значение
                            else
                            {
                                newPickedValue = currentRecord.GetType().GetProperty(componentField).GetValue(currentRecord).ToString();
                                result["ErrorMessages"] = new object[] { "This pick list does not allow you to enter your values. Choose a value from the suggested." };
                                result["Status"] = "Fail";
                            }
                        }
                        // Иначе в текущюю запись проставляется пустая строка, она же и возвращается на фронт
                        else
                        {
                            // Пик на основе пикмапы
                            pickMaps.Where(c => c.Constrain == false).OrderBy(s => s.Sequence).ToList().ForEach(map =>
                            {
                                // Для каждого поля из пикмапы установка дефолтового значения на основании его типа
                                dynamic newValue = null;
                                componentField = busComp.Fields.FirstOrDefault(i => i.Id == map.BusCompFieldId).Name;
                                PropertyInfo property = currentRecord.GetType().GetProperty(componentField);
                                Type propertyType = property.GetType();
                                if (propertyType.GetTypeInfo().IsValueType)
                                    newValue = Activator.CreateInstance(propertyType);
                                if (property?.Name == "ActionType")
                                {
                                    newValue = ActionType.None;
                                    newPickedValue = "None";
                                }
                                else newPickedValue = string.Empty;
                                currentRecord.GetType().GetProperty(componentField).SetValue(currentRecord, newValue);
                            });
                        }
                    }
                    // Иначе
                    else
                    {
                        currentRecord.GetType().GetProperty(componentField).SetValue(currentRecord, model.Value);
                        newPickedValue = model.Value;
                    }
                }

                // Установка текущей записи
                busFactory.SetRecord(null, context, viewInfo, busComp, currentRecord);
            }
            if (result["Status"].ToString() == string.Empty)
                result["Status"] = "Done";
            result["NewPickedValue"] = newPickedValue;
            return result;
        }

        [HttpPost("FilterEntities")]
        public List<TTable> FilterEntities(FilterEntitiesModel model)
           => EntitiesUtils<TTable, TBusinessComponent, TDataBUSFactory, TContext>.FilterEntities(context, model, orderedEntities);

        [HttpPost("FilterEntitiesGetId")]
        public List<string> FilterEntitiesGetId(FilterEntitiesModel model)
           => EntitiesUtils<TTable, TBusinessComponent, TDataBUSFactory, TContext>.FilterEntities(context, model, orderedEntities).Select(I => I.Id.ToString()).ToList();

        private string GetRandomName()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 15; i++)
                str += "qwertyuiopasdfghjklzxcvbnmйцукенгшщзхъфывапролдячсмитьбюэ"[random.Next(0, 56)].ToString();
            return str;
        }
    }
}