using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Services.Info;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUITableColumnFR<TContext> : BUSUIFactory<BUSTableColumn, UITableColumn, TContext>
        where TContext : MainContext, new()
    {
        public override BUSTableColumn UIToBusiness(UITableColumn UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSTableColumn businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Table table = context.Tables
                .AsNoTracking()
                .Select(t => new
                {
                    id = t.Id,
                    name = t.Name,
                    tableColumns = t.TableColumns.Select(tableColumn => new
                    {
                        id = tableColumn.Id,
                        name = tableColumn.Name,
                    })
                })
                .Select(t => new Table
                {
                    Id = t.id,
                    Name = t.name,
                    TableColumns = t.tableColumns.Select(tableColumn => new TableColumn
                    {
                        Id = tableColumn.id,
                        Name = tableColumn.name
                    }).ToList()
                })
                .FirstOrDefault(n => n.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Table"));
            if (table == null)
                businessEntity.ErrorMessage = "First you need create table.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                TableColumn tableColumn = table.TableColumns?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (table.TableColumns != null && tableColumn != null && tableColumn.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Table column with this name is already exists in table {table.Name}.";
                else
                {
                    // Table
                    businessEntity.Table = table;
                    businessEntity.TableId = table.Id;

                    // Foreign Key
                    if (UIEntity.IsForeignKey)
                    {
                        // Внешняя таблица
                        Table foreignTable = context.Tables
                            .AsNoTracking()
                            .Select(t => new
                            {
                                id = t.Id,
                                name = t.Name,
                                tableColumns = t.TableColumns.Select(tableColumn => new
                                {
                                    id = tableColumn.Id,
                                    name = tableColumn.Name,
                                })
                            })
                            .Select(t => new Table
                            {
                                Id = t.id,
                                Name = t.name,
                                TableColumns = t.tableColumns.Select(tableColumn => new TableColumn
                                {
                                    Id = tableColumn.id,
                                    Name = tableColumn.name
                                }).ToList()
                            })
                            .FirstOrDefault(n => n.Name == UIEntity.ForeignTableName);

                        if (foreignTable != null)
                        {
                            businessEntity.ForeignTable = foreignTable;
                            businessEntity.ForeignTableId = foreignTable.Id;
                            businessEntity.ForeignTableName = foreignTable.Name;

                            // Внешний ключ для таблицы
                            TableColumn foreignTableColumn = foreignTable.TableColumns.FirstOrDefault(n => n.Name == UIEntity.ForeignTableKeyName);
                            if (foreignTableColumn != null)
                            {
                                businessEntity.ForeignTableKey = foreignTableColumn;
                                businessEntity.ForeignTableKeyId = foreignTableColumn.Id;
                                businessEntity.ForeignTableKeyName = foreignTableColumn.Name;
                                businessEntity.ExtencionType = UIEntity.ExtencionType;
                                businessEntity.OnDelete = UIEntity.OnDelete;
                                businessEntity.OnUpdate = UIEntity.OnUpdate;
                            }
                        }
                    }

                    businessEntity.Type = UIEntity.Type;
                    businessEntity.IsForeignKey = UIEntity.IsForeignKey;
                    businessEntity.IsNullable = UIEntity.IsNullable;
                    businessEntity.Length = Convert.ToInt32(UIEntity.Length);
                }
            }
            return businessEntity;
        }
        public override UITableColumn BusinessToUI(BUSTableColumn businessEntity)
        {
            UITableColumn UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ForeignTableName = businessEntity.ForeignTableName;
            UIEntity.ForeignTableKeyName = businessEntity.ForeignTableKeyName;
            UIEntity.IsForeignKey = businessEntity.IsForeignKey;
            UIEntity.ExtencionType = businessEntity.ExtencionType;
            UIEntity.Type = businessEntity.Type;
            UIEntity.IsNullable = businessEntity.IsNullable;
            UIEntity.Length = businessEntity.Length.ToString();
            UIEntity.OnDelete = businessEntity.OnDelete;
            UIEntity.OnUpdate = businessEntity.OnUpdate;
            return UIEntity;
        }
        public override BUSTableColumn Init(TContext context)
        {
            BUSTableColumn businessEntity = base.Init(context);
            businessEntity.ForeignTableId = Guid.Empty;
            businessEntity.Length = 0;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UITableColumn UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (UIEntity.IsForeignKey)
            {
                if (string.IsNullOrWhiteSpace(UIEntity.ForeignTableName) || string.IsNullOrWhiteSpace(UIEntity.ForeignTableKeyName))
                    result.Add(new ValidationResult(
                        "You must indicate the name of the external table and the column in it.",
                        new List<string>() { "ForeignTable" }));

                if (string.IsNullOrWhiteSpace(UIEntity.OnDelete) || string.IsNullOrWhiteSpace(UIEntity.OnUpdate))
                    result.Add(new ValidationResult(
                        "You must specify actions in case of deleting or updating a record.",
                        new List<string>() { "ForeignTable" }));

                if (string.IsNullOrWhiteSpace(UIEntity.ExtencionType))
                    result.Add(new ValidationResult(
                        "You must specify type of extencion.",
                        new List<string>() { "ForeignTable" }));
            }
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSTableColumn businessComponent, UITableColumn UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.IsForeignKey)
                {
                    if (businessComponent.ForeignTable == null)
                        result.Add(new ValidationResult(
                            "Table with this name not found.",
                            new List<string>() { "ForeignTable" }));

                    if (businessComponent.ForeignTableKey == null)
                        result.Add(new ValidationResult(
                            "Table column with this name not found.",
                            new List<string>() { "ForeignTable" }));
                }
            }
            return result;
        }
    }
}
