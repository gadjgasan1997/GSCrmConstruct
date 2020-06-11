using GSCrmLibrary.Models.MainEntities;
using System.Collections.Generic;

namespace GSCrmLibrary.Models.TableModels
{
    public class Table : DataEntity
    {
        public bool IsApply { get; set; }
        public List<TableColumn> TableColumns { get; set; }
        public Table()
        {
            TableColumns = new List<TableColumn>();
        }
    }
}
