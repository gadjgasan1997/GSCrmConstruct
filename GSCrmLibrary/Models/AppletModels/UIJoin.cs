using System.Text.Json.Serialization;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIJoin : UIEntity
    {
        [JsonPropertyName("TableName")]
        public string TableName { get; set; }
    }
}
