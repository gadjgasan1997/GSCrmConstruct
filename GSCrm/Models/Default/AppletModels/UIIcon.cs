using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIIcon : MainApplet
    {
        [JsonPropertyName("ImgPath")]
        public string ImgPath { get; set; }
        [JsonPropertyName("CssClass")]
        public string CssClass { get; set; }
        public UIIcon() { }
    }
}
