using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.BusinessComponentModels
{
	public class Position : BUSEntity
	{
        public string PrimaryEmployeeLastName { get; set; }
        public string? PrimaryEmployeeFullName { get; set; }
        public Guid? ParentPositionId { get; set; }
        public Guid PrimaryEmployeeId { get; set; }
        public string ParentPositionName { get; set; }
        public Guid AccountId { get; set; }
        public string PrimaryEmployeeFirstName { get; set; }
        public string Name { get; set; }
        public string? PrimaryEmployeeMiddleName { get; set; }
	}
}
