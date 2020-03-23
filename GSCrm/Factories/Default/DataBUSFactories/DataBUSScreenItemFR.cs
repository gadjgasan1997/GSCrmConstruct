using GSCrm.Data;
using GSCrm.Factories.Default.MainFactories;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSScreenItemFR : MainDataBUSFR<ScreenItem, BUSScreenItem>
    {
        public override ScreenItem BusinessToData(BUSScreenItem businessEntity, DbSet<ScreenItem> entityDBSet, bool NewRecord)
        {
            ScreenItem dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Screen = businessEntity.Screen;
            dataEntity.ScreenId = businessEntity.ScreenId;
            dataEntity.View = businessEntity.View;
            dataEntity.ViewId = businessEntity.ViewId;
            dataEntity.ParentCategory = businessEntity.ParentCategory;
            dataEntity.ParentItem = businessEntity.ParentItem;
            dataEntity.Header = businessEntity.Header;
            return dataEntity;
        }
        public override BUSScreenItem DataToBusiness(ScreenItem dataEntity, ApplicationContext context)
        {
            BUSScreenItem businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.Screen = dataEntity.Screen;
            businessEntity.ScreenId = dataEntity.ScreenId;
            View view = context.Views.FirstOrDefault(i => i.Id == dataEntity.ViewId);
            if (view != null)
            {
                businessEntity.View = dataEntity.View;
                businessEntity.ViewId = dataEntity.ViewId;
                businessEntity.ViewName = dataEntity.View.Name;
            }
            businessEntity.ParentCategory = dataEntity.ParentCategory;
            businessEntity.ParentItem = dataEntity.ParentItem;
            businessEntity.Header = dataEntity.Header;
            return businessEntity;
        }
    }
}