using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIScreenItem : UIEntity
    {
        [JsonPropertyName("ScreenName")]
        public string ScreenName { get; set; }
        [JsonPropertyName("ViewName")]
        public string ViewName { get; set; }
        [JsonPropertyName("ParentCategoryName")]
        public string ParentCategoryName { get; set; }
        [JsonPropertyName("ParentItemName")]
        public string ParentItemName { get; set; }
        [JsonPropertyName("Header")]
        public string Header { get; set; }
        [JsonPropertyName("Type")]
        public string Type { get; set; }
        [JsonPropertyName("DisplayInSiteMap")]
        public bool DisplayInSiteMap { get; set; } = true;
    }
}
