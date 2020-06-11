using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSCrmLibrary.Models.RequestModels
{
    public class RequestAppletModel
    {
        public Guid? Id { get; set; }
        public string AppletName { get; set; }
        public bool RefreshCurrentApplet { get; set; } = false;
    }
}
