using System;
using System.Collections.Generic;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.AppletModels;

namespace GSCrmLibrary.Services.Info
{
    public interface IScreenInfoUI : ICloneable
    {
        #region Properties
        string Name { get; set; }
        Dictionary<string, UIScreenItem> AllCategories { get; set; }
        Dictionary<string, List<UIScreenItem>> AllCategoriesViews { get; set; }
        UIScreenItem CurrentCategory { get; set; }
        List<UIScreenItem> CurrentCategoryViews { get; set; }
        UIScreenItem CurrentView { get; set; }
        List<UIScreenItem> ChildViews { get; set; }
        List<UIScreenItem> Crumbs { get; set; }
        #endregion

        #region Methods
        void Initialize<TContext>(IScreenInfo screenInfo, TContext context) where TContext : MainContext, new();
        object Serialize();
        #endregion
    }
}
