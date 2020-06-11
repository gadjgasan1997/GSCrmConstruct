using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.DataMapping;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Factories.BUSUIFactories;

namespace GSCrmLibrary.Controllers.ApiControllers.DataMappingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletMapController<TContext> : MainDataMapController<Applet, Applet, AppletMap<TContext>, BUSApplet, DataBUSAppletFR<TContext>, TContext>
        where TContext : MainContext, new()
    {
        private readonly TContext context;
        public AppletMapController(TContext context) : base(context, "Applet")
        {
            this.context = context;
        }
        public override Dictionary<string, object> CopyRecord()
        {
            Dictionary<string, object> result = base.CopyRecord();
            if (result["ErrorMessages"] == null)
            {
                Applet applet = (Applet)result["NewRecord"];
                applet.Columns.ForEach(column =>
                {
                    column.ColumnUPs.ForEach(up => { context.Entry(up).State = EntityState.Added; });
                    context.Entry(column).State = EntityState.Added;
                });
                applet.Controls.ForEach(control =>
                {
                    control.ControlUPs.ForEach(up => { context.Entry(up).State = EntityState.Added; });
                    context.Entry(control).State = EntityState.Added;
                });
                context.SaveChanges();
                DataBUSAppletFR<TContext> dataBUSAppletFR = new DataBUSAppletFR<TContext>();
                BUSUIAppletFR<TContext> busUIAppletFR = new BUSUIAppletFR<TContext>();
                result["NewRecord"] = busUIAppletFR.BusinessToUI(dataBUSAppletFR.DataToBusiness(applet, context));
            }
            return result;
        }
    }
}
