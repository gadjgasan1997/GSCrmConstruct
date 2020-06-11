using System;
using GSCrmLibrary.Models.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class ViewItem : DataEntity
    {
        [ForeignKey("ViewId")]
        public View View { get; set; }
        public Guid ViewId { get; set; }
        [ForeignKey("AppletId")]
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        public bool Autofocus { get; set; }
        public int AutofocusRecord { get; set; }
    }
}
