using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMapController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DataMapController<ToolsContext, BUSFactory>
    {
        public DataMapController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}