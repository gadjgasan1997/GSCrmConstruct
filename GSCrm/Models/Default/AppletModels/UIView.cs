using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIView : MainApplet
    {
        [JsonPropertyName("BusObjectName")]
        public string BusObjectName { get; set; }
        public UIView() { }
    }
}
