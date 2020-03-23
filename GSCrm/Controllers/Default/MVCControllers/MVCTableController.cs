using Microsoft.AspNetCore.Mvc;

namespace GSCrm.Controllers.Default.MVCControllers
{
    public class MVCTableController : Controller
    {
        [Route("DevTools/Tables")]
        public IActionResult Tables()
        {
            return View();
        }
    }
}
