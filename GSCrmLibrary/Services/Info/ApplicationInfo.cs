using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Data;

namespace GSCrmLibrary.Services.Info
{
    public class ApplicationInfo : IApplicationInfo
    {
        #region Свойства
        public Application Application { get; set; }
        public List<Screen> Screens { get; set; }
        public Screen CurrentScreen { get; set; }
        public View CurrentView { get; set; }
        public Dictionary<string, string> ScreensRouting { get; set; }
        #endregion

        #region Методы
        public void Initialize<TContext>(string appName, string currentScreenName, string currentViewName, TContext context)
            where TContext : MainContext, new()
        {
            Screens = new List<Screen>();
            ScreensRouting = new Dictionary<string, string>();
            Application = context.Applications
                .AsNoTracking()
                .Include(ai => ai.ApplicationItems)
                .FirstOrDefault(n => n.Name == appName);
            Application.ApplicationItems.ForEach(item =>
            {
                Screen screen = context.Screens
                    .AsNoTracking()
                    .Include(si => si.ScreenItems)
                    .FirstOrDefault(n => n.Name == item.Name);
                if (screen != null)
                    Screens.Add(screen);
            });
            Screens = Screens.OrderBy(s => s.Sequence).ToList();
            CurrentScreen = string.IsNullOrWhiteSpace(currentScreenName) ? Screens.FirstOrDefault(n => n.Name == "Home Screen") : Screens.FirstOrDefault(n => n.Name == currentScreenName);

            ScreenItem currentScreenItem;
            if (CurrentScreen == null || CurrentScreen.ScreenItems == null)
                currentScreenItem = null;
            else currentScreenItem = CurrentScreen.ScreenItems.FirstOrDefault();
            Guid screenItemId = currentScreenItem == null ? Guid.Empty : currentScreenItem.Id;
            CurrentView = string.IsNullOrWhiteSpace(currentViewName) ? context.Views.FirstOrDefault(i => i.Id == screenItemId) : context.Views.FirstOrDefault(n => n.Name == currentViewName);
            Screens.ForEach(screen => ScreensRouting.Add(screen.Name, screen.Routing));
        }
        #endregion
    }
}
