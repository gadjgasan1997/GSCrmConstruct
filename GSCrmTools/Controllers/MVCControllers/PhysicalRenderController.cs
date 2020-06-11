using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class PhysicalRenderScreenController : Controller
    {
        [Route("Physical Render Screen")]
        public IActionResult Index() => View();
    }
}
