using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMapObjectComponentController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.DataMapObjectComponentController<ToolsContext, BUSFactory>
    {
        public DataMapObjectComponentController(ToolsContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}