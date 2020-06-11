using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIDataMapField : UIEntity
    {
        [JsonPropertyName("SourceFieldName")]
        public string SourceFieldName { get; set; }
        [JsonPropertyName("Destination")]
        public string Destination { get; set; }
        [JsonPropertyName("FieldValidation")]
        public string? FieldValidation { get; set; }
    }
}
