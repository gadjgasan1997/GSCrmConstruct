using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ScreenController<ToolsContext, BUSFactory>
    {
        public ScreenController(ToolsContext context, IViewInfo viewInfo)
            : base (context, viewInfo)
        { }
    }
}
