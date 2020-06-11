using System;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSViewItem : BUSEntity
    {
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        public string AppletName { get; set; }
        public View View { get; set; }
        public Guid ViewId { get; set; }
        public bool Autofocus { get; set; }
        public int AutofocusRecord { get; set; }
    }
}
