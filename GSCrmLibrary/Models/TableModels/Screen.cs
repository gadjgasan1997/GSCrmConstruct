using GSCrmLibrary.Models.MainEntities;
using System.Collections.Generic;

namespace GSCrmLibrary.Models.TableModels
{
    public class Screen : DataEntity
    {
        public string Header { get; set; }
        public string Routing { get; set; }
        public List<ScreenItem> ScreenItems { get; set; }
        public Screen()
        {
            ScreenItems = new List<ScreenItem>();
        }
    }
}
