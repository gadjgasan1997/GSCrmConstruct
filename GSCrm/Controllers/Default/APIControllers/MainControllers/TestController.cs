using Microsoft.AspNetCore.Mvc;
using GSCrm.Data.Context;
using GSCrm.Services.Info;
using Newtonsoft.Json;

namespace GSCrm.Controllers.Default.APIControllers.MainControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IScreenInfo screenInfo;
        private readonly IViewInfo viewInfo;
        private readonly IAppletInfo appletInfo; 
        public TestController(IScreenInfo screenInfo, IViewInfo viewInfo, IAppletInfo appletInfo)
        {
            this.screenInfo = screenInfo;
            this.viewInfo = viewInfo;
            this.appletInfo = appletInfo;
        }

        [HttpGet("GetDisplayedRecords/{componentName}")]
        public ActionResult<object> GetDisplayedRecords(string componentName) => new JsonResult(ComponentContext.GetDisplayedRecords(componentName));

        [HttpGet("GetSelectedRecords/")]
        public ActionResult<object> GetSelectedRecords(string componentName) => new JsonResult(ComponentContext.GetSelectedRecords());

        [HttpGet("GetScreenInfo/")]
        public ActionResult<string> GetScreenInfo()
        {
            return JsonConvert.SerializeObject(screenInfo, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet("GetViewInfo/")]
        public ActionResult<string> GetViewInfo()
        {
            return JsonConvert.SerializeObject(viewInfo, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet("GetAppletInfo/")]
        public ActionResult<string> GetAppletInfo()
        {
            return JsonConvert.SerializeObject(appletInfo, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}
