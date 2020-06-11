using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSDataMapObjectComponentFR<TContext> : DataBUSFactory<DataMapObjectComponent, BUSDataMapObjectComponent, TContext>
        where TContext : MainContext, new()
    {
        public override BUSDataMapObjectComponent DataToBusiness(DataMapObjectComponent dataEntity, TContext context)
        {
            BUSDataMapObjectComponent businessEntity = base.DataToBusiness(dataEntity, context);
            DataMapObject dataMapObject = context.DataMapObjects
                .AsNoTracking()
                .Select(mapObject => new
                {
                   id = mapObject.Id,
                   name = mapObject.Name,
                   sourceBusinessObjectId = mapObject.SourceBusinessObjectId,
                   destinationBusinessObjectId = mapObject.DestinationBusinessObjectId,
                   dataMapComponents = mapObject.DataMapObjectComponents.Select(mapComponent => new
                   {
                       id = mapComponent.Id,
                       name = mapComponent.Name
                   })
                })
                .Select(mapObject => new DataMapObject
                {
                    Id = mapObject.id,
                    Name = mapObject.name,
                    SourceBusinessObjectId = mapObject.sourceBusinessObjectId,
                    DestinationBusinessObjectId = mapObject.destinationBusinessObjectId,
                    DataMapObjectComponents = mapObject.dataMapComponents.Select(mapComponent => new DataMapObjectComponent
                    {
                        Id = mapComponent.id,
                        Name = mapComponent.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == dataEntity.DataMapObjectId);

            businessEntity.DataMapObject = dataMapObject;
            businessEntity.DataMapObjectId = dataMapObject.Id;

            // SourceBusinessObject
            BusinessObject sourceBusinessObject = context.BusinessObjects
                .Select(bo => new
                {
                    id = bo.Id,
                    busObjectComponents = bo.BusObjectComponents.Select(boc => new
                    {
                        id = boc.Id,
                        busCompId = boc.BusCompId,
                        name = boc.Name
                    })
                })
                .Select(bo => new BusinessObject
                {
                    Id = bo.id,
                    BusObjectComponents = bo.busObjectComponents.Select(boc => new BusinessObjectComponent
                    {
                        Id = boc.id,
                        BusCompId = boc.busCompId,
                        Name = boc.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == dataMapObject.SourceBusinessObjectId);
            if (sourceBusinessObject != null)
            {
                businessEntity.SourceBusinessObject = sourceBusinessObject;
                businessEntity.SourceBusinessObjectId = sourceBusinessObject.Id;
            }

            // SourceBusinessComponent
            BusinessObjectComponent sourceBOComponent = sourceBusinessObject.BusObjectComponents.FirstOrDefault(i => i.Id == dataEntity.SourceBOComponentId);
            if (sourceBOComponent != null)
            {
                businessEntity.SourceBOComponent = sourceBOComponent;
                businessEntity.SourceBOComponentId = sourceBOComponent.Id;
                businessEntity.SourceBOComponentName = sourceBOComponent.Name;
                BusinessComponent sourceBusinessComponent = context.BusinessComponents.FirstOrDefault(i => i.Id == sourceBOComponent.BusCompId);
                businessEntity.SourceBusinessComponent = sourceBusinessComponent;
                businessEntity.SourceBusinessComponentId = sourceBusinessComponent.Id;
            }

            // DestinationBusinessObject
            BusinessObject destinationBusinessObject = context.BusinessObjects
                .Select(bo => new
                {
                    id = bo.Id,
                    busObjectComponents = bo.BusObjectComponents.Select(boc => new
                    {
                        id = boc.Id,
                        busCompId = boc.BusCompId,
                        name = boc.Name
                    })
                })
                .Select(bo => new BusinessObject
                {
                    Id = bo.id,
                    BusObjectComponents = bo.busObjectComponents.Select(boc => new BusinessObjectComponent
                    {
                        Id = boc.id,
                        BusCompId = boc.busCompId,
                        Name = boc.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == dataMapObject.DestinationBusinessObjectId);
            if (destinationBusinessObject != null)
            {
                businessEntity.DestinationBusinessObject = destinationBusinessObject;
                businessEntity.DestinationBusinessObjectId = destinationBusinessObject.Id;
            }

            // DestinationBusinessComponent
            BusinessObjectComponent destinationBOComponent = destinationBusinessObject.BusObjectComponents.FirstOrDefault(i => i.Id == dataEntity.DestinationBOComponentId);
            if (destinationBOComponent != null)
            {
                businessEntity.DestinationBOComponent = destinationBOComponent;
                businessEntity.DestinationBOComponentId = destinationBOComponent.Id;
                businessEntity.DestinationBOComponentName = destinationBOComponent.Name;
                BusinessComponent destinationBusinessComponent = context.BusinessComponents.FirstOrDefault(i => i.Id == destinationBOComponent.BusCompId);
                businessEntity.DestinationBusinessComponent = destinationBusinessComponent;
                businessEntity.DestinationBusinessComponentId = destinationBusinessComponent.Id;
            }

            // ParentDataMapComponent
            DataMapObjectComponent parentDataMapComponent = dataMapObject.DataMapObjectComponents.FirstOrDefault(i => i.Id == dataEntity.ParentDataMapComponentId);
            if (parentDataMapComponent != null)
            {
                businessEntity.ParentDataMapComponent = parentDataMapComponent;
                businessEntity.ParentDataMapComponentId = parentDataMapComponent.Id;
                businessEntity.ParentDataMapComponentName = parentDataMapComponent.Name;
            }

            businessEntity.SourceSearchSpecification = dataEntity.SourceSearchSpecification;
            return businessEntity;
        }
        public override DataMapObjectComponent BusinessToData(DataMapObjectComponent dataMapComponent, BUSDataMapObjectComponent businessEntity, TContext context, bool NewRecord)
        {
            DataMapObjectComponent dataEntity = base.BusinessToData(dataMapComponent, businessEntity, context, NewRecord);
            dataEntity.DataMapObject = businessEntity.DataMapObject;
            dataEntity.DataMapObjectId = businessEntity.DataMapObjectId;
            dataEntity.SourceBOComponent = businessEntity.SourceBOComponent;
            dataEntity.SourceBOComponentId = businessEntity.SourceBOComponentId;
            dataEntity.DestinationBOComponent = businessEntity.DestinationBOComponent;
            dataEntity.DestinationBOComponentId = businessEntity.DestinationBOComponentId;
            dataEntity.SourceSearchSpecification = businessEntity.SourceSearchSpecification;
            dataEntity.ParentDataMapComponentId = businessEntity.ParentDataMapComponentId;
            return dataEntity;
        }
    }
}
