using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.MainEntities
{
    public class MainAppletUP : MainApplet
    {
        [JsonPropertyName("Value")]
        public string Value { get; set; }
        public MainAppletUP() { }
    }
}
