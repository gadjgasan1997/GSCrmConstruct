using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using System.Collections.Generic;

namespace GSCrmLibrary.Services.Info
{
    public interface IAppletInfo
    {
        Applet Applet { get; set; }
        string Name { get; set; }
        string BusCompName { get; set; }
        string Type { get; set; }
        string Header { get; set; }
        int DisplayLines { get; set; }
        string EmptyState { get; set; }
        List<Control> Controls { get; set; }
        List<Column> Columns { get; set; }
        string PR { get; set; }
        string BusCompRouting { get; set; }
        bool Initflag { get; set; }
        string Routing { get; set; }
        void Initialize<TContext>(string appletName, TContext context) where TContext : MainContext, new();
    }
}