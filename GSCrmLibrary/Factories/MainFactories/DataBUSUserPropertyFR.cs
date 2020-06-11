using GSCrmLibrary.Data;

namespace GSCrmLibrary.Factories.MainFactories
{
    public class DataBUSUserPropertyFR<TTableUserProperty, TBUSUserProperty, TApplicationDBContext> 
        : DataBUSFactory<TTableUserProperty, TBUSUserProperty, TApplicationDBContext>
        where TTableUserProperty : Models.MainEntities.DataEntityUp, new()
        where TBUSUserProperty : Models.MainEntities.BUSEntityUP, new()
        where TApplicationDBContext : MainContext, new()
    {
        public override TBUSUserProperty DataToBusiness(TTableUserProperty dataEntity, TApplicationDBContext context)
        {
            TBUSUserProperty businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Value = dataEntity.Value;
            return businessEntity;
        }
        public override TTableUserProperty BusinessToData(TTableUserProperty userProperty, TBUSUserProperty businessEntity, TApplicationDBContext context, bool NewRecord)
        {
            TTableUserProperty dataEntity = base.BusinessToData(userProperty, businessEntity, context, NewRecord);
            dataEntity.Value = businessEntity.Value;
            return dataEntity;
        }
    }
}
