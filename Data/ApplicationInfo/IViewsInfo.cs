using System.Collections.Generic;
using static GSCrm.CommonConsts;

namespace GSCrm.Data.ApplicationInfo
{
    public interface IViewsInfo
    {
        void Set(string viewName, ViewInfo viewInfo);
        ViewInfo Get(string viewName);
        Dictionary<string, ViewInfo> Get();
        void Reset(string viewName, int currentPageNumber = DEFAULT_MIN_PAGE_NUMBER);
    }
}
