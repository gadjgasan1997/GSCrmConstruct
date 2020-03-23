using GSCrm.Models.Default.MainEntities;
using GSCrm.Models.Default.TableModels;
using System;

namespace GSCrm.Models.Default.BusinessComponentModels
{
    public class BUSScreenItem : MainBusinessComponent
    {
        public Guid ScreenId { get; set; }
        public Screen Screen { get; set; }
        public Guid? ViewId { get; set; }
        public View View { get; set; }
        public string ViewName { get; set; }
        public string ParentCategory { get; set; }
        public string ParentItem { get; set; }
    }
}
