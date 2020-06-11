using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class ScreensScreenController : Controller
    {
        [Route("Screens Screen")]
        public IActionResult Index() => View();
    }
}
