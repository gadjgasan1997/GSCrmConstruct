using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Create_Employee_Popup_Applet : UIEntity
	{
        [JsonPropertyName("UndoRecord")]
        public dynamic UndoRecord { get; set; }
        [JsonPropertyName("MiddleName")]
        public string? MiddleName { get; set; }
        [JsonPropertyName("ManagerFullName")]
        public string? ManagerFullName { get; set; }
        [JsonPropertyName("NewRecord")]
        public dynamic NewRecord { get; set; }
        [JsonPropertyName("LastName")]
        public string LastName { get; set; }
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }
	}
}
