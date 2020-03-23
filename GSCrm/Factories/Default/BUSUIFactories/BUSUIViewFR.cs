using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using GSCrm.Factories.Default.MainFactories;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIViewFR : MainBUSUIFR<BUSView, UIView>
    {
        public override UIView BusinessToUI(BUSView businessEntity)
        {
            UIView UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.BusObjectName = businessEntity.BusObjectName;
            return UIEntity;
        }
        public override BUSView UIToBusiness(UIView UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSView businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && context.Views.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "View with this name is already exists.";
            }
            else
            {
                businessEntity.BusObjectName = UIEntity.BusObjectName;
                BusObject busObject = context.BusinessObjects.FirstOrDefault(n => n.Name == UIEntity.BusObjectName);
                if (busObject != null)
                {
                    businessEntity.BusObject = busObject;
                    businessEntity.BusObjectId = busObject.Id;
                }
            }
            return businessEntity;
        }
        public override BUSView Init()
        {
            BUSView businessEntity = base.Init();
            businessEntity.BusObjectName = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSView businessComponent, UIView UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.ErrorMessage == string.Empty)
            {
                if (string.IsNullOrWhiteSpace(UIEntity.BusObjectName))
                    result.Add(new ValidationResult(
                        "Business object name is a required field",
                        new List<string>() { "BusObjectName" }
                        ));
                if (businessComponent.BusObject == null && !string.IsNullOrWhiteSpace(UIEntity.BusObjectName))
                    result.Add(new ValidationResult(
                        "Business object with this name not found",
                        new List<string>() { "BusObjectName" }
                        ));
            }
            return result;
        }
    }
}
