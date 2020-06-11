using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using static GSCrmLibrary.Data.ComponentsRecordsInfo;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Services.Info
{
    public class ViewInfo : IViewInfo
    {
        #region Properties
        public View View { get; set; }
        public BusinessObject ViewBO { get; set; }
        public List<BusinessComponent> ViewBCs { get; set; }
        public List<BusinessObjectComponent> BOComponents { get; set; }
        public List<Applet> ViewApplets { get; set; }
        public List<Applet> AppletsSortedByLinks { get; set; }
        public List<ViewItem> ViewItems { get; set; }
        public Applet CurrentPopupApplet { get; set; }
        public Applet CurrentApplet { get; set; }
        public Control CurrentControl { get; set; }
        public Column CurrentColumn { get; set; }
        public Control CurrentPopupControl { get; set; }
        public ActionType ActionType { get; set; }
        public string CurrentRecord { get; set; }
        #endregion

        #region Methods
        public void StartInitialize<TContext>(IViewInfo oldViewInfo, string viewName, TContext context)
            where TContext : MainContext, new()
        {
            // Инициализация списков
            ViewBCs = new List<BusinessComponent>();
            ViewApplets = new List<Applet>();
            AppletsSortedByLinks = oldViewInfo == null ? new List<Applet>() : oldViewInfo.AppletsSortedByLinks;
            ViewItems = new List<ViewItem>();
            BOComponents = new List<BusinessObjectComponent>();

            if (ActionType != ActionType.Drilldown)
                ActionType = ActionType.InitializeView;

            // Получение представления
            View = context.Views
                .AsNoTracking()
                .Select(v => new
                {
                    id = v.Id,
                    name = v.Name,
                    busObjectId = v.BusObjectId,
                    viewItems = v.ViewItems.Select(vi => new
                    {
                        id = vi.Id,
                        name = vi.Name,
                        sequence = vi.Sequence,
                        appletId = vi.AppletId,
                        applet = vi.Applet == null ? null : new
                        {
                            id = vi.Applet.Id,
                            name =  vi.Applet.Name,
                            busCompId = vi.Applet.BusCompId
                        }
                    })
                })
                .Select(v => new View
                {
                    Id = v.id,
                    Name = v.name,
                    BusObjectId = v.busObjectId,
                    ViewItems = v.viewItems.Select(vi => new ViewItem
                    {
                        Id = vi.id,
                        Name = vi.name,
                        Sequence = vi.sequence,
                        AppletId = vi.appletId,
                        Applet = vi.applet == null ? null : new Applet
                        {
                            Id = vi.applet.id,
                            Name = vi.applet.name,
                            BusCompId = vi.applet.busCompId
                        }
                    }).ToList()
                })
                .FirstOrDefault(n => n.Name == viewName);

            // Получение списка элементов представления
            ViewItems = View.ViewItems.OrderBy(s => s.Sequence).ToList();

            // Получение бизнес объекта, на котором основано представление
            ViewBO = context.BusinessObjects
                .AsNoTracking()
                .Select(bo => new
                {
                    id = bo.Id,
                    name = bo.Name,
                    boComponents = bo.BusObjectComponents.Select(boc => new
                    {
                        id = boc.Id,
                        name = boc.Name,
                        busCompId = boc.BusCompId,
                        busComp = boc.BusComp == null ? null : new
                        {
                            id = boc.BusComp.Id,
                            name = boc.BusComp.Name
                        },
                        linkId = boc.LinkId,
                        link = boc.Link == null ? null : new
                        {
                            id = boc.Link.Id,
                            name = boc.Link.Name,
                            parentBCId = boc.Link.ParentBCId,
                            parentFieldId = boc.Link.ParentFieldId,
                            childBCId = boc.Link.ChildBCId,
                            childFieldId = boc.Link.ChildFieldId
                        }
                    })
                })
                .Select(bo => new BusinessObject
                {
                    Id = bo.id,
                    Name = bo.name,
                    BusObjectComponents = bo.boComponents.Select(boc => new BusinessObjectComponent
                    {
                        Id = boc.id,
                        Name = boc.name,
                        BusCompId = boc.busCompId,
                        BusComp = boc.busComp == null ? null : new BusinessComponent
                        {
                            Id = boc.busComp.id,
                            Name = boc.busComp.name
                        },
                        LinkId = boc.linkId,
                        Link = boc.link == null ? null : new Link
                        {
                            Id = boc.link.id,
                            Name = boc.link.name,
                            ParentBCId = boc.link.parentBCId,
                            ParentFieldId = boc.link.parentFieldId,
                            ChildBCId = boc.link.childBCId,
                            ChildFieldId = boc.link.childFieldId
                        }
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == View.BusObjectId);

            // Заполнение списков с апплетами, бизнес компонентами и маршрутизацией
            ViewItems.ForEach(viewItem =>
            {
                // Получение апплета для каждого элемента представления
                Applet applet = context.Applets
                    .AsNoTracking()
                    .Include(b => b.BusComp)
                    .Include(col => col.Columns)
                        .ThenInclude(f => f.Field)
                    .Include(cntr => cntr.Controls)
                        .ThenInclude(cnUp => cnUp.ControlUPs)
                    .Include(cntr => cntr.Controls)
                        .ThenInclude(f => f.Field)
                    .FirstOrDefault(i => i.Id == viewItem.AppletId);

                // Добавление апплета и его бизнес компоненты в список
                if (ViewApplets.IndexOf(applet) == -1)
                    ViewApplets.Add(applet);

                if (applet.BusComp != null)
                {
                    BusinessObjectComponent component = ViewBO.BusObjectComponents.FirstOrDefault(i => i.BusCompId == applet.BusCompId);
                    if (ViewBCs.IndexOf(applet.BusComp) == -1)
                        ViewBCs.Add(applet.BusComp);
                    if (BOComponents.IndexOf(component) == -1)
                        BOComponents.Add(component);
                }
            });

            CurrentApplet = ViewApplets.FirstOrDefault();
            CurrentRecord = GetSelectedRecord(ViewBCs.FirstOrDefault(i => i.Id == CurrentApplet?.BusCompId)?.Name);

            /*BOComponents.ForEach(objectComponent =>
            {
                objectComponent.SearchSpecArgs = GetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecArgs);
                objectComponent.SearchSpecification = GetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecification);
                objectComponent.SearchSpecificationByParent = GetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecificationByParent);
            });*/
        }
        public void AddPopupApplet<TContext>(TContext context)
            where TContext : MainContext, new()
        {
            // Получение названия попапа из up контрола, на котором произошло действие
            ControlUP UP = CurrentControl?.ControlUPs?.FirstOrDefault(n => n.Name == "Applet");
            string popupName = UP == null ? string.Empty : UP.Value;
            Applet popup = context.Applets
                .AsNoTracking()
                .Include(b => b.BusComp)
                .Include(cntr => cntr.Controls)
                    .ThenInclude(cntrUp => cntrUp.ControlUPs)
                .Include(cntr => cntr.Controls)
                    .ThenInclude(f => f.Field)
                .FirstOrDefault(n => n.Name == popupName);

            // Добавление попапа и его маршрута в информацию о представлении
            if (!ViewApplets.Select(n => n.Name).ToList().Contains(popup.Name))
            {
                // Установка этого попапа как текущего
                CurrentPopupApplet = popup;
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
            }
        }
        public void RemovePopupApplet<TContext>(TContext context)
            where TContext : MainContext, new()
        {
            // Удаление попапа из списка аппетов представления и из информации о представлении
            ViewItem viewItem = View.ViewItems.FirstOrDefault(ap => ap.AppletId == CurrentPopupApplet.Id);
            Applet applet = ViewApplets.FirstOrDefault(i => i.Id == viewItem.AppletId);
            ViewItems.Remove(viewItem);
            View.ViewItems.Remove(viewItem);
            ViewApplets.Remove(applet);
            CurrentPopupApplet = null;
            CurrentPopupControl = null;
        }
        public void Dispose()
        {
            View = null;
            ViewBO = null;
            ViewBCs = new List<BusinessComponent>();
            BOComponents = new List<BusinessObjectComponent>();
            ViewApplets = new List<Applet>();
            ViewItems = new List<ViewItem>();
            CurrentPopupApplet = null;
            CurrentApplet = null;
            CurrentControl = null;
            CurrentPopupControl = null;
            CurrentRecord = null;
        }
        public void EndInitialize() => ActionType = ActionType.None;
        #endregion
    }
}
