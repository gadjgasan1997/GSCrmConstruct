using System;
using System.Collections.Generic;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Services.Info
{
    public interface IScreenInfo : ICloneable
    {
        #region Properties
        Screen Screen { get; set; }
        List<ScreenItem> AggregateCategories { get; set; }
        Dictionary<ScreenItem, List<ScreenItem>> AllCategoriesViews { get; set; }
        ScreenItem CurrentCategory { get; set; }
        List<ScreenItem> CurrentCategoryViews { get; set; }
        ScreenItem CurrentView { get; set; }
        List<ScreenItem> ChildViews { get; set; }
        ScreenItem ParentView { get; set; }
        ActionType ActionType { get; set; }
        List<ScreenItem> Crumbs { get; set; }
        #endregion

        #region Methods
        void Initialize<TContext>(string screenName, string currentView, TContext context) where TContext : MainContext, new();
        #endregion
    }
}
