using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Factories.Default.MainFactories;
using GSCrm.Data;
using GSCrm.Services.Info;
using System.Linq;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUITableFR : MainBUSUIFR<BUSTable, UITable>
    {
        public override BUSTable UIToBusiness(UITable UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSTable businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && context.Tables.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Table with this name is already exists.";
            }
            return businessEntity;
        }
    }
}