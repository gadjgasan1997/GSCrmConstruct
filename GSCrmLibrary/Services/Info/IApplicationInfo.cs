using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using System.Collections.Generic;

namespace GSCrmLibrary.Services.Info
{
    public interface IApplicationInfo
    {
        Application Application { get; set; }
        List<Screen> Screens { get; set; }
        Screen CurrentScreen { get; set; }
        View CurrentView { get; set; }
        Dictionary<string, string> ScreensRouting { get; set; }
        void Initialize<TContext>(string appName, string currentScreenName, string currentViewName, TContext context) where TContext : MainContext, new();
    }
}
