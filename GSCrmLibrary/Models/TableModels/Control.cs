using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.TableModels
{
    public class Control : DataEntity
    {
        [ForeignKey("AppletId")]
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        [ForeignKey("FieldId")]
        public Field Field { get; set; }
        public Guid? FieldId { get; set; }
        public ActionType ActionType { get; set; }
        [ForeignKey("IconId")]
        public Icon Icon { get; set; }
        public Guid? IconId { get; set; }
        public string CssClass { get; set; }
        public string Header { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public bool Readonly { get; set; }
        public List<ControlUP> ControlUPs { get; set; }
        public Control()
        {
            ControlUPs = new List<ControlUP>();
        }
    }
}
