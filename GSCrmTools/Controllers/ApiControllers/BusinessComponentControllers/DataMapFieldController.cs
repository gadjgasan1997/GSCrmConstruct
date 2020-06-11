using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataMapFieldController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DataMapFieldController<ToolsContext, BUSFactory>
    {
        public DataMapFieldController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}