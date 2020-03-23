using System;
using System.Collections.Generic;
using GSCrm.Models.Default.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Класс для апплета, описывающий все типы(тайл, форм, попап)
    public class Applet : MainTable
    {
        // Physical render
        [ForeignKey("PhysicalRenderId")]
        public PR PhysicalRender { get; set; }
        public Guid PhysicalRenderId { get; set; }

        // Bysiness component
        [ForeignKey("BusCompId")]
        public BusComp BusComp { get; set; }
        public Guid BusCompId { get; set; }

        // Props
        public string Header { get; set; }
        public string EmptyState { get; set; }
        public int DisplayLines { get; set; }
        public bool Initflag { get; set; }
        public List<Control> Controls { get; set; }
        public List<Column> Columns { get; set; }
        public Applet()
        {
            Controls = new List<Control>();
            Columns = new List<Column>();
        }
    }
}
