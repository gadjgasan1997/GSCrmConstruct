using System;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSJoinSpecification : BUSEntity
    {
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }
        public Table Table { get; set; }
        public Guid TableId { get; set; }
        public Join Join { get; set; }
        public Guid JoinId { get; set; }
        public Field SourceField { get; set; }
        public Guid SourceFieldId { get; set; }
        public string SourceFieldName { get; set; }
        public TableColumn DestinationColumn { get; set; }
        public Guid DestinationColumnId { get; set; }
        public string DestinationColumnName { get; set; }
    }
}
