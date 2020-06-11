using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessObjectController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.BusinessObjectController<ToolsContext, BUSFactory>
    {
        public BusinessObjectController(ToolsContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo) 
        { }
    }
}