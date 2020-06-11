using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlUPController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ControlUPController<ToolsContext, BUSFactory>
    {
        public ControlUPController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}