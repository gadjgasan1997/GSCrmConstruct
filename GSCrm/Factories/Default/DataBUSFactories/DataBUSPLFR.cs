using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Factories.Default.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSPLFR : MainDataBUSFR<PL, BUSPL>
    {
        public override BUSPL DataToBusiness(PL dataEntity, ApplicationContext context)
        {
            BUSPL businessEntity = base.DataToBusiness(dataEntity, context);
            BusComp busComp = context.BusinessComponents.FirstOrDefault(i => i.Id == dataEntity.BusCompId);
            if (busComp != null)
            {
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp.Id;
                businessEntity.BusCompName = busComp.Name;
            }
            return businessEntity;
        }
        public override PL BusinessToData(BUSPL businessEntity, DbSet<PL> entityDBSet, bool NewRecord)
        {
            PL dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.BusCompId = businessEntity.BusCompId;
            return dataEntity;
        }
    }
}
