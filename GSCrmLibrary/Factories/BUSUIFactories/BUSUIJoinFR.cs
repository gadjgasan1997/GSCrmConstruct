using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIJoinFR<TContext> : BUSUIFactory<BUSJoin, UIJoin, TContext>
        where TContext : MainContext, new()
    {
        public override BUSJoin UIToBusiness(UIJoin UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSJoin businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            BusinessComponent busComp = context.BusinessComponents
                .AsNoTracking()
                .Select(bc => new
                {
                    id = bc.Id,
                    name = bc.Name,
                    joins = bc.Joins.Select(join => new
                    {
                        id = join.Id,
                        name = join.Name
                    })
                })
                .Select(bc => new BusinessComponent
                {
                    Id = bc.id,
                    Name = bc.name,
                    Joins = bc.joins.Select(join => new Join
                    {
                        Id = join.id,
                        Name = join.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Business Component"));
            if(busComp == null)
                businessEntity.ErrorMessage = "First you need create business component.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                Join join = busComp.Joins?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (join != null && join.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Join with this name is already exists in business component {busComp.Name}.";
                else
                {
                    // BusComp
                    businessEntity.BusComp = busComp;
                    businessEntity.BusCompId = busComp.Id;

                    // Table
                    Table table = context.Tables.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.TableName);
                    if (table != null)
                    {
                        businessEntity.Table = table;
                        businessEntity.TableId = table.Id;
                        businessEntity.TableName = table.Name;
                    }
                }
            }
            return businessEntity;
        }
        public override UIJoin BusinessToUI(BUSJoin businessEntity)
        {
            UIJoin UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.TableName = businessEntity.TableName;
            return UIEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIJoin UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.TableName))
                result.Add(new ValidationResult(
                    "Table name is a required field.",
                    new List<string>() { "TableName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSJoin businessComponent, UIJoin UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if ((string.IsNullOrWhiteSpace(businessComponent.ErrorMessage)))
            {
                if (businessComponent.Table == null)
                    result.Add(new ValidationResult(
                        "Table with this name not found.",
                        new List<string>() { "TableName" }));
            }
            return result;
        }
    }
}
