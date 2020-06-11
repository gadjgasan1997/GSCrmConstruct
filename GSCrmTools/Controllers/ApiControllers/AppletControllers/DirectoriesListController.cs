using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.AppletControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoriesListController : GSCrmLibrary.Controllers.ApiControllers.AppletControllers.DirectoriesListController<ToolsContext, BUSFactory>
    {
        public DirectoriesListController(ToolsContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        { }
    }
}