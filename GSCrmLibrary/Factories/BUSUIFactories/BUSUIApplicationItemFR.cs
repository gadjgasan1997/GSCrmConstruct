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
using System.Linq.Dynamic.Core;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIApplicationItemFR<TContext> : BUSUIFactory<BUSApplicationItem, UIApplicationItem, TContext>
        where TContext : MainContext, new()
    {
        public override BUSApplicationItem UIToBusiness(UIApplicationItem UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSApplicationItem businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Application application = context.Applications
                .AsNoTracking()
                .Select(app => new 
                {
                    id = app.Id,
                    name = app.Name,
                    items = app.ApplicationItems.Select(item => new
                    {
                        id = item.Id,
                        name = item.Name
                    })
                })
                .Select(app => new Application
                {
                    Id = app.id,
                    Name = app.name,
                    ApplicationItems = app.items.Select(item => new ApplicationItem
                    {
                        Id = item.id,
                        Name = item.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Application"));
            if (application == null)
                businessEntity.ErrorMessage = "First you need create application.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                ApplicationItem applicationItem = application?.ApplicationItems.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (applicationItem != null && applicationItem.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Application item with this name is already exists in application {application.Name}";

                else
                {
                    businessEntity.Application = application;
                    businessEntity.ApplicationId = application.Id;

                    // Экран
                    Screen screen = context.Screens.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.ScreenName);
                    if (screen != null)
                    {
                        businessEntity.Screen = screen;
                        businessEntity.ScreenId = screen.Id;
                        businessEntity.ScreenName = screen.Name;
                    }
                }
            }
            return businessEntity;
        }

        public override UIApplicationItem BusinessToUI(BUSApplicationItem businessEntity)
        {
            UIApplicationItem UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ScreenName = businessEntity.ScreenName;
            return UIEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIApplicationItem UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.ScreenName))
                result.Add(new ValidationResult(
                    "Screen name is a required field.",
                    new List<string>() { "ScreenName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSApplicationItem businessComponent, UIApplicationItem UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.Screen == null)
                    result.Add(new ValidationResult(
                        "Screen with this name not found.",
                        new List<string>() { "ScreenName" }));
            }
            return result;
        }
    }
}
