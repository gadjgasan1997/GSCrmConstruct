using System.Collections.Generic;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Services.Info
{
    public interface IViewInfo
    {
        #region Properties
        View View { get; set; }
        BusinessObject ViewBO { get; set; }
        List<BusinessComponent> ViewBCs { get; set; }
        List<BusinessObjectComponent> BOComponents { get; set; }
        List<Applet> ViewApplets { get; set; }
        List<Applet> AppletsSortedByLinks { get; set; }
        List<ViewItem> ViewItems { get; set; }
        Applet CurrentPopupApplet { get; set; }
        Applet CurrentApplet { get; set; }
        Control CurrentControl { get; set; }
        Column CurrentColumn { get; set; }
        Control CurrentPopupControl { get; set; }
        ActionType ActionType { get; set; }
        string CurrentRecord { get; set; }
        #endregion

        #region Methods
        void EndInitialize();
        void StartInitialize<TContext>(IViewInfo viewInfo, string viewName, TContext context) where TContext : MainContext, new();
        void AddPopupApplet<TContext>(TContext context) where TContext : MainContext, new();
        void RemovePopupApplet<TContext>(TContext context) where TContext : MainContext, new();
        void Dispose();
        #endregion
    }
}
