using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Create_Account_Popup_Applet : UIEntity
	{
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("UndoRecord")]
        public dynamic UndoRecord { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
	}
}
