using System;
using GSCrmLibrary.Models.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class JoinSpecification : DataEntity
    {
        [ForeignKey("JoinId")]
        public Join Join { get; set; }
        public Guid JoinId { get; set; }
        [ForeignKey("SourceFieldId")]
        public Field SourceField { get; set; }
        public Guid SourceFieldId { get; set; }
        [ForeignKey("DestinationColumnId")]
        public TableColumn DestinationColumn { get; set; }
        public Guid DestinationColumnId { get; set; }
    }
}
