using GSCrm.Models.Default.MainEntities;
using System.Collections.Generic;

namespace GSCrm.Models.Default.TableModels
{
    // Таблица
    public class Table : MainTable
    {
        // Props
        public List<TableColumn> TableColumns { get; set; }
        public Table()
        {
            TableColumns = new List<TableColumn>();
        }
    }
}
