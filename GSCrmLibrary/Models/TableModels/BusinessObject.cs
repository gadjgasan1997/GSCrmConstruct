using System;
using System.Collections.Generic;
using GSCrmLibrary.Models.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class BusinessObject : DataEntity
    {
        public BusinessComponent PrimaryBusComp { get; set; }
        public Guid? PrimaryBusCompId { get; set; }
        public List<BusinessObjectComponent> BusObjectComponents { get; set; }
        public BusinessObject()
        {
            BusObjectComponents = new List<BusinessObjectComponent>();
        }
    }
}
