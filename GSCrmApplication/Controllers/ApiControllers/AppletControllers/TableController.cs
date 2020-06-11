using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.TableController<GSAppContext, BUSFactory>
    {
        public TableController(GSAppContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base (context, screenInfo, viewInfo)
        { }
    }
}
