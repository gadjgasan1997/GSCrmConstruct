using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Controllers.ApiControllers.DataMappingControllers;
using GSCrmTools.Data;

namespace GSCrm_Tools.Controllers.ApiControllers.DataMappingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppletMapController : AppletMapController<ToolsContext>
    {
        public AppletMapController(ToolsContext context) : base(context)
        { }
    }
}
