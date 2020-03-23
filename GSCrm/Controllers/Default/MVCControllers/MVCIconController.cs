using Microsoft.AspNetCore.Mvc;

namespace GSCrm.Controllers.Default.MVCControllers
{
    public class MVCIconController : Controller
    {
        [Route("DevTools/Icons")]
        public IActionResult Icons()
        {
            return View();
        }
    }
}
