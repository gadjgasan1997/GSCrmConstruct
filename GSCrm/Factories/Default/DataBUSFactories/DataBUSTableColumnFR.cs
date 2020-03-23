using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSTableColumnFR : MainDataBUSFR<TableColumn, BUSTableColumn>
    {
        public override BUSTableColumn DataToBusiness(TableColumn dataEntity, ApplicationContext context)
        {
            BUSTableColumn businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Table = context.Tables.FirstOrDefault(i => i.Id == dataEntity.TableId);
            businessEntity.TableId = dataEntity.TableId;
            return businessEntity;
        }
        public override TableColumn BusinessToData(BUSTableColumn businessEntity, DbSet<TableColumn> entityDBSet, bool NewRecord)
        {
            TableColumn dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Table = businessEntity.Table;
            dataEntity.TableId = businessEntity.TableId;
            return dataEntity;
        }
    }
}
