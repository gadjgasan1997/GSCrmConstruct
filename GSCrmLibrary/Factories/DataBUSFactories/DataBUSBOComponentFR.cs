using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSBOComponentFR<TContext> : DataBUSFactory<BusinessObjectComponent, BUSBusinessObjectComponent, TContext>
        where TContext : MainContext, new()
    {
        public override BUSBusinessObjectComponent DataToBusiness(BusinessObjectComponent dataEntity, TContext context)
        {
            BUSBusinessObjectComponent businessEntity = base.DataToBusiness(dataEntity, context);

            // BusObject
            BusinessObject busObject = context.BusinessObjects.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.BusObjectId);
            businessEntity.BusObject = busObject;
            businessEntity.BusObjectId = busObject.Id;
            businessEntity.BusObjectName = busObject.Name;

            // BusComp
            BusinessComponent busComp = context.BusinessComponents.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.BusCompId);
            if (busComp != null)
            {
                businessEntity.BusCompId = busComp.Id;
                businessEntity.BusCompName = busComp.Name;
            }

            // Link
            Link link = context.Links.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.LinkId);
            if (link != null)
            {
                businessEntity.Link = link;
                businessEntity.LinkId = link.Id;
                businessEntity.LinkName = link.Name;
            }
            return businessEntity;
        }
        public override BusinessObjectComponent BusinessToData(BusinessObjectComponent boComponent, BUSBusinessObjectComponent businessEntity, TContext context, bool NewRecord)
        {
            BusinessObjectComponent dataEntity = base.BusinessToData(boComponent, businessEntity, context, NewRecord);
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
