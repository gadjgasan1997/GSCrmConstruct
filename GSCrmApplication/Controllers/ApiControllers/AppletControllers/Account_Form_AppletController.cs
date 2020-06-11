using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmApplication.Models.TableModels;
using GSCrmApplication.Models.AppletModels;
using GSCrmApplication.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.BUSUIFactories;
using GSCrmApplication.Factories.DataBUSFactories;
using GSCrmLibrary.Controllers.ApiControllers.MainControllers;
using GSCrmApplication.Factories.MainFactories;
using SysAccount_1 = GSCrmApplication.Models.TableModels.Account;
using SysAccount_2 = GSCrmApplication.Models.BusinessComponentModels.Account;
using SysAccount_Form_Applet_3 = GSCrmApplication.Models.AppletModels.Account_Form_Applet;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Account_Form_AppletController : MainAppletController
        <SysAccount_1, SysAccount_2, SysAccount_Form_Applet_3, GSAppContext, DataBUSAccountFR, BUSFactory, BUSUIAccount_Form_AppletFR>
	{
        public Account_Form_AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {}
	}
}