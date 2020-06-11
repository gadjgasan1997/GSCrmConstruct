using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class AppletScreenController : Controller
    {
        [Route("Applet Screen")]
        public IActionResult Index() => View();
    }
}
