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
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIColumnFR : MainBUSUIFR<BUSColumn, UIColumn>
    {
        public override UIColumn BusinessToUI(BUSColumn businessEntity)
        {
            UIColumn UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.Header = businessEntity.Header;
            UIEntity.FieldName = businessEntity.FieldName;
            return UIEntity;
        }
        public override BUSColumn UIToBusiness(UIColumn UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            Applet applet = context.Applets
                .Include(col => col.Columns)
                .FirstOrDefault(n => n.Id.ToString() == ComponentContext.GetSelectedRecord("Applet"));
            BUSColumn businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && applet != null && applet.Columns != null && 
                applet.Columns.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Column with this name is already exists in applet " + applet.Name + ".";
            }
            else
            {
                businessEntity.Header = UIEntity.Header;
                businessEntity.Applet = applet;
                businessEntity.AppletId = applet.Id;

                // BusComp
                BusComp busComp = context.BusinessComponents
                    .Include(field => field.Fields)
                    .FirstOrDefault(i => i.Id == applet.BusCompId);
                businessEntity.BusComp = busComp;

                // Field
                businessEntity.FieldName = UIEntity.FieldName;
                Field field = busComp.Fields.FirstOrDefault(n => n.Name == UIEntity.FieldName);
                if (field != null)
                {
                    businessEntity.Field = field;
                    businessEntity.FieldId = field.Id;
                }
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSColumn businessComponent, UIColumn UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.ErrorMessage == string.Empty)
            {
                if (businessComponent.BusComp == null)
                    result.Add(new ValidationResult(
                        "First you need to choose business component for this applet.",
                        new List<string>() { "BusComp" }
                        ));
                if (businessComponent.BusComp != null && string.IsNullOrWhiteSpace(UIEntity.FieldName))
                    result.Add(new ValidationResult(
                        "Field name is a required field.",
                        new List<string>() { "FieldName" }
                        ));
                if (businessComponent.BusComp != null && businessComponent.Field == null && !string.IsNullOrWhiteSpace(UIEntity.FieldName))
                    result.Add(new ValidationResult(
                        "Field with this name not found.",
                        new List<string>() { "FieldName" }
                        ));
            }
            return result;
        }
    }
}
