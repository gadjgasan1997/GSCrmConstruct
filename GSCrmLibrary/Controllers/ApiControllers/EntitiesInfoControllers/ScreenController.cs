using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.RequestModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;
using System;

namespace GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController<TContext> : ControllerBase
        where TContext : MainContext, new()
    {
        private readonly TContext context;
        private readonly IScreenInfo screenInfo;
        private readonly IScreenInfoUI screenInfoUI;
        private readonly IViewInfo viewInfo;

        public ScreenController(TContext context,
            IScreenInfo screenInfo,
            IScreenInfoUI screenInfoUI,
            IViewInfo viewInfo)
        {
            this.context = context;
            this.screenInfo = screenInfo;
            this.screenInfoUI = screenInfoUI;
            this.viewInfo = viewInfo;
        }

        #region Requests
        [HttpPost("InitializeScreen")]
        public virtual ActionResult<object> InitializeScreen([FromBody] RequestScreenModel model)
        {
            // Проверка, что такой скрин уже не проинициализирован
            if (screenInfo.Screen == null || screenInfo.Screen.Name != model.NewScreenName)
            {
                // Очистка информации
                viewInfo.Dispose();

                // Инициализация скрина
                screenInfo.Initialize(model.NewScreenName, model.NewViewName, context);
                screenInfo.ActionType = ActionType.InitializeScreen;
                screenInfoUI.Initialize(screenInfo, context);

                // Восстановление контекста при переходе на экран, который присутствует в хлебных крошках
                if (ComponentsRecordsInfo.GetCrumbs().FirstOrDefault(i => i.ViewId == screenInfo.CurrentView?.ViewId) != null)
                {
                    ComponentsRecordsInfo.RestoreContext(new List<View>() {
                        context.Views
                            .Include(bo => bo.BusObject)
                                .ThenInclude(boc => boc.BusObjectComponents)
                                    .ThenInclude(bc => bc.BusComp)
                            .FirstOrDefault(i => i.Id == screenInfo.CurrentView.ViewId)
                    });
                }
            }
            else screenInfo.ActionType = ActionType.ReloadScreen;
            return Ok(screenInfoUI.Serialize());
        }

        [HttpGet("ScreenInfo/{screenName}")]
        public virtual ActionResult<object> ScreenInfo(string screenName)
        {
            if (screenInfo.Screen != null && screenInfo.Screen.Name != screenName)
            {
                IScreenInfo newScreenInfo = (IScreenInfo)screenInfo.Clone();
                newScreenInfo.Initialize(screenName, null, context);
                IScreenInfoUI newScreenInfoUI = (IScreenInfoUI)screenInfoUI.Clone();
                newScreenInfoUI.Initialize(newScreenInfo, context);
                return Ok(newScreenInfoUI.Serialize());
            }
            else return Ok(screenInfoUI.Serialize());
        }
        
        [HttpPost("UpdateScreenInfo")]
        public virtual ActionResult<object> UpdateScreenInfo([FromBody] RequestScreenModel model)
        {
            switch (model.Action)
            {
                case "SelectScreenItem":
                    Screen newScreen = context.Screens
                        .Include(si => si.ScreenItems)
                            .ThenInclude(v => v.View)
                        .FirstOrDefault(n => n.Name == model.NewScreenName);
                    View oldView = context.Views.FirstOrDefault(n => n.Name == model.OldViewName);
                    View newView = context.Views
                        .Include(bo => bo.BusObject)
                            .ThenInclude(boc => boc.BusObjectComponents)
                                .ThenInclude(bc => bc.BusComp)
                        .FirstOrDefault(n => n.Name == model.NewViewName);
                    ScreenItem oldScreenItem = newScreen.ScreenItems.FirstOrDefault(n => n.ViewId == oldView.Id);
                    ScreenItem newScreenItem = newScreen.ScreenItems.FirstOrDefault(n => n.ViewId == newView.Id);
                    List<ScreenItem> crumbs = ComponentsRecordsInfo.GetCrumbs();

                    /* Если не произошел переход на представление того же уровня внутри того же родителя и выполняется одно из двух следующих условий:
                     * - уже есть хлебные крошки;
                       - у выбранного представления есть родитель, который не является последней хлебной крошкой.
                       Тогда текущее выбранное представление добавляется в хлебные крошки */
                    if (oldScreenItem.ParentItemId != newScreenItem.ParentItemId && (crumbs.Count > 0 || newScreenItem.ParentItemId != null))
                        ComponentsRecordsInfo.AppendCrumbs("SelectScreenItem", newScreen, oldScreenItem, newScreenItem);

                    // Обновление контекста в случае, если представление, на которое произошел переход уже есть в хлебных крошках и если для него уничтожен контекст
                    if (crumbs.FirstOrDefault(i => i.ViewId == newView.Id) != null && !ComponentsRecordsInfo.IsInitComponent(newView.BusObject.BusObjectComponents.FirstOrDefault()?.Name))
                        ComponentsRecordsInfo.RestoreContext(new List<View>() { newView });

                    screenInfo.Initialize(model.NewScreenName, model.NewViewName, context);
                    break;

                case "SelectScreen":
                    if (screenInfo.Screen.Name != model.NewScreenName)
                    {
                        // Добавление хлебной крошки
                        if (screenInfo.Crumbs?.Count > 0)
                        {
                            Screen oldScreen = context.Screens
                                .Include(si => si.ScreenItems)
                                    .ThenInclude(v => v.View)
                                .FirstOrDefault(n => n.Name == model.OldScreenName);
                            ComponentsRecordsInfo.AppendCrumb(oldScreen.ScreenItems.FirstOrDefault(n => n.View != null && n.View.Name == model.OldViewName));
                            screenInfo.Initialize(model.OldScreenName, model.OldViewName, context);
                        }

                        ComponentsRecordsInfo.Dispose();
                    }
                    break;

                case "SelectCrumb":
                    // Удаление хлебных крошек
                    ComponentsRecordsInfo.RemoveCrumbs(model.CrumbId);
                    List<ScreenItem> screenItems = ComponentsRecordsInfo.GetCrumbs();
                    List<View> viewsToResore = new List<View>();
                    // Добавление всех представлений, оставшихся в хлебных крошках в представления, контекст которых необходимо восстановить
                    screenItems.ForEach(crumb =>
                    {
                        viewsToResore.Add(context.Views
                            .Include(bo => bo.BusObject)
                                .ThenInclude(boc => boc.BusObjectComponents)
                                    .ThenInclude(bc => bc.BusComp)
                            .FirstOrDefault(i => i.Id == crumb.ViewId));
                    });
                    // Добавление представления, на которое перешли по хлебным крошкам в представления, контекст которых необходимо восстановить
                    viewsToResore.Add(context.Views
                        .Include(bo => bo.BusObject)
                            .ThenInclude(boc => boc.BusObjectComponents)
                                .ThenInclude(bc => bc.BusComp)
                        .FirstOrDefault(n => n.Name == model.NewViewName));
                    // При переходе не другой экран необходимо очистить информацию о выбранных и отображаемых записях
                    if (screenInfo.Screen.Name != model.NewScreenName)
                        ComponentsRecordsInfo.Dispose();
                    // Восстановление контекста
                    ComponentsRecordsInfo.RestoreContext(viewsToResore);
                    screenInfo.Initialize(model.NewScreenName, model.NewViewName, context);
                    break;
            }
            screenInfo.ActionType = (ActionType)Enum.Parse(typeof(ActionType), model.Action);
            screenInfoUI.Initialize(screenInfo, context);
            return Ok(screenInfoUI.Serialize());
        }
        #endregion
    }
}
