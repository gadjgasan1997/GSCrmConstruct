using System;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSBusinessObject : BUSEntity
    {
        public BusinessComponent PrimaryBusComp { get; set; }
        public Guid? PrimaryBusCompId { get; set; }
        public string PrimaryBusCompName { get; set; }
    }
}
