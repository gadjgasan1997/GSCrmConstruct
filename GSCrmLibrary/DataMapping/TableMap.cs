using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.DataMapping
{
    public class TableMap<TContext> : IDataMapping<TContext, Table, Table>
        where TContext : MainContext
    {
        public IEnumerable<Table> Map(IEnumerable<Table> records, TContext context)
        {
            foreach (Table record in records)
            {
                // Получение нового id и названия записи
                Guid newTableId = Guid.NewGuid();
                string recordName = $"{record.Name}_2";
                Table sameNameTable = context.Tables
                    .AsNoTracking()
                    .Select(t => new { id = t.Id, name = t.Name })
                    .Select(t => new Table { Id = t.id, Name = t.name })
                    .FirstOrDefault(n => n.Name == recordName);
                while (sameNameTable != null)
                {
                    recordName = $"Copy_{recordName}";
                    sameNameTable = context.Tables
                        .AsNoTracking()
                        .Select(t => new { id = t.Id, name = t.Name })
                        .Select(t => new Table { Id = t.id, Name = t.name })
                        .FirstOrDefault(n => n.Name == recordName);
                }

                // Select из базы
                Table table = context.Tables
                    .AsNoTracking()
                    .Include(tc => tc.TableColumns)
                    .FirstOrDefault(i => i.Id == record.Id);
                List<TableColumn> tableColumns = new List<TableColumn>();
                table.TableColumns.ForEach(tableColumn =>
                {
                    tableColumns.Add(new TableColumn
                    {
                        Id = Guid.NewGuid(),
                        TableId = newTableId,
                        Changed = tableColumn.Changed,
                        Created = DateTime.Now,
                        CreatedBy = tableColumn.CreatedBy,
                        ExtencionType = tableColumn.ExtencionType,
                        ForeignTableId = tableColumn.ForeignTableId,
                        ForeignTableKeyId = tableColumn.ForeignTableKeyId,
                        IsForeignKey = tableColumn.IsForeignKey,
                        Inactive = tableColumn.Inactive,
                        IsNullable = tableColumn.IsNullable,
                        LastUpdated = tableColumn.LastUpdated,
                        Length = tableColumn.Length,
                        Name = tableColumn.Name,
                        NeedCreate = tableColumn.NeedCreate,
                        NeedUpdate = tableColumn.NeedUpdate,
                        OnDelete = tableColumn.OnDelete,
                        OnUpdate = tableColumn.OnUpdate,
                        Sequence = tableColumn.Sequence,
                        Type = tableColumn.Type,
                        UpdatedBy = tableColumn.UpdatedBy
                    });
                });

                // Возврат новой таблицы
                yield return new Table()
                {
                    Id = newTableId,
                    Name = recordName,
                    Created = DateTime.Now,
                    CreatedBy = table.CreatedBy,
                    LastUpdated = DateTime.Now,
                    UpdatedBy = table.UpdatedBy,
                    Changed = table.Changed,
                    Inactive = table.Inactive,
                    Sequence = table.Sequence,
                    IsApply = table.IsApply,
                    TableColumns = tableColumns
                };
            }
        }
    }
}
