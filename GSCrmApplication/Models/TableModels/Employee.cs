using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.TableModels
{
	public class Employee : DataEntity
	{
        public string LastName { get; set; }
        public Guid DivisionId { get; set; }
        public Guid PrimaryPositionId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
	}
}
