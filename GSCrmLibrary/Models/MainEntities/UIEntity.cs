using System;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.MainEntities
{
    public class UIEntity : IUIEntity
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Sequence")]
        public string Sequence { get; set; }
        [JsonPropertyName("Inactive")]
        public bool Inactive { get; set; }
        [JsonPropertyName("Changed")]
        public bool Changed { get; set; }
    }
}
