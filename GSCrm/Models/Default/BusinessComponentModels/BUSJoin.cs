using System;
using GSCrm.Models.Default.MainEntities;
using GSCrm.Models.Default.TableModels;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSJoin : MainBusinessComponent
    {
        // BusComp
        public BusComp BusComp { get; set; }
        public Guid BusCompId { get; set; }

        // Table
        public Table Table { get; set; }
        public Guid TableId { get; set; }
        public string TableName { get; set; }
    }
}
