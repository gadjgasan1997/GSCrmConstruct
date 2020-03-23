using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;
using System;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSBusComp : MainBusinessComponent
    {
        public Table Table { get; set; }
        public Guid? TableId { get; set; }
        public string TableName { get; set; }
        public BUSBusComp() { }
    }
}
