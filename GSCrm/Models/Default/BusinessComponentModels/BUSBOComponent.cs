using System;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSBOComponent : MainBusinessComponent
    {
        public BusObject BusObject { get; set; }
        public Guid BusObjectId { get; set; }
        public string BusObjectName { get; set; }
        public BusComp BusComp { get; set; }
        public Guid BusCompId { get; set; }
        public string BusCompName { get; set; }
        public Link Link { get; set; }
        public Guid? LinkId { get; set; }
        public string LinkName { get; set; }
        public BUSBOComponent() { }
    }
}
