using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GSCrm.Services.Info
{
    public class ScreenInfo : IScreenInfo
    {
        private readonly IEntitiesInfo entitiesInfo;
        public ScreenInfo(IEntitiesInfo entitiesInfo)
        {
            this.entitiesInfo = entitiesInfo;
        }
        #region Properties
        public Screen Screen { get; set; }
        public List<ScreenItem> ScreenItems { get; set; }
        public List<ScreenItem> AggregateCategories { get; set; }
        public ScreenItem CurrentCategory { get; set; }
        public List<ScreenItem> CategoryAllViews { get; set; }
        public ScreenItem CurrentView { get; set; }
        public List<ScreenItem> CurrentViews { get; set; }
        public List<ScreenItem> ChildViews { get; set; }
        public ScreenItem ParentView { get; set; }
        public Action Action { get; set; }
        public Dictionary<string, string> Routing { get; set; }
        #endregion

        #region Methods
        public void Initialize(string screenName, string currentCategory, string currentView, ApplicationContext context)
        {
            ScreenItems = new List<ScreenItem>();
            AggregateCategories = new List<ScreenItem>();
            CategoryAllViews = new List<ScreenItem>();
            CurrentViews = new List<ScreenItem>();
            ChildViews = new List<ScreenItem>();
            Routing = new Dictionary<string, string>();

            // Screen
            Screen = context.Screens
                .Include(si => si.ScreenItems)
                    .ThenInclude(v => v.View)
                .FirstOrDefault(n => n.Name == screenName);

            // Все элементы скрина
            ScreenItems = Screen.ScreenItems.ToList();

            // Все категории в скрине
            AggregateCategories = ScreenItems
                .Where(t => t.Type == "Aggregate Category")
                .OrderBy(s => s.Sequence)
                .ToList();

            /* Если не было подано название текущей категории, значит происходит первая загрузка скрина
             * и надо взять первую категорию. */
            CurrentCategory = currentCategory == null ? AggregateCategories.FirstOrDefault() : AggregateCategories.FirstOrDefault(n => n.Name == currentCategory);

            // Все представления текущей категории
            CategoryAllViews = ScreenItems
                .Where(p => p.ParentCategory == CurrentCategory.Name)
                .ToList();

            // При первой загрузке скрина, когда отсутсвует текущее представление
            if (currentView == null)
            {
                // В таком случае текущим представлением становится первое представление в текущей категории, у которого нет родителей
                CurrentView = CategoryAllViews
                    .Where(p => p.ParentItem == null)
                    .OrderBy(s => s.Sequence)
                    .FirstOrDefault();
            }

            // Иначе текущее представление - то, что пришло с фронта
            else CurrentView = CategoryAllViews.FirstOrDefault(n => n.View.Name == currentView);

            // Дочерние представления
            ChildViews = CategoryAllViews
                .Where(p => p.ParentItem == CurrentView.Name)
                .OrderBy(s => s.Sequence)
                .ToList();

            // Родительское представление
            ParentView = CategoryAllViews.FirstOrDefault(n => n.Name == CurrentView.ParentItem);

            /* Представления, которые необходимо отобразить - сюда входят дочерние представления первого уровня и текущее представления
               На этапе разработки вывожу все представления
            if (ParentView != null)
                CurrentViews.Add(ParentView);
            CurrentViews.Add(CurrentView);
            if (ChildViews.Count != 0)
                CurrentViews.AddRange(ChildViews);*/
            CurrentViews.AddRange(CategoryAllViews);

            CategoryAllViews.ForEach(screenItem => Routing.Add(screenItem.View.Name, entitiesInfo.ViewRouting.GetValueOrDefault(screenItem.View.Name)));
        }
        #endregion
    }
}