using System;
using System.Collections.Generic;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Join
    public class Join : MainTable
    {
        // Business component
        [ForeignKey("BusCompId")]
        public BusComp BusComp { get; set; }
        public Guid BusCompId { get; set; }

        // Table
        [ForeignKey("TableId")]
        public Table Table { get; set; }
        public Guid TableId { get; set; }

        // Props
        public List<JoinSpecification> JoinSpecifications { get; set; }
        public Join()
        {
            JoinSpecifications = new List<JoinSpecification>();
        }
    }
}
