using Microsoft.AspNetCore.Mvc;

namespace GSCrm.Controllers.Default.MVCControllers
{
    public class MVCPhysicalRenderController : Controller
    {
        [Route("DevTools/PhysicalRenders")]
        public IActionResult PhysicalRenders()
        {
            return View();
        }
    }
}
