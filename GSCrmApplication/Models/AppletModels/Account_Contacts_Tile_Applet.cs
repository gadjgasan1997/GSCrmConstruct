using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Account_Contacts_Tile_Applet : UIEntity
	{
        [JsonPropertyName("WorkNumber")]
        public string? WorkNumber { get; set; }
        [JsonPropertyName("Email")]
        public string? Email { get; set; }
        [JsonPropertyName("AccountName")]
        public string AccountName { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
        [JsonPropertyName("PreviousRecords")]
        public dynamic PreviousRecords { get; set; }
        [JsonPropertyName("NextRecords")]
        public dynamic NextRecords { get; set; }
        [JsonPropertyName("DeleteRecord")]
        public dynamic DeleteRecord { get; set; }
	}
}
