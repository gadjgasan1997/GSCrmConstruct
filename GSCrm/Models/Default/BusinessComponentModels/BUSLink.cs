using System;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.MainEntities;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSLink : MainBusinessComponent
    {
        public BusComp ParentBusComp { get; set; }
        public Guid? ParentBCId { get; set; }
        public string ParentBCName { get; set; }
        public Field ParentField { get; set; }
        public Guid? ParentFieldId { get; set; }
        public string ParentFieldName { get; set; }
        public BusComp ChildBusComp { get; set; }
        public Guid? ChildBCId { get; set; }
        public string ChildBCName { get; set; }
        public Field ChildField { get; set; }
        public Guid? ChildFieldId { get; set; }
        public string ChildFieldName { get; set; }
        public BUSLink() { }
    }
}
