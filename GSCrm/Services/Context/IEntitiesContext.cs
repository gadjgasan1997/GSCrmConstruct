using GSCrm.Models.Default.MainEntities;
using System.Collections.Generic;

namespace GSCrm.Services.Context
{
    public interface IEntitiesContext
    {
        Dictionary<string, IEnumerable<MainTable>> ContextDictionary { get; set; }
    }
}
