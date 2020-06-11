using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Services.Info;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIDrilldownFR<TContext> : BUSUIFactory<BUSDrilldown, UIDrilldown, TContext>
        where TContext : MainContext, new()
    {
        public override UIDrilldown BusinessToUI(BUSDrilldown businessEntity)
        {
            UIDrilldown UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.DestinationBusinessComponentName = businessEntity.DestinationBusinessComponentName;
            UIEntity.DestinationFieldName = businessEntity.DestinationFieldName;
            UIEntity.DestinationScreenName = businessEntity.DestinationScreenName;
            UIEntity.DestinationScreenItemName = businessEntity.DestinationScreenItemName;
            UIEntity.SourceFieldName = businessEntity.SourceFieldName;
            UIEntity.HyperLinkFieldName = businessEntity.HyperLinkFieldName;
            return UIEntity;
        }
        public override BUSDrilldown UIToBusiness(UIDrilldown UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSDrilldown businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Applet applet = context.Applets
                .AsNoTracking()
                .Select(a => new
                {
                    id = a.Id,
                    name = a.Name,
                    busCompId = a.BusCompId,
                    busComp = new
                    {
                        id = a.BusComp.Id,
                        name = a.BusComp.Name,
                        fields = a.BusComp.Fields.Select(f => new
                        {
                            id = f.Id,
                            name = f.Name
                        })
                    },
                    drilldowns = a.Drilldowns.Select(drilldown => new
                    {
                        id = drilldown.Id,
                        name = drilldown.Name,
                    })
                })
                .Select(a => new Applet
                {
                    Id = a.id,
                    Name = a.name,
                    BusCompId = a.busCompId,
                    BusComp = new BusinessComponent
                    {
                        Id = a.busComp.id,
                        Name = a.busComp.name,
                        Fields = a.busComp.fields.Select(f => new Field
                        {
                            Id = f.id,
                            Name = f.name
                        }).ToList()
                    },
                    Drilldowns = a.drilldowns.Select(drilldown => new Drilldown
                    {
                        Id = drilldown.id,
                        Name = drilldown.name
                    }).ToList()
                })
                .FirstOrDefault(n => n.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Applet"));
            if (applet == null)
                businessEntity.ErrorMessage = "First you need create applet.";
            if (applet.BusComp == null)
                businessEntity.ErrorMessage = $"First you need to add a business component to applet \"{applet.Name}\".";
            else
            {
                Drilldown drilldown = applet.Drilldowns?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (drilldown != null && drilldown.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Drilldown with this name is already exists in applet \"{applet.Name}\".";
                else
                {
                    businessEntity.Applet = applet;
                    businessEntity.AppletId = applet.Id;
                    businessEntity.SourceBusinessComponent = applet.BusComp;
                    businessEntity.SourceBusinessComponentId = (Guid)applet.BusCompId;

                    // Destination screen and screen item
                    Screen destinationScreen = context.Screens
                        .AsNoTracking()
                        .Select(s => new
                        {
                            id = s.Id,
                            name = s.Name,
                            screenItems = s.ScreenItems.Select(si => new
                            {
                                id = si.Id,
                                name = si.Name
                            })
                        })
                        .Select(s => new Screen
                        {
                            Id = s.id,
                            Name = s.name,
                            ScreenItems = s.screenItems.Select(si => new ScreenItem
                            {
                                Id = si.id,
                                Name = si.name
                            }).ToList()
                        })
                        .FirstOrDefault(n => n.Name == UIEntity.DestinationScreenName);

                    if (destinationScreen != null)
                    {
                        businessEntity.DestinationScreen = destinationScreen;
                        businessEntity.DestinationScreenId = destinationScreen.Id;
                        businessEntity.DestinationScreenName = destinationScreen.Name;

                        ScreenItem destinationScreenItem = destinationScreen.ScreenItems.FirstOrDefault(n => n.Name == UIEntity.DestinationScreenItemName);
                        if (destinationScreenItem != null)
                        {
                            businessEntity.DestinationScreenItem = destinationScreenItem;
                            businessEntity.DestinationScreenItemId = destinationScreenItem.Id;
                            businessEntity.DestinationScreenItemName = destinationScreenItem.Name;

                            BusinessComponent destinationBusinessComponent = context.BusinessComponents
                                .AsNoTracking()
                                .Select(bc => new
                                {
                                    id = bc.Id,
                                    name = bc.Name,
                                    fields = bc.Fields.Select(f => new
                                    {
                                        id = f.Id,
                                        name = f.Name
                                    })
                                })
                                .Select(bc => new BusinessComponent
                                {
                                    Id = bc.id,
                                    Name = bc.name,
                                    Fields = bc.fields.Select(f => new Field
                                    {
                                        Id = f.id,
                                        Name = f.name
                                    }).ToList()
                                })
                                .FirstOrDefault(n => n.Name == UIEntity.DestinationBusinessComponentName);
                            if (destinationBusinessComponent != null)
                            {
                                businessEntity.DestinationBusinessComponent = destinationBusinessComponent;
                                businessEntity.DestinationBusinessComponentId = destinationBusinessComponent.Id;
                                businessEntity.DestinationBusinessComponentName = destinationBusinessComponent.Name;

                                Field destinationField = destinationBusinessComponent.Fields.FirstOrDefault(n => n.Name == UIEntity.DestinationFieldName);
                                if (destinationField != null)
                                {
                                    businessEntity.DestinationField = destinationField;
                                    businessEntity.DestinationFieldId = destinationField.Id;
                                    businessEntity.DestinationFieldName = destinationField.Name;
                                }

                                Field sourceField = applet.BusComp.Fields.FirstOrDefault(n => n.Name == UIEntity.SourceFieldName);
                                if (sourceField != null)
                                {
                                    businessEntity.SourceField = sourceField;
                                    businessEntity.SourceFieldId = sourceField.Id;
                                    businessEntity.SourceFieldName = sourceField.Name;
                                }

                                Field hyperLinkField = applet.BusComp.Fields.FirstOrDefault(n => n.Name == UIEntity.HyperLinkFieldName);
                                if (hyperLinkField != null)
                                {
                                    businessEntity.HyperLinkField = hyperLinkField;
                                    businessEntity.HyperLinkFieldId = hyperLinkField.Id;
                                    businessEntity.HyperLinkFieldName = hyperLinkField.Name;
                                }
                            }
                        }
                    }
                }
            }
            return businessEntity;
        }
        public override BUSDrilldown Init(TContext context)
        {
            BUSDrilldown businessEntity = base.Init(context);
            Applet applet = context.Applets
                .AsNoTracking()
                .Select(a => new
                {
                    id = a.Id,
                    busCompId = a.BusCompId,
                    busComp = a.BusComp == null ? null : new
                    {
                        id = a.BusComp.Id,
                        name = a.BusComp.Name
                    }
                })
                .Select(a => new Applet
                {
                    Id = a.id,
                    BusCompId = a.busCompId,
                    BusComp = a.busComp == null ? null : new BusinessComponent
                    {
                        Id = a.busComp.id,
                        Name = a.busComp.name
                    }
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Applet"));
            if (applet?.BusComp != null)
            {
                businessEntity.SourceBusinessComponent = applet.BusComp;
                businessEntity.SourceBusinessComponentId = (Guid)applet.BusCompId;
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIDrilldown UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.SourceFieldName))
                result.Add(new ValidationResult("Source field is a required field.", new List<string>() { "SourceFieldName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.HyperLinkFieldName))
                result.Add(new ValidationResult("Hyper link field is a required field.", new List<string>() { "HyperLinkFieldName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.DestinationScreenName))
                result.Add(new ValidationResult("Destination screen is a required field.", new List<string>() { "DestinationScreenName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.DestinationScreenItemName))
                result.Add(new ValidationResult("Destination screen item is a required field.", new List<string>() { "DestinationScreenItemName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.DestinationBusinessComponentName))
                result.Add(new ValidationResult("Destination business component is a required field.", new List<string>() { "DestinationBusinessComponentName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.DestinationFieldName))
                result.Add(new ValidationResult("Destination field is a required field.", new List<string>() { "DestinationFieldName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSDrilldown businessComponent, UIDrilldown UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.SourceField == null)
                    result.Add(new ValidationResult("Source field with this name not found.", new List<string>() { "SourceFieldName" }));
                if (businessComponent.HyperLinkFieldName == null)
                    result.Add(new ValidationResult("Hyper link field with this name not found.", new List<string>() { "HyperLinkFieldName" }));
                if (businessComponent.DestinationScreen == null)
                    result.Add(new ValidationResult("Destination screen with this name not found.", new List<string>() { "DestinationScreenName" }));
                if (businessComponent.DestinationScreenItem == null)
                    result.Add(new ValidationResult("Destination screen item with this name not found.", new List<string>() { "DestinationScreenItemName" }));
                if (businessComponent.DestinationBusinessComponent == null)
                    result.Add(new ValidationResult("Destination business component with this name not found.", new List<string>() { "DestinationBusinessComponentName" }));
                if (businessComponent.DestinationField == null)
                    result.Add(new ValidationResult("Destination field with this name not found.", new List<string>() { "DestinationFieldName" }));
            }
            return result;
        }
    }
}
