using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Account_Positions_Tile_Applet : UIEntity
	{
        [JsonPropertyName("ParentPositionName")]
        public string ParentPositionName { get; set; }
        [JsonPropertyName("PrimaryEmployeeFullName")]
        public string? PrimaryEmployeeFullName { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("NextRecords")]
        public dynamic NextRecords { get; set; }
        [JsonPropertyName("DeleteRecord")]
        public dynamic DeleteRecord { get; set; }
        [JsonPropertyName("PreviousRecords")]
        public dynamic PreviousRecords { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
	}
}
