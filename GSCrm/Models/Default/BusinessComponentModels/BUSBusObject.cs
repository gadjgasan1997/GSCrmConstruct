using System;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSBusObject : MainBusinessComponent
    {
        public BusComp PrimaryBusComp { get; set; }
        public Guid? PrimaryBusCompId { get; set; }
        public string PrimaryBusCompName { get; set; }
        public BUSBusObject() { }
    }
}
