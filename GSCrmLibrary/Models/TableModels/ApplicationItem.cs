using System;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.TableModels
{
    public class ApplicationItem : DataEntity
    {
        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
        public Guid ApplicationId { get; set; }
        public Screen Screen { get; set; }
        public Guid ScreenId { get; set; }
    }
}
