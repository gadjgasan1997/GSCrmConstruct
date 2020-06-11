using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmApplication.Models.TableModels;
using GSCrmApplication.Models.AppletModels;
using GSCrmApplication.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.DataBUSFactories;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmApplication.Factories.MainFactories;
using SysAccount_1 = GSCrmApplication.Models.TableModels.Account;
using SysAccount_2 = GSCrmApplication.Models.BusinessComponentModels.Account;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : MainBusinessComponentController
        <SysAccount_1, SysAccount_2, GSAppContext, DataBUSAccountFR, BUSFactory>
	{
        public AccountController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        {}
	}
}