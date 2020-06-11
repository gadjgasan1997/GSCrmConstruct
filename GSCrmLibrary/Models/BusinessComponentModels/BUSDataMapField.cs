using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;
using System;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSDataMapField : BUSEntity
    {
        public DataMapObjectComponent DataMapComponent { get; set; }
        public Guid DataMapComponentId { get; set; }
        public Guid SourceBusinessComponentId { get; set; }
        public Guid DestinationBusinessComponentId { get; set; }
        public Field SourceField { get; set; }
        public Guid SourceFieldId { get; set; }
        public string SourceFieldName { get; set; }
        public string Destination { get; set; }
        public string? FieldValidation { get; set; }
    }
}
