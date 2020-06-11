using System;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.TableModels
{
    public class ScreenItem : DataEntity
    {
        [ForeignKey("ScreenId")]
        public Screen Screen { get; set; }
        public Guid ScreenId { get; set; }
        public View View { get; set; }
        public Guid? ViewId { get; set; }
        [ForeignKey("ParentCategoryId")]
        public ScreenItem ParentCategory { get; set; }
        public Guid? ParentCategoryId { get; set; }
        [ForeignKey("ParentItemId")]
        public ScreenItem ParentItem { get; set; }
        public Guid? ParentItemId { get; set; }
        public string Header { get; set; }
        public string Type { get; set; }
        public bool DisplayInSiteMap { get; set; } = true;
    }
}