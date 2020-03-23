using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIFieldFR : MainBUSUIFR<BUSField, UIField>
    {
        public override UIField BusinessToUI(BUSField businessEntity)
        {
            UIField UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.JoinName = businessEntity.JoinName;
            UIEntity.PickListName = businessEntity.PickListName;
            UIEntity.TableColumnName = businessEntity.Join == null ? businessEntity.TableColumnName : null;
            UIEntity.JoinColumnName = businessEntity.Join == null ? null : businessEntity.TableColumnName;
            return UIEntity;
        }
        public override BUSField UIToBusiness(UIField UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            // BusComp
            BusComp busComp = context.BusinessComponents
                .Include(t => t.Table)
                    .ThenInclude(tc => tc.TableColumns)
                .Include(f => f.Fields)
                .FirstOrDefault(n => n.Id.ToString() == ComponentContext.GetSelectedRecord("Business Component"));
            BUSField businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && busComp != null && busComp.Fields != null &&
                    busComp.Fields.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Field with this name is already exists in business component " + busComp.Name + ".";
            }

            else
            {
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp.Id;
                businessEntity.BusCompName = busComp.Name;

                // Join
                Join join = context.Joins
                    .Include(t => t.Table)
                        .ThenInclude(tc => tc.TableColumns)
                    .FirstOrDefault(n => n.Name == UIEntity.JoinName);

                if (join != null)
                {
                    businessEntity.Join = join;
                    businessEntity.JoinId = join.Id;
                }

                // Table column
                string tableColumnName = UIEntity.TableColumnName != null ? UIEntity.TableColumnName : UIEntity.JoinColumnName;
                if (tableColumnName != null)
                {
                    // Колонка в таблице может быть либо с той таблицы, на которой основана бк, либо с той таблицы, на которой основан join
                    Table table = join?.Table == null ? busComp.Table : join.Table;
                    businessEntity.Table = table;
                    if (table != null && table.TableColumns != null)
                    {
                        TableColumn tableColumn = table.TableColumns.FirstOrDefault(n => n.Name == tableColumnName);
                        if (tableColumn != null)
                        {
                            businessEntity.TableColumn = tableColumn;
                            businessEntity.TableColumnId = tableColumn.Id;
                            businessEntity.TableColumnName = tableColumnName;
                        }
                    }
                }

                // PickList
                if (UIEntity.PickListName != string.Empty)
                {
                    PL pickList = context.PickLists.FirstOrDefault(n => n.Name == UIEntity.PickListName);
                    if (pickList != null)
                    {
                        businessEntity.PickList = pickList;
                        businessEntity.PickListId = pickList.Id;
                        businessEntity.PickListName = pickList.Name;
                    }
                }
            }
            return businessEntity;
        }
        public override BUSField Init()
        {
            BUSField businessEntity = base.Init();
            businessEntity.JoinName = "";
            businessEntity.TableColumnName = "";
            businessEntity.PickListName = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSField businessComponent, UIField UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.ErrorMessage == string.Empty)
            {
                if (businessComponent.PickList == null && !string.IsNullOrWhiteSpace(UIEntity.PickListName))
                    result.Add(new ValidationResult(
                        "Picklist with this name not found.",
                        new List<string>() { "PickListName" }
                        ));
                if (businessComponent.Table == null && (!string.IsNullOrWhiteSpace(UIEntity.TableColumnName) || !string.IsNullOrWhiteSpace(UIEntity.JoinColumnName)))
                    result.Add(new ValidationResult(
                        "At first you need to add a table to business component " + businessComponent.BusCompName + ".",
                        new List<string>() { "Table" }
                        ));
                if (businessComponent.Table != null && businessComponent.TableColumn == null
                    && (!string.IsNullOrWhiteSpace(UIEntity.TableColumnName) || !string.IsNullOrWhiteSpace(UIEntity.JoinColumnName)))
                    result.Add(new ValidationResult(
                        "Table column with this name not found in table " + businessComponent.Table.Name + ".",
                        new List<string>() { "TableColumnName" }
                        ));
            }
            return result;
        }
    }
}
