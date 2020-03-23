using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Data.Context;

namespace GSCrm.Services.Info
{
    public class ViewInfo : IViewInfo
    {
        private readonly IEntitiesInfo entitiesInfo;
        public ViewInfo(IEntitiesInfo entitiesInfo)
        {
            this.entitiesInfo = entitiesInfo;
        }

        #region Properties
        public View View { get; set; }
        public BusObject ViewBO { get; set; }
        public List<BusComp> ViewBCs { get; set; }
        public List<BOComponent> BOComponents { get; set; }
        public List<Applet> ViewApplets { get; set; }
        public List<ViewItem> ViewItems { get; set; }
        public Applet CurrentPopupApplet { get; set; }
        public Applet CurrentApplet { get; set; }
        public Control CurrentControl { get; set; }
        public Control CurrentPopupControl { get; set; }
        public Action Action { get; set; }
        public string CurrentRecord { get; set; }
        public Dictionary<string, string> Routing { get; set; }
        #endregion

        #region Methods
        public void Initialize(string viewName, ApplicationContext context)
        {
            // Инициализация списков
            ViewBCs = new List<BusComp>();
            BOComponents = new List<BOComponent>();
            ViewApplets = new List<Applet>();
            ViewItems = new List<ViewItem>();
            Routing = new Dictionary<string, string>();
            Action = context.Actions.FirstOrDefault(n => n.Name == "InitializeView");

            // Получение представления
            View = context.Views
                .Include(vi => vi.ViewItems)
                .FirstOrDefault(n => n.Name == viewName);

            // Получение списка элементов представления
            ViewItems = View.ViewItems.OrderBy(s => s.Sequence).ToList();

            // Получение бизнес объекта, на котором основано представление
            ViewBO = context.BusinessObjects
                .Include(boc => boc.BusObjectComponents)
                    .ThenInclude(link => link.Link)
                .FirstOrDefault(i => i.Id == View.BusObjectId);

            // Заполнение списков с апплетами, бизнес компонентами и маршрутизацией
            ViewItems.ForEach(viewItem =>
            {
                // Получение апплета для каждого элемента представления
                Applet applet = context.Applets
                    .Include(col => col.Columns)
                    .Include(cntr => cntr.Controls)
                        .ThenInclude(cnUp => cnUp.ControlUPs)
                    .Include(cntr => cntr.Controls)
                        .ThenInclude(a => a.Action)
                    .FirstOrDefault(i => i.Id == viewItem.AppletId);

                // Поулчение бизнес компоненты для каждого элемента представления
                BusComp busComp = context.BusinessComponents
                    .Include(t => t.Table)
                    .Include(field => field.Fields)
                    .FirstOrDefault(i => i.Id == applet.BusCompId);

                BOComponent component = ViewBO.BusObjectComponents.FirstOrDefault(i => i.BusCompId == busComp.Id);

                // Добавление апплета, его бизнес компоненты и маршрута в список
                if (ViewApplets.IndexOf(applet) == -1)
                    ViewApplets.Add(applet);
                if (Routing.GetValueOrDefault(applet.Name) == null)
                    Routing.Add(applet.Name, entitiesInfo.AppletRouting.GetValueOrDefault(applet.Name));
                if (ViewBCs.IndexOf(busComp) == -1)
                    ViewBCs.Add(busComp);
                if (BOComponents.IndexOf(component) == -1)
                    BOComponents.Add(component);
            });

            CurrentApplet = ViewApplets.FirstOrDefault();

            /* Пока что вызов метода заккоменчен, так как придумано следующее решение: Перед обновлением записей из списка старых
             * проверяется, есть ли текущая выбранные запись среди них. До этого была проблема, описанная ниже.
             * Для всех бизнес компонент, которые отсутсвуют в представлении необходимо очищать текущие отображаемые записи.
             * Иначе может возникнуть следующий баг. Например, есть 3 представления: BusComp View, Join View, JoinSpec View
             * (с бк: BusComp, BusComp и Join, Join и JoinSpec).
             * Если выбрать запись на BusComp View, перейти затем на Join View и вернуться обратно, 
             * то для бк Join запишутся отображаемые записи.
             * Если же затем, миную Join View, сразу перейти на JoinSpec View, то, при инициализации представления, записи в бк Join
             * будут заменены прежними, не относящимеся к новой выбранной бизнес компоненте в BusComp View, что приведет к ошибкам. 
             * Если при инициализации представления убрать замену отображаемых записей на старые сохраненные записи,
             * то при переходе с BusComp View на Join View, бк BusComp не будет пролистываться до выбранной записи.
             * Возможно есть лучшее решение, но пока что так
            */

            // ComponentContext.ClearOldRecords(ViewBCs);
        }
        public void AddPopupApplet(ApplicationContext context)
        {
            // Получение названия попапа из up контрола, на котором произошло действие
            ControlUP UP = CurrentControl?.ControlUPs?.FirstOrDefault(n => n.Name == "Applet");
            string popupName = UP == null ? string.Empty : UP.Value;
            Applet popup = context.Applets
                .Include(col => col.Columns)
                .Include(cntr => cntr.Controls)
                    .ThenInclude(cntrUp => cntrUp.ControlUPs)
                .Include(cntr => cntr.Controls)
                    .ThenInclude(a => a.Action)
                .FirstOrDefault(n => n.Name == popupName);

            // Установка этого попапа как текущего
            CurrentPopupApplet = popup;

            // Добавление попапа и его маршрута в информацию о представлении
            ViewApplets.Add(popup);
            ViewItem viewItem = new ViewItem()
            {
                Name = popup.Name,
                Applet = popup,
                AppletId = popup.Id,
                Autofocus = true,
                AutofocusRecord = 0,
                View = View,
                ViewId = View.Id
            };
            ViewItems.Add(viewItem);
            View.ViewItems.Add(viewItem);
            if (Routing.GetValueOrDefault(popup.Name) == null)
                Routing.Add(popup.Name, entitiesInfo.AppletRouting.GetValueOrDefault(popup.Name));
        }
        public void RemovePopupApplet(ApplicationContext context)
        {
            // Удаление попапа из списка аппетов представления и из информации о представлении
            ViewItem viewItem = View.ViewItems.FirstOrDefault(ap => ap.AppletId == CurrentPopupApplet.Id);
            Applet applet = ViewApplets.FirstOrDefault(i => i.Id == viewItem.AppletId);
            ViewItems.Remove(viewItem);
            View.ViewItems.Remove(viewItem);
            ViewApplets.Remove(applet);
            Routing.Remove(applet.Name);
            CurrentPopupApplet = null;
            CurrentPopupControl = null;
        }
        public void Dispose()
        {
            View = null;
            ViewBO = null;
            ViewBCs = new List<BusComp>();
            BOComponents = new List<BOComponent>();
            ViewApplets = new List<Applet>();
            ViewItems = new List<ViewItem>();
            CurrentPopupApplet = null;
            CurrentApplet = null;
            CurrentControl = null;
            CurrentPopupControl = null;
            CurrentRecord = null;
            Routing = null;
        }
        #endregion
    }
}
