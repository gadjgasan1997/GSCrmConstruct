using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIScreen : UIEntity
    {
        [JsonPropertyName("Header")]
        public string Header { get; set; }
    }
}
