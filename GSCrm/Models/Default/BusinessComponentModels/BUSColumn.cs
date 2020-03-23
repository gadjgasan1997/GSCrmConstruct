using System;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSColumn : MainBusinessComponent
    {
        public Guid? IconId { get; set; }
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        public BusComp BusComp { get; set; }
        public Field Field { get; set; }
        public Guid? FieldId { get; set; }
        public string FieldName { get; set; }
        public BUSColumn() { }
    }
}
