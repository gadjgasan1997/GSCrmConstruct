using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Create_Account_Address_Popup_Applet : UIEntity
	{
        [JsonPropertyName("Street")]
        public string Street { get; set; }
        [JsonPropertyName("Country")]
        public string Country { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
        [JsonPropertyName("House")]
        public int House { get; set; }
        [JsonPropertyName("UndoRecord")]
        public dynamic UndoRecord { get; set; }
        [JsonPropertyName("City")]
        public string City { get; set; }
	}
}
