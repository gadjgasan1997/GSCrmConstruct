using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class IconScreenController : Controller
    {
        [Route("Icon Screen")]
        public IActionResult Index() => View();
    }
}
