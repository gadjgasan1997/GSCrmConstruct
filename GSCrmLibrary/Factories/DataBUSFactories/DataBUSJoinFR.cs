using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using System.Linq;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSJoinFR<TContext> : DataBUSFactory<Join, BUSJoin, TContext>
        where TContext : MainContext, new()
    {
        public override BUSJoin DataToBusiness(Join dataEntity, TContext context)
        {
            BUSJoin businessEntity = base.DataToBusiness(dataEntity, context);

            // BusComp
            businessEntity.BusComp = dataEntity.BusComp;
            businessEntity.BusCompId = dataEntity.BusCompId;

            // Table
            Table table = context.Tables.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.TableId);
            if (table != null)
            {
                businessEntity.Table = table;
                businessEntity.TableId = table.Id;
                businessEntity.TableName = table.Name;
            }
            return businessEntity;
        }
        public override Join BusinessToData(Join join, BUSJoin businessEntity, TContext context, bool NewRecord)
        {
            Join dataEntity = base.BusinessToData(join, businessEntity, context, NewRecord);

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
