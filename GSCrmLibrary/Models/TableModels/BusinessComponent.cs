using GSCrmLibrary.Models.MainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class BusinessComponent : DataEntity
    {
        [ForeignKey("TableId")]
        public Table Table { get; set; }
        public Guid? TableId { get; set; }
        public string Routing { get; set; }
        public bool ShadowCopy { get; set; }
        public List<Field> Fields { get; set; }
        public List<Join> Joins { get; set; }
        public BusinessComponent()
        {
            Fields = new List<Field>();
            Joins = new List<Join>();
        }
    }
}
