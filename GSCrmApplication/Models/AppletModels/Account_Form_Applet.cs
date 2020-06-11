using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Account_Form_Applet : UIEntity
	{
        [JsonPropertyName("INN")]
        public string? INN { get; set; }
        [JsonPropertyName("OGRN")]
        public string? OGRN { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Resident")]
        public bool Resident { get; set; }
        [JsonPropertyName("OKPO")]
        public string? OKPO { get; set; }
        [JsonPropertyName("KPP")]
        public string? KPP { get; set; }
	}
}
