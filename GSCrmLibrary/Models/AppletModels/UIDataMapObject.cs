using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIDataMapObject : UIEntity
    {
        [JsonPropertyName("SourceBusinessObjectName")]
        public string SourceBusinessObjectName { get; set; }
        [JsonPropertyName("DestinationBusinessObjectName")]
        public string DestinationBusinessObjectName { get; set; }
    }
}
