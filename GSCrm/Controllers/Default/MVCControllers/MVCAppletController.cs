using Microsoft.AspNetCore.Mvc;

namespace GSCrm.Controllers.Default.MVCControllers
{
    public class MVCAppletController : Controller
    {
        [Route("DevTools/Applets")]
        public IActionResult Applets()
        {
            return View();
        }
    }
}
