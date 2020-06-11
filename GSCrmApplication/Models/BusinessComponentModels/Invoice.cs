using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.BusinessComponentModels
{
	public class Invoice : BUSEntity
	{
        public string BIC { get; set; }
        public string BankAccount { get; set; }
        public Guid AccountId { get; set; }
	}
}
