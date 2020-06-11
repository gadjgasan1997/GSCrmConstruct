using System;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSApplet : BUSEntity
    {
        public string EmptyState { get; set; }
        public int DisplayLines { get; set; }
        public PhysicalRender PhysicalRender { get; set; }
        public Guid PhysicalRenderId { get; set; }
        public string PhysicalRenderName { get; set; }
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }
        public string BusCompName { get; set; }
        public bool Initflag { get; set; }
        public string Header { get; set; }
        public string Type { get; set; }
        public bool Virtual { get; set; }
    }
}
