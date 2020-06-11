using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.RequestModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController<TContext, TBUSFactory> : ControllerBase
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        private readonly TContext context;
        private readonly IViewInfo viewInfo;
        private readonly IViewInfoUI viewInfoUI;

        public ViewController(TContext context, IViewInfo viewInfo, IViewInfoUI viewInfoUI)
        {
            this.context = context;
            this.viewInfo = viewInfo;
            this.viewInfoUI = viewInfoUI;
        }

        #region Requests
        [HttpGet("InitializeView/{viewName}")]
        public virtual ActionResult<object> InitializeView(string viewName)
        {
            // Если представления не были проинициализированны ранее
            if (viewInfo.View == null)
            {
                viewInfo.StartInitialize(null, viewName, context);
                viewInfoUI.Initialize(context);
            }

            // При переходе на новое представление
            else if (viewInfo.View.Name != viewName)
            {
                if (viewInfo.ActionType == ActionType.Drilldown)
                    viewInfo.StartInitialize(viewInfo, viewInfo.View.Name, context);
                else viewInfo.StartInitialize(viewInfo, viewName, context);
                viewInfoUI.Initialize(context);
            }

            // При обновлении страницы и других не предусмотренных случаях
            else
            {
                if (viewInfo.ActionType == ActionType.Drilldown)
                {
                    viewInfo.StartInitialize(viewInfo, viewInfo.View.Name, context);
                    viewInfoUI.Initialize(context);
                }
                else
                {
                    viewInfo.ActionType = ActionType.ReloadView;
                    viewInfo.CurrentApplet = viewInfo.ViewApplets.FirstOrDefault();

                    // Уничтожение открытого попапа
                    if (viewInfo.CurrentPopupApplet != null)
                    {
                        viewInfo.RemovePopupApplet(context);
                        viewInfoUI.Initialize(context);
                    }
                }
            }
            return viewInfoUI.Serialize();
        }

        [HttpGet("ViewInfo")]
        public virtual ActionResult<object> ViewInfo()
        {
            viewInfoUI.Initialize(context);
            return viewInfoUI.Serialize();
        }

        [HttpPost("UpdateViewInfo")]
        public virtual ActionResult<object> UpdateViewInfo([FromBody] RequestViewModel model)
        {
            Applet applet = viewInfo.ViewApplets.FirstOrDefault(n => n.Name == model.TargetApplet);
            viewInfo.ActionType = (ActionType)Enum.Parse(typeof(ActionType), model.ActionType);
            if (applet != null)
            {
                BusinessComponent component = viewInfo.ViewBCs.FirstOrDefault(i => i.Id == applet.BusCompId);
                // Текущий апплет
                viewInfo.CurrentApplet = applet;
                // Текущий контрол
                viewInfo.CurrentControl = applet.Controls.FirstOrDefault(n => n.Name == model.CurrentControl);
                viewInfo.CurrentColumn = applet.Columns.FirstOrDefault(n => n.Name == model.CurrentColumn);
                // При выборе из списка устанавливается выбранная запись
                if (model.CurrentRecord != null)
                {
                    viewInfo.CurrentRecord = model.CurrentRecord;
                    ComponentsRecordsInfo.SetSelectedRecord(component.Name, model.CurrentRecord);
                }
            }
            // В процессе работы с попапом
            if (viewInfo.CurrentPopupApplet != null)
            {
                // Текущий контрол на попапе
                viewInfo.CurrentPopupControl =
                    viewInfo.CurrentPopupApplet?.Controls.FirstOrDefault(n => n.Name == model.CurrentPopupControl);
            }

            // Открытие/закрытие попапа
            if (model.OpenPopup)
                viewInfo.AddPopupApplet(context);
            if (model.ClosePopup && viewInfo.CurrentPopupApplet != null)
                viewInfo.RemovePopupApplet(context);

            // В случае отмены создания записи, проставление null в контексте бизнес компоненты апплета
            if (viewInfo.ActionType == ActionType.UndoRecord)
            {
                TBUSFactory busFactory = new TBUSFactory();
                busFactory.SetRecord(null, context, viewInfo, viewInfo.CurrentPopupApplet?.BusComp ?? viewInfo.CurrentApplet.BusComp, null);
            }
            viewInfoUI.Initialize(context);
            return viewInfoUI.Serialize();
        }

        [HttpGet("UpdateContext")]
        public virtual ActionResult<string> UpdateContext()
        {
            // Список проверенных бк, список с верхними в иерархии бк и список с апплетами для обновления
            List<Applet> appletsToUpdate = new List<Applet>();
            List<BusinessObjectComponent> topComponents = new List<BusinessObjectComponent>();
            viewInfo.AppletsSortedByLinks.Clear();

            // Необходимо выбрать те компоненты, у которых нет связей, либо те, которые являются верхними в иерархии
            viewInfo.BOComponents.ForEach(component =>
            {
                // Если у компоненты нет линки, то она добавляется в список верхних в иерархии
                if (component.Link == null)
                    topComponents.Add(component);

                // Иначе находится ее родительская компонента из связи
                else
                {
                    BusinessObjectComponent parentComponent = viewInfo.BOComponents.FirstOrDefault(i => i.BusCompId == component.Link.ParentBCId);

                    // Если родительской бк нет в представлении, текущая компонента добавляется в список верхних в иерархии
                    if (parentComponent == null)
                        topComponents.Add(component);

                    // Иначе до тех пор пока по связям можно подниматься наверх в списке бк текущего представления
                    else
                        while (viewInfo.BOComponents.IndexOf(parentComponent) != -1)
                        {
                            // Если у родительской компоненты нет ссылки она добавляется в список верхних в иерархии
                            if (parentComponent.Link == null)
                            {
                                if (topComponents.IndexOf(parentComponent) == -1)
                                    topComponents.Add(parentComponent);
                                break;
                            }

                            // Иначе компонента становится равной родительской компоненте из линки
                            parentComponent = viewInfo.BOComponents.FirstOrDefault(i => i.BusCompId == parentComponent.Link.ParentBCId);
                        }
                }
            });

            /* Для каждой компоненты, верхней в иерархии, запускается формирование списка апплетов, необходимых для обновления
             * И для всех компонент, отсутствующих в представлении, но являющихся верхними в иерархии для списка topComponents необходимо установить контекст */
            topComponents.ForEach(component =>
            {
                List<BusinessObjectComponent> boComponents = viewInfo.ViewBO.BusObjectComponents;
                List<BusinessObjectComponent> parentComponents = new List<BusinessObjectComponent>();
                if (component.Link != null)
                {
                    BusinessObjectComponent parentComponent = boComponents.FirstOrDefault(i => i.BusCompId == component.Link.ParentBCId);
                    parentComponents.Add(parentComponent);
                    while (parentComponent?.Link != null)
                    {
                        parentComponent = boComponents.FirstOrDefault(i => i.BusCompId == parentComponent.Link.ParentBCId);
                        parentComponents.Add(parentComponent);
                    }

                    parentComponents.AsEnumerable().Reverse().ToList().ForEach(item =>
                    {
                        BusinessComponent busComp = context.BusinessComponents
                            .Include(f => f.Fields)
                            .FirstOrDefault(i => i.Id == item.BusCompId);

                        TBUSFactory busFactory = new TBUSFactory();
                        if (!ComponentsRecordsInfo.IsInitComponent(busComp.Name))
                        {
                            busFactory.InitializeComponentsRecords(null, context, viewInfo, new FilterEntitiesModel()
                            {
                                BOComponents = boComponents,
                                Link = item.Link,
                                BusComp = busComp
                            });
                        }
                    });
                }
                // Добавление всех апплетов не являющихся попапами в список для обновления
                viewInfo.ViewApplets.ForEach(viewApplet =>
                {
                    if (viewApplet.Type != "Popup")
                        appletsToUpdate.Add(viewApplet);
                });
                viewInfo.AppletsSortedByLinks.AddRange(AppletListBuilding(new List<BusinessObjectComponent>(), component, component, appletsToUpdate));
            });

            return Ok();
        }

        [HttpPost("PartialUpdateContext")]
        public virtual ActionResult<string> PartialUpdateContext([FromBody] RequestAppletModel model)
        {
            // Получение всех необходимых сущностей
            Applet applet = viewInfo.ViewApplets.FirstOrDefault(n => n.Name == model.AppletName);
            BusinessObjectComponent component = viewInfo.BOComponents.FirstOrDefault(bcId => bcId.BusCompId == applet.BusCompId);

            // Получение списка с апплетами, необходимыми для обновления. Если есть флаг обновления текущего апплета, то он добавляется в выходной список
            List<Applet> appletsToUpdate = new List<Applet>();
            if (model.RefreshCurrentApplet)
                appletsToUpdate.Add(applet);

            // Добавление всех апплетов, основанных на той же компоненте и не являющихся попапами в список для обновления
            viewInfo.ViewApplets.ForEach(viewApplet =>
            {
                if (viewApplet.BusCompId == applet.BusCompId && viewApplet.Id != applet.Id&& viewApplet.Type != "Popup")
                    appletsToUpdate.Add(viewApplet);
            });

            AppletListBuilding(new List<BusinessObjectComponent>(), component, component, appletsToUpdate);
            appletsToUpdate = appletsToUpdate.Where(i => i.Initflag == false).ToList();
            //viewInfo.AppletsSortedByLinks.AddRange(appletsToUpdate);

            // Result
            return JsonConvert.SerializeObject(appletsToUpdate.Select(n => n.Name), Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        #endregion

        #region Addion

        // Проходится по бк, формируя список апплетов для обновления
        private List<Applet> AppletListBuilding(List<BusinessObjectComponent> checkedComponents, BusinessObjectComponent masterBusComp, BusinessObjectComponent currentBusComp, List<Applet> appletsToUpdate)
        {
            // Формирование списка дочерних бк
            List<BusinessObjectComponent> childComponents = new List<BusinessObjectComponent>();
            viewInfo
                .BOComponents
                .Except(checkedComponents)
                .ToList()
                .ForEach(component =>
                {
                    // Связь
                    Link link = context.Links.FirstOrDefault(i => i.Id == component.LinkId);

                    // Если у связи в БКО Id родительской компоненты совпадает с id текущей, добавляю эту БКО в список дочерних
                    if (link != null && link.ParentBCId == currentBusComp.BusCompId)
                        childComponents.Add(component);
                });

            // До тех пор, пока есть дочерние компоненты
            while (childComponents.Count > 0)
            {
                // Получение всех необходимых сущностей
                BusinessObjectComponent component = childComponents.FirstOrDefault();
                Applet applet = viewInfo.ViewApplets
                    .Where(bcId => bcId.BusCompId == component.BusCompId)
                    .Where(type => type.Type != "Popup")
                    .FirstOrDefault();

                // Если в списке апплетов для обновления нет текущего, его надо добавить
                if (!appletsToUpdate.Contains(applet))
                    appletsToUpdate.Add(applet);

                // Добавление всех апплетов, основанных на той же компоненте и не являющихся попапми в список для обновления
                viewInfo.ViewApplets.ForEach(viewApplet =>
                {
                    if (viewApplet.BusCompId == applet.BusCompId && viewApplet.Id != applet.Id
                    && viewApplet.Type != "Popup" && !appletsToUpdate.Contains(viewApplet))
                        appletsToUpdate.Add(viewApplet);
                });

                // Запустить заново
                return AppletListBuilding(checkedComponents, masterBusComp, component, appletsToUpdate);
            }

            // Если дочерних не осталось, значит дошли до низа иерархии, либо связи изначально отсутствовали
            if (masterBusComp != currentBusComp)
            {
                // Добавляю бк в список проверенных, делаю текущей бк верхнюю, запускаю заново
                checkedComponents.Add(currentBusComp);
                return AppletListBuilding(checkedComponents, masterBusComp, masterBusComp, appletsToUpdate);
            }

            else
            {
                if (appletsToUpdate.Count > 0)
                {
                    List<Applet> sortedApplets = new List<Applet>();
                    List<Applet> formApplets = new List<Applet>();
                    BusinessComponent businessComponent = appletsToUpdate.FirstOrDefault().BusComp;
                    appletsToUpdate.ForEach(applet =>
                    {
                        if (businessComponent.Id == applet.BusCompId)
                        {
                            if (applet.Type == "Tile") sortedApplets.Add(applet);
                            else formApplets.Add(applet);
                        }
                        else
                        {
                            businessComponent = applet.BusComp;
                            sortedApplets.AddRange(formApplets);
                            formApplets.Clear();
                            if (applet.Type == "Tile") sortedApplets.Add(applet);
                            else formApplets.Add(applet);
                        }
                    });
                    sortedApplets.AddRange(formApplets);
                    return sortedApplets;
                }
                else return appletsToUpdate;
            }
        }
        #endregion
    }
}
