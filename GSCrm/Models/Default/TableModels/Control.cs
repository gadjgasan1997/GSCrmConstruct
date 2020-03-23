using GSCrm.Models.Default.MainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Контрол апплета
    public class Control : MainTable
    {
        // Applet
        [ForeignKey("AppletId")]
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }

        // Field
        [ForeignKey("FieldId")]
        public Field Field { get; set; }
        public Guid? FieldId { get; set; }

        // Action
        [ForeignKey("ActionId")]
        public Action Action { get; set; }
        public Guid? ActionId { get; set; }

        // Icon
        [ForeignKey("IconId")]
        public Icon Icon { get; set; }
        public Guid? IconId { get; set; }

        // Props
        public string Header { get; set; }
        public string CssClass { get; set; }
        public List<ControlUP> ControlUPs { get; set; }
        public Control()
        {
            ControlUPs = new List<ControlUP>();
        }
    }
}
