using System;
using GSCrm.Models.Default.MainEntities;
using GSCrm.Models.Default.TableModels;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSField : MainBusinessComponent
    {
        // BusComp
        public BusComp BusComp { get; set; }
        public Guid BusCompId { get; set; }
        public string BusCompName { get; set; }
        
        // PickList
        public PL PickList { get; set; }
        public Guid? PickListId { get; set; }
        public string PickListName { get; set; }

        // Join
        public Join Join { get; set; }
        public Guid? JoinId { get; set; }
        public string JoinName { get; set; }

        // Table and column
        public Table Table { get; set; }
        public TableColumn TableColumn { get; set; }
        public Guid? TableColumnId { get; set; }
        public string TableColumnName { get; set; }
    }
}