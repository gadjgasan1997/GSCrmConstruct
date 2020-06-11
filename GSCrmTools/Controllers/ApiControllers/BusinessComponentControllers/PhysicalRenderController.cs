using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhysicalRenderController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.PhysicalRenderController<ToolsContext, BUSFactory>
    {
        public PhysicalRenderController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}
