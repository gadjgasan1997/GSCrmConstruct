using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class BusCompScreenController : Controller
    {
        [Route("Business Component Screen")]
        public IActionResult Index() => View();
    }
}
