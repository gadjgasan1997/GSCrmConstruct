using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;
using System;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSAppletItem : BUSEntity
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
