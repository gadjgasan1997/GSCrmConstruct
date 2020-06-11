using System;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSColumn : BUSEntity
    {
        public Guid? IconId { get; set; }
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
