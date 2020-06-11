using System;
using GSCrmLibrary.Models.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class Link : DataEntity
    {
        [ForeignKey("ParentBCId")]
        public BusinessComponent ParentBC { get; set; }
        public Guid? ParentBCId { get; set; }
        [ForeignKey("ParentFieldId")]
        public Field ParentField { get; set; }
        public Guid? ParentFieldId { get; set; }
        [ForeignKey("ChildBCId")]
        public BusinessComponent ChildBC { get; set; }
        public Guid? ChildBCId { get; set; }
        [ForeignKey("ChildFieldId")]
        public Field ChildField { get; set; }
        public Guid? ChildFieldId { get; set; }
    }
}
