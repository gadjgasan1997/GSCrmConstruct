using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.BusinessComponentModels
{
	public class Address : BUSEntity
	{
        public string? FullAddress { get; set; }
        public Guid AccountId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int House { get; set; }
	}
}
