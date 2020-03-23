using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Services.Info;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIColumnUPFR : MainUPBUSUIFR<BUSColumnUP, UIColumnUP>
    {
        public override BUSColumnUP UIToBusiness(UIColumnUP UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            Column column = context.Columns
                .Include(cntrUp => cntrUp.ColumnUPs)
                .FirstOrDefault(i => i.Id.ToString() == ComponentContext.GetSelectedRecord("Column"));
            BUSColumnUP businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && column != null && column.ColumnUPs != null &&
                column.ColumnUPs.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Column user property with this name is already exists in column " + column.Name + ".";
            }
            else
            {
                businessEntity.Column = column;
                businessEntity.ColumnId = column.Id;
            }
            return businessEntity;
        }
    }
}
