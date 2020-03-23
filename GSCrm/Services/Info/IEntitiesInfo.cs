using GSCrm.Data;
using System.Collections.Generic;

namespace GSCrm.Services.Info
{
    public interface IEntitiesInfo
    {
        Dictionary<string, string> ViewRouting { get; }
        Dictionary<string, string> AppletRouting { get; }
    }
}
