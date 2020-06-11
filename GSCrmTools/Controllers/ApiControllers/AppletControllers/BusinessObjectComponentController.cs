using GSCrmTools.Data;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessObjectComponentController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.BusinessObjectComponentController<ToolsContext, BUSFactory>
    {
        public BusinessObjectComponentController(ToolsContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo) 
        { }
    }
}