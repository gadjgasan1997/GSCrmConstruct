using Microsoft.AspNetCore.Mvc;

namespace GSCrmApplication.Controllers.MVCControllers
{
	public class Employee_ScreenController : Controller
	{
        [Route("Employee Screen")]
        public IActionResult Index() => View();
	}
}