using System;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.MainEntities
{
    public class MainApplet
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Sequence")]
        public string Sequence { get; set; }
        [JsonPropertyName("Header")]
        public string Header { get; set; }
        [JsonPropertyName("Type")]
        public string Type { get; set; }
        [JsonPropertyName("Required")]
        public bool Required { get; set; }
        [JsonPropertyName("Inactive")]
        public bool Inactive { get; set; }
        [JsonPropertyName("Display")]
        public bool Display { get; set; }
        public MainApplet() { }
    }
}
