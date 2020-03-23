using System.Collections.Generic;

namespace GSCrm.Models.Default.RequestModels
{
    public class RequestViewModel
    {
        // Текущее представление
        public string ViewName { get; set; }

        // Текущий апплет
        public string CurrentApplet { get; set; }
        public string CurrentRecord { get; set; }
        public string CurrentControl { get; set; }
        public string Action { get; set; }

        // Текущий попап
        public bool OpenPopup { get; set; } = false;
        public bool ClosePopup { get; set; } = false;
        public string CurrentPopupApplet { get; set; }
        public string CurrentPopupControl { get; set; }
    }
}
