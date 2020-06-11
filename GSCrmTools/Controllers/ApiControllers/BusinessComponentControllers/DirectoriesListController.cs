using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoriesListController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DirectoriesListController<ToolsContext, BUSFactory>
    {
        public DirectoriesListController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}