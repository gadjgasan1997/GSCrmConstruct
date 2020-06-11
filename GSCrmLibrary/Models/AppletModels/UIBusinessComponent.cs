using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIBusinessComponent : UIEntity
    {
        [JsonPropertyName("TableName")]
        public string TableName { get; set; }
        [JsonPropertyName("ShadowCopy")]
        public bool ShadowCopy { get; set; }
    }
}
