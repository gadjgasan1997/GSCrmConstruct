using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.BusinessComponentModels
{
	public class Contact : BUSEntity
	{
        public string? MiddleName { get; set; }
        public string? HomeNumber { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? PersonId { get; set; }
        public string LastName { get; set; }
        public string? CellNumber { get; set; }
        public string AccountName { get; set; }
        public string? Email { get; set; }
        public string? WorkNumber { get; set; }
        public string FirstName { get; set; }
	}
}
