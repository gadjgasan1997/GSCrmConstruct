using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMapObjectController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DataMapObjectController<ToolsContext, BUSFactory>
    {
        public DataMapObjectController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}