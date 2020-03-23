using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Services.Info;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIIconFR : MainBUSUIFR<BUSIcon, UIIcon>
    {
        public override UIIcon BusinessToUI(BUSIcon businessEntity)
        {
            UIIcon UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ImgPath = businessEntity.ImgPath;
            UIEntity.CssClass = businessEntity.CssClass;
            return UIEntity;
        }
        public override BUSIcon UIToBusiness(UIIcon UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSIcon businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && context.Icons.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Icon with this name is already exists.";
            }
            else
            {
                businessEntity.ImgPath = UIEntity.ImgPath;
                businessEntity.CssClass = UIEntity.CssClass;
            }
            return businessEntity;
        }
    }
}
