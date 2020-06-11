using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Models.RequestModels;
using static GSCrmLibrary.Configuration.ApplicationConfig;
using GSCrmLibrary.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Factories.MainFactories;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController<TContext, TBUSFactory> : ControllerBase
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        private readonly TContext context;
        private readonly IApplicationInfo applicationInfo;
        private readonly IApplicationInfoUI applicationInfoUI;
        private readonly IScreenInfo screenInfo;
        private readonly IScreenInfoUI screenInfoUI;
        private readonly IViewInfo viewInfo;
        public ApplicationController(TContext context,
            IApplicationInfo applicationInfo,
            IApplicationInfoUI applicationInfoUI,
            IScreenInfo screenInfo,
            IScreenInfoUI screenInfoUI,
            IViewInfo viewInfo)
        {
            this.context = context;
            this.applicationInfo = applicationInfo;
            this.applicationInfoUI = applicationInfoUI;
            this.screenInfo = screenInfo;
            this.screenInfoUI = screenInfoUI;
            this.viewInfo = viewInfo;
        }

        #region Requests
        [HttpGet("InitializeApplication")]
        public ActionResult<object> InitializeApplication(string screenName, string viewName)
        {
            if (applicationInfo.Application == null)
            {
                applicationInfo.Initialize(context.GetType().Name == "GSAppContext" ? ApplicationName : "GSCrm Tools", screenName, viewName, context);
                applicationInfoUI.Initialize(context);
            }
            return Ok(applicationInfoUI.Serialize());
        }

        [HttpPost("UpdateApplicationInfo")]
        public ActionResult<object> UpdateApplicationInfo([FromBody] RequestApplicationModel model)
        {
            applicationInfo.CurrentScreen = context.Screens.FirstOrDefault(n => n.Name == model.ScreenName);
            applicationInfo.CurrentView = context.Views.FirstOrDefault(n => n.Name == model.ViewName);
            applicationInfoUI.Initialize(context);
            var test = applicationInfoUI.Serialize();
            return Ok(test);
        }

        [HttpGet("ApplicationInfo")]
        public ActionResult<object> ApplicationInfo()
        {
            applicationInfoUI.Initialize(context);
            return Ok(applicationInfoUI.Serialize());
        }

        [HttpGet("Drilldown")]
        public ActionResult Drilldown()
        {
            Applet currentApplet = viewInfo.CurrentPopupApplet ?? viewInfo.CurrentApplet;
            currentApplet = context.Applets
                .AsNoTracking()
                .Select(a => new
                {
                    id = a.Id,
                    name = a.Name,
                    type = a.Type,
                    busComp = new
                    {
                        id = a.BusComp.Id,
                        name = a.BusComp.Name,
                        routing = a.BusComp.Routing
                    },
                    drilldowns = a.Drilldowns.Select(d => new
                    {
                        id = d.Id,
                        name = d.Name,
                        hyperLinkFieldId = d.HyperLinkFieldId,
                        sourceField = new
                        {
                            id = d.SourceField.Id,
                            name = d.SourceField.Name
                        },
                        destinationBusinessComponent = new
                        {
                            id = d.DestinationBusinessComponent.Id,
                            name = d.DestinationBusinessComponent.Name,
                            routing = d.DestinationBusinessComponent.Routing
                        },
                        destinationField = new
                        {
                            id = d.DestinationField.Id,
                            name = d.DestinationField.Name
                        },
                        destinationScreenId = d.DestinationScreenId,
                        destinationScreenItemId = d.DestinationScreenItemId,
                        destinationScreenItem = new
                        {
                            id = d.DestinationScreenItem.Id,
                            view = new
                            {
                                id = d.DestinationScreenItem.View.Id,
                                busObjectId = d.DestinationScreenItem.View.BusObjectId
                            }
                        }
                    })
                })
                .Select(a => new Applet
                {
                    Id = a.id,
                    Name = a.name,
                    Type = a.type,
                    BusComp = new BusinessComponent
                    {
                        Id = a.busComp.id,
                        Name = a.busComp.name,
                        Routing = a.busComp.routing
                    },
                    Drilldowns = a.drilldowns.Select(d => new Drilldown
                    {
                        Id = d.id,
                        Name = d.name,
                        HyperLinkFieldId = d.hyperLinkFieldId,
                        SourceField = new Field
                        {
                            Id = d.sourceField.id,
                            Name = d.sourceField.name
                        },
                        DestinationBusinessComponent = new BusinessComponent
                        {
                            Id = d.destinationBusinessComponent.id,
                            Name = d.destinationBusinessComponent.name,
                            Routing = d.destinationBusinessComponent.routing
                        },
                        DestinationField = new Field
                        {
                            Id = d.destinationField.id,
                            Name = d.destinationField.name
                        },
                        DestinationScreenId = d.destinationScreenId,
                        DestinationScreenItem = new ScreenItem
                        {
                            Id = d.destinationScreenItem.id,
                            View = new View
                            {
                                Id = d.destinationScreenItem.view.id,
                                BusObjectId = d.destinationScreenItem.view.busObjectId
                            }
                        }
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == currentApplet.Id);
            if (currentApplet != null)
            {
                string controlName;
                Field field;
                switch (currentApplet.Type)
                {
                    case "Tile":
                        controlName = viewInfo.CurrentColumn.Name;
                        field = viewInfo.CurrentColumn.Field;
                        break;
                    default:
                        controlName = viewInfo.CurrentPopupControl?.Name ?? viewInfo.CurrentControl?.Name;
                        field = viewInfo.CurrentPopupControl?.Field ?? viewInfo.CurrentControl?.Field;
                        break;
                }
                if (field != null)
                {
                    TBUSFactory BUSFactory = new TBUSFactory();
                    Drilldown drilldown = currentApplet.Drilldowns.FirstOrDefault(i => i.HyperLinkFieldId == field.Id);
                    if (drilldown != null && drilldown.DestinationBusinessComponent?.Name != null && drilldown.DestinationField?.Name != null && drilldown.SourceField?.Name != null)
                    {
                        // Целевой экран
                        Screen destinationScreen = context.Screens.AsNoTracking().FirstOrDefault(i => i.Id == drilldown.DestinationScreenId);
                        if (destinationScreen != null)
                        {
                            applicationInfo.CurrentScreen = destinationScreen;

                            // Целевыое представление
                            View destinationView = context.Views.AsNoTracking().FirstOrDefault(i => i.Id == drilldown.DestinationScreenItem.View.Id);
                            if (destinationView != null)
                            {
                                applicationInfo.CurrentView = destinationView;

                                // Исходное поле
                                dynamic sourceRecord = BUSFactory.GetRecord(null, context, viewInfo, currentApplet.BusComp, "Id", viewInfo.CurrentRecord);
                                PropertyInfo sourceProperty = sourceRecord.GetType().GetProperty(drilldown.SourceField.Name);
                                if (sourceProperty != null)
                                {
                                    // Очистка старой информации о выбранных записях
                                    if (screenInfo.Screen.Name != destinationScreen.Name)
                                        ComponentsRecordsInfo.Dispose();

                                    // Установка текущей выбранной записи на целевой бизнес компоненте
                                    var sourcePropertyValue = sourceProperty.GetValue(sourceRecord);
                                    if (sourcePropertyValue != null)
                                    {
                                        dynamic destinationRecord = BUSFactory.GetRecord(null, context, viewInfo, drilldown.DestinationBusinessComponent, drilldown.DestinationField.Name, sourcePropertyValue.ToString());
                                        if (destinationRecord != null)
                                            ComponentsRecordsInfo.SetSelectedRecord(drilldown.DestinationBusinessComponent.Name, destinationRecord.Id.ToString());

                                        // Установка текущих выбранных записей на всех родительских бизнес компонентах целевой компоненты
                                        BusinessObject destinationBO = context.BusinessObjects
                                            .AsNoTracking()
                                            .Include(boc => boc.BusObjectComponents)
                                                .ThenInclude(l => l.Link)
                                                    .ThenInclude(cf => cf.ChildField)
                                            .Include(boc => boc.BusObjectComponents)
                                                .ThenInclude(l => l.Link)
                                                    .ThenInclude(pf => pf.ParentField)
                                            .FirstOrDefault(i => i.Id == destinationView.BusObjectId);
                                        BusinessObjectComponent destinationComponent = destinationBO.BusObjectComponents.FirstOrDefault(i => i.BusCompId == drilldown.DestinationBusinessComponent.Id);
                                        dynamic childRecord = destinationRecord;
                                        if (destinationComponent.Link != null)
                                        {
                                            BusinessComponent parentBusComp = context.BusinessComponents
                                                .AsNoTracking()
                                                .Select(bc => new
                                                {
                                                    id = bc.Id,
                                                    name = bc.Name,
                                                    table = new
                                                    {
                                                        id = bc.Table.Id,
                                                        name = bc.Table.Name
                                                    }
                                                })
                                                .Select(bc => new BusinessComponent
                                                {
                                                    Id = bc.id,
                                                    Name = bc.name,
                                                    Table = new Table
                                                    {
                                                        Id = bc.table.id,
                                                        Name = bc.table.name
                                                    }
                                                })
                                                .FirstOrDefault(i => i.Id == destinationComponent.Link.ParentBCId);
                                            BusinessComponent childBusComp = context.BusinessComponents
                                                .AsNoTracking()
                                                .Select(bc => new { id = bc.Id, name = bc.Name })
                                                .Select(bc => new BusinessComponent { Id = bc.id, Name = bc.name })
                                                .FirstOrDefault(i => i.Id == destinationComponent.Link.ChildBCId);
                                            string childFieldValue = childRecord.GetType().GetProperty(destinationComponent.Link.ChildField.Name).GetValue(childRecord).ToString();
                                            string parentFieldName = destinationComponent.Link.ParentField.Name;
                                            IEnumerable<dynamic> parentRecords = (IEnumerable<dynamic>)(context.GetType().GetProperty(parentBusComp.Table.Name).GetValue(context));
                                            string searchSpecificationByParent = $"{parentFieldName} = \"{childFieldValue}\"";
                                            dynamic parentRecord = parentRecords.AsQueryable().Where(searchSpecificationByParent).FirstOrDefault();
                                            ComponentsRecordsInfo.SetSearchSpecification(childBusComp.Name, SearchSpecTypes.SearchSpecificationByParent, searchSpecificationByParent);
                                            ComponentsRecordsInfo.SetSelectedRecord(parentBusComp.Name, parentRecord.Id.ToString());
                                            destinationComponent = destinationBO.BusObjectComponents.FirstOrDefault(i => i.BusCompId == destinationComponent.Link.ParentBCId);
                                            childRecord = parentRecord;
                                        }

                                        // Установка хлебных крошек
                                        ScreenItem crumb = screenInfo.Screen.ScreenItems.FirstOrDefault(n => n.View != null && n.View.Name == screenInfo.CurrentView.Name);
                                        ComponentsRecordsInfo.AppendCrumb(crumb);
                                        if (screenInfo.Screen.Name == destinationScreen.Name)
                                        {
                                            screenInfo.Initialize(screenInfo.Screen.Name, destinationView.Name, context);
                                            screenInfoUI.Initialize(screenInfo, context);
                                            viewInfo.View = destinationView;
                                        }
                                        viewInfo.ActionType = ActionType.Drilldown;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Ok();
        }
        #endregion
    }
}
