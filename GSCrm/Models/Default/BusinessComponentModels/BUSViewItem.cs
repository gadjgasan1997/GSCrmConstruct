using System;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSViewItem : MainBusinessComponent
    {
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        public string AppletName { get; set; }
        public View View { get; set; }
        public Guid ViewId { get; set; }
        public bool Autofocus { get; set; }
        public int AutofocusRecord { get; set; }
        public BUSViewItem() { }
    }
}
