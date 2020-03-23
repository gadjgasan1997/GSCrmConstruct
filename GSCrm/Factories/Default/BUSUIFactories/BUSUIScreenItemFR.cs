using GSCrm.Data;
using GSCrm.Data.Context;
using GSCrm.Factories.Default.MainFactories;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using GSCrm.Services.Info;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIScreenItemFR : MainBUSUIFR<BUSScreenItem, UIScreenItem>
    {
        public override UIScreenItem BusinessToUI(BUSScreenItem businessEntity)
        {
            UIScreenItem UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ViewName = businessEntity.ViewName;
            UIEntity.ParentCategory = businessEntity.ParentCategory;
            UIEntity.ParentItem = businessEntity.ParentItem;
            return UIEntity;
        }
        public override BUSScreenItem UIToBusiness(UIScreenItem UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSScreenItem businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Screen
            Screen screen = context.Screens.FirstOrDefault(i => i.Id.ToString() == ComponentContext.GetSelectedRecord("Screen"));
            if (screen != null)
            {
                businessEntity.Screen = screen;
                businessEntity.ScreenId = screen.Id;
            }

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && screen != null && screen.ScreenItems != null &&
                screen.ScreenItems.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Screen item with this name is already exists in screen " + screen.Name + ".";
            }
            else
            {
                // View
                View view = context.Views.FirstOrDefault(n => n.Name == UIEntity.ViewName);
                if (view != null)
                {
                    businessEntity.View = view;
                    businessEntity.ViewId = view.Id;
                    businessEntity.ViewName = view.Name;
                }
                businessEntity.ParentItem = UIEntity.ParentItem;
                businessEntity.ParentCategory = UIEntity.ParentCategory;
            }
            return businessEntity;
        }
        public override BUSScreenItem Init()
        {
            BUSScreenItem businessEntity = base.Init();
            businessEntity.ViewName = "";
            businessEntity.ParentCategory = "";
            businessEntity.ParentItem = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSScreenItem businessComponent, UIScreenItem UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.View == null  && !string.IsNullOrWhiteSpace(UIEntity.ViewName))
                result.Add(new ValidationResult(
                    "View with this name not found.",
                    new List<string>() { "ViewName" }
                    ));
            return result;
        }
    }
}
