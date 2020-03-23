using GSCrm.Models.Default.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrm.Models.Default.AppletModels
{
    public class UIViewItem : MainApplet
    {
        [JsonPropertyName("AppletName")]
        public string AppletName { get; set; }
        [JsonPropertyName("Autofocus")]
        public bool Autofocus { get; set; }
        [JsonPropertyName("AutofocusRecord")]
        public int AutofocusRecord { get; set; }
        public UIViewItem() { }
    }
}
