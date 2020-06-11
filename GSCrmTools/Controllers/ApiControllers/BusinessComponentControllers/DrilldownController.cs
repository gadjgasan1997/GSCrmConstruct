using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrilldownController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DrilldownController<ToolsContext, BUSFactory>
    {
        public DrilldownController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}