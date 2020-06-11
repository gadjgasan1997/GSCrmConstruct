using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.ViewController<ToolsContext, BUSFactory>
    {
        public ViewController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}