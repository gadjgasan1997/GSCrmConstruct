using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;
using System;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSTableColumn : BUSEntity
    {
        public Table Table { get; set; }
        public Guid TableId { get; set; }
        public Table ForeignTable { get; set; }
        public Guid? ForeignTableId { get; set; }
        public string ForeignTableName { get; set; }
        public TableColumn ForeignTableKey { get; set; }
        public Guid? ForeignTableKeyId { get; set; }
        public string ForeignTableKeyName { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsNullable { get; set; }
        public string ExtencionType { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public string OnDelete { get; set; }
        public string OnUpdate { get; set; }
    }
}
