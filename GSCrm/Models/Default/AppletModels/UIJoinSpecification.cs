using System.Text.Json.Serialization;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIJoinSpecification : MainApplet
    {
        [JsonPropertyName("SourceFieldName")]
        public string SourceFieldName { get; set; }
        [JsonPropertyName("DestinationColumnName")]
        public string DestinationColumnName { get; set; }
    }
}
