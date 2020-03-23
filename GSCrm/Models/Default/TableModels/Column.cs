using System;
using System.Collections.Generic;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Колонка апплета
    public class Column : MainTable
    {
        // Applet
        [ForeignKey("AppletId")]
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }

        // Field
        [ForeignKey("FieldId")]
        public Field Field { get; set; }
        public Guid? FieldId { get; set; }

        // Action
        [ForeignKey("ActionId")]
        public Action Action { get; set; }
        public Guid? ActionId { get; set; }

        // Icon
        [ForeignKey("IconId")]
        public Icon Icon { get; set; }
        public Guid? IconId { get; set; }

        // Props
        public string Header { get; set; }
        public List<ColumnUP> ColumnUPs { get; set; }
        public Column()
        {
            ColumnUPs = new List<ColumnUP>();
        }
    }
}
