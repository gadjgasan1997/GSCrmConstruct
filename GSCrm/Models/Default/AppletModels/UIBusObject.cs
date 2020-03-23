using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIBusObject : MainApplet
    {
        [JsonPropertyName("PrimaryBusCompName")]
        public string PrimaryBusCompName { get; set; }
        public UIBusObject() { }
    }
}
