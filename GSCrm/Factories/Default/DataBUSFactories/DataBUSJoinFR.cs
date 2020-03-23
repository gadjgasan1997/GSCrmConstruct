using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSJoinFR : MainDataBUSFR<Join, BUSJoin>
    {
        public override BUSJoin DataToBusiness(Join dataEntity, ApplicationContext context)
        {
            BUSJoin businessEntity = base.DataToBusiness(dataEntity, context);

            // BusComp
            businessEntity.BusComp = dataEntity.BusComp;
            businessEntity.BusCompId = dataEntity.BusCompId;

            // Table
            Table table = context.Tables.FirstOrDefault(i => i.Id == dataEntity.TableId);
            businessEntity.Table = table;
            businessEntity.TableId = table.Id;
            businessEntity.TableName = table.Name;
            return businessEntity;
        }
        public override Join BusinessToData(BUSJoin businessEntity, DbSet<Join> entityDBSet, bool NewRecord)
        {
            Join dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);

            // BusComp
            dataEntity.BusComp = businessEntity.BusComp;
            dataEntity.BusCompId = businessEntity.BusCompId;

            // Table
            dataEntity.Table = businessEntity.Table;
            dataEntity.TableId = businessEntity.TableId;
            return dataEntity;
        }
    }
}
