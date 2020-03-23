using System;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Список для выброра
    public class PL : MainTable
    {
        // Business component
        [ForeignKey("BusCompId")]
        public BusComp BusComp { get; set; }
        public Guid? BusCompId { get; set; }
    }
}
