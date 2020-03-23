using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIBusObjectFR : MainBUSUIFR<BUSBusObject, UIBusObject>
    {
        public override UIBusObject BusinessToUI(BUSBusObject businessEntity)
        {
            UIBusObject UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.PrimaryBusCompName = businessEntity.PrimaryBusCompName;
            return UIEntity;
        }
        public override BUSBusObject UIToBusiness(UIBusObject UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSBusObject businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && context.BusinessObjects.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Record with this name is already exists.";
            }
            else
            {
                businessEntity.PrimaryBusCompName = UIEntity.PrimaryBusCompName;
                BusComp primaryBusComp = context.BusinessComponents.FirstOrDefault(n => n.Name == UIEntity.PrimaryBusCompName);
                if (primaryBusComp != null)
                {
                    businessEntity.PrimaryBusComp = primaryBusComp;
                    businessEntity.PrimaryBusCompId = primaryBusComp.Id;
                }
            }
            return businessEntity;
        }
        public override BUSBusObject Init()
        {
            BUSBusObject businessEntity = base.Init();
            businessEntity.PrimaryBusCompName = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSBusObject businessComponent, UIBusObject UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.ErrorMessage == string.Empty)
            {
                if (string.IsNullOrWhiteSpace(UIEntity.PrimaryBusCompName))
                    result.Add(new ValidationResult(
                        "Primary business component name is a required field.",
                        new List<string>() { "PrimaryBusCompName" }
                        ));
                if (businessComponent.PrimaryBusComp == null && !string.IsNullOrWhiteSpace(UIEntity.PrimaryBusCompName))
                    result.Add(new ValidationResult(
                        "Business component with this name not found.",
                        new List<string>() { "PrimaryBusCompName" }
                        ));
            }
            return result;
        }
    }
}
