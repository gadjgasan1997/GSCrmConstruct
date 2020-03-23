using GSCrm.Data;
using GSCrm.Services.Info;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GSCrm.Factories.Default.MainFactories
{
    public class MainUPBUSUIFR<MainBUSUP, MainAppletUP>
        : MainBUSUIFR<MainBUSUP, MainAppletUP>
        where MainBUSUP : Models.Default.MainEntities.MainBUSUP, new()
        where MainAppletUP : Models.Default.MainEntities.MainAppletUP, new()
    {
        public override MainAppletUP BusinessToUI(MainBUSUP businessEntity)
        {
            MainAppletUP UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.Value = businessEntity.Value;
            return UIEntity;
        }
        public override MainBUSUP UIToBusiness(MainAppletUP UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            MainBUSUP businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.Value = UIEntity.Value;
            return businessEntity;
        }
        public override MainBUSUP Init()
        {
            MainBUSUP businessEntity = base.Init();
            businessEntity.Value = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, MainBUSUP businessComponent, MainAppletUP UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.Value))
                result.Add(new ValidationResult(
                    "Value is a required field.",
                    new List<string>() { "Value" }
                    ));
            return result;
        }
    }
}
