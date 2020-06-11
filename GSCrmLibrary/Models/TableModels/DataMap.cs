using GSCrmLibrary.Models.MainEntities;
using System.Collections.Generic;

namespace GSCrmLibrary.Models.TableModels
{
    public class DataMap : DataEntity
    {
        public string Routing { get; set; }
        public List<DataMapObject> DataMapObjects { get; set; }
    }
}
