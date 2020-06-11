using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GSCrmApplication.Models.MainEntities;

namespace GSCrmApplication.Models.TableModels
{
	public class Position : DataEntity
	{
        public Guid? ParentPositionId { get; set; }
        public string Name { get; set; }
        public Guid PrimaryEmployeeId { get; set; }
        public Guid AccountId { get; set; }
	}
}
