using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSDataMapObjectComponent : BUSEntity
    {
        public DataMapObject DataMapObject { get; set; }
        public Guid DataMapObjectId { get; set; }
        public BusinessObject SourceBusinessObject { get; set; }
        public Guid SourceBusinessObjectId { get; set; }
        public BusinessObjectComponent SourceBOComponent { get; set; }
        public Guid SourceBOComponentId { get; set; }
        public string SourceBOComponentName { get; set; }
        public BusinessComponent SourceBusinessComponent { get; set; }
        public Guid SourceBusinessComponentId { get; set; }
        public BusinessObject DestinationBusinessObject { get; set; }
        public Guid DestinationBusinessObjectId { get; set; }
        public BusinessObjectComponent DestinationBOComponent { get; set; }
        public Guid DestinationBOComponentId { get; set; }
        public string DestinationBOComponentName { get; set; }
        public BusinessComponent DestinationBusinessComponent { get; set; }
        public Guid DestinationBusinessComponentId { get; set; }
        public DataMapObjectComponent ParentDataMapComponent { get; set; }
        public Guid? ParentDataMapComponentId { get; set; }
        public string ParentDataMapComponentName { get; set; }
        public string SourceSearchSpecification { get; set; }
    }
}
