using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Controllers.ApiControllers.DataMappingControllers;
using GSCrmTools.Data;

namespace GSCrm_Tools.Controllers.ApiControllers.DataMappingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableMapController : TableMapController<ToolsContext>
    {
        public TableMapController(ToolsContext context) : base(context)
        { }
    }
}
