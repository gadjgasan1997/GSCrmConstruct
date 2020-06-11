using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IconController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.IconController<ToolsContext, BUSFactory>
    {
        public IconController(ToolsContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}
