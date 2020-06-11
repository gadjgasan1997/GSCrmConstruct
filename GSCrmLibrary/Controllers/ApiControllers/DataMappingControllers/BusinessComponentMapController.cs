using GSCrmLibrary.Data;
using GSCrmLibrary.DataMapping;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmLibrary.Models.BusinessComponentModels;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Factories.DataBUSFactories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Factories.BUSUIFactories;

namespace GSCrmLibrary.Controllers.ApiControllers.DataMappingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessComponentMapController<TContext> : MainDataMapController<BusinessComponent, BusinessComponent, BusinessComponentMap<TContext>, BUSBusinessComponent, DataBUSBusCompFR<TContext>, TContext>
        where TContext : MainContext, new()
    {
        private readonly TContext context;
        public BusinessComponentMapController(TContext context) : base(context, "Business Component")
        {
            this.context = context;
        }

        [HttpGet("CopyRecord")]
        public override Dictionary<string, object> CopyRecord()
        {
            Dictionary<string, object> result = base.CopyRecord();
            if (result["ErrorMessages"] == null)
            {
                BusinessComponent businessComponent = (BusinessComponent)result["NewRecord"];
                businessComponent.Fields.ForEach(field =>
                {
                    context.Entry(field).State = EntityState.Added;
                    field.PickMaps.ForEach(pickMap => { context.Entry(pickMap).State = EntityState.Added; });
                });
                businessComponent.Joins.ForEach(join =>
                {
                    context.Entry(join).State = EntityState.Added;
                    join.JoinSpecifications.ForEach(joinSpecification => { context.Entry(joinSpecification).State = EntityState.Added; });
                });
                context.SaveChanges();
                DataBUSBusCompFR<TContext> dataBUSBusCompFR = new DataBUSBusCompFR<TContext>();
                BUSUIBusCompFR<TContext> busUIBusCompFR = new BUSUIBusCompFR<TContext>();
                result["NewRecord"] = busUIBusCompFR.BusinessToUI(dataBUSBusCompFR.DataToBusiness((BusinessComponent)result["NewRecord"], context));
            }
            return result;
        }
    }
}
