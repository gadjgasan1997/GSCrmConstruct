using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Employee_Tile_Applet : UIEntity
	{
        [JsonPropertyName("FullName")]
        public dynamic FullName { get; set; }
        [JsonPropertyName("ManagerFullName")]
        public dynamic ManagerFullName { get; set; }
        [JsonPropertyName("PreviousRecords")]
        public dynamic PreviousRecords { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
        [JsonPropertyName("UpdateRecord")]
        public dynamic UpdateRecord { get; set; }
        [JsonPropertyName("NextRecords")]
        public dynamic NextRecords { get; set; }
        [JsonPropertyName("DeleteRecord")]
        public dynamic DeleteRecord { get; set; }
	}
}
