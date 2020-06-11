using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Services.Info
{
    public class ScreenInfo : IScreenInfo
    {
        #region Properties
        public Screen Screen { get; set; }
        public List<ScreenItem> AggregateViews { get; set; }
        public List<ScreenItem> AggregateCategories { get; set; }
        public Dictionary<ScreenItem, List<ScreenItem>> AllCategoriesViews { get; set; }
        public ScreenItem CurrentCategory { get; set; }
        public List<ScreenItem> CurrentCategoryViews { get; set; }
        public ScreenItem CurrentView { get; set; }
        public List<ScreenItem> ChildViews { get; set; }
        public ScreenItem ParentView { get; set; }
        public ActionType ActionType { get; set; }
        public List<ScreenItem> Crumbs { get; set; }

        #endregion

        #region Methods
        public object Clone() => MemberwiseClone();
        public void Initialize<TContext>(string screenName, string currentView, TContext context)
            where TContext : MainContext, new()
        {
            AggregateViews = new List<ScreenItem>();
            AggregateCategories = new List<ScreenItem>();
            AllCategoriesViews = new Dictionary<ScreenItem, List<ScreenItem>>();
            CurrentCategoryViews = new List<ScreenItem>();
            ChildViews = new List<ScreenItem>();
            Crumbs = new List<ScreenItem>();

            Screen = context.Screens
                .AsNoTracking()
                .Include(si => si.ScreenItems)
                    .ThenInclude(v => v.View)
                .Include(si => si.ScreenItems)
                    .ThenInclude(p => p.ParentCategory)
                .FirstOrDefault(n => n.Name == screenName);

            List<ScreenItem> ScreenItems = Screen.ScreenItems.ToList();

            AggregateViews = ScreenItems.Where(t => t.Type == ScreenItemTypes.AggregateView.ToString()).OrderBy(s => s.Sequence).ToList();
            AggregateCategories = ScreenItems.Where(t => t.Type == ScreenItemTypes.AggregateCategory.ToString()).OrderBy(s => s.Sequence).ToList();
            AggregateCategories.ForEach(category =>
            {
                AllCategoriesViews.Add(category, Screen.ScreenItems.Where(c => c.ParentCategoryId == category.Id).ToList());
            });

            // При первой загрузке скрина, когда отсутсвует текущее представление
            if (currentView == null)
            {
                // В таком случае текущим представлением становится первое представление в текущей категории, у которого нет родителей
                CurrentView = AggregateViews.Where(p => p.ParentItemId == null).OrderBy(s => s.Sequence).FirstOrDefault();
            }

            // Иначе текущее представление - то, что пришло с фронта
            else CurrentView = AggregateViews.FirstOrDefault(n => n.View.Name == currentView);

            if (CurrentView?.ParentCategory != null)
            {
                /* Если не было подано название текущей категории, значит происходит первая загрузка скрина
                 * и надо взять первую категорию. */
                CurrentCategory = AggregateCategories.FirstOrDefault(i => i.Id == CurrentView.ParentCategoryId);

                // Все представления текущей категории
                CurrentCategoryViews = ScreenItems.Where(p => p.ParentCategoryId == CurrentCategory.Id).ToList();

                // Дочерние представления
                ChildViews = CurrentCategoryViews
                    .Where(p => p.ParentItemId == CurrentView.Id)
                    .OrderBy(s => s.Sequence).ToList();

                // Родительское представление
                ParentView = CurrentCategoryViews.FirstOrDefault(n => n.Id == CurrentView.ParentItemId);
            }

            // Хлебные крошки
            Crumbs = ComponentsRecordsInfo.GetCrumbs().ToList();
        }
        #endregion
    }
}