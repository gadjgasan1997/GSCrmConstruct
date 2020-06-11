using GSCrmLibrary.Models.MainEntities;
using System.Collections.Generic;

namespace GSCrmLibrary.Models.TableModels
{
    public class Application : DataEntity
    {
        public List<ApplicationItem> ApplicationItems { get; set; }
        public Application()
        {
            ApplicationItems = new List<ApplicationItem>();
        }
    }
}
