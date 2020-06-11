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
    public class BUSUIFieldFR<TContext> : BUSUIFactory<BUSField, UIField, TContext>
        where TContext : MainContext, new()
    {
        public override UIField BusinessToUI(BUSField businessEntity)
        {
            UIField UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.JoinName = businessEntity.JoinName;
            UIEntity.PickListName = businessEntity.PickListName;
            UIEntity.TableColumnName = businessEntity.TableColumnName;
            UIEntity.JoinColumnName = businessEntity.JoinColumnName;
            UIEntity.IsCalculate = businessEntity.IsCalculate;
            UIEntity.CalculatedValue = businessEntity.CalculatedValue;
            UIEntity.Type = businessEntity.Type;
            UIEntity.Required = businessEntity.Required;
            UIEntity.Readonly = businessEntity.Readonly;
            return UIEntity;
        }
        public override BUSField UIToBusiness(UIField UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSField businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            BusinessComponent busComp = context.BusinessComponents
                .AsNoTracking()
                .Select(bc => new
                {
                    id = bc.Id,
                    name = bc.Name,
                    table = new 
                    {
                        id = bc.Table.Id,
                        tableColumns = bc.Table.TableColumns.Select(tableColumn => new
                        {
                            id = tableColumn.Id,
                            name = tableColumn.Name
                        })
                    },
                    tableId = bc.TableId,
                    fields = bc.Fields.Select(field => new
                    {
                        id = field.Id,
                        name = field.Name
                    })
                })
                .Select(bc => new BusinessComponent
                {
                    Id = bc.id,
                    Name = bc.name,
                    Table = new Table
                    {
                        Id = bc.table.id,
                        TableColumns = bc.table.tableColumns.Select(tableColumn => new TableColumn
                        {
                            Id = tableColumn.id,
                            Name = tableColumn.name
                        }).ToList()
                    },
                    TableId = bc.tableId,
                    Fields = bc.fields.Select(field => new Field
                    {
                        Id = field.id,
                        Name = field.name
                    }).ToList()
                })
                .FirstOrDefault(n => n.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Business Component"));
            if (busComp == null)
                businessEntity.ErrorMessage = "First you need create business component.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                Field field = busComp.Fields?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (field != null && field.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Field with this name is already exists in business component {busComp.Name}.";
                else
                {
                    businessEntity.BusComp = busComp;
                    businessEntity.BusCompId = busComp.Id;
                    businessEntity.BusCompName = busComp.Name;
                    businessEntity.Table = busComp.Table;
                    businessEntity.TableId = busComp.TableId;

                    // Join
                    Join join = context.Joins
                        .AsNoTracking()
                        .Select(j => new
                        {
                            id = j.Id,
                            name = j.Name,
                            table = new
                            {
                                id = j.Table.Id,
                                tableColumns = j.Table.TableColumns.Select(joinColumn => new
                                {
                                    id = joinColumn.Id,
                                    name = joinColumn.Name
                                })
                            },
                            tableId = j.TableId
                        })
                        .Select(j => new Join
                        {
                            Id = j.id,
                            Name = j.name,
                            Table = new Table
                            {
                                Id = j.table.id,
                                TableColumns = j.table.tableColumns.Select(joinColumn => new TableColumn
                                {
                                    Id = joinColumn.id,
                                    Name = joinColumn.name
                                }).ToList()
                            },
                            TableId = j.tableId
                        })
                        .FirstOrDefault(n => n.Name == UIEntity.JoinName);

                    if (join != null)
                    {
                        businessEntity.Join = join;
                        businessEntity.JoinId = join.Id;
                        businessEntity.JoinName = join.Name;
                        businessEntity.JoinTableId = join.TableId;
                    }

                    // Table column
                    if (businessEntity.Table != null && businessEntity.Table.TableColumns != null)
                    {
                        TableColumn tableColumn = businessEntity.Table.TableColumns.FirstOrDefault(n => n.Name == UIEntity.TableColumnName);
                        if (tableColumn != null)
                        {
                            businessEntity.TableColumn = tableColumn;
                            businessEntity.TableColumnId = tableColumn.Id;
                            businessEntity.TableColumnName = tableColumn.Name;
                        }
                    }

                    // Join column
                    if (join?.Table != null && join.Table.TableColumns != null)
                    {
                        TableColumn joinColumn = join.Table.TableColumns.FirstOrDefault(n => n.Name == UIEntity.JoinColumnName);
                        if (joinColumn != null)
                        {
                            businessEntity.JoinColumn = joinColumn;
                            businessEntity.JoinColumnId = joinColumn.Id;
                            businessEntity.JoinColumnName = joinColumn.Name;
                        }
                    }

                    // PickList
                    PickList pickList = context.PickLists.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.PickListName);
                    if (pickList != null)
                    {
                        businessEntity.PickList = pickList;
                        businessEntity.PickListId = pickList.Id;
                        businessEntity.PickListName = pickList.Name;
                    }
                }

                businessEntity.IsCalculate = UIEntity.IsCalculate;
                businessEntity.CalculatedValue = UIEntity.CalculatedValue;
                if (!UIEntity.IsCalculate)
                    businessEntity.CalculatedValue = string.Empty;
                businessEntity.Type = UIEntity.Type;
                businessEntity.Required = UIEntity.Required;
                businessEntity.Readonly = UIEntity.Readonly;
                if (UIEntity.IsCalculate && businessEntity.PickList == null)
                    businessEntity.Readonly = true;
            }
            return businessEntity;
        }
        public override BUSField Init(TContext context)
        {
            BUSField businessEntity = base.Init(context);
            BusinessComponent busComp = context.BusinessComponents
                .AsNoTracking()
                .Select(bc => new
                {
                    id = bc.Id,
                    table = bc.Table,
                    tableId = bc.TableId
                })
                .Select(bc => new BusinessComponent
                {
                    Id = bc.id,
                    Table = bc.table,
                    TableId = bc.tableId
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Business Component"));
            if (busComp != null)
            {
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp.Id;
                businessEntity.Table = busComp.Table;
                businessEntity.TableId = busComp.TableId;
            }
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSField businessComponent, UIField UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.PickList == null && !string.IsNullOrWhiteSpace(UIEntity.PickListName))
                    result.Add(new ValidationResult(
                        "Picklist with this name not found.",
                        new List<string>() { "PickListName" }));
                if (businessComponent.Table == null && (!string.IsNullOrWhiteSpace(UIEntity.TableColumnName)))
                    result.Add(new ValidationResult(
                        "At first you need to add a table to business component " + businessComponent.BusCompName + ".",
                        new List<string>() { "Table" }));
                else if (businessComponent.TableColumn == null && (!string.IsNullOrWhiteSpace(UIEntity.TableColumnName)))
                    result.Add(new ValidationResult(
                        "Table column with this name not found in table " + businessComponent.Table.Name + ".",
                        new List<string>() { "TableColumnName" }));
                if (businessComponent.Join == null && (!string.IsNullOrWhiteSpace(UIEntity.JoinColumnName)))
                    result.Add(new ValidationResult(
                        "At first you need to add a join to this field.",
                        new List<string>() { "Table" }));
                else if (businessComponent.JoinColumn == null && (!string.IsNullOrWhiteSpace(UIEntity.JoinColumnName)))
                    result.Add(new ValidationResult(
                        "Table column with this name not found in table " + businessComponent.Join.Table.Name + ".",
                        new List<string>() { "TableColumnName" }));
                if (businessComponent.CalculatedValue != null && businessComponent.CalculatedValue.Length > 500)
                    result.Add(new ValidationResult(
                        "Calculated field cannot be more than 500 characters.",
                        new List<string>() { "CalculatedValue" }));
            }
            return result;
        }
    }
}
