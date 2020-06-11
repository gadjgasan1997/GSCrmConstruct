using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.TableModels
{
    public class DataMapObjectComponent : DataEntity
    {
        [ForeignKey("DataMapObjectId")]
        public DataMapObject DataMapObject { get; set; }
        public Guid DataMapObjectId { get; set; }
        [ForeignKey("SourceBOComponentId")]
        public BusinessObjectComponent SourceBOComponent { get; set; }
        public Guid SourceBOComponentId { get; set; }
        [ForeignKey("DestinationBOComponentId")]
        public BusinessObjectComponent DestinationBOComponent { get; set; }
        public Guid DestinationBOComponentId { get; set; }
        public Guid? ParentDataMapComponentId { get; set; }
        public string SourceSearchSpecification { get; set; }
        public List<DataMapField> DataMapFields { get; set; }
    }
}
