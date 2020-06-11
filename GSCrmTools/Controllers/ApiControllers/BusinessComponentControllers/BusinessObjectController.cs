using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessObjectController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.BusinessObjectController<ToolsContext, BUSFactory>
    {
        public BusinessObjectController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}