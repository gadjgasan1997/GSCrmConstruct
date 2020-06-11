using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using System.Collections.Generic;
using System.Linq;

namespace GSCrmLibrary.Services.Info
{
    public class AppletInfo : IAppletInfo
    {
        private readonly IViewInfo viewInfo;
        public AppletInfo(IViewInfo viewInfo)
        {
            this.viewInfo = viewInfo;
        }
        public Applet Applet { get; set; }
        public string Name { get; set; }
        public string BusCompName { get; set; }
        public string Type { get; set; }
        public string Header { get; set; }
        public int DisplayLines { get; set; }
        public string EmptyState { get; set; }
        public List<Control> Controls { get; set; } = new List<Control>();
        public List<Column> Columns { get; set; } = new List<Column>();
        public string PR { get; set; }
        public string Routing { get; set; }
        public string BusCompRouting { get; set; }
        public bool Initflag { get; set; }
        public void Initialize<TContext>(string appletName, TContext context)
            where TContext : MainContext, new()
        {
            Applet = viewInfo.ViewApplets.FirstOrDefault(n => n.Name == appletName);
            BusCompName = Applet.BusComp?.Name;
            Name = Applet.Name;
            Type = Applet.Type;
            Header = Applet.Header;
            DisplayLines = Applet.DisplayLines;
            EmptyState = Applet.EmptyState;
            Controls = Applet.Controls;
            Columns = Applet.Columns;
            PR = context.PhysicalRenders.FirstOrDefault(i => i.Id == Applet.PhysicalRenderId).Name;
            BusCompRouting = Applet.BusComp?.Routing;
            Routing = Applet.Routing;
            Initflag = Applet.Initflag;
        }
    }
}
