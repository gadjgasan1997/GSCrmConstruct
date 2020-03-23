using System;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Бизнес компонента внутри объекта
    public class BOComponent : MainTable
    {
        // Business object
        [ForeignKey("BusObjectId")]
        public BusObject BusObject { get; set; }
        public Guid BusObjectId { get; set; }

        // Business component
        [ForeignKey("BusCompId")]
        public BusComp BusComp { get; set; }
        public Guid BusCompId { get; set; }

        // Link
        [ForeignKey("LinkId")]
        public Link Link { get; set; }
        public Guid? LinkId { get; set; }

        // Props
        public string SearchSpecification { get; set; }
    }
}
