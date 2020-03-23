using GSCrm.Data;
using GSCrm.Factories.Default.BUSUIFactories;
using GSCrm.Factories.Default.DataBUSFactories;
using GSCrm.Models.Default.AppletModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GSCrm.Services.Info
{
    public class ScreenInfoUI : IScreenInfoUI
    {
        private readonly IScreenInfo screenInfo;
        public ScreenInfoUI(IScreenInfo screenInfo)
        {
            this.screenInfo = screenInfo;
        }
        #region Properties
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("CurrentView")]
        public UIScreenItem CurrentView { get; set; }
        [JsonPropertyName("CurrentCategory")]
        public UIScreenItem CurrentCategory { get; set; }
        [JsonPropertyName("CategoryAllViews")]
        public List<UIScreenItem> CategoryAllViews { get; set; }
        [JsonPropertyName("CurrentViews")]
        public List<UIScreenItem> CurrentViews { get; set; }
        [JsonPropertyName("ChildViews")]
        public List<UIScreenItem> ChildViews { get; set; }
        [JsonPropertyName("Routing")]
        public Dictionary<string, string> Routing { get; set; }
        #endregion

        #region Methods
        public void Initialize(ApplicationContext context)
        {
            CategoryAllViews = new List<UIScreenItem>();
            CurrentViews = new List<UIScreenItem>();
            ChildViews = new List<UIScreenItem>();
            DataBUSScreenItemFR dataBUSScreenItemFR = new DataBUSScreenItemFR();
            BUSUIScreenItemFR busUIScreenItemFR = new BUSUIScreenItemFR();

            // Screen
            Name = screenInfo.Screen.Name;

            // Current View
            CurrentView = busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(screenInfo.CurrentView, context));

            // Current Aggregate Category
            CurrentCategory = busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(screenInfo.CurrentCategory, context));

            // AggregateViews
            screenInfo.CategoryAllViews.ForEach(view =>
            {
                CategoryAllViews.Add(busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(view, context)));
            });

            // CurrentViews
            screenInfo.CurrentViews.ForEach(view =>
            {
                CurrentViews.Add(busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(view, context)));
            });

            // ChildViews
            screenInfo.ChildViews.ForEach(view =>
            {
                ChildViews.Add(busUIScreenItemFR.BusinessToUI(dataBUSScreenItemFR.DataToBusiness(view, context)));
            });

            // Routing
            Routing = screenInfo.Routing;
        }
        public object Serialize()
        {
            return new JsonResult(this).Value;
        }
        #endregion
    }
}
