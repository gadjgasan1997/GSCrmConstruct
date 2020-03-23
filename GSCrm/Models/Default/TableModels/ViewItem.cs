using System;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Элемент представления
    public class ViewItem : MainTable
    {
        // View
        [ForeignKey("ViewId")]
        public View View { get; set; }
        public Guid ViewId { get; set; }

        // Applet
        [ForeignKey("AppletId")]
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }

        // Props
        public bool Autofocus { get; set; }
        public int AutofocusRecord { get; set; }
    }
}
