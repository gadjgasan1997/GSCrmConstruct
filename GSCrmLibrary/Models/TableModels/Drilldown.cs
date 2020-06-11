using System;
using System.ComponentModel.DataAnnotations.Schema;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.TableModels
{
    public class Drilldown : DataEntity
    {
        [ForeignKey("AppletId")]
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        [ForeignKey("HyperLinkFieldId")]
        public Field HyperLinkField { get; set; }
        public Guid HyperLinkFieldId { get; set; }
        [ForeignKey("DestinationScreenId")]
        public Screen DestinationScreen { get; set; }
        public Guid DestinationScreenId { get; set; }
        [ForeignKey("DestinationScreenItemId")]
        public ScreenItem DestinationScreenItem { get; set; }
        public Guid DestinationScreenItemId { get; set; }
        [ForeignKey("DestinationBusinessComponentId")]
        public BusinessComponent DestinationBusinessComponent { get; set; }
        public Guid DestinationBusinessComponentId { get; set; }
        [ForeignKey("SourceFieldId")]
        public Field SourceField { get; set; }
        public Guid SourceFieldId { get; set; }
        [ForeignKey("DestinationFieldId")]
        public Field DestinationField { get; set; }
        public Guid DestinationFieldId { get; set; }
    }
}
