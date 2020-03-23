using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UILink : MainApplet
    {
        [JsonPropertyName("ParentBCName")]
        public string ParentBCName { get; set; }
        [JsonPropertyName("ParentFieldName")]
        public string ParentFieldName { get; set; }
        [JsonPropertyName("ChildBCName")]
        public string ChildBCName { get; set; }
        [JsonPropertyName("ChildFieldName")]
        public string ChildFieldName { get; set; }
    }
}
