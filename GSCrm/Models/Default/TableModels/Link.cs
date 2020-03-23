using System;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Связь между сущностями
    public class Link : MainTable
    {
        // Parent bc and field
        [ForeignKey("ParentBCId")]
        public BusComp ParentBC { get; set; }
        public Guid? ParentBCId { get; set; }
        [ForeignKey("ParentFieldId")]
        public Field ParentField { get; set; }
        public Guid? ParentFieldId { get; set; }

        // Child bc and field
        [ForeignKey("ChildBCId")]
        public BusComp ChildBC { get; set; }
        public Guid? ChildBCId { get; set; }
        [ForeignKey("ChildFieldId")]
        public Field ChildField { get; set; }
        public Guid? ChildFieldId { get; set; }
    }
}
