using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Services.Info;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIActionUPFR : MainUPBUSUIFR<BUSActionUP, UIActionUP>
    {
        public override BUSActionUP UIToBusiness(UIActionUP UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            Action action = context.Actions
                .Include(up => up.ActionUPs)
                .FirstOrDefault(i => i.Id.ToString() == ComponentContext.GetSelectedRecord("Action"));
            BUSActionUP businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && action != null && action.ActionUPs != null && 
                action.ActionUPs.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Record with this name is already exists.";
            }

            else
            {
                businessEntity.Action = action;
                businessEntity.ActionId = action.Id;
            }
            return businessEntity;
        }
    }
}
