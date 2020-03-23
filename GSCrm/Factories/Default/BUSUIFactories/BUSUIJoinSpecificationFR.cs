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
    public class BUSUIJoinSpecificationFR : MainBUSUIFR<BUSJoinSpecification, UIJoinSpecification>
    {
        public override BUSJoinSpecification UIToBusiness(UIJoinSpecification UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            Join join = context.Joins
                .Include(t => t.Table)
                    .ThenInclude(tc => tc.TableColumns)
                .Include(bc => bc.BusComp)
                    .ThenInclude(t => t.Table)
                        .ThenInclude(tc => tc.TableColumns)
                .Include(bc => bc.BusComp)
                    .ThenInclude(f => f.Fields)
                .Include(js => js.JoinSpecifications)
                .FirstOrDefault(i => i.Id.ToString() == ComponentContext.GetSelectedRecord("Join"));
            BUSJoinSpecification businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && join != null && join.JoinSpecifications.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Record with this name is already exists.";
            }
            else
            {
                // BusComp
                BusComp busComp = join.BusComp;

                // Join
                businessEntity.Join = join;
                businessEntity.JoinId = join.Id;

                // Source field
                Field field = busComp.Fields.FirstOrDefault(n => n.Name == UIEntity.SourceFieldName);
                if (field != null)
                {
                    businessEntity.SourceField = field;
                    businessEntity.SourceFieldId = field.Id;
                    businessEntity.SourceFieldName = field.Name;
                }

                // Destination column
                TableColumn destinationColumn = join.Table.TableColumns.FirstOrDefault(n => n.Name == UIEntity.DestinationColumnName);
                if (destinationColumn != null)
                {
                    businessEntity.DestinationColumn = destinationColumn;
                    businessEntity.DestinationColumnId = destinationColumn.Id;
                    businessEntity.DestinationColumnName = destinationColumn.Name;
                }
            }
            return businessEntity;
        }
        public override UIJoinSpecification BusinessToUI(BUSJoinSpecification businessEntity)
        {
            UIJoinSpecification UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.SourceFieldName = businessEntity.SourceFieldName;
            UIEntity.DestinationColumnName = businessEntity.DestinationColumnName;
            return UIEntity;
        }
        public override BUSJoinSpecification Init()
        {
            BUSJoinSpecification businesEntity = base.Init();
            businesEntity.SourceFieldName = "";
            businesEntity.DestinationColumnName = "";
            return businesEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSJoinSpecification businessComponent, UIJoinSpecification UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.SourceFieldName))
                result.Add(new ValidationResult(
                    "Source field name is a required field.",
                    new List<string>() { "SourceFieldName" }
                    ));
            if (businessComponent.SourceField == null && !string.IsNullOrWhiteSpace(UIEntity.SourceFieldName))
                result.Add(new ValidationResult(
                    "Source field with this name not found.",
                    new List<string>() { "SourceFieldName" }
                    ));
            if (string.IsNullOrWhiteSpace(UIEntity.DestinationColumnName))
                result.Add(new ValidationResult(
                    "Destination column name is a required field.",
                    new List<string>() { "DestinationColumnName" }
                    ));
            if (businessComponent.DestinationColumn == null && !string.IsNullOrWhiteSpace(UIEntity.DestinationColumnName))
                result.Add(new ValidationResult(
                    "Destination column with this name not found.",
                    new List<string>() { "SourceFieldName" }
                    ));
            return result;
        }
    }
}