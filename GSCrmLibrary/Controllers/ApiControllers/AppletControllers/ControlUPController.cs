using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Factories.BUSUIFactories;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers;

namespace GSCrmLibrary.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlUPController<TContext, TBUSFactory>
        : MainAppletController<ControlUP, BUSControlUP, UIControlUP, TContext, DataBUSControlUPFR<TContext>, TBUSFactory, BUSUIControlUPFR<TContext>>
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        public ControlUPController(TContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}