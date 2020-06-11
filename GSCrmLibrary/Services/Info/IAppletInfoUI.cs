using GSCrmLibrary.Data;
using GSCrmLibrary.Models.AppletModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GSCrmLibrary.Services.Info
{
    public interface IAppletInfoUI
    {
        string Name { get; set; }
        string BusCompName { get; set; }
        string Type { get; set; }
        string Header { get; set; }
        int DisplayLines { get; set; }
        string EmptyState { get; set; }
        List<UIControl> Controls { get; set; }
        Dictionary<string, List<UIControlUP>> ControlUPs { get; set; }
        List<UIColumn> Columns { get; set; }
        string PR { get; set; }
        string Routing { get; set; }
        string BusCompRouting { get; set; }
        bool Initflag { get; set; }
        void Initialize<TContext>(TContext context) where TContext : MainContext, new();
        object Serialize();
    }
}
