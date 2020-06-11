using GSCrmLibrary.Data;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Factories.MainFactories;
using System.Linq;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSColumnUPFR<TContext> : DataBUSUserPropertyFR<ColumnUP, BUSColumnUP, TContext>
        where TContext : MainContext, new()
    {
        public override BUSColumnUP DataToBusiness(ColumnUP dataEntity, TContext context)
        {
            BUSColumnUP businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.ColumnId = dataEntity.ColumnId;
            businessEntity.Column = context.Columns.FirstOrDefault(i => i.Id == businessEntity.ColumnId);
            return businessEntity;
        }
        public override ColumnUP BusinessToData(ColumnUP columnUP, BUSColumnUP businessEntity, TContext context, bool NewRecord)
        {
            ColumnUP dataEntity = base.BusinessToData(columnUP, businessEntity, context, NewRecord);
            dataEntity.Column = businessEntity.Column;
            dataEntity.ColumnId = businessEntity.ColumnId;
            return dataEntity;
        }
    }
}
