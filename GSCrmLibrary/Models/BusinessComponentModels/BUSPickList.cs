using System;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSPickList : BUSEntity
    {
        public BusinessComponent BusComp { get; set; }
        public string BusCompName { get; set; }
        public Guid BusCompId { get; set; }
        public bool Bounded { get; set; }
        public string SearchSpecification { get; set; }
    }
}
