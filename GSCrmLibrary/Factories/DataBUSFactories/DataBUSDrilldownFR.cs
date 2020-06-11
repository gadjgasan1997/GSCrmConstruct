using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using System;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSDrilldownFR<TContext> : DataBUSFactory<Drilldown, BUSDrilldown, TContext>
        where TContext : MainContext, new()
    {
        public override Drilldown BusinessToData(Drilldown drilldown, BUSDrilldown businessEntity, TContext context, bool NewRecord)
        {
            Drilldown dataEntity = base.BusinessToData(drilldown, businessEntity, context, NewRecord);
            dataEntity.Applet = businessEntity.Applet;
            dataEntity.AppletId = businessEntity.AppletId;
            dataEntity.DestinationBusinessComponent = businessEntity.DestinationBusinessComponent;
            dataEntity.DestinationBusinessComponentId = businessEntity.DestinationBusinessComponentId;
            dataEntity.DestinationField = businessEntity.DestinationField;
            dataEntity.DestinationFieldId = businessEntity.DestinationFieldId;
            dataEntity.DestinationScreen = businessEntity.DestinationScreen;
            dataEntity.DestinationScreenId = businessEntity.DestinationScreenId;
            dataEntity.DestinationScreenItem = businessEntity.DestinationScreenItem;
            dataEntity.DestinationScreenItemId = businessEntity.DestinationScreenItemId;
            dataEntity.HyperLinkField = businessEntity.HyperLinkField;
            dataEntity.HyperLinkFieldId = businessEntity.HyperLinkFieldId;
            dataEntity.SourceField = businessEntity.SourceField;
            dataEntity.SourceFieldId = businessEntity.SourceFieldId;
            return dataEntity;
        }
        public override BUSDrilldown DataToBusiness(Drilldown dataEntity, TContext context)
        {
            // Applet
            BUSDrilldown businessEntity = base.DataToBusiness(dataEntity, context);
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
                    }
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
                    }
                })
                .FirstOrDefault(i => i.Id == dataEntity.AppletId);
            businessEntity.Applet = applet;
            businessEntity.AppletId = applet.Id;

            if (applet.BusComp != null)
            {
                businessEntity.SourceBusinessComponent = applet.BusComp;
                businessEntity.SourceBusinessComponentId = (Guid)applet.BusCompId;

                // Source and hyperlink fields
                if (applet.BusComp.Fields?.Count > 0)
                {
                    Field sourceField = applet.BusComp.Fields.FirstOrDefault(i => i.Id == dataEntity.SourceFieldId);
                    if (sourceField != null)
                    {
                        businessEntity.SourceField = sourceField;
                        businessEntity.SourceFieldId = sourceField.Id;
                        businessEntity.SourceFieldName = sourceField.Name;
                    }

                    Field hyperLinkField = applet.BusComp.Fields.FirstOrDefault(i => i.Id == dataEntity.HyperLinkFieldId);
                    if (hyperLinkField != null)
                    {
                        businessEntity.HyperLinkField = hyperLinkField;
                        businessEntity.HyperLinkFieldId = hyperLinkField.Id;
                        businessEntity.HyperLinkFieldName = hyperLinkField.Name;
                    }
                }

                // Destination business component and destination field
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
                    .FirstOrDefault(i => i.Id == dataEntity.DestinationBusinessComponentId);
                if (destinationBusinessComponent != null)
                {
                    businessEntity.DestinationBusinessComponent = destinationBusinessComponent;
                    businessEntity.DestinationBusinessComponentId = destinationBusinessComponent.Id;
                    businessEntity.DestinationBusinessComponentName = destinationBusinessComponent.Name;
                    Field destinationField = destinationBusinessComponent.Fields.FirstOrDefault(i => i.Id == dataEntity.DestinationFieldId);
                    if (destinationField != null)
                    {
                        businessEntity.DestinationField = destinationField;
                        businessEntity.DestinationFieldId = destinationField.Id;
                        businessEntity.DestinationFieldName = destinationField.Name;
                    }
                }

                // Destination screen and destination view
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
                    .FirstOrDefault(i => i.Id == dataEntity.DestinationScreenId);
                if (destinationScreen != null)
                {
                    businessEntity.DestinationScreen = destinationScreen;
                    businessEntity.DestinationScreenId = destinationScreen.Id;
                    businessEntity.DestinationScreenName = destinationScreen.Name;

                    ScreenItem destinationScreenItem = destinationScreen.ScreenItems.FirstOrDefault(i => i.Id == dataEntity.DestinationScreenItemId);
                    if (destinationScreenItem != null)
                    {
                        businessEntity.DestinationScreenItem = destinationScreenItem;
                        businessEntity.DestinationScreenItemId = destinationScreenItem.Id;
                        businessEntity.DestinationScreenItemName = destinationScreenItem.Name;
                    }
                }
            }

            return businessEntity;
        }
    }
}
