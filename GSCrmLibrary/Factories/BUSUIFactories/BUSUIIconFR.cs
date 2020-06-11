using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIIconFR<TContext> : BUSUIFactory<BUSIcon, UIIcon, TContext>
        where TContext : MainContext, new()
    {
        public override UIIcon BusinessToUI(BUSIcon businessEntity)
        {
            UIIcon UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ImgPath = businessEntity.ImgPath;
            UIEntity.CssClass = businessEntity.CssClass;
            return UIEntity;
        }
        public override BUSIcon UIToBusiness(UIIcon UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSIcon businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            businessEntity.ImgPath = UIEntity.ImgPath;
            businessEntity.CssClass = UIEntity.CssClass;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIIcon UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            Icon icon = context.Icons.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (icon != null && icon.Id != UIEntity.Id)
                result.Add(new ValidationResult("Icon with this name is already exists.", new List<string>() { "Name" }));
            return result;
        }
    }
}
