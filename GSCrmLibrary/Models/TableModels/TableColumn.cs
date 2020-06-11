using GSCrmLibrary.Models.MainEntities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class TableColumn : DataEntity
    {
        [ForeignKey("TableId")]
        public Table Table { get; set; }
        public Guid TableId { get; set; }
        public bool IsForeignKey { get; set; }
        public Guid? ForeignTableId { get; set; }
        public Guid? ForeignTableKeyId { get; set; }
        public string ExtencionType { get; set; }
        public bool IsNullable { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string OnDelete { get; set; }
        public string OnUpdate { get; set; }
        public bool NeedCreate { get; set; }
        public bool NeedUpdate { get; set; }
    }
}
