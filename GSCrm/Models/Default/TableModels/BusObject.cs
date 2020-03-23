using System;
using System.Collections.Generic;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Бизнес объект
    public class BusObject : MainTable
    {
        // primaty business component
        [ForeignKey("PrimaryBusCompId")]
        public BusComp PrimaryBusComp { get; set; }
        public Guid? PrimaryBusCompId { get; set; }

        // Props
        public List<BOComponent> BusObjectComponents { get; set; }
        public BusObject()
        {
            BusObjectComponents = new List<BOComponent>();
        }
    }
}
