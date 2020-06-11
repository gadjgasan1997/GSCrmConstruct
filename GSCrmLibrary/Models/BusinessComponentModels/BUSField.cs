using System;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSField : BUSEntity
    {
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }
        public string BusCompName { get; set; }
        public Table Table { get; set; }
        public Guid? TableId { get; set; }
        public TableColumn TableColumn { get; set; }
        public Guid? TableColumnId { get; set; }
        public string TableColumnName { get; set; }
        public Join Join { get; set; }
        public Guid JoinTableId { get; set; }
        public Guid? JoinId { get; set; }
        public string JoinName { get; set; }
        public TableColumn JoinColumn { get; set; }
        public Guid? JoinColumnId { get; set; }
        public string JoinColumnName { get; set; }
        public PickList PickList { get; set; }
        public Guid? PickListId { get; set; }
        public string PickListName { get; set; }
        public bool Required { get; set; }
        public bool Readonly { get; set; }
        public string Type { get; set; }
        public bool IsCalculate { get; set; }
        public string CalculatedValue { get; set; }
    }
}