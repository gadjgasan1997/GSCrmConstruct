using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;
using System;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSBusinessComponent : BUSEntity
    {
        public Table Table { get; set; }
        public Guid? TableId { get; set; }
        public string TableName { get; set; }
        public bool ShadowCopy { get; set; }
    }
}
