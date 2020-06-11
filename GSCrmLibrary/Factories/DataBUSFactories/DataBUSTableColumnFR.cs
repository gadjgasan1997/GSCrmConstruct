using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.CodeGeneration;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSTableColumnFR<TContext> : DataBUSFactory<TableColumn, BUSTableColumn, TContext>
        where TContext : MainContext, new()
    {
        public override BUSTableColumn DataToBusiness(TableColumn dataEntity, TContext context)
        {
            BUSTableColumn businessEntity = base.DataToBusiness(dataEntity, context);
            
            // Table
            businessEntity.Table = context.Tables.FirstOrDefault(i => i.Id == dataEntity.TableId);
            businessEntity.TableId = dataEntity.TableId;

            // Foreign Key
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
                .FirstOrDefault(i => i.Id == dataEntity.ForeignTableId);

            if (foreignTable != null)
            {
                businessEntity.ForeignTable = foreignTable;
                businessEntity.ForeignTableId = foreignTable.Id;
                businessEntity.ForeignTableName = foreignTable.Name;

                TableColumn foreignTableKey = foreignTable.TableColumns.FirstOrDefault(i => i.Id == dataEntity.ForeignTableKeyId);
                if (foreignTableKey != null)
                {
                    businessEntity.ForeignTableKey = foreignTableKey;
                    businessEntity.ForeignTableKeyId = foreignTableKey.Id;
                    businessEntity.ForeignTableKeyName = foreignTableKey.Name;
                }
                businessEntity.ExtencionType = dataEntity.ExtencionType;
            }

            businessEntity.Type = dataEntity.Type;
            businessEntity.IsForeignKey = dataEntity.IsForeignKey;
            businessEntity.IsNullable = dataEntity.IsNullable;
            businessEntity.Length = dataEntity.Length;
            businessEntity.OnDelete = dataEntity.OnDelete;
            businessEntity.OnUpdate = dataEntity.OnUpdate;

            return businessEntity;
        }
        public override TableColumn BusinessToData(TableColumn tableColumn, BUSTableColumn businessEntity, TContext context, bool NewRecord)
        {
            TableColumn dataEntity = base.BusinessToData(tableColumn, businessEntity, context, NewRecord);
            dataEntity.Table = businessEntity.Table;
            dataEntity.TableId = businessEntity.TableId;
            dataEntity.ForeignTableId = businessEntity.ForeignTableId;
            dataEntity.ForeignTableKeyId = businessEntity.ForeignTableKeyId;
            dataEntity.IsForeignKey = businessEntity.IsForeignKey;
            dataEntity.ExtencionType = businessEntity.ExtencionType;
            dataEntity.Type = businessEntity.Type;
            dataEntity.IsNullable = businessEntity.IsNullable;
            dataEntity.Length = businessEntity.Length;
            dataEntity.OnDelete = businessEntity.OnDelete;
            dataEntity.OnUpdate = businessEntity.OnUpdate;
            return dataEntity;
        }
        public override void OnRecordCreate(TableColumn recordToCreate, DbSet<TableColumn> entities, TContext context)
        {
            recordToCreate.NeedCreate = true;
            base.OnRecordCreate(recordToCreate, entities, context);
        }
        public override void OnRecordDelete(TableColumn recordToDelete, DbSet<TableColumn> entities, TContext context)
        {
            // Удадение внешнего ключа
            if (recordToDelete.IsForeignKey)
            {
                Table foreignTable = context.Tables.FirstOrDefault(i => i.Id == recordToDelete.ForeignTableId);
                if (foreignTable != null)
                {
                    Table currentTable = context.Tables.FirstOrDefault(i => i.Id == recordToDelete.TableId);
                    CodGenModels.DeleteForeignProperty(foreignTable, currentTable);
                }
            }

            // Обновление контекста
            base.OnRecordDelete(recordToDelete, entities, context);
        }
        public override void OnRecordUpdate(TableColumn oldRecord, TableColumn changedRecord, DbSet<TableColumn> entityDBSet, TContext context)
        {
            // При снятии внешнего ключа с колонки или при изменении внешней таблицы, необходимо в классе той таблицы, что была во внешнем ключе до этого, удалить навигационное свойство
            changedRecord.NeedUpdate = true;
            if ((!changedRecord.IsForeignKey && oldRecord.IsForeignKey) || 
                (changedRecord.IsForeignKey && oldRecord.IsForeignKey && changedRecord.ForeignTableId != oldRecord.ForeignTableId))
            {
                Table foreignTable = context.Tables.FirstOrDefault(i => i.Id == oldRecord.ForeignTableId);
                if (foreignTable != null)
                {
                    Table currentTable = context.Tables.FirstOrDefault(i => i.Id == changedRecord.TableId);
                    CodGenModels.DeleteForeignProperty(foreignTable, currentTable);
                }
            }
            base.OnRecordUpdate(oldRecord, changedRecord, entityDBSet, context);
        }
    }
}
