using System;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSBusinessObjectComponent : BUSEntity
    {
        public BusinessObject BusObject { get; set; }
        public Guid BusObjectId { get; set; }
        public string BusObjectName { get; set; }
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }
        public string BusCompName { get; set; }
        public Link Link { get; set; }
        public Guid? LinkId { get; set; }
        public string LinkName { get; set; }
    }
}
