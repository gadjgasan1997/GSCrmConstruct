using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.AppletController<ToolsContext, BUSFactory>
    {
        public AppletController(ToolsContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base (context, screenInfo, viewInfo) 
        { }
    }
}