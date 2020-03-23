using System.Collections.Generic;
using GSCrm.Data;
using GSCrm.Models.Default.AppletModels;

namespace GSCrm.Services.Info
{
    public interface IViewInfoUI
    {
        UIView View { get; set; }
        Dictionary<string, List<UIApplet>> Applets { get; set; }
        List<UIViewItem> ViewItems { get; set; }
        Dictionary<string, string> Routing { get; set; }
        void Initialize(ApplicationContext context);
        object Serialize();
    }
}
