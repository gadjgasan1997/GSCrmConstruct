using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.BUSUIFactories;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Models.AppletModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Services.Info
{
    public class ApplicationInfoUI : IApplicationInfoUI
    {
        private readonly IApplicationInfo applicationInfo;
        public ApplicationInfoUI(IApplicationInfo applicationInfo)
        {
            this.applicationInfo = applicationInfo;
        }

        #region Свойства
        [JsonPropertyName("Application")]
        public UIApplication Application { get; set; }
        [JsonPropertyName("Screens")]
        public List<UIScreen> Screens { get; set; }
        [JsonPropertyName("CurrentScreen")]
        public UIScreen CurrentScreen { get; set; }
        [JsonPropertyName("CurrentView")]
        public UIView CurrentView { get; set; }
        [JsonPropertyName("ScreensRouting")]
        public Dictionary<string, string> ScreensRouting { get; set; }
        #endregion

        #region Методы
        public void Initialize<TContext>(TContext context)
            where TContext : MainContext, new()
        {
            Screens = new List<UIScreen>();
            ScreensRouting = new Dictionary<string, string>();
            DataBUSApplicationFR<TContext> dataApplicationFR = new DataBUSApplicationFR<TContext>();
            BUSUIApplicationFR<TContext> UIApplicationFR = new BUSUIApplicationFR<TContext>();
            DataBUSScreenFR<TContext> dataScreenFR = new DataBUSScreenFR<TContext>();
            BUSUIScreenFR<TContext> UIScreenFR = new BUSUIScreenFR<TContext>();
            DataBUSViewFR<TContext> dataBUSViewFR = new DataBUSViewFR<TContext>();
            BUSUIViewFR<TContext> busUIViewFR = new BUSUIViewFR<TContext>();

            Application = UIApplicationFR.BusinessToUI(dataApplicationFR.DataToBusiness(applicationInfo.Application, context));
            applicationInfo.Screens.ForEach(screen =>
            {
                Screens.Add(UIScreenFR.BusinessToUI(dataScreenFR.DataToBusiness(screen, context)));
            });
            CurrentScreen = applicationInfo.CurrentScreen == null ? null : UIScreenFR.BusinessToUI(dataScreenFR.DataToBusiness(applicationInfo.CurrentScreen, context));
            CurrentView = applicationInfo.CurrentView == null ? null : busUIViewFR.BusinessToUI(dataBUSViewFR.DataToBusiness(applicationInfo.CurrentView, context));
            ScreensRouting = applicationInfo.ScreensRouting;
        }
        public object Serialize() => new JsonResult(this).Value;
        #endregion
    }
}
