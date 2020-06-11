using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Configuration;
using Applet = GSCrmLibrary.Models.TableModels.Applet;
using BusinessComponent = GSCrmLibrary.Models.TableModels.BusinessComponent;
using static GSCrmLibrary.CodeGeneration.CodGenUtils;
using static GSCrmLibrary.Utils;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSAppletFR<TContext> : DataBUSFactory<Applet, BUSApplet, TContext>
        where TContext : MainContext, new()
    {
        public override BUSApplet DataToBusiness(Applet dataEntity, TContext context)
        {
            BUSApplet businessEntity = base.DataToBusiness(dataEntity, context);
            PhysicalRender physicalRender = context.PhysicalRenders.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.PhysicalRenderId);
            if (physicalRender != null)
            {
                businessEntity.PhysicalRender = physicalRender;
                businessEntity.PhysicalRenderId = physicalRender.Id;
                businessEntity.PhysicalRenderName = physicalRender.Name;
            }
            BusinessComponent busComp = context.BusinessComponents.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.BusCompId);
            if (busComp != null)
            {
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp.Id;
                businessEntity.BusCompName = busComp.Name;
            }

            businessEntity.Virtual = dataEntity.Virtual;
            businessEntity.Header = dataEntity.Header;
            businessEntity.Type = dataEntity.Type;
            businessEntity.EmptyState = dataEntity.EmptyState;
            businessEntity.DisplayLines = dataEntity.DisplayLines;
            businessEntity.Initflag = dataEntity.Initflag;
            return businessEntity;
        }
        public override Applet BusinessToData(Applet applet, BUSApplet businessEntity, TContext context, bool NewRecord)
        {
            Applet dataEntity = base.BusinessToData(applet, businessEntity, context, NewRecord);
            dataEntity.Header = businessEntity.Header;
            dataEntity.Type = businessEntity.Type;
            dataEntity.Initflag = businessEntity.Initflag;
            dataEntity.EmptyState = businessEntity.EmptyState;
            dataEntity.PhysicalRenderId = businessEntity.PhysicalRenderId;
            dataEntity.DisplayLines = businessEntity.DisplayLines;
            dataEntity.BusCompId = businessEntity.BusCompId;
            dataEntity.Routing = "/api/" + GetPermissibleName(businessEntity.Name) + "/";
            dataEntity.Virtual = businessEntity.Virtual;
            return dataEntity;
        }
        public override void OnRecordDelete(Applet recordToDelete, DbSet<Applet> entities, TContext context)
        {
            string permissibleName = GetPermissibleName(recordToDelete.Name);
            if (EntityFileExists(permissibleName, "Applet"))
            {
                DeleteEntityFile(permissibleName, "Applet");
                DeleteEntityFile(EntitiesConfig.UIFR + permissibleName + "FR", "BUSUIFactory");
                DeleteEntityFile(permissibleName + "Controller", "AppletController");
            }
            base.OnRecordDelete(recordToDelete, entities, context);
        }
    }
}
