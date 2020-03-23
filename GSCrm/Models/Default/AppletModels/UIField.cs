using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIField : MainApplet
    {
        [JsonPropertyName("PickListName")]
        public string PickListName { get; set; }
        [JsonPropertyName("TableColumnName")]
        public string TableColumnName { get; set; }
        [JsonPropertyName("Join")]
        public string JoinName { get; set; }
        [JsonPropertyName("JoinColumn")]
        public string JoinColumnName { get; set; }
    }
}