using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ControlController<ToolsContext, BUSFactory>
    {
        public ControlController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}