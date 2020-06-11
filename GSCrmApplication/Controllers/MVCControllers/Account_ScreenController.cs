using Microsoft.AspNetCore.Mvc;

namespace GSCrmApplication.Controllers.MVCControllers
{
	public class Account_ScreenController : Controller
	{
        [Route("Account Screen")]
        public IActionResult Index() => View();
	}
}