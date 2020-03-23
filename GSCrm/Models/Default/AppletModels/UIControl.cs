using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIControl : MainApplet
    {
        [JsonPropertyName("IconName")]
        public string IconName { get; set; }
        [JsonPropertyName("CssClass")]
        public string CssClass { get; set; }
        [JsonPropertyName("FieldName")]
        public string FieldName { get; set; }
        [JsonPropertyName("ActionName")]
        public string ActionName { get; set; }
    }
}
