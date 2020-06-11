using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class ApplicationScreenController : Controller
    {
        [Route("Application Screen")]
        public IActionResult Index() => View();
    }
}
