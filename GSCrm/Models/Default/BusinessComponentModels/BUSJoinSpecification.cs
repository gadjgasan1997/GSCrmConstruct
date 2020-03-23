using System;
using GSCrm.Models.Default.MainEntities;
using GSCrm.Models.Default.TableModels;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSJoinSpecification : MainBusinessComponent
    {
        // Join
        public Join Join { get; set; }
        public Guid JoinId { get; set; }

        // Source field
        public Field SourceField { get; set; }
        public Guid SourceFieldId { get; set; }
        public string SourceFieldName { get; set; }

        // Destination column
        public TableColumn DestinationColumn { get; set; }
        public Guid DestinationColumnId { get; set; }
        public string DestinationColumnName { get; set; }
    }
}
