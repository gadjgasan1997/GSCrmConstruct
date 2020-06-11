using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIDataMapObjectComponent : UIEntity
    {
        [JsonPropertyName("SourceBOComponentName")]
        public string SourceBOComponentName { get; set; }
        [JsonPropertyName("DestinationBOComponentName")]
        public string DestinationBOComponentName { get; set; }
        [JsonPropertyName("ParentDataMapComponentName")]
        public string ParentDataMapComponentName { get; set; }
        [JsonPropertyName("SourceSearchSpecification")]
        public string SourceSearchSpecification { get; set; }
    }
}
