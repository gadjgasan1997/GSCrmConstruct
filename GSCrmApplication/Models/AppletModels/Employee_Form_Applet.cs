using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Employee_Form_Applet : UIEntity
	{
        [JsonPropertyName("ManagerFullName")]
        public dynamic ManagerFullName { get; set; }
        [JsonPropertyName("FirstName")]
        public dynamic FirstName { get; set; }
        [JsonPropertyName("MiddleName")]
        public string? MiddleName { get; set; }
        [JsonPropertyName("LastName")]
        public dynamic LastName { get; set; }
	}
}
