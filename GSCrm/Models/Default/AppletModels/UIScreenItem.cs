using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIScreenItem : MainApplet
    {
        [JsonPropertyName("ViewName")]
        public string ViewName { get; set; }
        [JsonPropertyName("ParentCategory")]
        public string ParentCategory { get; set; }
        [JsonPropertyName("ParentItem")]
        public string ParentItem { get; set; }
    }
}
