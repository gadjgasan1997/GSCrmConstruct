using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using GSCrm.Factories.Default.MainFactories;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSBusCompFR : MainDataBUSFR<BusComp, BUSBusComp>
    {
        public override BUSBusComp DataToBusiness(BusComp dataEntity, ApplicationContext context)
        {
            BUSBusComp businessEntity = base.DataToBusiness(dataEntity, context);
            Table table = context.Tables.FirstOrDefault(i => i.Id == dataEntity.TableId);
            if (table != null)
            {
                businessEntity.Table = table;
                businessEntity.TableId = table.Id;
                businessEntity.TableName = table.Name;
            }
            return businessEntity;
        }
        public override BusComp BusinessToData(BUSBusComp businessEntity, DbSet<BusComp> entityDBSet, bool NewRecord)
        {
            BusComp dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Table = businessEntity.Table;
            dataEntity.TableId = businessEntity.TableId;
            return dataEntity;
        }
        public override void OnRecordCreate(BusComp recordToCreate, DbSet<BusComp> entityDBSet, IWebHostEnvironment environment, ApplicationContext context)
        {
            if (recordToCreate.Table != null)
            {
                // Table
                Table table = context.Tables
                    .Include(tc => tc.TableColumns)
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
                        Name = "Updated",
                        BusComp = recordToCreate,
                        BusCompId = recordToCreate.Id,
                        TableColumnId = tableColumns.FirstOrDefault(n => n.Name == "Updated").Id
                    },
                    new Field()
                    {
                        Name = "UpdatedBy",
                        BusComp = recordToCreate,
                        BusCompId = recordToCreate.Id,
                        TableColumnId = tableColumns.FirstOrDefault(n => n.Name == "UpdatedBy").Id
                    }
                });
            }
            context.BusinessComponents.Add(recordToCreate);
            context.SaveChanges();
        }
    }
}
