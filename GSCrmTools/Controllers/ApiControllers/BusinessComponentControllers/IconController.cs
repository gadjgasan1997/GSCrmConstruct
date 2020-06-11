using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IconController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.IconController<ToolsContext, BUSFactory>
    {
        public IconController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}
