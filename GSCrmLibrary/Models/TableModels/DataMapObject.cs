using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.TableModels
{
    public class DataMapObject : DataEntity
    {
        [ForeignKey("DataMapId")]
        public DataMap DataMap { get; set; }
        public Guid DataMapId { get; set; }
        [ForeignKey("SourceBusinessObjectId")]
        public BusinessObject SourceBusinessObject { get; set; }
        public Guid SourceBusinessObjectId { get; set; }
        [ForeignKey("DestinationBusinessObjectId")]
        public BusinessObject DestinationBusinessObject { get; set; }
        public Guid DestinationBusinessObjectId { get; set; }
        public List<DataMapObjectComponent> DataMapObjectComponents { get; set; }
    }
}
