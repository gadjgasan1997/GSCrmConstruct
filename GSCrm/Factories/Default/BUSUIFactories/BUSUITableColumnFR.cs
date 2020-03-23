using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Services.Info;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Collections.Generic;
using System.Linq;
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUITableColumnFR : MainBUSUIFR<BUSTableColumn, UITableColumn>
    {
        public override BUSTableColumn UIToBusiness(UITableColumn UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            Table table = context.Tables
                .Include(tc => tc.TableColumns)
                .FirstOrDefault(n => n.Id.ToString() == ComponentContext.GetSelectedRecord("Table"));
            BUSTableColumn businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && table != null && table.TableColumns != null && 
                table.TableColumns.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Table column with this name is already exists in table " + table.Name + ".";
            }
            else
            {
                businessEntity.Table = table;
                businessEntity.TableId = table.Id;
            }
            return businessEntity;
        }
    }
}
