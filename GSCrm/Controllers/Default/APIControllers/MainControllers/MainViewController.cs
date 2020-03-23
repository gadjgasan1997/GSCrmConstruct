using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using GSCrm.Services.Info;
using GSCrm.Data.Context;

namespace GSCrm.Controllers.Default.APIControllers.MainControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainViewController : ControllerBase
    {
        private readonly ApplicationContext context;
        private readonly IViewInfo viewInfo;
        private readonly IViewInfoUI viewInfoUI;

        public MainViewController(ApplicationContext context,
            IViewInfo viewInfo,
            IViewInfoUI viewInfoUI)
        {
            this.context = context;
            this.viewInfo = viewInfo;
            this.viewInfoUI = viewInfoUI;
        }

        #region Requests
        [HttpGet("InitializeView/{viewName}")]
        public virtual object InitializeView(string viewName)
        {
            // Если представления не были проинициализированны ранее
            if (viewInfo.View == null || viewInfo.View.Name != viewName)
            {
                // Инициализация
                viewInfo.Initialize(viewName, context);
                viewInfoUI.Initialize(context);
            }
            else viewInfo.Action = context.Actions.FirstOrDefault(n => n.Name == "ReloadView");
            return viewInfoUI.Serialize();
        }

        [HttpGet("ViewInfo")]
        public virtual object ViewInfo()
        {
            viewInfoUI.Initialize(context);
            return viewInfoUI.Serialize();
        }

        [HttpPost("UpdateViewInfo")]
        public virtual object UpdateViewInfo([FromBody] RequestViewModel model)
        {
            Applet applet = viewInfo.ViewApplets.FirstOrDefault(n => n.Name == model.CurrentApplet);
            viewInfo.Action = context.Actions.FirstOrDefault(n => n.Name == model.Action);
            if (applet != null)
            {
                BOComponent component = viewInfo.BOComponents.FirstOrDefault(i => i.BusCompId == applet.BusCompId);
                // Текущий апплет
                viewInfo.CurrentApplet = applet;
                // Текущий контрол
                viewInfo.CurrentControl = applet.Controls.FirstOrDefault(n => n.Name == model.CurrentControl);
                // При выборе из списка устанавливается выбранная запись
                if (model.CurrentRecord != null)
                {
                    viewInfo.CurrentRecord = model.CurrentRecord;
                    ComponentContext.SetSelectedRecord(component.Name, model.CurrentRecord);
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

            if (model.ClosePopup)
                viewInfo.RemovePopupApplet(context);

            // Результат
            viewInfoUI.Initialize(context);
            return viewInfoUI.Serialize();
        }

        [HttpPost("UpdateContext")]
        public virtual ActionResult<string> UpdateContext([FromBody] RequestAppletModel model)
        {
            try
            {
                // Список проверенных бк, список с верхними в иерархии бк и список с апплетами для обновления
                List<BOComponent> checkedComponents = new List<BOComponent>();
                List<BOComponent> topComponents = new List<BOComponent>();
                List<string> appletsToUpdate = new List<string>();

                // Необходимо выбрать те компоненты, у которых нет связей, либо те, которые являются верхними в иерархии
                viewInfo.BOComponents.ForEach(component =>
                {
                    // Если у компоненты нет линки, то она добавляется в список верхних в иерархии
                    if (component.Link == null)
                        topComponents.Add(component);

                    // Иначе находится ее родительская компонента из связи
                    else
                    {
                        BOComponent parentComponent = viewInfo.BOComponents.FirstOrDefault(i => i.BusCompId == component.Link.ParentBCId);

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

                // Для каждой компоненты, верхней в иерархии, запускается формирование списка апплетов, необходимых для обновления
                topComponents.ForEach(component =>
                {
                    Applet applet = viewInfo.ViewApplets.FirstOrDefault(i => i.BusCompId == component.BusCompId);
                    appletsToUpdate.Add(applet.Name);
                    ListBuilding(checkedComponents, component, component, appletsToUpdate);
                });

                // Result
                return JsonConvert.SerializeObject(appletsToUpdate, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PartialUpdateContext")]
        public virtual ActionResult<string> PartialUpdateContext([FromBody] RequestAppletModel model)
        {
            try
            {
                // Получение всех необходимых сущностей
                Applet applet = viewInfo.ViewApplets.FirstOrDefault(n => n.Name == model.AppletName);
                BOComponent component = viewInfo.BOComponents.FirstOrDefault(bcId => bcId.BusCompId == applet.BusCompId);

                // Получение списка с апплетами, необходимыми для обновления. Если есть флаг обновления текущего апплета, то он добавляется в выходной список
                List<string> appletsToUpdate = new List<string>();
                if (model.RefreshCurrentApplet)
                    appletsToUpdate.Add(applet.Name);

                ListBuilding(new List<BOComponent>(), component, component, appletsToUpdate);

                // Result
                return JsonConvert.SerializeObject(appletsToUpdate, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Addion

        // Проходится по бк, формируя список апплетов для обновления
        private List<string> ListBuilding(List<BOComponent> checkedComponents, BOComponent masterBusComp, BOComponent currentBusComp, List<string> appletsToUpdate)
        {
            // Формирование списка дочерних бк
            List<BOComponent> childComponents = new List<BOComponent>();
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
                BOComponent component = childComponents.FirstOrDefault();
                Applet applet = viewInfo.ViewApplets
                    .Where(bcId => bcId.BusCompId == component.BusCompId)
                    .Where(type => type.Type != "Popup")
                    .FirstOrDefault();

                // Если в списке апплетов для обновления нет текущего, его надо добавить
                if (appletsToUpdate.IndexOf(applet.Name) == -1)
                    appletsToUpdate.Add(applet.Name);

                // Запустить заново
                return ListBuilding(checkedComponents, masterBusComp, component, appletsToUpdate);
            }

            // Если дочерних не осталось, значит дошли до низа иерархии, либо связи изначально отсутствовали
            if (masterBusComp != currentBusComp)
            {
                // Добавляю бк в список проверенных, делаю текущей бк верхнюю, запускаю заново
                checkedComponents.Add(currentBusComp);
                return ListBuilding(checkedComponents, masterBusComp, masterBusComp, appletsToUpdate);
            }

            else return appletsToUpdate;
        }
        #endregion
    }
}
