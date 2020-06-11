using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSFieldFR<TContext> : DataBUSFactory<Field, BUSField, TContext>
        where TContext : MainContext, new()
    {
        public override BUSField DataToBusiness(Field dataEntity, TContext context)
        {
            BUSField businessEntity = base.DataToBusiness(dataEntity, context);

            // BusComp
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
                    tableId = bc.TableId
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
                    TableId = bc.tableId
                })
                .FirstOrDefault(i => i.Id == dataEntity.BusCompId);

            businessEntity.BusComp = busComp;
            businessEntity.BusCompId = busComp.Id;
            businessEntity.BusCompName = busComp.Name;
            businessEntity.Table = busComp.Table;
            businessEntity.TableId = busComp.TableId;

            // Join
            Join join = context.Joins
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
                    tableId = bc.TableId
                })
                .Select(bc => new Join
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
                    TableId = bc.tableId
                })
                .FirstOrDefault(i => i.Id == dataEntity.JoinId);

            if (join != null)
            {
                businessEntity.Join = join;
                businessEntity.JoinId = join.Id;
                businessEntity.JoinName = join.Name;
                businessEntity.JoinTableId = join.TableId;
            }

            // Table column
            if (busComp.Table != null && busComp.Table.TableColumns != null && dataEntity.TableColumnId != null)
            {
                TableColumn tableColumn = busComp.Table.TableColumns.FirstOrDefault(i => i.Id == dataEntity.TableColumnId);
                if (tableColumn != null)
                {
                    businessEntity.TableColumn = tableColumn;
                    businessEntity.TableColumnId = tableColumn.Id;
                    businessEntity.TableColumnName = tableColumn.Name;
                }
                else
                {
                    dataEntity.TableColumn = null;
                    dataEntity.TableColumnId = null;
                    context.Entry(dataEntity).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }

            // Join column
            if (join?.Table != null && join.Table?.TableColumns != null && dataEntity.JoinColumnId != null)
            {
                TableColumn joinColumn = join.Table.TableColumns.FirstOrDefault(i => i.Id == dataEntity.JoinColumnId);
                if (joinColumn != null)
                {
                    businessEntity.JoinColumn = joinColumn;
                    businessEntity.JoinColumnId = joinColumn.Id;
                    businessEntity.JoinColumnName = joinColumn.Name;
                }
                else
                {
                    dataEntity.JoinColumn = null;
                    dataEntity.JoinColumnId = null;
                    context.Entry(dataEntity).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }

            // PickList
            if (dataEntity.PickListId != null)
            {
                PickList pickList = context.PickLists.FirstOrDefault(i => i.Id == dataEntity.PickListId);
                if (pickList != null)
                {
                    businessEntity.PickListId = dataEntity.PickListId;
                    businessEntity.PickListName = pickList.Name;
                }
            }

            businessEntity.Readonly = dataEntity.Readonly;
            businessEntity.IsCalculate = dataEntity.IsCalculate;
            businessEntity.CalculatedValue = dataEntity.CalculatedValue;
            businessEntity.Required = dataEntity.Required;
            businessEntity.Type = dataEntity.Type;
            return businessEntity;
        }
        public override Field BusinessToData(Field field, BUSField businessEntity, TContext context, bool NewRecord)
        {
            Field dataEntity = base.BusinessToData(field, businessEntity, context, NewRecord);
            dataEntity.BusComp = businessEntity.BusComp;
            dataEntity.BusCompId = businessEntity.BusCompId;
            dataEntity.Join = businessEntity.Join;
            dataEntity.JoinId = businessEntity.JoinId;
            dataEntity.PickListId = businessEntity.PickListId;
            dataEntity.TableColumn = businessEntity.TableColumn;
            dataEntity.TableColumnId = businessEntity.TableColumnId;
            dataEntity.JoinColumn = businessEntity.JoinColumn;
            dataEntity.JoinColumnId = businessEntity.JoinColumnId;
            dataEntity.Required = businessEntity.Required;
            dataEntity.Readonly = businessEntity.Readonly;
            dataEntity.Type = businessEntity.Type;
            dataEntity.IsCalculate = businessEntity.IsCalculate;
            dataEntity.CalculatedValue = businessEntity.CalculatedValue;
            return dataEntity;
        }
    }
}
