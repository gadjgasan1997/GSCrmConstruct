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
using GSCrmLibrary.Models.MainEntities;
using System;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIColumnFR<TContext> : BUSUIFactory<BUSColumn, UIColumn, TContext>
        where TContext : MainContext, new()
    {
        public override UIColumn BusinessToUI(BUSColumn businessEntity)
        {
            UIColumn UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.FieldName = businessEntity.FieldName;
            UIEntity.ActionType = businessEntity.ActionType.ToString();
            UIEntity.Header = businessEntity.Header;
            UIEntity.Type = businessEntity.Type;
            UIEntity.Required = businessEntity.Required;
            UIEntity.Readonly = businessEntity.Readonly;
            return UIEntity;
        }
        public override BUSColumn UIToBusiness(UIColumn UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSColumn businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Applet applet = context.Applets
                .AsNoTracking()
                .Select(a => new 
                {
                    id = a.Id,
                    name = a.Name,
                    busCompId = a.BusCompId,
                    columns = a.Columns.Select(column => new 
                    {
                        id = column.Id,
                        name = column.Name
                    })
                })
                .Select(a => new Applet
                {
                    Id = a.id,
                    Name = a.name,
                    BusCompId = a.busCompId,
                    Columns = a.columns.Select(column => new Column
                    {
                        Id = column.id,
                        Name = column.name
                    }).ToList()
                })
                .FirstOrDefault(n => n.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Applet"));
            if (applet == null)
                businessEntity.ErrorMessage = "First you need create applet.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                Column column = applet.Columns?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (column != null && column.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Column with this name is already exists in applet {applet.Name}.";

                else
                {
                    // Applet
                    businessEntity.Applet = applet;
                    businessEntity.AppletId = applet.Id;

                    // BusComp
                    BusinessComponent busComp = context.BusinessComponents
                        .AsNoTracking()
                        .Select(bc => new
                        {
                            id = bc.Id,
                            name = bc.Name,
                            fields = bc.Fields.Select(field => new
                            {
                                id = field.Id,
                                name = field.Name
                            })
                        })
                        .Select(bc => new BusinessComponent
                        {
                            Id = bc.id,
                            Name = bc.name,
                            Fields = bc.fields.Select(field => new Field
                            {
                                Id = field.id,
                                Name = field.name
                            }).ToList()
                        })
                        .FirstOrDefault(i => i.Id == applet.BusCompId);

                    // BusComp
                    if (busComp != null)
                    {
                        businessEntity.BusComp = busComp;
                        businessEntity.BusCompId = busComp.Id;

                        // Field
                        Field field = busComp.Fields.FirstOrDefault(n => n.Name == UIEntity.FieldName);
                        if (field != null)
                        {
                            businessEntity.Field = field;
                            businessEntity.FieldId = field.Id;
                            businessEntity.FieldName = field.Name;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(UIEntity.ActionType))
                    businessEntity.ActionType = ActionType.None;
                else businessEntity.ActionType = (ActionType)Enum.Parse(typeof(ActionType), UIEntity.ActionType);
                businessEntity.Readonly = UIEntity.Readonly;
                businessEntity.Header = UIEntity.Header;
                businessEntity.Type = UIEntity.Type;
                businessEntity.Required = UIEntity.Required;
            }
            return businessEntity;
        }
        public override BUSColumn Init(TContext context)
        {
            BUSColumn businessEntity = base.Init(context);
            Applet applet = context.Applets
                .AsNoTracking()
                .Select(a => new
                {
                    id = a.Id,
                    busCompId = a.BusCompId
                })
                .Select(a => new Applet
                {
                    Id = a.id,
                    BusCompId = a.busCompId
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Applet"));
            if (applet != null)
            {
                businessEntity.BusComp = applet.BusComp;
                businessEntity.BusCompId = applet.BusCompId;
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIColumn UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.ActionType))
                result.Add(new ValidationResult("Action type is a required field.", new List<string>() { "ActionType" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSColumn businessComponent, UIColumn UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (!string.IsNullOrWhiteSpace(UIEntity.FieldName))
                {
                    if (businessComponent.BusComp == null)
                    {
                        result.Add(new ValidationResult(
                            $"At first you need to add a business component to applet {businessComponent.Applet.Name}.",
                            new List<string>() { "BusComp" }));
                    }
                    else if (businessComponent.Field == null)
                        result.Add(new ValidationResult("Field with this name not found.", new List<string>() { "FieldName" }));
                }
            }
            return result;
        }
    }
}
