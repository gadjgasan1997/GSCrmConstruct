using System;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.TableModels
{
    public class DataMapField : DataEntity
    {
        [ForeignKey("DataMapComponentId")]
        public DataMapObjectComponent DataMapObjectComponent { get; set; }
        public Guid DataMapComponentId { get; set; }
        [ForeignKey("SourceFieldId")]
        public Field SourceField { get; set; }
        public Guid SourceFieldId { get; set; }
        public string Destination { get; set; }
        public string? FieldValidation { get; set; }
    }
}
