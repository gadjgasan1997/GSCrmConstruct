using GSCrm.Models.Default.TableModels;
using Action = GSCrm.Models.Default.TableModels.Action;
using System;

namespace GSCrm.Models.Default.MainEntities
{
    // Основной класс для пользовательских настроек бизнес уровня
    public class MainBUSUP : MainBusinessComponent
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
