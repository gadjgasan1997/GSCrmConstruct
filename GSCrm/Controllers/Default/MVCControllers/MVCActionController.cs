using Microsoft.AspNetCore.Mvc;

namespace GSCrm.Controllers.Default.MVCControllers
{
    public class MVCActionController : Controller
    {
        [Route("DevTools/Actions")]
        public IActionResult Actions()
        {
            return View();
        }
    }
}
