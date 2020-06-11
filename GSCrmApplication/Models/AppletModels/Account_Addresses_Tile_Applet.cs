using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Account_Addresses_Tile_Applet : UIEntity
	{
        [JsonPropertyName("FullAddress")]
        public string? FullAddress { get; set; }
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
