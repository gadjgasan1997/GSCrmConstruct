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
using System;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIBusCompFR<TContext> : BUSUIFactory<BUSBusinessComponent, UIBusinessComponent, TContext>
        where TContext : MainContext, new()
    {
        public override UIBusinessComponent BusinessToUI(BUSBusinessComponent businessEntity)
        {
            UIBusinessComponent UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.TableName = businessEntity.TableName;
            UIEntity.ShadowCopy = businessEntity.ShadowCopy;
            return UIEntity;
        }
        public override BUSBusinessComponent UIToBusiness(UIBusinessComponent UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSBusinessComponent businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Table table = context.Tables.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.TableName);
            if (table != null)
            {
                businessEntity.Table = table;
                businessEntity.TableId = table.Id;
                businessEntity.TableName = table.Name;
            }
            else
            {
                businessEntity.Table = null;
                businessEntity.TableId = Guid.Empty;
            }
            businessEntity.ShadowCopy = UIEntity.ShadowCopy;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIBusinessComponent UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            BusinessComponent businessComponent = context.BusinessComponents.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (businessComponent != null && businessComponent.Id != UIEntity.Id)
                result.Add(new ValidationResult("Business component with this name is already exists.", new List<string>() { "Name" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSBusinessComponent businessComponent, UIBusinessComponent UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.Table == null && !string.IsNullOrWhiteSpace(UIEntity.TableName))
                    result.Add(new ValidationResult("Table with this name not found.", new List<string>() { "TableName" }));
            }
            return result;
        }
    }
}
