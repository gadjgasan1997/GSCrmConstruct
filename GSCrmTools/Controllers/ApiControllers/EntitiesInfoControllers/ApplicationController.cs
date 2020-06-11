using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Data;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers.ApplicationController<ToolsContext, BUSFactory>
    {
        public ApplicationController(ToolsContext context,
            IApplicationInfo applicationInfo,
            IApplicationInfoUI applicationInfoUI,
            IScreenInfo screenInfo,
            IScreenInfoUI screenInfoUI,
            IViewInfo viewInfo)
            : base(context, applicationInfo, applicationInfoUI, screenInfo, screenInfoUI, viewInfo)
        { }
    }
}
