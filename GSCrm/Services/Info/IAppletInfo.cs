using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using System.Collections.Generic;

namespace GSCrm.Services.Info
{
    public interface IAppletInfo
    {
        Applet Applet { get; set; }
        string Name { get; set; }
        string Type { get; set; }
        string Header { get; set; }
        int DisplayLines { get; set; }
        string EmptyState { get; set; }
        List<Control> Controls { get; set; }
        List<Column> Columns { get; set; }
        string PR { get; set; }
        void InitializeAppletInfo(string appletName, ApplicationContext context);
    }
}