using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIColumn : UIEntity
    {
        [JsonPropertyName("FieldName")]
        public string FieldName { get; set; }
        [JsonPropertyName("ActionType")]
        public string ActionType { get; set; }
        [JsonPropertyName("Routing")]
        public string Routing { get; set; }
        [JsonPropertyName("Header")]
        public string Header { get; set; }
        [JsonPropertyName("Type")]
        public string Type { get; set; }
        [JsonPropertyName("Required")]
        public bool Required { get; set; }
        [JsonPropertyName("Readonly")]
        public bool Readonly { get; set; }
    }
}
