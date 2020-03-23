using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;
using System;
using Action = GSCrm.Models.Default.TableModels.Action;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSControl : MainBusinessComponent
    {
        public Icon Icon { get; set; }
        public Guid? IconId { get; set; }
        public string CssClass { get; set; }
        public string IconName { get; set; }
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        public BusComp BusComp { get; set; }
        public Field Field { get; set; }
        public Guid? FieldId { get; set; }
        public string FieldName { get; set; }
        public Action Action { get; set; }
        public Guid? ActionId { get; set; }
        public string ActionName { get; set; }
        public BUSControl() { }
    }
}
