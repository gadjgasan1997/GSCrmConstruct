using GSCrmLibrary.Services.Info;
using GSCrmTools.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSCrm_Tools.Controllers.ApiControllers.EntitiesInfoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ToolsContext context;
        private readonly IScreenInfo screenInfo;
        private readonly IViewInfo viewInfo;
        public TestController(ToolsContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
        {
            this.context = context;
            this.screenInfo = screenInfo;
            this.viewInfo = viewInfo;
        }

        [HttpGet("GetViewInfo")]
        public ActionResult<object> GetBackViewInfo()
        {
            return Ok(JsonConvert.SerializeObject(viewInfo, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        [HttpGet("GetScreenInfo")]
        public ActionResult<object> GetBackScreenInfo()
        {
            return Ok(JsonConvert.SerializeObject(screenInfo, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }
    }
}
