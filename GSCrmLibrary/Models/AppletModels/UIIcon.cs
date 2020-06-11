using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIIcon : UIEntity
    {
        [JsonPropertyName("ImgPath")]
        public string ImgPath { get; set; }
        [JsonPropertyName("CssClass")]
        public string CssClass { get; set; }
        public UIIcon() { }
    }
}
