using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ColumnController<ToolsContext, BUSFactory>
    {
        public ColumnController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }

    }
}
