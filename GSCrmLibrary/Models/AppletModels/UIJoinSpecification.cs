using System.Text.Json.Serialization;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIJoinSpecification : UIEntity
    {
        [JsonPropertyName("SourceFieldName")]
        public string SourceFieldName { get; set; }
        [JsonPropertyName("DestinationColumnName")]
        public string DestinationColumnName { get; set; }
    }
}
