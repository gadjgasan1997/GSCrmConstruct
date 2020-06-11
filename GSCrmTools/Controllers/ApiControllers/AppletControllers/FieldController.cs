using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.FieldController<ToolsContext, BUSFactory>
    {
        public FieldController(ToolsContext context,
            IScreenInfo screenInfo,
            IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo) 
        { }
    }
}