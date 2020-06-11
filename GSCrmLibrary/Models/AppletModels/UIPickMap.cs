using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIPickMap : UIEntity
    {
        [JsonPropertyName("BusCompFieldName")]
        public string BusCompFieldName { get; set; }

        [JsonPropertyName("PickListFieldName")]
        public string PickListFieldName { get; set; }
        [JsonPropertyName("Constrain")]
        public bool Constrain { get; set; }
    }
}
