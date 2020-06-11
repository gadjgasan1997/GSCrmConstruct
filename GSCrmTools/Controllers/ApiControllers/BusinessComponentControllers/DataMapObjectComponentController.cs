using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMapObjectComponentController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DataMapObjectComponentController<ToolsContext, BUSFactory>
    {
        public DataMapObjectComponentController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}