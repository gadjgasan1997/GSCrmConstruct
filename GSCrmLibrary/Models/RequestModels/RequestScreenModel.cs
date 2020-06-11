using System;

namespace GSCrmLibrary.Models.RequestModels
{
    public class RequestScreenModel
    {
        public string OldScreenName { get; set; }
        public string NewScreenName { get; set; }
        public string CategoryName { get; set; }
        public Guid CrumbId { get; set; }
        public string NewViewName { get; set; }
        public string OldViewName { get; set; }
        public string Action { get; set; }
    }
}
