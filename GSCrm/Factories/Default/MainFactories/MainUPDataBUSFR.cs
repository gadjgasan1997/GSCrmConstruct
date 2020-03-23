using GSCrm.Data;
using Microsoft.EntityFrameworkCore;

namespace GSCrm.Factories.Default.MainFactories
{
    public class MainUPDataBUSFR<MainTableUP, MainBUSUP>
        : MainDataBUSFR<MainTableUP, MainBUSUP>
        where MainTableUP : Models.Default.MainEntities.MainTableUP, new()
        where MainBUSUP : Models.Default.MainEntities.MainBUSUP, new()
    {
        public override MainBUSUP DataToBusiness(MainTableUP dataEntity, ApplicationContext context)
        {
            MainBUSUP businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Value = dataEntity.Value;
            return businessEntity;
        }
        public override MainTableUP BusinessToData(MainBUSUP businessEntity, DbSet<MainTableUP> tableEntities, bool NewRecord)
        {
            MainTableUP dataEntity = base.BusinessToData(businessEntity, tableEntities, NewRecord);
            dataEntity.Value = businessEntity.Value;
            return dataEntity;
        }
    }
}
