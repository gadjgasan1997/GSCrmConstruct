using GSCrm.Models.Default.MainEntities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Table column
    public class TableColumn : MainTable
    {
        // Table
        [ForeignKey("TableId")]
        public Table Table { get; set; }
        public Guid TableId { get; set; }
    }

}
