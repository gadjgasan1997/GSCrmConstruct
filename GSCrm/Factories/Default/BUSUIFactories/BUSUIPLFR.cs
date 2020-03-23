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
    public class BUSUIPLFR : MainBUSUIFR<BUSPL, UIPL>
    {
        public override UIPL BusinessToUI(BUSPL businessEntity)
        {
            UIPL UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.BusCompName = businessEntity.BusCompName;
            return UIEntity;
        }
        public override BUSPL UIToBusiness(UIPL UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSPL businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && context.PickLists.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Picklist with this name is already exists.";
            }
            else
            {
                businessEntity.BusCompName = UIEntity.BusCompName;
                BusComp busComp = context.BusinessComponents.FirstOrDefault(n => n.Name == UIEntity.BusCompName);
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp == null ? Guid.Empty : busComp.Id;
            }
            return businessEntity;
        }
        public override BUSPL Init()
        {
            BUSPL businessEntity = base.Init();
            businessEntity.BusCompName = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSPL businessComponent, UIPL UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.ErrorMessage == string.Empty)
            {
                if (string.IsNullOrWhiteSpace(UIEntity.BusCompName))
                    result.Add(new ValidationResult(
                        "Business component name is a required field",
                        new List<string>() { "BusCompName" }
                        ));
                if (businessComponent.BusComp == null && !string.IsNullOrWhiteSpace(UIEntity.BusCompName))
                    result.Add(new ValidationResult(
                        "Business component with this name not found",
                        new List<string>() { "BusCompName" }
                        ));
            }
            return result;
        }
    }
}
