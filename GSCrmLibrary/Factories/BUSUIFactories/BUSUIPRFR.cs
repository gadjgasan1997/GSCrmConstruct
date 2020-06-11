using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIPRFR<TContext> : BUSUIFactory<BUSPhysicalRender, UIPhysicalRender, TContext>
        where TContext : MainContext, new()
    {
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIPhysicalRender UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            PhysicalRender physicalRender = context.PhysicalRenders.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (physicalRender != null && physicalRender.Id != UIEntity.Id)
                result.Add(new ValidationResult("Physical render with this name is already exists.", new List<string>() { "Name" }));
            return result;
        }
    }
}
