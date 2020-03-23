using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIAppletItem : MainApplet
    {
        [JsonPropertyName("ColumnName")]
        public string ColumnName { get; set; }
        [JsonPropertyName("ControlName")]
        public string ControlName { get; set; }
        [JsonPropertyName("PropertyName")]
        public string PropertyName { get; set; }
        public UIAppletItem() { }
    }
}
