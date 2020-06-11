using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenItemController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.ScreenItemController<ToolsContext, BUSFactory>
    {
        public ScreenItemController(ToolsContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base (context, screenInfo, viewInfo)
        { }
    }
}
