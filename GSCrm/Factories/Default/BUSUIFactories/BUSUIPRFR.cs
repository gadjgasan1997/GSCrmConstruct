using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Services.Info;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIPRFR : MainBUSUIFR<BUSPR, UIPR>
    {
        public override BUSPR UIToBusiness(UIPR UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSPR businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && context.PhysicalRenders.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Physical render with this name is already exists.";
            }
            return businessEntity;
        }
    }
}
