using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSAppletFR : MainDataBUSFR<Applet, BUSApplet>
    {
        public override BUSApplet DataToBusiness(Applet dataEntity, ApplicationContext context)
        {
            BUSApplet businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Header = dataEntity.Header;
            businessEntity.EmptyState = dataEntity.EmptyState;
            businessEntity.DisplayLines = dataEntity.DisplayLines;
            PR physicalRender = context.PhysicalRenders.FirstOrDefault(i => i.Id == dataEntity.PhysicalRenderId);
            if (physicalRender != null)
            {
                businessEntity.PhysicalRender = physicalRender;
                businessEntity.PhysicalRenderId = physicalRender.Id;
                businessEntity.PhysicalRenderName = physicalRender.Name;
            }
            BusComp busComp = context.BusinessComponents.FirstOrDefault(i => i.Id == dataEntity.BusCompId);
            if (busComp != null)
            {
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp.Id;
                businessEntity.BusCompName = busComp.Name;
            }
            return businessEntity;
        }
        public override Applet BusinessToData(BUSApplet businessEntity, DbSet<Applet> tableEntities, bool NewRecord)
        {
            Applet dataEntity = base.BusinessToData(businessEntity, tableEntities, NewRecord);
            dataEntity.Header = businessEntity.Header;
            dataEntity.EmptyState = businessEntity.EmptyState;
            dataEntity.PhysicalRenderId = businessEntity.PhysicalRenderId;
            dataEntity.DisplayLines = businessEntity.DisplayLines;
            dataEntity.BusCompId = businessEntity.BusCompId;
            return dataEntity;
        }
    }
}
