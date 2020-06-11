using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIField : UIEntity
    {
        [JsonPropertyName("PickListName")]
        public string PickListName { get; set; }
        [JsonPropertyName("TableColumnName")]
        public string TableColumnName { get; set; }
        [JsonPropertyName("JoinName")]
        public string JoinName { get; set; }
        [JsonPropertyName("JoinColumnName")]
        public string JoinColumnName { get; set; }
        [JsonPropertyName("Required")]
        public bool Required { get; set; }
        [JsonPropertyName("Readonly")]
        public bool Readonly { get; set; }
        [JsonPropertyName("Type")]
        public string Type { get; set; }
        [JsonPropertyName("IsCalculate")]
        public bool IsCalculate { get; set; }
        [JsonPropertyName("CalculatedValue")]
        public string CalculatedValue { get; set; }
    }
}