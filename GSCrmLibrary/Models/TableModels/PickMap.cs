using System;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.TableModels
{
    public class PickMap : DataEntity
    {
        [ForeignKey("FieldId")]
        public Field Field { get; set; }
        public Guid FieldId { get; set; }
        public Guid BusCompFieldId { get; set; }
        public Guid PickListFieldId { get; set; }
        public bool Constrain { get; set; }
    }
}
