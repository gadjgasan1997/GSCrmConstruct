using GSCrm.Models.Default.AppletModels;
using System.Collections.Generic;
using GSCrm.Data;
using GSCrm.Factories.Default.DataBUSFactories;
using GSCrm.Factories.Default.BUSUIFactories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace GSCrm.Services.Info
{
    public class ViewInfoUI : IViewInfoUI
    {
        private readonly IViewInfo viewInfo;
        public ViewInfoUI(IViewInfo viewInfo)
        {
            this.viewInfo = viewInfo;
        }
        [JsonPropertyName("View")]
        public UIView View { get; set; }
        [JsonPropertyName("Applets")]
        public Dictionary<string, List<UIApplet>> Applets { get; set; }
        [JsonPropertyName("ViewItems")]
        public List<UIViewItem> ViewItems { get; set; }
        [JsonPropertyName("Routing")]
        public Dictionary<string, string> Routing { get; set; }
        public void Initialize(ApplicationContext context)
        {
            // Инициализация списков
            Applets = new Dictionary<string, List<UIApplet>>()
            {
                { "Tile", new List<UIApplet>() },
                { "Form", new List<UIApplet>() },
                { "Popup", new List<UIApplet>() }
            };
            ViewItems = new List<UIViewItem>();
            Routing = new Dictionary<string, string>();

            // Получение фабрик
            DataBUSViewFR dataBUSViewFactory = new DataBUSViewFR();
            BUSUIViewFR busUIViewFactory = new BUSUIViewFR();
            DataBUSAppletFR dataBUSAppletFactory = new DataBUSAppletFR();
            BUSUIAppletFR busUIAppletFactory = new BUSUIAppletFR();
            DataBUSViewItemFR dataBUSViewItemFactory = new DataBUSViewItemFR();
            BUSUIViewItemFR busUIViewItemFactory = new BUSUIViewItemFR();

            // Маппинг в UI уровень представления
            View = busUIViewFactory.BusinessToUI(dataBUSViewFactory.DataToBusiness(viewInfo.View, context));
            viewInfo.ViewApplets.ForEach(applet =>
            {
                Applets
                    .GetValueOrDefault(applet.Type)
                    .Add(busUIAppletFactory.BusinessToUI(dataBUSAppletFactory.DataToBusiness(applet, context)));
            });

            // Маппинг в UI уровень элементов представления
            viewInfo.ViewItems.ForEach(viewItem =>
            {
                ViewItems.Add(busUIViewItemFactory.BusinessToUI(dataBUSViewItemFactory.DataToBusiness(viewItem, context)));
            });
            Routing = viewInfo.Routing;
        }
        public object Serialize()
        {
            return new JsonResult(this).Value;
        }
    }
}
