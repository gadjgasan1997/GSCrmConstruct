using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewItemController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.ViewItemController<ToolsContext, BUSFactory>
    {
        public ViewItemController(ToolsContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo) 
        { }
    }
}