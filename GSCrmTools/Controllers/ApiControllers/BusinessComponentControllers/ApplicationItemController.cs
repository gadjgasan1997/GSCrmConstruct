using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationItemController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ApplicationItemController<ToolsContext, BUSFactory>
    {
        public ApplicationItemController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}
