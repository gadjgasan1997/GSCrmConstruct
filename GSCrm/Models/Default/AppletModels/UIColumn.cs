using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIColumn : MainApplet
    {
        [JsonPropertyName("FieldName")]
        public string FieldName { get; set; }
        public UIColumn() { }
    }
}
