using GSCrm.Models.ViewModels;
using System.Collections.Generic;

namespace GSCrm.Data.ApplicationInfo
{
    public static class ModelCash<TViewModel>
        where TViewModel : BaseViewModel, new()
    {
        private static Dictionary<string, TViewModel> modelsCash = new Dictionary<string, TViewModel>();

        public static TViewModel GetViewModel(string viewName)
        {
            if (!modelsCash.ContainsKey(viewName))
                modelsCash.Add(viewName, new TViewModel());
            return modelsCash[viewName];
        }

        public static void SetViewModel(string viewName, TViewModel viewModel)
        {
            if (modelsCash.ContainsKey(viewName))
                modelsCash[viewName] = viewModel;
            else modelsCash.Add(viewName, viewModel);
        }

        public static void RemoveViewModel(string viewName) => modelsCash.Remove(viewName);
    }
}
