using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIBusObjectFR<TContext> : BUSUIFactory<BUSBusinessObject, UIBusinessObject, TContext>
        where TContext : MainContext, new()
    {
        public override UIBusinessObject BusinessToUI(BUSBusinessObject businessEntity)
        {
            UIBusinessObject UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.PrimaryBusCompName = businessEntity.PrimaryBusCompName;
            return UIEntity;
        }
        public override BUSBusinessObject UIToBusiness(UIBusinessObject UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSBusinessObject businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            BusinessComponent primaryBusComp = context.BusinessComponents.FirstOrDefault(n => n.Name == UIEntity.PrimaryBusCompName);
            if (primaryBusComp != null)
            {
                businessEntity.PrimaryBusComp = primaryBusComp;
                businessEntity.PrimaryBusCompId = primaryBusComp.Id;
                businessEntity.PrimaryBusCompName = primaryBusComp.Name;
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIBusinessObject UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            BusinessObject businessObject = context.BusinessObjects.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (businessObject != null && businessObject.Id != UIEntity.Id)
                result.Add(new ValidationResult("Business object with this name is already exists.", new List<string>() { "Name" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSBusinessObject businessComponent, UIBusinessObject UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (!string.IsNullOrWhiteSpace(UIEntity.PrimaryBusCompName) && businessComponent.PrimaryBusComp == null)
            {
                result.Add(new ValidationResult(
                    "Business component with this name not found.",
                    new List<string>() { "PrimaryBusCompName" }));
            }
            return result;
        }
    }
}
