using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIPL : MainApplet
    {
        [JsonPropertyName("BusCompName")]
        public string BusCompName { get; set; }
    }
}
