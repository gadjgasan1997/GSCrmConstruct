using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessComponentController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.BusinessComponentController<ToolsContext, BUSFactory>
    {
        public BusinessComponentController(ToolsContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}