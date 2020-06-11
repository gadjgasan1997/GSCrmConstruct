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
using SysCreate_Account_Popup_Applet_3 = GSCrmApplication.Models.AppletModels.Create_Account_Popup_Applet;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Create_Account_Popup_AppletController : MainAppletController
        <SysAccount_1, SysAccount_2, SysCreate_Account_Popup_Applet_3, GSAppContext, DataBUSAccountFR, BUSFactory, BUSUICreate_Account_Popup_AppletFR>
	{
        public Create_Account_Popup_AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {}
	}
}