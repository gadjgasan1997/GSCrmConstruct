using GSCrmLibrary.Models.MainEntities;
using System.Collections.Generic;

namespace GSCrmLibrary.Models.RequestModels
{
    public class RequestViewModel
    {
        public string ActionType { get; set; }
        public string TargetApplet { get; set; }
        public string CurrentRecord { get; set; }
        public string CurrentControl { get; set; }
        public string CurrentColumn { get; set; }
        public bool OpenPopup { get; set; } = false;
        public bool ClosePopup { get; set; } = false;
        public string PopupApplet { get; set; }
        public string CurrentPopupControl { get; set; }
    }
}
