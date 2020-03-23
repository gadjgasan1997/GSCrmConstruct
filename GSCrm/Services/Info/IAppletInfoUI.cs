using GSCrm.Data;
using GSCrm.Models.Default.AppletModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GSCrm.Services.Info
{
    public interface IAppletInfoUI
    {
        string Name { get; set; }
        string Type { get; set; }
        string Header { get; set; }
        int DisplayLines { get; set; }
        string EmptyState { get; set; }
        List<UIControl> Controls { get; set; }
        Dictionary<string, List<UIControlUP>> ControlUPs { get; set; }
        List<UIColumn> Columns { get; set; }
        string PR { get; set; }
        string Path { get; set; }
        object Serialize(ApplicationContext context);
    }
}
