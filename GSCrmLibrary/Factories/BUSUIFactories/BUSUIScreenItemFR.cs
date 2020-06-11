using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Services.Info;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIScreenItemFR<TContext> : BUSUIFactory<BUSScreenItem, UIScreenItem, TContext>
        where TContext : MainContext, new()
    {
        public override UIScreenItem BusinessToUI(BUSScreenItem businessEntity)
        {
            UIScreenItem UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ScreenName = businessEntity.ScreenName;
            UIEntity.ViewName = businessEntity.ViewName;
            UIEntity.ParentCategoryName = businessEntity.ParentCategoryName;
            UIEntity.ParentItemName = businessEntity.ParentItemName;
            UIEntity.Header = businessEntity.Header;
            UIEntity.Type = businessEntity.Type;
            UIEntity.DisplayInSiteMap = businessEntity.DisplayInSiteMap;
            return UIEntity;
        }
        public override BUSScreenItem UIToBusiness(UIScreenItem UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSScreenItem businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);

            // Screen
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
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Screen"));
            if(screen == null)
                businessEntity.ErrorMessage = "First you need create screen.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                ScreenItem screenItem = screen.ScreenItems?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (screen.ScreenItems != null && screenItem != null && screenItem.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Screen item with this name is already exists in screen {screen.Name}.";
                else
                {
                    businessEntity.Screen = screen;
                    businessEntity.ScreenId = screen.Id;
                    businessEntity.ScreenName = screen.Name;

                    // View
                    View view = context.Views.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.ViewName);
                    if (view != null)
                    {
                        businessEntity.View = view;
                        businessEntity.ViewId = view.Id;
                        businessEntity.ViewName = view.Name;
                    }

                    ScreenItem parentCategory = screen.ScreenItems.FirstOrDefault(n => n.Name == UIEntity.ParentCategoryName);
                    if (parentCategory != null)
                    {
                        businessEntity.ParentCategory = parentCategory;
                        businessEntity.ParentCategoryId = parentCategory.Id;
                        businessEntity.ParentCategoryName = parentCategory.Name;
                    }

                    ScreenItem parentItem = screen.ScreenItems.FirstOrDefault(n => n.Name == UIEntity.ParentItemName);
                    if (parentItem != null)
                    {
                        businessEntity.ParentItem = parentItem;
                        businessEntity.ParentItemId = parentItem.Id;
                        businessEntity.ParentItemName = parentItem.Name;
                    }
                }

                businessEntity.DisplayInSiteMap = UIEntity.DisplayInSiteMap;
                businessEntity.Header = UIEntity.Header;
                businessEntity.Type = UIEntity.Type;
            }

            return businessEntity;
        }
        public override BUSScreenItem Init(TContext context)
        {
            BUSScreenItem businessEntity = base.Init(context);
            Screen screen = context.Screens.AsNoTracking().FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Screen"));
            if (screen != null)
            {
                businessEntity.Screen = screen;
                businessEntity.ScreenId = screen.Id;
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSScreenItem businessComponent, UIScreenItem UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.View == null && !string.IsNullOrWhiteSpace(UIEntity.ViewName))
                    result.Add(new ValidationResult(
                        "View with this name not found.",
                        new List<string>() { "ViewName" }));
                if (businessComponent.ParentCategory == null && !string.IsNullOrWhiteSpace(UIEntity.ParentCategoryName))
                    result.Add(new ValidationResult(
                        "Screen category with this name not found.",
                        new List<string>() { "ParentCategory" }));
                if (businessComponent.ParentItem == null && !string.IsNullOrWhiteSpace(UIEntity.ParentItemName))
                    result.Add(new ValidationResult(
                        "Screen item with this name not found.",
                        new List<string>() { "ParentItem" }));
            }
            return result;
        }
    }
}
