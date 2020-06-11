using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSScreenItemFR<TContext> : DataBUSFactory<ScreenItem, BUSScreenItem, TContext>
        where TContext : MainContext, new()
    {
        public override BUSScreenItem DataToBusiness(ScreenItem dataEntity, TContext context)
        {
            BUSScreenItem businessEntity = base.DataToBusiness(dataEntity, context);
            Screen screen = context.Screens
                .AsNoTracking()
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    screenItems = s.ScreenItems.Select(screenItem => new
                    {
                        id = screenItem.Id,
                        name = screenItem.Name,
                    })
                })
                .Select(s => new Screen
                {
                    Id = s.id,
                    Name = s.name,
                    ScreenItems = s.screenItems.Select(screenItem => new ScreenItem
                    {
                        Id = screenItem.id,
                        Name = screenItem.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == dataEntity.ScreenId);

            businessEntity.Screen = screen;
            businessEntity.ScreenId = screen.Id;
            businessEntity.ScreenName = screen.Name;

            View view = context.Views.FirstOrDefault(i => i.Id == dataEntity.ViewId);
            if (view != null)
            {
                businessEntity.View = view;
                businessEntity.ViewId = view.Id;
                businessEntity.ViewName = view.Name;
            }

            ScreenItem parentCategory = screen.ScreenItems.FirstOrDefault(i => i.Id == dataEntity.ParentCategoryId);
            if (parentCategory != null)
            {
                businessEntity.ParentCategory = parentCategory;
                businessEntity.ParentCategoryId = parentCategory.Id;
                businessEntity.ParentCategoryName = parentCategory.Name;
            }

            ScreenItem parentItem = screen.ScreenItems.FirstOrDefault(i => i.Id == dataEntity.ParentItemId);
            if (parentItem != null)
            {
                businessEntity.ParentItem = parentItem;
                businessEntity.ParentItemId = parentItem.Id;
                businessEntity.ParentItemName = parentItem.Name;
            }

            businessEntity.DisplayInSiteMap = dataEntity.DisplayInSiteMap;
            businessEntity.Header = dataEntity.Header;
            businessEntity.Type = dataEntity.Type;
            return businessEntity;
        }
        public override ScreenItem BusinessToData(ScreenItem screenItem, BUSScreenItem businessEntity, TContext context, bool NewRecord)
        {
            ScreenItem dataEntity = base.BusinessToData(screenItem, businessEntity, context, NewRecord);
            dataEntity.Screen = businessEntity.Screen;
            dataEntity.ScreenId = businessEntity.ScreenId;
            dataEntity.View = businessEntity.View;
            dataEntity.ViewId = businessEntity.ViewId;
            dataEntity.ParentCategory = businessEntity.ParentCategory;
            dataEntity.ParentCategoryId = businessEntity.ParentCategoryId;
            dataEntity.ParentItem = businessEntity.ParentItem;
            dataEntity.ParentItemId = businessEntity.ParentItemId;
            dataEntity.Header = businessEntity.Header;
            dataEntity.Type = businessEntity.Type;
            dataEntity.DisplayInSiteMap = businessEntity.DisplayInSiteMap;
            return dataEntity;
        }
    }
}