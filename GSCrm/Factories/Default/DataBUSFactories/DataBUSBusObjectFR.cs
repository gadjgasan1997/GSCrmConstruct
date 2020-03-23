using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Factories.Default.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSBusObjectFR : MainDataBUSFR<BusObject, BUSBusObject>
    {
        public override BUSBusObject DataToBusiness(BusObject dataEntity, ApplicationContext context)
        {
            BUSBusObject businessEntity = base.DataToBusiness(dataEntity, context);
            BusComp primaryBusComp = context.BusinessComponents.FirstOrDefault(i => i.Id == dataEntity.PrimaryBusCompId);
            if (primaryBusComp != null)
            {
                businessEntity.PrimaryBusComp = primaryBusComp;
                businessEntity.PrimaryBusCompId = primaryBusComp.Id;
                businessEntity.PrimaryBusCompName = primaryBusComp.Name;
            }
            return businessEntity;
        }
        public override BusObject BusinessToData(BUSBusObject businessEntity, DbSet<BusObject> entityDBSet, bool NewRecord)
        {
            BusObject dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.PrimaryBusCompId = businessEntity.PrimaryBusCompId;
            return dataEntity;
        }
    }
}
