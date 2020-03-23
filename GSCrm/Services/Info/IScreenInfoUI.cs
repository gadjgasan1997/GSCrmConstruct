using GSCrm.Data;
using GSCrm.Models.Default.AppletModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GSCrm.Services.Info
{
    public interface IScreenInfoUI
    {
        #region Properties
        string Name { get; set; }
        UIScreenItem CurrentView { get; set; }
        UIScreenItem CurrentCategory { get; set; }
        List<UIScreenItem> CategoryAllViews { get; set; }
        List<UIScreenItem> ChildViews { get; set; }
        Dictionary<string, string> Routing { get; set; }
        #endregion

        #region Methods
        void Initialize(ApplicationContext context);
        object Serialize();
        #endregion
    }
}
