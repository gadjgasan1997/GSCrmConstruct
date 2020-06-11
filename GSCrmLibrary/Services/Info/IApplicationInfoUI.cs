using GSCrmLibrary.Models.AppletModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;

namespace GSCrmLibrary.Services.Info
{
    public interface IApplicationInfoUI
    {
        UIApplication Application { get; set; }
        List<UIScreen> Screens { get; set; }
        UIScreen CurrentScreen { get; set; }
        UIView CurrentView { get; set; }
        Dictionary<string, string> ScreensRouting { get; set; }
        void Initialize<TContext>(TContext context) where TContext : MainContext, new();
        object Serialize() => new JsonResult(this).Value;
    }
}
