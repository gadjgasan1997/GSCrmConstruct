using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using System;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIPickMapFR<TContext> : BUSUIFactory<BUSPickMap, UIPickMap, TContext>
        where TContext : MainContext, new()
    {
        public override UIPickMap BusinessToUI(BUSPickMap businessEntity)
        {
            UIPickMap UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.BusCompFieldName = businessEntity.BusCompFieldName;
            UIEntity.PickListFieldName = businessEntity.PickListFieldName;
            UIEntity.Constrain = businessEntity.Constrain;
            return UIEntity;
        }
        public override BUSPickMap UIToBusiness(UIPickMap UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSPickMap businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Field field = context.Fields.FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Field"));
            if (field == null)
                businessEntity.ErrorMessage = "First you need create field.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                PickMap pickMap = field.PickMaps?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (pickMap != null && pickMap.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Field pick map with this name is already exists in field {UIEntity.Name}.";
                else
                {

                    businessEntity.Field = field;
                    businessEntity.FieldId = field.Id;

                    // BusComp and pickList
                    PickList pickList = context.PickLists
                        .AsNoTracking()
                        .Select(pl => new
                        {
                            id = pl.Id,
                            busComp = new
                            {
                                id = pl.BusCompId,
                                fields = pl.BusComp.Fields.Select(field => new
                                {
                                    id = field.Id,
                                    name = field.Name,
                                })
                            }
                        })
                        .Select(pl => new PickList
                        {
                            Id = pl.id,
                            BusComp = new BusinessComponent
                            {
                                Id = pl.busComp.id,
                                Fields = pl.busComp.fields.Select(field => new Field
                                {
                                    Id = field.id,
                                    Name = field.name
                                }).ToList()
                            }
                        })
                        .FirstOrDefault(i => i.Id == field.PickListId);

                    // PickList
                    if (pickList != null)
                    {
                        businessEntity.PickList = pickList;
                        businessEntity.PickListId = pickList.Id;

                        BusinessComponent busComp = context.BusinessComponents
                            .AsNoTracking()
                            .Select(bc => new
                            {
                                id = bc.Id,
                                fields = bc.Fields.Select(field => new
                                {
                                    id = field.Id,
                                    name = field.Name,
                                })
                            })
                            .Select(bc => new BusinessComponent
                            {
                                Id = bc.id,
                                Fields = bc.fields.Select(field => new Field
                                {
                                    Id = field.id,
                                    Name = field.name
                                }).ToList()
                            })
                            .FirstOrDefault(i => i.Id == field.BusCompId);

                        BusinessComponent pickListBusComp = pickList.BusComp;
                        businessEntity.BusComp = busComp;
                        businessEntity.BusCompId = busComp.Id;

                        if (pickListBusComp != null)
                        {
                            businessEntity.PickListBusComp = pickListBusComp;
                            businessEntity.PickListBusCompId = pickListBusComp.Id;

                            // BusCompField
                            Field busCompField = busComp.Fields.FirstOrDefault(n => n.Name == UIEntity.BusCompFieldName);
                            if (busCompField != null)
                            {
                                businessEntity.BusCompField = busCompField;
                                businessEntity.BusCompFieldId = busCompField.Id;
                                businessEntity.BusCompFieldName = busCompField.Name;
                            }

                            // PickListField
                            Field pickListField = pickListBusComp.Fields.FirstOrDefault(n => n.Name == UIEntity.PickListFieldName);
                            if (pickListField != null)
                            {
                                businessEntity.PickListField = pickListField;
                                businessEntity.PickListFieldId = pickListField.Id;
                                businessEntity.PickListFieldName = pickListField.Name;
                            }
                        }
                    }

                    businessEntity.Constrain = UIEntity.Constrain;
                }
            }
            return businessEntity;
        }
        public override BUSPickMap Init(TContext context)
        {
            BUSPickMap businessEntity = base.Init(context);
            Field field = context.Fields
                .AsNoTracking()
                .Select(f => new
                {
                    id = f.Id,
                    busComp = f.BusComp,
                    pl = new
                    {
                        id = f.PickListId,
                        busComp = f.PickList.BusComp
                    }
                })
                .Select(f => new Field
                {
                    Id = f.id,
                    BusComp = f.busComp,
                    BusCompId = f.busComp.Id,
                    PickList = new PickList
                    {
                        Id = (Guid)f.pl.id,
                        BusComp = f.pl.busComp,
                        BusCompId = f.pl.busComp.Id
                    }
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Field"));
            if (field != null)
            {
                businessEntity.BusComp = field.BusComp;
                businessEntity.BusCompId = field.BusCompId;
                if (field.PickList?.BusComp != null)
                {
                    businessEntity.PickListBusComp = field.PickList.BusComp;
                    businessEntity.PickListBusCompId = field.PickList.BusCompId;
                }
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIPickMap UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.BusCompFieldName))
                result.Add(new ValidationResult(
                    "Business component field name is a required field.",
                    new List<string>() { "BusCompFieldName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.PickListFieldName))
                result.Add(new ValidationResult(
                    "Pick list field name is a required field.",
                    new List<string>() { "PickListFieldName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSPickMap businessComponent, UIPickMap UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.PickList == null)
                {
                    result.Add(new ValidationResult(
                        "At first you need to add a picklist to field " + businessComponent.Field.Name + ".",
                        new List<string>() { "PickList" }));
                }
                else if (businessComponent.PickListBusComp == null)
                    result.Add(new ValidationResult(
                        "At first you need to add a business component to picklist " + businessComponent.PickList.Name + ".",
                        new List<string>() { "PickListBusComp" }));
                else
                {
                    if (businessComponent.BusCompField == null)
                        result.Add(new ValidationResult(
                            "Field with this name not found.",
                            new List<string>() { "BusCompField" }));
                    
                    if (businessComponent.PickListField == null)
                        result.Add(new ValidationResult(
                            "Field with this name not found.",
                            new List<string>() { "PickListField" }));
                }
            }
            return result;
        }
    }
}
