using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UITableColumn : UIEntity
    {
        [JsonPropertyName("IsForeignKey")]
        public bool IsForeignKey { get; set; }
        [JsonPropertyName("IsNullable")]
        public bool IsNullable { get; set; }
        [JsonPropertyName("ForeignTableName")]
        public string ForeignTableName { get; set; }
        [JsonPropertyName("ForeignTableKeyName")]
        public string ForeignTableKeyName { get; set; }
        [JsonPropertyName("ExtencionType")]
        public string ExtencionType { get; set; }
        [JsonPropertyName("Type")]
        public string Type { get; set; }
        [JsonPropertyName("Length")]
        public string Length { get; set; }
        [JsonPropertyName("OnDelete")]
        public string OnDelete { get; set; }
        [JsonPropertyName("OnUpdate")]
        public string OnUpdate { get; set; }
    }
}
