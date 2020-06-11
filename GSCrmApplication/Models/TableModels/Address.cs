using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.TableModels
{
	public class Address : DataEntity
	{
        public int House { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public Guid AccountId { get; set; }
        public string Street { get; set; }
	}
}
