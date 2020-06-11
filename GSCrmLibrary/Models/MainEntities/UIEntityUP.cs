using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.MainEntities
{
    public class UIEntityUP : UIEntity
    {
        [JsonPropertyName("Value")]
        public string Value { get; set; }
    }
}
