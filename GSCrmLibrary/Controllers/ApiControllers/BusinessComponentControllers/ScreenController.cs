using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.BUSUIFactories;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController<TContext, TBUSFactory>
        : MainBusinessComponentController<Screen, BUSScreen, TContext, DataBUSScreenFR<TContext>, TBUSFactory>
        where TContext : MainContext, new()
        where TBUSFactory : BUSFactory<TContext>, new()
    {
        public ScreenController(TContext context, IViewInfo viewInfo)
            : base (context, viewInfo)
        { }
    }
}
