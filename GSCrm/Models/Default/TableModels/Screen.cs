using GSCrm.Models.Default.MainEntities;
using System.Collections.Generic;

namespace GSCrm.Models.Default.TableModels
{
    // Экран
    public class Screen : MainTable
    {
        // Props
        public List<ScreenItem> ScreenItems { get; set; }
        public Screen()
        {
            ScreenItems = new List<ScreenItem>();
        }
    }
}
