using GSCrm.Models.Default.TableModels;
using System;
using Action = GSCrm.Models.Default.TableModels.Action;

namespace GSCrm.Models.Default.MainEntities
{
    // Основной класс для пользовательских настроек уровня данных
    public class MainTableUP : MainTable
    {
        // Action
        public Action Action { get; set; }
        public Guid? ActionId { get; set; }

        // Control
        public Control Control { get; set; }
        public Guid? ControlId { get; set; }

        // Column
        public Column Column { get; set; }
        public Guid? ColumnId { get; set; }

        // Props
        public string Value { get; set; }
    }
}