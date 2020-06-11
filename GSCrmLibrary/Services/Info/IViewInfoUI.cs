using System.Collections.Generic;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.AppletModels;

namespace GSCrmLibrary.Services.Info
{
    public interface IViewInfoUI
    {
        UIView View { get; set; }
        Dictionary<string, List<UIApplet>> Applets { get; set; }
        List<UIViewItem> ViewItems { get; set; }
        void Initialize<TContext>(TContext context) where TContext : MainContext, new();
        object Serialize();
    }
}
