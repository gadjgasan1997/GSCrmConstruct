using System;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.TableModels
{
    public class Field : MainTable
    {
        // Business component
        [ForeignKey("BusCompId")]
        public BusComp BusComp { get; set; }
        public Guid BusCompId { get; set; }

        // PickList
        [ForeignKey("PickListId")]
        public PL PL { get; set; }
        public Guid? PickListId { get; set; }

        // Join
        [ForeignKey("JoinId")]
        public Join Join { get; set; }
        public Guid? JoinId { get; set; }

        // Table and column
        [ForeignKey("TableColumnId")]
        public TableColumn TableColumn { get; set; }
        public Guid? TableColumnId { get; set; }
    }
}