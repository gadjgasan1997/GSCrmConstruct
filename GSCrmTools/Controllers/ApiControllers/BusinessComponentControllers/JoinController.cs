using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoinController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.JoinController<ToolsContext, BUSFactory>
    {
        public JoinController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}