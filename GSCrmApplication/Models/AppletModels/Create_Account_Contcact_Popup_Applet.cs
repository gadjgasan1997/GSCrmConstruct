using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Create_Account_Contcact_Popup_Applet : UIEntity
	{
        [JsonPropertyName("WorkNumber")]
        public string? WorkNumber { get; set; }
        [JsonPropertyName("UndoRecord")]
        public dynamic UndoRecord { get; set; }
        [JsonPropertyName("Email")]
        public string? Email { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
        [JsonPropertyName("AccountName")]
        public string AccountName { get; set; }
	}
}
