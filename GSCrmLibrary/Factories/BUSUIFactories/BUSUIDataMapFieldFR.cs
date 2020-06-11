using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using System;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIDataMapFieldFR<TContext> : BUSUIFactory<BUSDataMapField, UIDataMapField, TContext>
        where TContext : MainContext, new()
    {
        public override UIDataMapField BusinessToUI(BUSDataMapField businessEntity)
        {
            UIDataMapField UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.SourceFieldName = businessEntity.SourceFieldName;
            UIEntity.Destination = businessEntity.Destination;
            UIEntity.FieldValidation = businessEntity.FieldValidation;
            return UIEntity;
        }
        public override BUSDataMapField UIToBusiness(UIDataMapField UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSDataMapField businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            DataMapObjectComponent dataMapComponent = context.DataMapObjectComponents
                .AsNoTracking()
                .Select(mapComponent => new
                {
                    id = mapComponent.Id,
                    name = mapComponent.Name,
                    sourceBOComponentId = mapComponent.SourceBOComponentId,
                    destinationBOComponentId = mapComponent.DestinationBOComponentId,
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
                    SourceBOComponentId = mapComponent.sourceBOComponentId,
                    DestinationBOComponentId = mapComponent.destinationBOComponentId,
                    DataMapFields = mapComponent.dataMapFields.Select(mapField => new DataMapField
                    {
                        Id = mapField.id,
                        Name = mapField.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Data Map Object Component"));
            if (dataMapComponent == null)
                businessEntity.ErrorMessage = "First you need create data map component.";
            else
            {
                DataMapField dataMapField = dataMapComponent?.DataMapFields.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (dataMapComponent?.SourceBOComponentId == Guid.Empty)
                    businessEntity.ErrorMessage = $"At first you need to add a source business component to data map component {dataMapComponent.Name}.";
                if (dataMapComponent?.DestinationBOComponentId == Guid.Empty)
                    businessEntity.ErrorMessage = $"At first you need to add a destination business component to data map component {dataMapComponent.Name}.";
                if (dataMapField != null && dataMapField.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Data map field with this name is already exists in data map component {dataMapComponent.Name}.";
                else
                {
                    businessEntity.DataMapComponent = dataMapComponent;
                    businessEntity.DataMapComponentId = dataMapComponent.Id;
                    BusinessObjectComponent sourceBOComponent = context.BusinessObjectComponents.FirstOrDefault(i => i.Id == dataMapComponent.SourceBOComponentId);
                    BusinessComponent sourceBusinessComponent = context.BusinessComponents
                        .Include(f => f.Fields)
                        .FirstOrDefault(i => i.Id == sourceBOComponent.BusCompId);
                    businessEntity.SourceBusinessComponentId = sourceBusinessComponent.Id;
                    Field sourceField = sourceBusinessComponent.Fields.FirstOrDefault(n => n.Name == UIEntity.SourceFieldName);
                    businessEntity.SourceField = sourceField;
                    businessEntity.SourceFieldId = sourceField.Id;
                    businessEntity.SourceFieldName = sourceField.Name;
                    BusinessObjectComponent destinationBOComponentId = context.BusinessObjectComponents.FirstOrDefault(i => i.Id == dataMapComponent.DestinationBOComponentId);
                    businessEntity.DestinationBusinessComponentId = destinationBOComponentId.BusCompId;
                    businessEntity.Destination = UIEntity.Destination;
                    businessEntity.FieldValidation = UIEntity.FieldValidation;
                }
            }
            return businessEntity;
        }
        public override BUSDataMapField Init(TContext context)
        {
            BUSDataMapField businessEntity = base.Init(context);
            DataMapObjectComponent dataMapComponent = context.DataMapObjectComponents
                .AsNoTracking()
                .Select(mapComponent => new
                {
                    id = mapComponent.Id,
                    sourceBOComponentId = mapComponent.SourceBOComponentId,
                    destinationBOComponentId = mapComponent.DestinationBOComponentId
                })
                .Select(mapComponent => new DataMapObjectComponent
                {
                    Id = mapComponent.id,
                    SourceBOComponentId = mapComponent.sourceBOComponentId,
                    DestinationBOComponentId = mapComponent.destinationBOComponentId,
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Data Map Object Component"));
            if (dataMapComponent != null)
            {
                businessEntity.DataMapComponent = dataMapComponent;
                businessEntity.DataMapComponentId = dataMapComponent.Id;
                BusinessObjectComponent sourceBOComponent = context.BusinessObjectComponents.FirstOrDefault(i => i.Id == dataMapComponent.SourceBOComponentId);
                businessEntity.SourceBusinessComponentId = sourceBOComponent.BusCompId;
                BusinessObjectComponent destinationBOComponentId = context.BusinessObjectComponents.FirstOrDefault(i => i.Id == dataMapComponent.DestinationBOComponentId);
                businessEntity.DestinationBusinessComponentId = destinationBOComponentId.BusCompId;
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIDataMapField UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.SourceFieldName))
                result.Add(new ValidationResult("Source is a required field.", new List<string>() { "SourceFieldName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSDataMapField businessComponent, UIDataMapField UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (businessComponent.SourceField == null)
                result.Add(new ValidationResult("Source field with this name not found.", new List<string>() { "SourceFieldName" }));
            return result;
        }
    }
}
