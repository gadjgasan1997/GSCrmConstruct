using GSCrmLibrary.Models.MainEntities;
using System.Text.Json.Serialization;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIDirectoriesList : UIEntity
    {
        [JsonPropertyName("Language")]
        public string Language { get; set; }
        [JsonPropertyName("LIC")]
        public string LIC { get; set; }
        [JsonPropertyName("DisplayValue")]
        public string DisplayValue { get; set; }
        [JsonPropertyName("DirectoryType")]
        public string DirectoryType { get; set; }
    }
}
