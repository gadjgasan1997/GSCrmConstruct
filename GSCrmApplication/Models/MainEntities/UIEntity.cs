using System;
using System.Text.Json.Serialization;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmApplication.Models.MainEntities
{
    public class UIEntity : IUIEntity
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }
        [JsonPropertyName("Created")]
        public DateTime Created { get; set; }
        [JsonPropertyName("CreatedBy")]
        public string CreatedBy { get; set; }
        [JsonPropertyName("LastUpdated")]
        public DateTime LastUpdated { get; set; }
        [JsonPropertyName("UpdatedBy")]
        public string UpdatedBy { get; set; }
    }
}
