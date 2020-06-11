using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.BusinessComponentModels
{
	public class Drilldown : BUSEntity
	{
        public string HyperLinkFieldName { get; set; }
        public string DestinationScreenName { get; set; }
        public string Name { get; set; }
        public Guid DestinationScreenItemId { get; set; }
        public string SourceFieldName { get; set; }
        public Guid DestinationScreenId { get; set; }
        public string DestinationScreenItemName { get; set; }
        public Guid DestinationBusinessComponentId { get; set; }
        public string DestinationFieldName { get; set; }
        public Guid SourceBusinessComponentId { get; set; }
        public Guid HyperLinkFieldId { get; set; }
        public Guid SourceFieldId { get; set; }
        public Guid AppletId { get; set; }
        public Guid DestinationFieldId { get; set; }
        public string DestinationBusinessComponentName { get; set; }
	}
}
