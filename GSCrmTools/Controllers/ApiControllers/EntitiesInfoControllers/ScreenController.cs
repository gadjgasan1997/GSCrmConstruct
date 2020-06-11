using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Data;

namespace GSCrmTools.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers.ScreenController<ToolsContext>
    {
        public ScreenController(ToolsContext context,
            IScreenInfo screenInfo,
            IScreenInfoUI screenInfoUI,
            IViewInfo viewInfo)
            : base(context, screenInfo, screenInfoUI, viewInfo)
        { }
    }
}
