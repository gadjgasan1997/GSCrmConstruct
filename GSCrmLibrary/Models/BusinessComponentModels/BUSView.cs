using System;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSView : BUSEntity
    {
        public BusinessObject BusObject { get; set; }
        public string BusObjectName { get; set; }
        public Guid BusObjectId { get; set; }
        public BUSView() { }
    }
}
