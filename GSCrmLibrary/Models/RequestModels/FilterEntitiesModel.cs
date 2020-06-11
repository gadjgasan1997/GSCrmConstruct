using System.Collections.Generic;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Models.RequestModels
{
    public class FilterEntitiesModel
    {
        public List<BusinessObjectComponent> BOComponents { get; set; }
        public Link Link { get; set; }
        public BusinessComponent BusComp { get; set; }
    }
}
