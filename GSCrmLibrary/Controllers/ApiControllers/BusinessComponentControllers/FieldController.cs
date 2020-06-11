using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController<TContext, TBUSFactory>
        : MainBusinessComponentController<Field, BUSField, TContext, DataBUSFieldFR<TContext>, TBUSFactory>
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        public FieldController(TContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}