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
    public class BUSUIDataMapObjectComponentFR<TContext> : BUSUIFactory<BUSDataMapObjectComponent, UIDataMapObjectComponent, TContext>
        where TContext : MainContext, new()
    {
        public override UIDataMapObjectComponent BusinessToUI(BUSDataMapObjectComponent businessEntity)
        {
            UIDataMapObjectComponent UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.SourceBOComponentName = businessEntity.SourceBOComponentName;
            UIEntity.DestinationBOComponentName = businessEntity.DestinationBOComponentName;
            UIEntity.ParentDataMapComponentName = businessEntity.ParentDataMapComponentName;
            UIEntity.SourceSearchSpecification = businessEntity.SourceSearchSpecification;
            return UIEntity;
        }
        public override BUSDataMapObjectComponent UIToBusiness(UIDataMapObjectComponent UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSDataMapObjectComponent businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
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
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Data Map Object"));
            if (dataMapObject == null)
                businessEntity.ErrorMessage = "First you need create data map object.";
            else
            {
                DataMapObjectComponent mapObjectComponent = dataMapObject?.DataMapObjectComponents.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (dataMapObject?.SourceBusinessObjectId == Guid.Empty)
                    businessEntity.ErrorMessage = $"At first you need to add a source business object to data map object {dataMapObject.Name}.";
                if (dataMapObject?.DestinationBusinessObjectId == Guid.Empty)
                    businessEntity.ErrorMessage = $"At first you need to add a destination business object to data map object {dataMapObject.Name}.";
                else if (mapObjectComponent != null && mapObjectComponent.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Data map component with this name is already exists in data map object {dataMapObject.Name}.";
                else
                {
                    businessEntity.DataMapObject = dataMapObject;
                    businessEntity.DataMapObjectId = dataMapObject.Id;

                    // SourceBusinessObject
                    BusinessObject sourceBusinessObject = context.BusinessObjects
                        .AsNoTracking()
                        .Select(bo => new
                        {
                            id = bo.Id,
                            busObjectComponents = bo.BusObjectComponents.Select(boc => new
                            {
                                id = boc.Id,
                                name = boc.Name,
                                busComp = new
                                {
                                    id = boc.BusComp.Id,
                                    name = boc.BusComp.Name
                                }
                            })
                        })
                        .Select(bo => new BusinessObject
                        {
                            Id = bo.id,
                            BusObjectComponents = bo.busObjectComponents.Select(boc => new BusinessObjectComponent
                            {
                                Id = boc.id,
                                Name = boc.name,
                                BusComp = new BusinessComponent
                                {
                                    Id = boc.busComp.id,
                                    Name = boc.busComp.name
                                }
                            }).ToList()
                        })
                        .FirstOrDefault(i => i.Id == dataMapObject.SourceBusinessObjectId);
                    if (sourceBusinessObject != null)
                    {
                        businessEntity.SourceBusinessObject = sourceBusinessObject;
                        businessEntity.SourceBusinessObjectId = sourceBusinessObject.Id;

                        // SourceBusinessComponent
                        BusinessObjectComponent sourceBOComponent = sourceBusinessObject.BusObjectComponents.FirstOrDefault(n => n.BusComp.Name == UIEntity.SourceBOComponentName);
                        if (sourceBOComponent != null)
                        {
                            businessEntity.SourceBOComponent = sourceBOComponent;
                            businessEntity.SourceBOComponentId = sourceBOComponent.Id;
                            businessEntity.SourceBOComponentName = sourceBOComponent.Name;
                            BusinessComponent sourceBusinessComponent = context.BusinessComponents.FirstOrDefault(i => i.Id == sourceBOComponent.BusComp.Id);
                            businessEntity.SourceBusinessComponent = sourceBusinessComponent;
                            businessEntity.SourceBusinessComponentId = sourceBusinessComponent.Id;
                        }
                    }

                    // DestinationBusinessObject
                    BusinessObject destinationBusinessObject = context.BusinessObjects
                        .AsNoTracking()
                        .Select(bo => new
                        {
                            id = bo.Id,
                            busObjectComponents = bo.BusObjectComponents.Select(boc => new
                            {
                                id = boc.Id,
                                name = boc.Name,
                                busComp = new
                                {
                                    id = boc.BusComp.Id,
                                    name = boc.BusComp.Name
                                }
                            })
                        })
                        .Select(bo => new BusinessObject
                        {
                            Id = bo.id,
                            BusObjectComponents = bo.busObjectComponents.Select(boc => new BusinessObjectComponent
                            {
                                Id = boc.id,
                                Name = boc.name,
                                BusComp = new BusinessComponent
                                {
                                    Id = boc.busComp.id,
                                    Name = boc.busComp.name
                                }
                            }).ToList()
                        })
                        .FirstOrDefault(i => i.Id == dataMapObject.DestinationBusinessObjectId);
                    if (destinationBusinessObject != null)
                    {
                        businessEntity.DestinationBusinessObject = destinationBusinessObject;
                        businessEntity.DestinationBusinessObjectId = destinationBusinessObject.Id;

                        // DestinationBusinessComponent
                        BusinessObjectComponent destinationBOComponent = destinationBusinessObject.BusObjectComponents.FirstOrDefault(n => n.BusComp.Name == UIEntity.DestinationBOComponentName);
                        if (destinationBOComponent != null)
                        {
                            businessEntity.DestinationBOComponent = destinationBOComponent;
                            businessEntity.DestinationBOComponentId = destinationBOComponent.Id;
                            businessEntity.DestinationBOComponentName = destinationBOComponent.Name;
                            BusinessComponent destinationBusinessComponent = context.BusinessComponents.FirstOrDefault(i => i.Id == destinationBOComponent.BusComp.Id);
                            businessEntity.DestinationBusinessComponent = destinationBusinessComponent;
                            businessEntity.DestinationBusinessComponentId = destinationBusinessComponent.Id;
                        }
                    }

                    // ParentDataMapComponent
                    DataMapObjectComponent parentDataMapComponent = dataMapObject.DataMapObjectComponents.FirstOrDefault(n => n.Name == UIEntity.ParentDataMapComponentName);
                    if (parentDataMapComponent != null)
                    {
                        businessEntity.ParentDataMapComponent = parentDataMapComponent;
                        businessEntity.ParentDataMapComponentId = parentDataMapComponent.Id;
                        businessEntity.ParentDataMapComponentName = parentDataMapComponent.Name;
                    }
                    businessEntity.SourceSearchSpecification = UIEntity.SourceSearchSpecification;
                }
            }
            return businessEntity;
        }
        public override BUSDataMapObjectComponent Init(TContext context)
        {
            BUSDataMapObjectComponent businessEntity = base.Init(context);
            DataMapObject dataMapObject = context.DataMapObjects
                .AsNoTracking()
                .Select(mapObject => new 
                { 
                    id = mapObject.Id, 
                    sourceBusinessObjectId = mapObject.SourceBusinessObjectId, 
                    destinationBusinessObjectId = mapObject.DestinationBusinessObjectId
                })
                .Select(mapObject => new DataMapObject
                {
                    Id = mapObject.id,
                    SourceBusinessObjectId = mapObject.sourceBusinessObjectId,
                    DestinationBusinessObjectId = mapObject.destinationBusinessObjectId
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Data Map Object"));
            if (dataMapObject != null)
            {
                businessEntity.DataMapObject = dataMapObject;
                businessEntity.DataMapObjectId = dataMapObject.Id;
                businessEntity.SourceBusinessObjectId = dataMapObject.SourceBusinessObjectId;
                businessEntity.DestinationBusinessObjectId = dataMapObject.DestinationBusinessObjectId;
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIDataMapObjectComponent UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.SourceBOComponentName))
                result.Add(new ValidationResult("Source business component is a required field.", new List<string>() { "SourceBusinessComponentName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.DestinationBOComponentName))
                result.Add(new ValidationResult("Destination business component is a required field.", new List<string>() { "DestinationBusinessComponentName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSDataMapObjectComponent businessComponent, UIDataMapObjectComponent UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrEmpty(businessComponent.ErrorMessage))
            {
                if (businessComponent.SourceBusinessObject == null)
                {
                    result.Add(new ValidationResult(
                        $"Business object attached to data map object {businessComponent.DataMapObject.Name} as source business object not found.",
                        new List<string>() { "SourceBusinessObjectName" }));
                }
                else if (businessComponent.SourceBOComponentName == null)
                    result.Add(new ValidationResult("Business component with this name not found.", new List<string>() { "SourceBusinessComponentName" }));
                if (businessComponent.DestinationBusinessObject == null)
                {
                    result.Add(new ValidationResult(
                        $"Business object attached to data map object {businessComponent.DataMapObject.Name} as destination business object not found.",
                        new List<string>() { "DestinationBusinessObjectName" }));
                }
                else if (businessComponent.DestinationBOComponentName == null)
                    result.Add(new ValidationResult("Business component with this name not found.", new List<string>() { "DestinationBusinessComponentName" }));
                if (!string.IsNullOrWhiteSpace(UIEntity.ParentDataMapComponentName) && businessComponent.ParentDataMapComponent == null)
                    result.Add(new ValidationResult("Data map component with this name not found.", new List<string>() { "ParentDataMapComponentName" }));
            }
            return result;
        }
    }
}
