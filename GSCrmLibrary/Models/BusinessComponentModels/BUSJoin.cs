using System;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSJoin : BUSEntity
    {
        // BusComp
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }

        // Table
        public Table Table { get; set; }
        public Guid TableId { get; set; }
        public string TableName { get; set; }
    }
}
