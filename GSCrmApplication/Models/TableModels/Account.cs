using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.TableModels
{
	public class Account : DataEntity
	{
        public string? OKPO { get; set; }
        public string? KPP { get; set; }
        public string Name { get; set; }
        public bool Resident { get; set; }
        public string? INN { get; set; }
        public string? OGRN { get; set; }
	}
}
