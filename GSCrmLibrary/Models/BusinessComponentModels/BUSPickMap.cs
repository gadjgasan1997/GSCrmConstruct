using System;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSPickMap : BUSEntity
    {
        public BusinessComponent BusComp { get; set; }
        public Guid BusCompId { get; set; }
        public Field Field { get; set; }
        public Guid FieldId { get; set; }
        public Field BusCompField { get; set; }
        public Guid BusCompFieldId { get; set; }
        public string BusCompFieldName { get; set; }
        public PickList PickList { get; set; }
        public Guid PickListId { get; set; }
        public BusinessComponent PickListBusComp { get; set; }
        public Guid PickListBusCompId { get; set; }
        public Field PickListField { get; set; }
        public Guid PickListFieldId { get; set; }
        public string PickListFieldName { get; set; }
        public bool Constrain { get; set; }
    }
}
