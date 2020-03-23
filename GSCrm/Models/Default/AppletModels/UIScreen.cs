using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIScreen : MainApplet
    {
        [JsonPropertyName("ViewName")]
        public string ViewName { get; set; }
    }
}
