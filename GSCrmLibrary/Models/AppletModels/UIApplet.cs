using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIApplet : UIEntity
    {
        [JsonPropertyName("EmptyState")]
        public string EmptyState { get; set; }
        [JsonPropertyName("DisplayLines")]
        public string DisplayLines { get; set; }
        [JsonPropertyName("PhysicalRenderName")]
        public string PhysicalRenderName { get; set; }
        [JsonPropertyName("BusCompName")]
        public string BusCompName { get; set; }
        [JsonPropertyName("InitFlag")]
        public bool InitFlag { get; set; }
        [JsonPropertyName("Header")]
        public string Header { get; set; }
        [JsonPropertyName("Type")]
        public string Type { get; set; }
        [JsonPropertyName("Virtual")]
        public bool Virtual { get; set; }
    }
}
