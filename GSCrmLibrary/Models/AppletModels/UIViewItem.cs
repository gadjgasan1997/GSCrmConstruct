using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIViewItem : UIEntity
    {
        [JsonPropertyName("AppletName")]
        public string AppletName { get; set; }
        [JsonPropertyName("Autofocus")]
        public bool Autofocus { get; set; }
        [JsonPropertyName("AutofocusRecord")]
        public int AutofocusRecord { get; set; }
        [JsonPropertyName("Type")]
        public string Type { get; set; }
    }
}
