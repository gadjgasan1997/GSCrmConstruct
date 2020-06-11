using System;
using GSCrmLibrary.Models.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class PickList : DataEntity
    {
        [ForeignKey("BusCompId")]
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }
        public bool Bounded { get; set; }
        public string SearchSpecification { get; set; }
    }
}
