using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessObjectComponentController<TContext, TBUSFactory>
        : MainBusinessComponentController<BusinessObjectComponent, BUSBusinessObjectComponent, TContext, DataBUSBOComponentFR<TContext>, TBUSFactory>
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        public BusinessObjectComponentController(TContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}