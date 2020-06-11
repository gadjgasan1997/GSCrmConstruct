using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.CodeGeneration;
using static GSCrmLibrary.CodeGeneration.CodGenUtils;
using static GSCrmLibrary.Utils;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSTableFR<TContext> : DataBUSFactory<Table, BUSTable, TContext>
        where TContext : MainContext, new()
    {
        public override void OnRecordCreate(Table recordToCreate, DbSet<Table> entities, TContext context)
        {
            context.TableColumns.AddRange(new List<TableColumn>()
            {
                new TableColumn()
                {
                    Name = "Id",
                    Type = "Guid",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id,
                    NeedCreate = true,
                    Length = 36
                },
                new TableColumn()
                {
                    Name = "Created",
                    Type = "DateTime",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id,
                    NeedCreate = true,
                    Length = 4000
                },
                new TableColumn()
                {
                    Name = "CreatedBy",
                    Type = "String",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id,
                    NeedCreate = true,
                    Length = 120,
                },
                new TableColumn()
                {
                    Name = "LastUpdated",
                    Type = "DateTime",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id,
                    NeedCreate = true,
                    Length = 4000
                },
                new TableColumn()
                {
                    Name = "UpdatedBy",
                    Type = "String",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id,
                    NeedCreate = true,
                    Length = 120
                }
            });
            base.OnRecordCreate(recordToCreate, entities, context);
        }

        public override void OnRecordDelete(Table recordToDelete, DbSet<Table> entities, TContext context)
        {
            string permissibleName = GetPermissibleName(recordToDelete.Name);
            if (EntityFileExists(permissibleName, "Table"))
            {
                CodGenContext.DeleteEntityFromContext(permissibleName);
                DeleteEntityFile(permissibleName, "Table");
                Table table = context.Tables
                    .AsNoTracking()
                    .Select(t => new
                    {
                        id = t.Id,
                        name = t.Name,
                        tableColumns = t.TableColumns.Select(tableColumn => new
                        {
                            id = tableColumn.Id,
                            table = tableColumn.Table,
                            foreignTableId = tableColumn.ForeignTableId,
                        })
                    })
                    .Select(t => new Table
                    {
                        Id = t.id,
                        Name = t.name,
                        TableColumns = t.tableColumns.Select(tableColumn => new TableColumn
                        {
                            Id = tableColumn.id,
                            Table = tableColumn.table,
                            ForeignTableId = tableColumn.foreignTableId
                        }).ToList()
                    })
                    .FirstOrDefault(i => i.Id == recordToDelete.Id);

                // Удаление навигационных свойств во всех классах, где данная таблица является дочерней
                table.TableColumns.Where(f => f.IsForeignKey).ToList().ForEach(foreignKey =>
                {
                    Table foreignTable = context.Tables.AsNoTracking().FirstOrDefault(i => i.Id == foreignKey.ForeignTableId);
                    if (foreignTable != null) CodGenModels.DeleteForeignProperty(foreignTable, foreignKey.Table);
                });
            }
            base.OnRecordDelete(recordToDelete, entities, context);
        }
    }
}