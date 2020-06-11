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

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIViewFR<TContext> : BUSUIFactory<BUSView, UIView, TContext>
        where TContext : MainContext, new()
    {
        public override UIView BusinessToUI(BUSView businessEntity)
        {
            UIView UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.BusObjectName = businessEntity.BusObjectName;
            return UIEntity;
        }
        public override BUSView UIToBusiness(UIView UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSView businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            BusinessObject busObject = context.BusinessObjects.FirstOrDefault(n => n.Name == UIEntity.BusObjectName);
            if (busObject != null)
            {
                businessEntity.BusObject = busObject;
                businessEntity.BusObjectId = busObject.Id;
                businessEntity.BusObjectName = busObject.Name;
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIView UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            View view = context.Views.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (view != null && view.Id != UIEntity.Id)
                result.Add(new ValidationResult("View with this name is already exists.", new List<string>() { "Name" }));
            if (string.IsNullOrWhiteSpace(UIEntity.BusObjectName))
                result.Add(new ValidationResult(
                    "Business object name is a required field",
                    new List<string>() { "BusObjectName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSView businessComponent, UIView UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.BusObject == null)
                    result.Add(new ValidationResult(
                        "Business object with this name not found",
                        new List<string>() { "BusObjectName" }));
            }
            return result;
        }
    }
}
