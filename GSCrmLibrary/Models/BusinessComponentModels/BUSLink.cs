using System;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSLink : BUSEntity
    {
        public BusinessComponent ParentBusComp { get; set; }
        public Guid? ParentBCId { get; set; }
        public string ParentBCName { get; set; }
        public Field ParentField { get; set; }
        public Guid? ParentFieldId { get; set; }
        public string ParentFieldName { get; set; }
        public BusinessComponent ChildBusComp { get; set; }
        public Guid? ChildBCId { get; set; }
        public string ChildBCName { get; set; }
        public Field ChildField { get; set; }
        public Guid? ChildFieldId { get; set; }
        public string ChildFieldName { get; set; }
        public BUSLink() { }
    }
}
