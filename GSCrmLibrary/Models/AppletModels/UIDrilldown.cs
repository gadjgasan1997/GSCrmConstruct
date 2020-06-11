using System.Text.Json.Serialization;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIDrilldown : UIEntity
    {
        [JsonPropertyName("HyperLinkFieldName")]
        public string HyperLinkFieldName { get; set; }
        [JsonPropertyName("DestinationScreenName")]
        public string DestinationScreenName { get; set; }
        [JsonPropertyName("DestinationScreenItemName")]
        public string DestinationScreenItemName { get; set; }
        [JsonPropertyName("DestinationBusinessComponentName")]
        public string DestinationBusinessComponentName { get; set; }
        [JsonPropertyName("SourceFieldName")]
        public string SourceFieldName { get; set; }
        [JsonPropertyName("DestinationFieldName")]
        public string DestinationFieldName { get; set; }
    }
}
