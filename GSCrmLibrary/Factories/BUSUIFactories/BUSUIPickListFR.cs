using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIPickListFR<TContext> : BUSUIFactory<BUSPickList, UIPickList, TContext>
        where TContext : MainContext, new()
    {
        public override UIPickList BusinessToUI(BUSPickList businessEntity)
        {
            UIPickList UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.BusCompName = businessEntity.BusCompName;
            UIEntity.Bounded = businessEntity.Bounded;
            UIEntity.SearchSpecification = businessEntity.SearchSpecification;
            return UIEntity;
        }
        public override BUSPickList UIToBusiness(UIPickList UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSPickList businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            BusinessComponent busComp = context.BusinessComponents.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.BusCompName);
            if (busComp != null)
            {
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp.Id;
                businessEntity.BusCompName = busComp.Name;
            }
            businessEntity.SearchSpecification = UIEntity.SearchSpecification;
            businessEntity.Bounded = UIEntity.Bounded;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIPickList UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            PickList pickList = context.PickLists.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (pickList != null && pickList.Id != UIEntity.Id)
                result.Add(new ValidationResult("Picklist with this name is already exists.", new List<string>() { "Name" }));
            if (string.IsNullOrWhiteSpace(UIEntity.BusCompName))
                result.Add(new ValidationResult(
                    "Business component name is a required field",
                    new List<string>() { "BusCompName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSPickList businessComponent, UIPickList UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.BusComp == null)
                    result.Add(new ValidationResult(
                        "Business component with this name not found",
                        new List<string>() { "BusCompName" }));
            }
            return result;
        }
    }
}
