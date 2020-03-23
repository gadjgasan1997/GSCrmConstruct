using System;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    public class JoinSpecification : MainTable
    {
        // Join
        [ForeignKey("JoinId")]
        public Join Join { get; set; }
        public Guid JoinId { get; set; }

        // Source field
        [ForeignKey("SourceFieldId")]
        public Field SourceField { get; set; }
        public Guid SourceFieldId { get; set; }

        // Destination column
        [ForeignKey("DestinationColumnId")]
        public TableColumn DestinationColumn { get; set; }
        public Guid DestinationColumnId { get; set; }
    }
}
