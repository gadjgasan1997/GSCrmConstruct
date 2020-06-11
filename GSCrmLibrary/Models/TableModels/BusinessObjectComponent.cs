using System;
using GSCrmLibrary.Models.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Collections;

namespace GSCrmLibrary.Models.TableModels
{
    public class BusinessObjectComponent : DataEntity
    {
        [ForeignKey("BusObjectId")]
        public BusinessObject BusObject { get; set; }
        public Guid BusObjectId { get; set; }
        [ForeignKey("BusCompId")]
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }
        [ForeignKey("LinkId")]
        public Link Link { get; set; }
        public Guid? LinkId { get; set; }
    }
}
