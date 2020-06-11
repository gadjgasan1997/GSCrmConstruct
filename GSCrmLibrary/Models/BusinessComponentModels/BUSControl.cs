using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;
using System;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSControl : BUSEntity
    {
        public Icon Icon { get; set; }
        public Guid? IconId { get; set; }
        public string CssClass { get; set; }
        public string IconName { get; set; }
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        public BusinessComponent BusComp { get; set; }
        public Guid? BusCompId { get; set; }
        public Field Field { get; set; }
        public Guid? FieldId { get; set; }
        public string FieldName { get; set; }
        public ActionType ActionType { get; set; }
        public string Header { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public bool Readonly { get; set; }
    }
}
