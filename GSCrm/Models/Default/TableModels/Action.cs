using GSCrm.Models.Default.MainEntities;
using System.Collections.Generic;

namespace GSCrm.Models.Default.TableModels
{
    // Действие, происходяшее при нажатии на контрол или колонку апплета
    public class Action : MainTable
    {
        // Props
        public List<ActionUP> ActionUPs { get; set; }
        public List<Control> Controls { get; set; }
        public List<Column> Columns { get; set; }
        public Action()
        {
            ActionUPs = new List<ActionUP>();
            Controls = new List<Control>();
            Columns = new List<Column>();
        }
    }
}