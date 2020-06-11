using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.BUSUIFactories;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Services.Info
{
    public class ScreenInfoUI : IScreenInfoUI
    {
        #region Properties
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("AllCategories")]
        public Dictionary<string, UIScreenItem> AllCategories { get; set; }
        [JsonPropertyName("AllCategoriesViews")]
        public Dictionary<string, List<UIScreenItem>> AllCategoriesViews { get; set; }
        [JsonPropertyName("CurrentCategory")]
        public UIScreenItem CurrentCategory { get; set; }
        [JsonPropertyName("CurrentCategoryViews")]
        public List<UIScreenItem> CurrentCategoryViews { get; set; }
        [JsonPropertyName("CurrentView")]
        public UIScreenItem CurrentView { get; set; }
        [JsonPropertyName("ChildViews")]
        public List<UIScreenItem> ChildViews { get; set; }
        [JsonPropertyName("Crumbs")]
        public List<UIScreenItem> Crumbs { get; set; }
        #endregion

        #region Methods
        public object Clone() => MemberwiseClone();
        public void Initialize<TContext>(IScreenInfo screenInfo, TContext context)
            where TContext : MainContext, new()
        {
            AllCategories = new Dictionary<string, UIScreenItem>();
            AllCategoriesViews = new Dictionary<string, List<UIScreenItem>>();
            CurrentCategoryViews = new List<UIScreenItem>();
            ChildViews = new List<UIScreenItem>();
            Crumbs = new List<UIScreenItem>();
            DataBUSScreenItemFR<TContext> dataBUSScreenItemFR = new DataBUSScreenItemFR<TContext>();
            BUSUIScreenItemFR<TContext> busUIScreenItemFR = new BUSUIScreenItemFR<TContext>();

            Name = screenInfo.Screen.Name;
            foreach (ScreenItem category in screenInfo.AllCategoriesViews.Keys)
            {
                List<UIScreenItem> screenItems = new List<UIScreenItem>();
                screenInfo.AllCategoriesViews[category].ForEach(screenItem =>
                {
                    screenItems.Add(busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(screenItem, context)));
                });
                AllCategoriesViews.TryAdd(category.Header, screenItems);
            }

            screenInfo.AggregateCategories.ForEach(category =>
            {
                AllCategories.Add(category.Header, busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(category, context)));
            });

            if (screenInfo.CurrentCategory != null)
            {
                CurrentCategory = busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(screenInfo.CurrentCategory, context));
                screenInfo.CurrentCategoryViews.ForEach(view =>
                {
                    CurrentCategoryViews.Add(busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(view, context)));
                });
                CurrentView = screenInfo.CurrentView == null ? null : busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(screenInfo.CurrentView, context));
                screenInfo.ChildViews.ForEach(view =>
                {
                    ChildViews.Add(busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(view, context)));
                });
            }
            
            screenInfo.Crumbs.ForEach(crumb =>
            {
                Crumbs.Add(busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(crumb, context)));
            });
        }
        public object Serialize() => new JsonResult(this).Value;
        #endregion
    }
}
