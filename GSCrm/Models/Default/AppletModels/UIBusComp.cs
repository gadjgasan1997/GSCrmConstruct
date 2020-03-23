using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIBusComp : MainApplet
    {
        [JsonPropertyName("TableName")]
        public string TableName { get; set; }
        public UIBusComp() { }
    }
}
