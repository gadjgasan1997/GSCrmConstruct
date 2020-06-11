using System;
using System.Collections.Generic;
using GSCrmLibrary.Models.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class View : DataEntity
    {
        [ForeignKey("BusObjectId")]
        public BusinessObject BusObject { get; set; }
        public Guid BusObjectId { get; set; }
        public List<ViewItem> ViewItems { get; set; }
        public View()
        {
            ViewItems = new List<ViewItem>();
        }
    }
}
