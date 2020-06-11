using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.TableModels
{
    public class Field : DataEntity
    {
        [ForeignKey("BusCompId")]
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }
        [ForeignKey("TableColumnId")]
        public TableColumn TableColumn { get; set; }
        public Guid? TableColumnId { get; set; }
        [ForeignKey("JoinId")]
        public Join Join { get; set; }
        public Guid? JoinId { get; set; }
        [ForeignKey("JoinColumnId")]
        public TableColumn JoinColumn { get; set; }
        public Guid? JoinColumnId { get; set; }
        [ForeignKey("PickListId")]
        public PickList PickList { get; set; }
        public Guid? PickListId { get; set; }
        public bool Required { get; set; }
        public bool Readonly { get; set; }
        public string Type { get; set; }
        public bool IsCalculate { get; set; }
        public string CalculatedValue { get; set; }
        public List<PickMap> PickMaps { get; set; }
        public Field()
        {
            PickMaps = new List<PickMap>();
        }
    }
}