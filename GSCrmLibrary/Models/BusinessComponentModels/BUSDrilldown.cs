using System;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Models.BusinessComponentModels
{
    public class BUSDrilldown : BUSEntity
    {
        public Applet Applet { get; set; }
        public Guid AppletId { get; set; }
        public BusinessComponent SourceBusinessComponent { get; set; }
        public Guid SourceBusinessComponentId { get; set; }
        public Field HyperLinkField { get; set; }
        public Guid HyperLinkFieldId { get; set; }
        public string HyperLinkFieldName { get; set; }
        public Screen DestinationScreen { get; set; }
        public Guid DestinationScreenId { get; set; }
        public string DestinationScreenName { get; set; }
        public ScreenItem DestinationScreenItem { get; set; }
        public Guid DestinationScreenItemId { get; set; }
        public string DestinationScreenItemName { get; set; }
        public BusinessComponent DestinationBusinessComponent { get; set; }
        public Guid DestinationBusinessComponentId { get; set; }
        public string DestinationBusinessComponentName { get; set; }
        public Field SourceField { get; set; }
        public Guid SourceFieldId { get; set; }
        public string SourceFieldName { get; set; }
        public Field DestinationField { get; set; }
        public Guid DestinationFieldId { get; set; }
        public string DestinationFieldName { get; set; }
    }
}
