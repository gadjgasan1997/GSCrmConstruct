using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Configuration;
using BusinessComponent = GSCrmLibrary.Models.TableModels.BusinessComponent;
using Table = GSCrmLibrary.Models.TableModels.Table;
using static GSCrmLibrary.CodeGeneration.CodGenUtils;
using static GSCrmLibrary.Utils;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSBusCompFR<TContext> : DataBUSFactory<BusinessComponent, BUSBusinessComponent, TContext>
        where TContext : MainContext, new()
    {
        public override BUSBusinessComponent DataToBusiness(BusinessComponent dataEntity, TContext context)
        {
            BUSBusinessComponent businessEntity = base.DataToBusiness(dataEntity, context);
            Table table = context.Tables.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.TableId);
            if (table != null)
            {
                businessEntity.Table = table;
                businessEntity.TableId = table.Id;
                businessEntity.TableName = table.Name;
            }
            businessEntity.ShadowCopy = dataEntity.ShadowCopy;
            return businessEntity;
        }
        public override BusinessComponent BusinessToData(BusinessComponent busComp, BUSBusinessComponent businessEntity, TContext context, bool NewRecord)
        {
            BusinessComponent dataEntity = base.BusinessToData(busComp, businessEntity, context, NewRecord);
            dataEntity.Table = businessEntity.Table;
            dataEntity.TableId = businessEntity.TableId;
            dataEntity.Routing = "/api/" + GetPermissibleName(businessEntity.Name) + "/";
            dataEntity.ShadowCopy = businessEntity.ShadowCopy;
            return dataEntity;
        }
        public override void OnRecordCreate(BusinessComponent recordToCreate, DbSet<BusinessComponent> entities, TContext context)
        {
            if (recordToCreate.Table != null)
            {
                // Table
                Table table = context.Tables
                    .AsNoTracking()
                    .Select(t => new
                    {
                        id = t.Id,
                        tableColumns = t.TableColumns.Select(tableColumn => new
                        {
                            id = tableColumn.Id,
                            name = tableColumn.Name
                        })
                    })
                    .Select(t => new Table
                    {
                        Id = t.id,
                        TableColumns = t.tableColumns.Select(tableColumn => new TableColumn
                        {
                            Id = tableColumn.id,
                            Name = tableColumn.name
                        }).ToList()
                    })
                    .FirstOrDefault(i => i.Id == recordToCreate.TableId);

                // Table columns
                IEnumerable<TableColumn> tableColumns = table.TableColumns;

                // Add fields
                context.Fields.AddRange(new List<Field>()
                {
                    new Field()
                    {
                        Name = "Id",
                        BusComp = recordToCreate,
                        BusCompId = recordToCreate.Id,
                        TableColumnId = tableColumns.FirstOrDefault(n => n.Name == "Id").Id
                    },
                    new Field()
                    {
                        Name = "Created",
                        BusComp = recordToCreate,
                        BusCompId = recordToCreate.Id,
                        TableColumnId = tableColumns.FirstOrDefault(n => n.Name == "Created").Id
                    },
                    new Field()
                    {
                        Name = "CreatedBy",
                        BusComp = recordToCreate,
                        BusCompId = recordToCreate.Id,
                        TableColumnId = tableColumns.FirstOrDefault(n => n.Name == "CreatedBy").Id
                    },
                    new Field()
                    {
                        Name = "LastUpdated",
                        BusComp = recordToCreate,
                        BusCompId = recordToCreate.Id,
                        TableColumnId = tableColumns.FirstOrDefault(n => n.Name == "LastUpdated").Id
                    },
                    new Field()
                    {
                        Name = "UpdatedBy",
                        BusComp = recordToCreate,
                        BusCompId = recordToCreate.Id,
                        TableColumnId = tableColumns.FirstOrDefault(n => n.Name == "UpdatedBy").Id
                    }
                });
                context.Entry(recordToCreate.Table).State = EntityState.Unchanged;
            }
            base.OnRecordCreate(recordToCreate, entities, context);
        }
        public override void OnRecordDelete(BusinessComponent recordToDelete, DbSet<BusinessComponent> entities, TContext context)
        {
            string permissibleName = GetPermissibleName(recordToDelete.Name);
            if (EntityFileExists(permissibleName, "BusinessComponent"))
            {
                DeleteEntityFile(permissibleName, "BusinessComponent");
                DeleteEntityFile(EntitiesConfig.DataFR + permissibleName + "FR", "DataBUSFactory");
                DeleteEntityFile(permissibleName + "Controller", "BusinessComponentController");
            }
            base.OnRecordDelete(recordToDelete, entities, context);
        }
    }
}
