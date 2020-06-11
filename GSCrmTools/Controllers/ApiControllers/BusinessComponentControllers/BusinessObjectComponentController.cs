using GSCrmTools.Data;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessObjectComponentController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.BusinessObjectComponentController<ToolsContext, BUSFactory>
    {
        public BusinessObjectComponentController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}