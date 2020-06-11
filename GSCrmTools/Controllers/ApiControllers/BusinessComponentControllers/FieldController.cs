using Microsoft.AspNetCore.Mvc;
using GSCrmTools.Data;
using GSCrmLibrary.Services.Info;
using GSCrmTools.Factories.MainFactories;

namespace GSCrmTools.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.FieldController<ToolsContext, BUSFactory>
    {
        public FieldController(ToolsContext context, IViewInfo viewInfo)
            : base(context, viewInfo) 
        { }
    }
}