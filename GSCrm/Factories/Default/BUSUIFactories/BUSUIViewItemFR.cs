using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIViewItemFR : MainBUSUIFR<BUSViewItem, UIViewItem>
    {
        public override UIViewItem BusinessToUI(BUSViewItem businessEntity)
        {
            UIViewItem UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.AppletName = businessEntity.AppletName;
            UIEntity.Autofocus = businessEntity.Autofocus;
            UIEntity.AutofocusRecord = businessEntity.AutofocusRecord;
            return UIEntity;
        }
        public override BUSViewItem UIToBusiness(UIViewItem UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            View view = context.Views
                .Include(vi => vi.ViewItems)
                .FirstOrDefault(n => n.Id.ToString() == ComponentContext.GetSelectedRecord("View"));
            BUSViewItem businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord  && view != null && viewInfo.ViewItems != null && 
                view.ViewItems.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "View item with this name is already exists in view " + view.Name + ".";
            }
            else
            {
                businessEntity.View = view;
                businessEntity.ViewId = view.Id;
                businessEntity.AppletName = UIEntity.AppletName;
                Applet applet = context.Applets.FirstOrDefault(n => n.Name == UIEntity.AppletName);
                if (applet != null)
                {
                    businessEntity.Applet = applet;
                    businessEntity.AppletId = applet.Id;
                }

                businessEntity.Autofocus = UIEntity.Autofocus.ToString() == string.Empty ? true : UIEntity.Autofocus;
                businessEntity.AutofocusRecord = UIEntity.AutofocusRecord.ToString() == string.Empty ? 0 : UIEntity.AutofocusRecord;
            }
            return businessEntity;
        }
        public override BUSViewItem Init()
        {
            BUSViewItem businessEntity = base.Init();
            businessEntity.AppletName = "";
            businessEntity.Autofocus = true;
            businessEntity.AutofocusRecord = 0;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSViewItem businessComponent, UIViewItem UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.ErrorMessage == string.Empty)
            {
                if (string.IsNullOrWhiteSpace(UIEntity.AppletName))
                    result.Add(new ValidationResult(
                        "Applet name is a required field.",
                        new List<string>() { "AppletName" }
                        ));
                if (businessComponent.Applet == null && !string.IsNullOrWhiteSpace(UIEntity.AppletName))
                    result.Add(new ValidationResult(
                        "Applet with this name not found.",
                        new List<string>() { "AppletName" }
                        ));
            }
            return result;
        }
    }
}