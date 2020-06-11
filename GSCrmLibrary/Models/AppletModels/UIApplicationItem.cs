using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIApplicationItem : UIEntity
    {
        [JsonPropertyName("ScreenName")]
        public string ScreenName { get; set; }
    }
}
