using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIApplet : MainApplet
    {
        [JsonPropertyName("EmptyState")]
        public string EmptyState { get; set; }
        [JsonPropertyName("DisplayLines")]
        public string DisplayLines { get; set; }
        [JsonPropertyName("PhysicalRenderName")]
        public string PhysicalRenderName { get; set; }
        [JsonPropertyName("BusCompName")]
        public string BusCompName { get; set; }
        public UIApplet() { }
    }
}
