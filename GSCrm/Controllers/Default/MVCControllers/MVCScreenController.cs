using Microsoft.AspNetCore.Mvc;

namespace GSCrm.Controllers.Default.MVCControllers
{
    public class MVCScreenController : Controller
    {
        [Route("DevTools/Screens")]
        public IActionResult Screens()
        {
            return View();
        }
    }
}
