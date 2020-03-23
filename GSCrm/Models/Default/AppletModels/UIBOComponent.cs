using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIBOComponent : MainApplet
    {
        [JsonPropertyName("BusCompName")]
        public string BusCompName { get; set; }
        [JsonPropertyName("LinkName")]
        public string LinkName { get; set; }
        public UIBOComponent() { }
    }
}
