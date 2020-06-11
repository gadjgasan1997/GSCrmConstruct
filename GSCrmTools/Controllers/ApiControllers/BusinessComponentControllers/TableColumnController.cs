using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableColumnController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.TableColumnController<ToolsContext, BUSFactory>
    {
        public TableColumnController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}
