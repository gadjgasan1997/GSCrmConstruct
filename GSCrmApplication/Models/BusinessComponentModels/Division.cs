using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.BusinessComponentModels
{
	public class Division : BUSEntity
	{
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string ParentDivisionName { get; set; }
        public Guid? ParentDivisionId { get; set; }
	}
}
