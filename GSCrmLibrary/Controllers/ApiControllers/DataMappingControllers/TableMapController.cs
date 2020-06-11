using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.DataMapping;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Factories.BUSUIFactories;

namespace GSCrmLibrary.Controllers.ApiControllers.DataMappingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableMapController<TContext> : MainDataMapController<Table, Table, TableMap<TContext>, BUSTable, DataBUSTableFR<TContext>, TContext>
        where TContext : MainContext, new()
    {
        private readonly TContext context;
        public TableMapController(TContext context) : base(context, "Table")
        {
            this.context = context;
        }
        public override Dictionary<string, object> CopyRecord()
        {
            Dictionary<string, object> result = base.CopyRecord();
            if (result["ErrorMessages"] == null)
            {
                Table table = (Table)result["NewRecord"];
                table.TableColumns.ForEach(tableColumn =>
                {
                    context.Entry(tableColumn).State = EntityState.Added;
                    context.SaveChanges();
                });
                DataBUSTableFR<TContext> dataBUSTableFR = new DataBUSTableFR<TContext>();
                BUSUITableFR<TContext> bUSUITableFR = new BUSUITableFR<TContext>();
                result["NewRecord"] = bUSUITableFR.BusinessToUI(dataBUSTableFR.DataToBusiness((Table)result["NewRecord"], context));
            }
            return result;
        }
    }
}
