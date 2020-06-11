using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIBusinessObject : UIEntity
    {
        [JsonPropertyName("PrimaryBusCompName")]
        public string PrimaryBusCompName { get; set; }
        public UIBusinessObject() { }
    }
}
