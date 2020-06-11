using System;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSScreenItem : BUSEntity
    {
        public Screen Screen { get; set; }
        public Guid ScreenId { get; set; }
        public string ScreenName { get; set; }
        public View View { get; set; }
        public Guid? ViewId { get; set; }
        public string ViewName { get; set; }
        public ScreenItem ParentCategory { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public ScreenItem ParentItem { get; set; }
        public Guid? ParentItemId { get; set; }
        public string ParentItemName { get; set; }
        public string Header { get; set; }
        public string Type { get; set; }
        public bool DisplayInSiteMap { get; set; } = true;
    }
}
