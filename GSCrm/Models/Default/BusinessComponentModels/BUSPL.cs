using System;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSPL : MainBusinessComponent
    {
        public BusComp BusComp { get; set; }
        public string BusCompName { get; set; }
        public Guid? BusCompId { get; set; }
        public BUSPL() { }
    }
}
