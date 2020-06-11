using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessComponentController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.BusinessComponentController<ToolsContext, BUSFactory>
    {
        public BusinessComponentController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}