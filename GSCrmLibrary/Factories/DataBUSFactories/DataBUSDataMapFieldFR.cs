using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSDataMapFieldFR<TContext> : DataBUSFactory<DataMapField, BUSDataMapField, TContext>
        where TContext : MainContext, new()
    {
        public override BUSDataMapField DataToBusiness(DataMapField dataEntity, TContext context)
        {
            BUSDataMapField businessEntity = base.DataToBusiness(dataEntity, context); ;
            DataMapObjectComponent dataMapComponent = context.DataMapObjectComponents
                .AsNoTracking()
                .Select(mapComponent => new
                {
                    id = mapComponent.Id,
                    name = mapComponent.Name,
                    sourceBusinessComponentId = mapComponent.SourceBOComponentId,
                    destinationBusinessComponentId = mapComponent.DestinationBOComponentId,
                    dataMapFields = mapComponent.DataMapFields.Select(mapField => new
                    {
                        id = mapField.Id,
                        name = mapField.Name
                    })
                })
                .Select(mapComponent => new DataMapObjectComponent
                {
                    Id = mapComponent.id,
                    Name = mapComponent.name,
                    SourceBOComponentId = mapComponent.sourceBusinessComponentId,
                    DestinationBOComponentId = mapComponent.destinationBusinessComponentId,
                    DataMapFields = mapComponent.dataMapFields.Select(mapField => new DataMapField
                    {
                        Id = mapField.id,
                        Name = mapField.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == dataEntity.DataMapComponentId);

            businessEntity.DataMapComponent = dataMapComponent;
            businessEntity.DataMapComponentId = dataMapComponent.Id;
            BusinessObjectComponent sourceBOComponent = context.BusinessObjectComponents.FirstOrDefault(i => i.Id == dataMapComponent.SourceBOComponentId);
            businessEntity.SourceBusinessComponentId = sourceBOComponent.BusCompId;
            BusinessObjectComponent destinationBOComponentId = context.BusinessObjectComponents.FirstOrDefault(i => i.Id == dataMapComponent.DestinationBOComponentId);
            businessEntity.DestinationBusinessComponentId = destinationBOComponentId.BusCompId;
            Field sourceField = context.Fields.FirstOrDefault(i => i.Id == dataEntity.SourceFieldId);
            if (sourceField != null)
            {
                businessEntity.SourceField = sourceField;
                businessEntity.SourceFieldId = sourceField.Id;
                businessEntity.SourceFieldName = sourceField.Name;
            }
            businessEntity.Destination = dataEntity.Destination;
            businessEntity.FieldValidation = dataEntity.FieldValidation;
            return businessEntity;
        }
        public override DataMapField BusinessToData(DataMapField dataMapField, BUSDataMapField businessEntity, TContext context, bool NewRecord)
        {
            DataMapField dataEntity = base.BusinessToData(dataMapField, businessEntity, context, NewRecord);
            dataEntity.DataMapObjectComponent = businessEntity.DataMapComponent;
            dataEntity.DataMapComponentId = businessEntity.DataMapComponentId;
            dataEntity.SourceField = businessEntity.SourceField;
            dataEntity.SourceFieldId = businessEntity.SourceFieldId;
            dataEntity.Destination = businessEntity.Destination;
            dataEntity.FieldValidation = businessEntity.FieldValidation;
            return dataEntity;
        }
    }
}
