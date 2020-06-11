using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIAppletItem : UIEntity
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
