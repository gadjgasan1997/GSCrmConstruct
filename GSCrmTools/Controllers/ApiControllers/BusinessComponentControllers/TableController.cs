using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.TableController<ToolsContext, BUSFactory>
    {
        public TableController(ToolsContext context, IViewInfo viewInfo)
            : base (context, viewInfo)
        { }
    }
}
