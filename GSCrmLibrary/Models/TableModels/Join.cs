using System;
using System.Collections.Generic;
using GSCrmLibrary.Models.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class Join : DataEntity
    {
        [ForeignKey("BusCompId")]
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }
        [ForeignKey("TableId")]
        public Table Table { get; set; }
        public Guid TableId { get; set; }
        public List<JoinSpecification> JoinSpecifications { get; set; }
        public Join()
        {
            JoinSpecifications = new List<JoinSpecification>();
        }
    }
}
