using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIScreenFR<TContext> : BUSUIFactory<BUSScreen, UIScreen, TContext>
        where TContext : MainContext, new()
    {
        public override BUSScreen UIToBusiness(UIScreen UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSScreen businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            businessEntity.Header = UIEntity.Header;
            return businessEntity;
        }
        public override UIScreen BusinessToUI(BUSScreen businessEntity)
        {
            UIScreen UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.Header = businessEntity.Header;
            return UIEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIScreen UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            Screen screen = context.Screens.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (screen != null && screen.Id != UIEntity.Id)
                result.Add(new ValidationResult("Screen with this name is already exists.", new List<string>() { "Name" }));
            return result;
        }
    }
}
