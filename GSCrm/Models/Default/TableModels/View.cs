using System;
using System.Collections.Generic;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Представление
    public class View : MainTable
    {
        // Business object
        [ForeignKey("BusObjectId")]
        public BusObject BusObject { get; set; }
        public Guid BusObjectId { get; set; }

        // Props
        public List<ViewItem> ViewItems { get; set; }
        public View()
        {
            ViewItems = new List<ViewItem>();
        }
    }
}
