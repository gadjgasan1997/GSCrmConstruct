using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIApplicationFR<TContext> : BUSUIFactory<BUSApplication, UIApplication, TContext>
        where TContext : MainContext, new()
    {
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIApplication UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            Application application = context.Applications.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (application != null && application.Id != UIEntity.Id)
                result.Add(new ValidationResult("Application with this name is already exists.", new List<string>() { "Name" }));
            return result;
        }
    }
}
