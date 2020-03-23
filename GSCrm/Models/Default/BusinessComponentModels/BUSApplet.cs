using System;
using GSCrm.Models.Default.MainEntities;
using GSCrm.Models.Default.TableModels;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSApplet : MainBusinessComponent
    {
        public string EmptyState { get; set; }
        public int DisplayLines { get; set; }
        public PR PhysicalRender { get; set; }
        public Guid PhysicalRenderId { get; set; }
        public string PhysicalRenderName { get; set; }
        public BusComp BusComp { get; set; }
        public Guid BusCompId { get; set; }
        public string BusCompName { get; set; }
        public BUSApplet() { }
    }
}
