using GSCrm.Data;
using GSCrm.Factories.Default.MainFactories;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using GSCrm.CodeDOM.Default;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSTableFR : MainDataBUSFR<Table, BUSTable>
    {
        public override void OnRecordCreate(Table recordToCreate, DbSet<Table> entityDBSet, IWebHostEnvironment environment, ApplicationContext context)
        {
            base.OnRecordCreate(recordToCreate, entityDBSet, environment, context);
            context.TableColumns.AddRange(new List<TableColumn>()
            {
                new TableColumn()
                {
                    Name = "Id",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id
                },
                new TableColumn()
                {
                    Name = "Created",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id
                },
                new TableColumn()
                {
                    Name = "CreatedBy",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id
                },
                new TableColumn()
                {
                    Name = "Updated",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id
                },
                new TableColumn()
                {
                    Name = "UpdatedBy",
                    Table = recordToCreate,
                    TableId = recordToCreate.Id
                }
            });
            context.SaveChanges();

            //CodeDOMTable codeDOM = new CodeDOMTable();
            //codeDOM.CreateTable(@"C:\Users\Гасан\source\repos\GSCrm\GSCrm\Models\Default\TableModels\" + recordToCreate.Name + ".cs", recordToCreate.Name);
        }
    }
}