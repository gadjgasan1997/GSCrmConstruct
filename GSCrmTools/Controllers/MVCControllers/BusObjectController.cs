using Microsoft.AspNetCore.Mvc;

namespace GSCrmTools.Controllers.MVCControllers
{
    public class BusObjectScreenController : Controller
    {
        [Route("Business Object Screen")]
        public IActionResult Index() => View();
    }
}
