using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Factories.Default.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSViewFR : MainDataBUSFR<View, BUSView>
    {
        public override BUSView DataToBusiness(View dataEntity, ApplicationContext context)
        {
            BUSView businessEntity = base.DataToBusiness(dataEntity, context);
            BusObject busObject = context.BusinessObjects.FirstOrDefault(i => i.Id == dataEntity.BusObjectId);
            businessEntity.BusObject = busObject;
            businessEntity.BusObjectId = busObject.Id;
            businessEntity.BusObjectName = busObject.Name;
            return businessEntity;
        }
        public override View BusinessToData(BUSView businessEntity, DbSet<View> entityDBSet, bool NewRecord)
        {
            View dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.BusObjectId = businessEntity.BusObjectId;
            return dataEntity;
        }
    }
}
