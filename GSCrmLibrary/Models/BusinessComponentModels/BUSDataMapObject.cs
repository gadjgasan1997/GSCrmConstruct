using System;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSDataMapObject : BUSEntity
    {
        public DataMap DataMap { get; set; }
        public Guid DataMapId { get; set; }
        public BusinessObject SourceBusinessObject { get; set; }
        public Guid SourceBusinessObjectId { get; set; }
        public string SourceBusinessObjectName { get; set; }
        public BusinessObject DestinationBusinessObject { get; set; }
        public Guid DestinationBusinessObjectId { get; set; }
        public string DestinationBusinessObjectName { get; set; }
    }
}
