using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UILink : UIEntity
    {
        [JsonPropertyName("ParentBCName")]
        public string ParentBCName { get; set; }
        [JsonPropertyName("ParentFieldName")]
        public string ParentFieldName { get; set; }
        [JsonPropertyName("ChildBCName")]
        public string ChildBCName { get; set; }
        [JsonPropertyName("ChildFieldName")]
        public string ChildFieldName { get; set; }
    }
}
