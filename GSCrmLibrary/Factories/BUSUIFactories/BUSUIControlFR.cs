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
    public class BUSUIControlFR<TContext> : BUSUIFactory<BUSControl, UIControl, TContext>
        where TContext : MainContext, new()
    {
        public override UIControl BusinessToUI(BUSControl businessEntity)
        {
            UIControl UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.CssClass = businessEntity.CssClass;
            UIEntity.IconName = businessEntity.IconName;
            UIEntity.FieldName = businessEntity.FieldName;
            UIEntity.ActionType = businessEntity.ActionType.ToString();
            UIEntity.Header = businessEntity.Header;
            UIEntity.Type = businessEntity.Type;
            UIEntity.Required = businessEntity.Required;
            UIEntity.Readonly = businessEntity.Readonly;
            return UIEntity;
        }
        public override BUSControl UIToBusiness(UIControl UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSControl businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Applet applet = context.Applets
                .AsNoTracking()
                .Select(a => new 
                {
                    id = a.Id,
                    name = a.Name,
                    busCompId = a.BusCompId,
                    controls = a.Controls.Select(control => new 
                    {
                        id = control.Id,
                        name = control.Name
                    })
                })
                .Select(a => new Applet
                {
                    Id = a.id,
                    Name = a.name,
                    BusCompId = a.busCompId,
                    Controls = a.controls.Select(control => new Control
                    {
                        Id = control.id,
                        Name = control.name
                    }).ToList()
                })
                .FirstOrDefault(n => n.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Applet"));
            if (applet == null)
                businessEntity.ErrorMessage = "First you need create applet.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                Control control = applet.Controls?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (control != null && control.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Control with this name is already exists in applet \"{applet.Name}\".";
                else
                {
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
                            businessEntity.FieldName = UIEntity.FieldName;
                        }
                    }

                    // Icon
                    Icon icon = context.Icons.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.IconName);
                    if (icon != null)
                    {
                        businessEntity.Icon = icon;
                        businessEntity.IconId = icon.Id;
                        businessEntity.IconName = icon.Name;
                    }

                    if (string.IsNullOrWhiteSpace(UIEntity.ActionType))
                        businessEntity.ActionType = ActionType.None;
                    businessEntity.ActionType = (ActionType)Enum.Parse(typeof(ActionType), UIEntity.ActionType);
                    businessEntity.Readonly = UIEntity.Readonly;
                    businessEntity.Header = UIEntity.Header;
                    businessEntity.Type = UIEntity.Type;
                    businessEntity.Required = UIEntity.Required;
                    businessEntity.CssClass = UIEntity.CssClass;
                }
            }
            return businessEntity;
        }
        public override BUSControl Init(TContext context)
        {
            BUSControl businessEntity = base.Init(context);
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
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIControl UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.Type))
                result.Add(new ValidationResult("Type is a required field.", new List<string>() { "Type" }));
            if (string.IsNullOrWhiteSpace(UIEntity.ActionType))
                result.Add(new ValidationResult("Action type is a required field.", new List<string>() { "ActionType" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSControl businessComponent, UIControl UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.BusComp == null && !string.IsNullOrWhiteSpace(UIEntity.FieldName))
                {
                    result.Add(new ValidationResult(
                        $"At first you need to add a business component to applet {businessComponent.Applet.Name}.",
                        new List<string>() { "BusComp" }));
                }
                else if (businessComponent.BusComp != null && businessComponent.Field == null && !string.IsNullOrWhiteSpace(UIEntity.FieldName))
                    result.Add(new ValidationResult("Field with this name not found.", new List<string>() { "FieldName" }));
                if (businessComponent.Icon == null && !string.IsNullOrWhiteSpace(UIEntity.IconName))
                    result.Add(new ValidationResult("Icon with this name not found.", new List<string>() { "IconName" }));
            }
            return result;
        }
    }
}
