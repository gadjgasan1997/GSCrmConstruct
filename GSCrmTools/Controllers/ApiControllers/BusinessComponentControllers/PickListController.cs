using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickListController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.PickListController<ToolsContext, BUSFactory>
    {
        public PickListController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}
