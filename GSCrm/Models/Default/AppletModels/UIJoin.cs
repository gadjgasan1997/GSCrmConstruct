using System.Text.Json.Serialization;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIJoin : MainApplet
    {
        [JsonPropertyName("TableName")]
        public string TableName { get; set; }
    }
}
