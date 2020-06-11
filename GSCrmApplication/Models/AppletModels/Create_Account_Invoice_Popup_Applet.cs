using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Create_Account_Invoice_Popup_Applet : UIEntity
	{
        [JsonPropertyName("UndoRecord")]
        public dynamic UndoRecord { get; set; }
        [JsonPropertyName("BIC")]
        public string BIC { get; set; }
        [JsonPropertyName("BankAccount")]
        public string BankAccount { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
	}
}
