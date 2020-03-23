using GSCrm.Models.Default.MainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Бизнес компонента
    public class BusComp : MainTable
    {
        // Table
        [ForeignKey("TableId")]
        public Table Table { get; set; }
        public Guid? TableId { get; set; }

        // Props
        public List<Field> Fields { get; set; }
        public List<Join> Joins { get; set; }
        public BusComp()
        {
            Fields = new List<Field>();
            Joins = new List<Join>();
        }
    }
}
