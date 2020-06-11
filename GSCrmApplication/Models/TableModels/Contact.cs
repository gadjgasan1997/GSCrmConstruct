using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.TableModels
{
	public class Contact : DataEntity
	{
        public string? CellNumber { get; set; }
        public string? HomeNumber { get; set; }
        public string? Email { get; set; }
        public Guid? AccountId { get; set; }
        public string? WorkNumber { get; set; }
        public Guid? PersonId { get; set; }
	}
}
