using GSCrmLibrary.Models.AppletModels;
using System.Collections.Generic;
using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Factories.BUSUIFactories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Services.Info
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
        public void Initialize<TContext>(TContext context)
            where TContext : MainContext, new()
        {
            // Инициализация списков
            Applets = new Dictionary<string, List<UIApplet>>()
            {
                { "Tile", new List<UIApplet>() },
                { "Form", new List<UIApplet>() },
                { "Popup", new List<UIApplet>() }
            };
            ViewItems = new List<UIViewItem>();

            // Получение фабрик
            DataBUSViewFR<TContext> dataBUSViewFactory = new DataBUSViewFR<TContext>();
            BUSUIViewFR<TContext> busUIViewFactory = new BUSUIViewFR<TContext>();
            DataBUSAppletFR<TContext> dataBUSAppletFactory = new DataBUSAppletFR<TContext>();
            BUSUIAppletFR<TContext> busUIAppletFactory = new BUSUIAppletFR<TContext>();
            DataBUSViewItemFR<TContext> dataBUSViewItemFactory = new DataBUSViewItemFR<TContext>();
            BUSUIViewItemFR<TContext> busUIViewItemFactory = new BUSUIViewItemFR<TContext>();

            // Маппинг в UI уровень представления
            View = busUIViewFactory.BusinessToUI(dataBUSViewFactory.DataToBusiness(viewInfo.View, context));
            viewInfo.ViewApplets.ForEach(applet =>
            {
                Applets.GetValueOrDefault(applet.Type)
                    .Add(busUIAppletFactory.BusinessToUI(dataBUSAppletFactory.DataToBusiness(applet, context)));
            });

            // Маппинг в UI уровень элементов представления
            viewInfo.ViewItems.ForEach(viewItem =>
            {
                ViewItems.Add(busUIViewItemFactory.BusinessToUI(dataBUSViewItemFactory.DataToBusiness(viewItem, context)));
            });
        }
        public object Serialize() => new JsonResult(this).Value;
    }
}
