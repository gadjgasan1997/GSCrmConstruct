using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Account_Tile_Applet : UIEntity
	{
        [JsonPropertyName("OGRN")]
        public string? OGRN { get; set; }
        [JsonPropertyName("KPP")]
        public string? KPP { get; set; }
        [JsonPropertyName("INN")]
        public string? INN { get; set; }
        [JsonPropertyName("Resident")]
        public bool Resident { get; set; }
        [JsonPropertyName("OKPO")]
        public string? OKPO { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("PreviousRecords")]
        public dynamic PreviousRecords { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
        [JsonPropertyName("NextRecords")]
        public dynamic NextRecords { get; set; }
        [JsonPropertyName("DeleteRecord")]
        public dynamic DeleteRecord { get; set; }
	}
}
