using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSDataMapObjectFR<TContext> : DataBUSFactory<DataMapObject, BUSDataMapObject, TContext>
        where TContext : MainContext, new()
    {
        public override BUSDataMapObject DataToBusiness(DataMapObject dataEntity, TContext context)
        {
            BUSDataMapObject businessEntity = base.DataToBusiness(dataEntity, context);
            BusinessObject sourceBusinessObject = context.BusinessObjects.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.SourceBusinessObjectId);
            if (sourceBusinessObject != null)
            {
                businessEntity.SourceBusinessObject = sourceBusinessObject;
                businessEntity.SourceBusinessObjectId = sourceBusinessObject.Id;
                businessEntity.SourceBusinessObjectName = sourceBusinessObject.Name;
            }
            BusinessObject destinationBusinessObject = context.BusinessObjects.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.DestinationBusinessObjectId);
            if (destinationBusinessObject != null)
            {
                businessEntity.DestinationBusinessObject = destinationBusinessObject;
                businessEntity.DestinationBusinessObjectId = destinationBusinessObject.Id;
                businessEntity.DestinationBusinessObjectName = destinationBusinessObject.Name;
            }
            return businessEntity;
        }
        public override DataMapObject BusinessToData(DataMapObject dataMapObject, BUSDataMapObject businessEntity, TContext context, bool NewRecord)
        {
            DataMapObject dataEntity = base.BusinessToData(dataMapObject, businessEntity, context, NewRecord);
            dataEntity.DataMap = businessEntity.DataMap;
            dataEntity.DataMapId = businessEntity.DataMapId;
            dataEntity.SourceBusinessObject = businessEntity.SourceBusinessObject;
            dataEntity.SourceBusinessObjectId = businessEntity.SourceBusinessObjectId;
            dataEntity.DestinationBusinessObject = businessEntity.DestinationBusinessObject;
            dataEntity.DestinationBusinessObjectId = businessEntity.DestinationBusinessObjectId;
            return dataEntity;
        }
    }
}
