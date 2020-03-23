using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSCrm.Models.Default.RequestModels
{
    public class RequestAppletModel
    {
        // Id
        public Guid? Id { get; set; }
        // Name of applet
        public string AppletName { get; set; }
        // Selected records in all view applets
        public Dictionary<string, string> SelectedRecords { get; set; }
        // Type of occured operation
        public string Operation { get; set; }
        // Count of display lines in applet
        public int DisplayLines { get; set; }
        // Sign indicating the need to update current applet
        public bool RefreshCurrentApplet { get; set; } = false;
    }
}
