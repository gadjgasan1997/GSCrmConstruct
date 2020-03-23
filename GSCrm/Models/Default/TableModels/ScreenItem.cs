using GSCrm.Models.Default.MainEntities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSCrm.Models.Default.TableModels
{
    // Элемент экрана
    public class ScreenItem : MainTable
    {
        // Screen
        [ForeignKey("ScreenId")]
        public Screen Screen { get; set; }
        public Guid ScreenId { get; set; }

        // View
        [ForeignKey("ViewId")]
        public View View { get; set; }
        public Guid? ViewId { get; set; }

        // Props
        public string Header { get; set; }
        public string ParentCategory { get; set; } = null;
        public string ParentItem { get; set; } = null;
    }
}