using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Data;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIDataMapObjectFR<TContext> : BUSUIFactory<BUSDataMapObject, UIDataMapObject, TContext>
        where TContext : MainContext, new()
    {
        public override UIDataMapObject BusinessToUI(BUSDataMapObject businessEntity)
        {
            UIDataMapObject UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.SourceBusinessObjectName = businessEntity.SourceBusinessObjectName;
            UIEntity.DestinationBusinessObjectName = businessEntity.DestinationBusinessObjectName;
            return UIEntity;
        }
        public override BUSDataMapObject UIToBusiness(UIDataMapObject UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSDataMapObject businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            DataMap dataMap = context.DataMaps
                .Select(map => new
                {
                    id = map.Id,
                    name = map.Name,
                    dataMapObjects = map.DataMapObjects.Select(mapObject => new
                    {
                        id = mapObject.Id,
                        name = mapObject.Name
                    })
                })
                .Select(map => new DataMap
                {
                    Id = map.id,
                    Name = map.name,
                    DataMapObjects = map.dataMapObjects.Select(mapObject => new DataMapObject
                    {
                        Id = mapObject.id,
                        Name = mapObject.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Data Map"));
            if (dataMap == null)
                businessEntity.ErrorMessage = "First you need create data map.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                DataMapObject dataMapObject = dataMap.DataMapObjects?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (dataMapObject != null && dataMapObject.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Data map object with this name is already exists in data map {businessEntity.DataMap.Name}.";
                else
                {
                    businessEntity.DataMap = dataMap;
                    businessEntity.DataMapId = dataMap.Id;

                    // SourceBusinessObject
                    BusinessObject sourceBusinessObject = context.BusinessObjects.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.SourceBusinessObjectName);
                    if (sourceBusinessObject != null)
                    {
                        businessEntity.SourceBusinessObject = sourceBusinessObject;
                        businessEntity.SourceBusinessObjectId = sourceBusinessObject.Id;
                        businessEntity.SourceBusinessObjectName = sourceBusinessObject.Name;
                    }

                    // DestinationBusinessObject
                    BusinessObject destinationBusinessObject = context.BusinessObjects.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.DestinationBusinessObjectName);
                    if (destinationBusinessObject != null)
                    {
                        businessEntity.DestinationBusinessObject = destinationBusinessObject;
                        businessEntity.DestinationBusinessObjectId = destinationBusinessObject.Id;
                        businessEntity.DestinationBusinessObjectName = destinationBusinessObject.Name;
                    }
                }
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIDataMapObject UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.SourceBusinessObjectName))
                result.Add(new ValidationResult("Source business object is a required field.", new List<string>() { "SourceBusinessObjectName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.DestinationBusinessObjectName))
                result.Add(new ValidationResult("Destination business object is a required field.", new List<string>() { "DestinationBusinessObjectName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSDataMapObject businessComponent, UIDataMapObject UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (businessComponent.SourceBusinessObject == null)
                result.Add(new ValidationResult("Business object with this name not found.", new List<string>() { "SourceBusinessObject" }));
            if (businessComponent.DestinationBusinessObject == null)
                result.Add(new ValidationResult("Business object with this name not found.", new List<string>() { "DestinationBusinessObject" }));
            return result;
        }
    }
}
