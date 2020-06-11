using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickMapController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.PickMapController<ToolsContext, BUSFactory>
    {
        public PickMapController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}