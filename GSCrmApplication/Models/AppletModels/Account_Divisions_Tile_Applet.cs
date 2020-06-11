using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Account_Divisions_Tile_Applet : UIEntity
	{
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("ParentDivisionName")]
        public string ParentDivisionName { get; set; }
        [JsonPropertyName("NextRecords")]
        public dynamic NextRecords { get; set; }
        [JsonPropertyName("PreviousRecords")]
        public dynamic PreviousRecords { get; set; }
        [JsonPropertyName("DeleteRecord")]
        public dynamic DeleteRecord { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
	}
}
