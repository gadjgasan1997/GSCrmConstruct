using System.Collections.Generic;
using static GSCrm.CommonConsts;

namespace GSCrm.Data.ApplicationInfo
{
    public class ViewsInfo : IViewsInfo
    {
        private Dictionary<string, ViewInfo> viewsData { get; set; } = new Dictionary<string, ViewInfo>();

        public void Set(string viewName, ViewInfo viewInfo)
        {
            if (viewsData.ContainsKey(viewName))
                viewsData[viewName] = viewInfo;
            else viewsData.Add(viewName, viewInfo);
        }

        public ViewInfo Get(string viewName)
        {
            if (!viewsData.ContainsKey(viewName))
                viewsData.Add(viewName, new ViewInfo());
            return viewsData[viewName];
        }

        public Dictionary<string, ViewInfo> Get() => viewsData;

        public void Reset(string viewName, int currentPageNumber = DEFAULT_MIN_PAGE_NUMBER)
        {
            ViewInfo viewInfo = viewsData.GetValueOrDefault(viewName);
            if (viewInfo != null)
            {
                viewInfo.CurrentPageNumber = currentPageNumber;
                viewInfo.SkipItemsCount = default;
                viewInfo.SkipSteps = default;
            }
        }
    }
}
