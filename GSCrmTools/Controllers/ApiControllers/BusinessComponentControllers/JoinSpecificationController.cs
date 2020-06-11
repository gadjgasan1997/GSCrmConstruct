using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoinSpecificationController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.JoinSpecificationController<ToolsContext, BUSFactory>
    {
        public JoinSpecificationController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}