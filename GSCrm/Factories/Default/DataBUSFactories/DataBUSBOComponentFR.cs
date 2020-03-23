using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSBOComponentFR : MainDataBUSFR<BOComponent, BUSBOComponent>
    {
        public override BUSBOComponent DataToBusiness(BOComponent dataEntity, ApplicationContext context)
        {
            BUSBOComponent businessEntity = base.DataToBusiness(dataEntity, context);

            // BusObject
            BusObject busObject = context.BusinessObjects.FirstOrDefault(i => i.Id == dataEntity.BusObjectId);
            businessEntity.BusObject = busObject;
            businessEntity.BusObjectId = busObject.Id;
            businessEntity.BusObjectName = busObject.Name;

            // BusComp
            BusComp busComp = context.BusinessComponents.FirstOrDefault(i => i.Id == dataEntity.BusCompId);
            businessEntity.BusCompId = busComp.Id;
            businessEntity.BusCompName = busComp.Name;

            // Link
            Link link = context.Links.FirstOrDefault(i => i.Id == dataEntity.LinkId);
            if (link != null)
            {
                businessEntity.Link = link;
                businessEntity.LinkId = link.Id;
                businessEntity.LinkName = link.Name;
            }
            return businessEntity;
        }
        public override BOComponent BusinessToData(BUSBOComponent businessEntity, DbSet<BOComponent> entityDBSet, bool NewRecord)
        {
            BOComponent dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.BusObject = businessEntity.BusObject;
            dataEntity.BusObjectId = businessEntity.BusObjectId;
            dataEntity.BusComp = businessEntity.BusComp;
            dataEntity.BusCompId = businessEntity.BusCompId;
            dataEntity.Link = businessEntity.Link;
            dataEntity.LinkId = businessEntity.LinkId;
            return dataEntity;
        }
    }
}
