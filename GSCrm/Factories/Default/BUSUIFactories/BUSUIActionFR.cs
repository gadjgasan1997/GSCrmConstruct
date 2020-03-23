using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Services.Info;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIActionFR : MainBUSUIFR<BUSAction, UIAction>
    {
        public override BUSAction UIToBusiness(UIAction UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSAction businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && context.Actions.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Record with this name is already exists.";
            }
            return businessEntity;
        }
    }
}
