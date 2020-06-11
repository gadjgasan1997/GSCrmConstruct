using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using System.Linq;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;
using System;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSJoinSpecificationFR<TContext> : DataBUSFactory<JoinSpecification, BUSJoinSpecification, TContext>
        where TContext : MainContext, new()
    {
        public override BUSJoinSpecification DataToBusiness(JoinSpecification dataEntity, TContext context)
        {
            BUSJoinSpecification businessEntity = base.DataToBusiness(dataEntity, context);

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
                    tableId = j.TableId,
                    busCompId = j.BusCompId
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
                    TableId = j.tableId,
                    BusCompId = j.busCompId
                })
                .FirstOrDefault(i => i.Id == dataEntity.JoinId);
            businessEntity.Join = join;
            businessEntity.JoinId = join.Id;

            // BusComp
            BusinessComponent busComp = context.BusinessComponents
                .AsNoTracking()
                .Select(bc => new
                {
                    id = bc.Id,
                    fields = bc.Fields.Select(f => new
                    {
                        id = f.Id,
                        name = f.Name
                    })
                })
                .Select(bc => new BusinessComponent
                {
                    Id = bc.id,
                    Fields = bc.fields.Select(f => new Field
                    {
                        Id = f.id,
                        Name = f.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == join.BusCompId);
            businessEntity.BusComp = busComp;
            businessEntity.BusCompId = busComp.Id;

            // Source field
            Field field = busComp.Fields.FirstOrDefault(i => i.Id == dataEntity.SourceFieldId);
            if (field != null)
            {
                businessEntity.SourceField = field;
                businessEntity.SourceFieldId = field.Id;
                businessEntity.SourceFieldName = field.Name;
            }

            // Destination column
            Table table = join.Table;
            businessEntity.Table = table;
            businessEntity.TableId = table.Id;

            TableColumn destinationColumn = table.TableColumns.FirstOrDefault(i => i.Id == dataEntity.DestinationColumnId);
            if (destinationColumn != null)
            {
                businessEntity.DestinationColumn = destinationColumn;
                businessEntity.DestinationColumnId = destinationColumn.Id;
                businessEntity.DestinationColumnName = destinationColumn.Name;
            }
            return businessEntity;
        }
        public override JoinSpecification BusinessToData(JoinSpecification joinSpecification, BUSJoinSpecification businessEntity, TContext context, bool NewRecord)
        {
            JoinSpecification dataEntity = base.BusinessToData(joinSpecification, businessEntity, context, NewRecord);
            dataEntity.Join = businessEntity.Join;
            dataEntity.JoinId = businessEntity.JoinId;
            dataEntity.SourceField = businessEntity.SourceField;
            dataEntity.SourceFieldId = businessEntity.SourceFieldId;
            dataEntity.DestinationColumn = businessEntity.DestinationColumn;
            dataEntity.DestinationColumnId = businessEntity.DestinationColumnId;
            return dataEntity;
        }
    }
}
