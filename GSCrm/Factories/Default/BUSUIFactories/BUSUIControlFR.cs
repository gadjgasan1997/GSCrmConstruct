using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using Action = GSCrm.Models.Default.TableModels.Action;
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIControlFR : MainBUSUIFR<BUSControl, UIControl>
    {
        public override UIControl BusinessToUI(BUSControl businessEntity)
        {
            UIControl UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.CssClass = businessEntity.CssClass;
            UIEntity.IconName = businessEntity.IconName;
            UIEntity.Header = businessEntity.Header;
            UIEntity.FieldName = businessEntity.FieldName;
            UIEntity.ActionName = businessEntity.ActionName;
            return UIEntity;
        }
        public override BUSControl UIToBusiness(UIControl UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            Applet applet = context.Applets
                .Include(cntr => cntr.Controls)
                .FirstOrDefault(n => n.Id.ToString() == ComponentContext.GetSelectedRecord("Applet"));
            BUSControl businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && applet != null && applet.Controls != null && 
                applet.Controls.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Control with this name is already exists in applet " + applet.Name + ".";
            }
            else
            {
                businessEntity.Header = UIEntity.Header;
                businessEntity.Applet = applet;
                businessEntity.AppletId = applet.Id;
                businessEntity.CssClass = UIEntity.CssClass;

                // Icon
                Icon icon = context.Icons.FirstOrDefault(n => n.Name == UIEntity.IconName);
                if (icon != null)
                {
                    businessEntity.Icon = icon;
                    businessEntity.IconId = icon.Id;
                    businessEntity.IconName = icon.Name;
                }

                // BusComp
                BusComp busComp = context.BusinessComponents
                    .Include(f => f.Fields)
                    .FirstOrDefault(i => i.Id == applet.BusCompId);
                businessEntity.BusComp = busComp;

                // Field
                Field field = busComp.Fields.FirstOrDefault(n => n.Name == UIEntity.FieldName);
                if (field != null)
                {
                    businessEntity.Field = field;
                    businessEntity.FieldId = field.Id;
                    businessEntity.FieldName = UIEntity.FieldName;
                }

                // Action
                Action action = context.Actions.FirstOrDefault(n => n.Name == UIEntity.ActionName);
                if (action != null)
                {
                    businessEntity.Action = action;
                    businessEntity.ActionId = action.Id;
                    businessEntity.ActionName = action.Name;
                }
            }
            return businessEntity;
        }
        public override BUSControl Init()
        {
            BUSControl businessEntity = base.Init();
            businessEntity.ActionName = "";
            businessEntity.CssClass = "";
            businessEntity.FieldName = "";
            businessEntity.Header = "";
            businessEntity.IconName = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSControl businessComponent, UIControl UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.ErrorMessage == string.Empty)
            {
                if (businessComponent.BusComp == null)
                    result.Add(new ValidationResult(
                        "First you need to choose business component for this applet.",
                        new List<string>() { "BusComp" }
                        ));
                if (businessComponent.Action == null && !string.IsNullOrWhiteSpace(UIEntity.ActionName))
                    result.Add(new ValidationResult(
                        "Action with this name not found.",
                        new List<string>() { "ActionName" }
                        ));
                if (businessComponent.Icon == null && !string.IsNullOrWhiteSpace(UIEntity.IconName))
                    result.Add(new ValidationResult(
                        "Icon with this name not found.",
                        new List<string>() { "IconName" }
                        ));
                if (businessComponent.BusComp == null)
                    result.Add(new ValidationResult(
                        "First you need to choose business component for this applet.",
                        new List<string>() { "BusComp" }
                        ));
                if (businessComponent.BusComp != null && businessComponent.Field == null && !string.IsNullOrWhiteSpace(UIEntity.FieldName))
                    result.Add(new ValidationResult(
                        "Field with this name not found.",
                        new List<string>() { "FieldName" }
                        ));
                if (string.IsNullOrWhiteSpace(businessComponent.Type))
                    result.Add(new ValidationResult(
                        "Type is a required field.",
                        new List<string>() { "Type" }
                        ));
            }
            return result;
        }
    }
}
