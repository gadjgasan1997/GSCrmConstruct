using System;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSView : MainBusinessComponent
    {
        public BusObject BusObject { get; set; }
        public string BusObjectName { get; set; }
        public Guid BusObjectId { get; set; }
        public BUSView() { }
    }
}
