using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIViewItemFR<TContext> : BUSUIFactory<BUSViewItem, UIViewItem, TContext>
        where TContext : MainContext, new()
    {
        public override UIViewItem BusinessToUI(BUSViewItem businessEntity)
        {
            UIViewItem UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.AppletName = businessEntity.AppletName;
            UIEntity.Autofocus = businessEntity.Autofocus;
            UIEntity.AutofocusRecord = businessEntity.AutofocusRecord;
            UIEntity.Type = businessEntity.Applet?.Type;
            return UIEntity;
        }
        public override BUSViewItem UIToBusiness(UIViewItem UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSViewItem businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            View view = context.Views
                .AsNoTracking()
                .Select(v => new
                {
                    id = v.Id,
                    name = v.Name,
                    viewItems = v.ViewItems.Select(viewItem => new
                    {
                        id = viewItem.Id,
                        name = viewItem.Name,
                    })
                })
                .Select(v => new View
                {
                    Id = v.id,
                    Name = v.name,
                    ViewItems = v.viewItems.Select(viewItem => new ViewItem
                    {
                        Id = viewItem.id,
                        Name = viewItem.name
                    }).ToList()
                })
                .FirstOrDefault(n => n.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("View"));
            if (view == null)
                businessEntity.ErrorMessage = "First you need create view.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                ViewItem viewItem = view.ViewItems?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (viewInfo.ViewItems != null && viewItem != null && viewItem.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"View item with this name is already exists in view {view.Name}.";
                else
                {
                    businessEntity.View = view;
                    businessEntity.ViewId = view.Id;
                
                    Applet applet = context.Applets.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.AppletName);
                    if (applet != null)
                    {
                        businessEntity.Applet = applet;
                        businessEntity.AppletId = applet.Id;
                        businessEntity.AppletName = applet.Name;
                    }

                    businessEntity.Autofocus = UIEntity.Autofocus.ToString() == string.Empty || UIEntity.Autofocus;
                    businessEntity.AutofocusRecord = UIEntity.AutofocusRecord.ToString() == string.Empty ? 0 : UIEntity.AutofocusRecord;
                }
            }

            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIViewItem UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.AppletName))
                result.Add(new ValidationResult(
                    "Applet name is a required field.",
                    new List<string>() { "AppletName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSViewItem businessComponent, UIViewItem UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.Applet == null)
                    result.Add(new ValidationResult(
                        "Applet with this name not found.",
                        new List<string>() { "AppletName" }));
            }
            return result;
        }
    }
}