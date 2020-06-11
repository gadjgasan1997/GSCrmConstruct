using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;

namespace GSCrmLibrary.Factories.MainFactories
{
    public class BUSUIUserPropertyFactory<TBUSUserProperty, TAppletUserProperty, TContext>
        : BUSUIFactory<TBUSUserProperty, TAppletUserProperty, TContext>
        where TBUSUserProperty : Models.MainEntities.BUSEntityUP, new()
        where TAppletUserProperty : Models.MainEntities.UIEntityUP, new()
        where TContext : MainContext, new()
    {
        public override TAppletUserProperty BusinessToUI(TBUSUserProperty businessEntity)
        {
            TAppletUserProperty UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.Value = businessEntity.Value;
            return UIEntity;
        }
        public override TBUSUserProperty UIToBusiness(TAppletUserProperty UIEntity, TContext context, IViewInfo viewInfo, bool NewRecord)
        {
            TBUSUserProperty businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.Value = UIEntity.Value;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, TBUSUserProperty businessComponent, TAppletUserProperty UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (string.IsNullOrWhiteSpace(UIEntity.Value))
                    result.Add(new ValidationResult("Value is a required field.", new List<string>() { "Value" }));
            }
            return result;
        }
    }
}
