using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.TableModels
{
	public class Invoice : DataEntity
	{
        public string BIC { get; set; }
        public Guid AccountId { get; set; }
        public string BankAccount { get; set; }
	}
}
