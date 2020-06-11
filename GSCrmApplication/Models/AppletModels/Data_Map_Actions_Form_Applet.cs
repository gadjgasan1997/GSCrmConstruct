using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.AppletModels
{
	public class Data_Map_Actions_Form_Applet : UIEntity
	{
        [JsonPropertyName("Publish")]
        public dynamic Publish { get; set; }
	}
}
