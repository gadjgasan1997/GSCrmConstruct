using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.BusinessComponentModels
{
	public class Employee : BUSEntity
	{
        public string? ManagerFullName { get; set; }
        public Guid PrimaryPositionId { get; set; }
        public string PrimaryPositionName { get; set; }
        public Guid DivisionId { get; set; }
        public string? ManagerMiddleName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public Guid? ParentPrimaryPositionId { get; set; }
        public string ManagerLastName { get; set; }
        public string? FullName { get; set; }
        public string ManagerFirstName { get; set; }
        public Guid ParentEmployeeId { get; set; }
	}
}
