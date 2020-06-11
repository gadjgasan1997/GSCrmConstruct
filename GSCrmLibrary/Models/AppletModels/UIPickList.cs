using System;
using System.Text.Json.Serialization;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.AppletModels
{
    public class UIPickList : UIEntity
    {
        [JsonPropertyName("BusCompName")]
        public string BusCompName { get; set; }
        [JsonPropertyName("CurrentRecordId")]
        public Guid? CurrentRecordId { get; set; }
        [JsonPropertyName("Bounded")]
        public bool Bounded { get; set; }
        [JsonPropertyName("SearchSpecification")]
        public string SearchSpecification { get; set; }
    }
}
