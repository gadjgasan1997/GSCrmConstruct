using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;
using System;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSAppletItem : MainBusinessComponent
    {
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        public Column Column { get; set; }
        public Guid ColumnId { get; set; }
        public Control Control { get; set; }
        public Guid ControlId { get; set; }
        public string ControlName { get; set; }
        public string ColumnName { get; set; }
        public string PropertyName { get; set; }
        public BUSAppletItem() { }
    }
}
