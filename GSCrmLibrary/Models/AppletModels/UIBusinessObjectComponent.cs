using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIBusinessObjectComponent : UIEntity
    {
        [JsonPropertyName("BusCompName")]
        public string BusCompName { get; set; }
        [JsonPropertyName("LinkName")]
        public string LinkName { get; set; }
        public UIBusinessObjectComponent() { }
    }
}
