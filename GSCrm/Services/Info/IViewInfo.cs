using System.Collections.Generic;
using GSCrm.Data;
using GSCrm.Models.Default.TableModels;

namespace GSCrm.Services.Info
{
    public interface IViewInfo
    {
        #region Properties
        View View { get; set; }
        BusObject ViewBO { get; set; }
        List<BusComp> ViewBCs { get; set; }
        List<BOComponent> BOComponents { get; set; }
        List<Applet> ViewApplets { get; set; }
        List<ViewItem> ViewItems { get; set; }
        Applet CurrentPopupApplet { get; set; }
        Applet CurrentApplet { get; set; }
        Control CurrentControl { get; set; }
        Control CurrentPopupControl { get; set; }
        Action Action { get; set; }
        string CurrentRecord { get; set; }
        Dictionary<string, string> Routing { get; set; }
        #endregion

        #region Methods
        void Initialize(string viewName, ApplicationContext context);
        void AddPopupApplet(ApplicationContext context);
        void RemovePopupApplet(ApplicationContext context);
        void Dispose();
        #endregion
    }
}
