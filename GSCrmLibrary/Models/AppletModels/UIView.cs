using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIView : UIEntity
    {
        [JsonPropertyName("BusObjectName")]
        public string BusObjectName { get; set; }
    }
}
