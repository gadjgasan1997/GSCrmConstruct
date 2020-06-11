using GSCrmLibrary.Models.TableModels;
using System;

namespace GSCrmLibrary.Models.MainEntities
{
    public class DataEntityUp : DataEntity
    {
        public Control Control { get; set; }
        public Guid? ControlId { get; set; }
        public Column Column { get; set; }
        public Guid? ColumnId { get; set; }
        public string Value { get; set; }
    }
}