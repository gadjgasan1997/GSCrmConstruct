using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.LinkController<ToolsContext, BUSFactory>
    {
        public LinkController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}