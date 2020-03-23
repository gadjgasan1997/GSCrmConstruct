using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using System.Collections.Generic;

namespace GSCrm.Services.Info
{
    public interface IScreenInfo
    {
        #region Properties
        Screen Screen { get; set; }
        List<ScreenItem> ScreenItems { get; set; }
        List<ScreenItem> AggregateCategories { get; set; }
        ScreenItem CurrentCategory { get; set; }
        List<ScreenItem> CategoryAllViews { get; set; }
        ScreenItem CurrentView { get; set; }
        public List<ScreenItem> CurrentViews { get; set; }
        List<ScreenItem> ChildViews { get; set; }
        public ScreenItem ParentView { get; set; }
        Action Action { get; set; }
        Dictionary<string, string> Routing { get; set; }
        #endregion

        #region Methods
        void Initialize(string screenName, string currentCategory, string currentView, ApplicationContext context);
        #endregion
    }
}
