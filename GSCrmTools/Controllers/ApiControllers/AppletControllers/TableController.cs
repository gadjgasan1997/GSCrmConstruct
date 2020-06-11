using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.TableController<ToolsContext, BUSFactory>
    {
        public TableController(ToolsContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base (context, screenInfo, viewInfo)
        { }
    }
}
