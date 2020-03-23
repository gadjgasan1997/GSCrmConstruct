using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Factories.Default.MainFactories;
using GSCrm.Models.Default.TableModels;
using GSCrm.Data;
using GSCrm.Services.Info;
using System.Linq;
using System.Collections.Generic;
using GSCrm.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIJoinFR : MainBUSUIFR<BUSJoin, UIJoin>
    {
        public override BUSJoin UIToBusiness(UIJoin UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BusComp busComp = context.BusinessComponents
                .Include(j => j.Joins)
                .FirstOrDefault(i => i.Id.ToString() == ComponentContext.GetSelectedRecord("Business Component"));
            BUSJoin businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && busComp != null && busComp.Joins.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Record with this name is already exists.";
            }
            else
            {
                // BusComp
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp.Id;

                // Table
                Table table = context.Tables.FirstOrDefault(n => n.Name == UIEntity.TableName);
                if (table != null)
                {
                    businessEntity.Table = table;
                    businessEntity.TableId = table.Id;
                    businessEntity.TableName = table.Name;
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
        public override BUSJoin Init()
        {
            BUSJoin businessEntity = base.Init();
            businessEntity.TableName = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSJoin businessComponent, UIJoin UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.TableName))
                result.Add(new ValidationResult(
                    "Table name is a required field.",
                    new List<string>() { "TableName" }
                    ));
            if (businessComponent.Table == null && !string.IsNullOrWhiteSpace(UIEntity.TableName))
                result.Add(new ValidationResult(
                    "Table with this name not found.",
                    new List<string>() { "TableName" }
                    ));
            return result;
        }
    }
}
