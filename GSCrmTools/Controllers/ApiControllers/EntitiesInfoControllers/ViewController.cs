using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Data;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController : GSCrmLibrary.Controllers.ApiControllers.EntitiesInfoControllers.ViewController<ToolsContext, BUSFactory>
    {
        public ViewController(ToolsContext context, IViewInfo viewInfo, IViewInfoUI viewInfoUI)
            : base(context, viewInfo, viewInfoUI)
        { }
    }
}
