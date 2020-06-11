using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;
using System;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSApplicationItem : BUSEntity
    {
        public Application Application { get; set; }
        public Guid ApplicationId { get; set; }

        // Props
        public Screen Screen { get; set; }
        public Guid ScreenId { get; set; }
        public string ScreenName { get; set; }
    }
}
