using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIJoinSpecificationFR<TContext> : BUSUIFactory<BUSJoinSpecification, UIJoinSpecification, TContext>
        where TContext : MainContext, new()
    {
        public override BUSJoinSpecification UIToBusiness(UIJoinSpecification UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSJoinSpecification businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Join join = context.Joins
                .AsNoTracking()
                .Select(j => new
                {
                    id = j.Id,
                    name = j.Name,
                    joinSpecifications = j.JoinSpecifications.Select(joinSpecification => new
                    {
                        id = joinSpecification.Id,
                        name = joinSpecification.Name
                    }),
                    table = new
                    {
                        id = j.Table.Id,
                        tableColumns = j.Table.TableColumns.Select(joinColumn => new
                        {
                            id = joinColumn.Id,
                            name = joinColumn.Name
                        })
                    },
                    tableId = j.TableId,
                    busComp = new
                    {
                        id = j.BusCompId,
                        table = new
                        {
                            id = j.BusComp.Table.Id,
                            tableColumns = j.BusComp.Table.TableColumns.Select(joinColumn => new
                            {
                                id = joinColumn.Id,
                                name = joinColumn.Name
                            })
                        },
                        fields = j.BusComp.Fields.Select(field => new
                        {
                            id = field.Id,
                            name = field.Name
                        })
                    },
                    busCompId = j.BusCompId,
                })
                .Select(j => new Join
                {
                    Id = j.id,
                    Name = j.name,
                    JoinSpecifications = j.joinSpecifications.Select(joinSpecification => new JoinSpecification
                    {
                        Id = joinSpecification.id,
                        Name = joinSpecification.name
                    }).ToList(),
                    Table = new Table
                    {
                        Id = j.table.id,
                        TableColumns = j.table.tableColumns.Select(joinColumn => new TableColumn
                        {
                            Id = joinColumn.id,
                            Name = joinColumn.name
                        }).ToList()
                    },
                    TableId = j.tableId,
                    BusComp = new BusinessComponent
                    {
                        Id = j.busCompId,
                        Table = new Table
                        {
                            Id = j.busComp.table.id,
                            TableColumns = j.busComp.table.tableColumns.Select(tableColumn => new TableColumn
                            {
                                Id = tableColumn.id,
                                Name = tableColumn.name
                            }).ToList()
                        },
                        Fields = j.busComp.fields.Select(field => new Field
                        {
                            Id = field.id,
                            Name = field.name
                        }).ToList()
                    },
                    BusCompId = j.busCompId,
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Join"));
            if (join == null)
                businessEntity.ErrorMessage = "First you need create join.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                JoinSpecification joinSpecification = join.JoinSpecifications?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (joinSpecification != null && joinSpecification.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Join specification with this name is already exists in join {join.Name}.";
                else
                {
                    // BusComp
                    BusinessComponent busComp = join.BusComp;

                    // Join
                    businessEntity.Join = join;
                    businessEntity.JoinId = join.Id;
                    businessEntity.BusComp = join.BusComp;
                    businessEntity.BusCompId = join.BusCompId;
                    businessEntity.Table = join.Table;
                    businessEntity.TableId = join.TableId;

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
        public override BUSJoinSpecification Init(TContext context)
        {
            BUSJoinSpecification businessEntity = base.Init(context);
            BusinessComponent busComp = context.BusinessComponents.AsNoTracking().FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Business Component"));
            Join join = context.Joins
                .AsNoTracking()
                .Select(j => new
                {
                    id = j.Id,
                    table = j.Table,
                    tableId = j.TableId
                })
                .Select(j => new Join
                {
                    Id = j.id,
                    Table = j.table,
                    TableId = j.tableId
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Join"));
            businessEntity.BusComp = busComp;
            businessEntity.BusCompId = busComp.Id;
            businessEntity.Table = join.Table;
            businessEntity.TableId = join.TableId;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIJoinSpecification UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.SourceFieldName))
                result.Add(new ValidationResult(
                    "Source field name is a required field.",
                    new List<string>() { "SourceFieldName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.DestinationColumnName))
                result.Add(new ValidationResult(
                    "Destination column name is a required field.",
                    new List<string>() { "DestinationColumnName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSJoinSpecification businessComponent, UIJoinSpecification UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.Table == null)
                {
                    result.Add(new ValidationResult(
                        "At first you need to add a table to join " + businessComponent.Join.Name + ".",
                        new List<string>() { "Table" }));
                }
                else
                {
                    if (businessComponent.SourceField == null)
                        result.Add(new ValidationResult(
                            "Source field with this name not found.",
                            new List<string>() { "SourceFieldName" }));
                    if (businessComponent.DestinationColumn == null)
                        result.Add(new ValidationResult(
                            "Destination column with this name not found.",
                            new List<string>() { "SourceFieldName" }));
                }
            }
            return result;
        }
    }
}