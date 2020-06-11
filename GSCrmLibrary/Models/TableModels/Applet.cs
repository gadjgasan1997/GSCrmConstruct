using System;
using System.Collections.Generic;
using GSCrmLibrary.Models.MainEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrmLibrary.Models.TableModels
{
    public class Applet : DataEntity
    {
        [ForeignKey("PhysicalRenderId")]
        public PhysicalRender PhysicalRender { get; set; }
        public Guid PhysicalRenderId { get; set; }
        [ForeignKey("BusCompId")]
        public BusinessComponent BusComp { get; set; }
        public Guid? BusCompId { get; set; }
        public string EmptyState { get; set; }
        public int DisplayLines { get; set; }
        public bool Initflag { get; set; }
        public string Routing { get; set; }
        public string Header { get; set; }
        public string Type { get; set; }
        public bool Virtual { get; set; }
        public List<Control> Controls { get; set; }
        public List<Column> Columns { get; set; }
        public List<Drilldown> Drilldowns { get; set; }
        public Applet()
        {
            Controls = new List<Control>();
            Columns = new List<Column>();
            Drilldowns = new List<Drilldown>();
        }
    }
}
