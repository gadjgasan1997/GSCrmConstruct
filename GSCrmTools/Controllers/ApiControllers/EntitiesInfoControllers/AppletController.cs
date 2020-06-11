using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Data;

namespace GSCrmTools.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletController : GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers.AppletController<ToolsContext>
    {
        public AppletController(ToolsContext context,
            IAppletInfo appletInfo,
            IAppletInfoUI appletInfoUI)
            : base(context, appletInfo, appletInfoUI)
        { }
    }
}