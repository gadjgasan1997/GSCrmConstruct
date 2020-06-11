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
using SysAddress_1 = GSCrmApplication.Models.TableModels.Address;
using SysAddress_2 = GSCrmApplication.Models.BusinessComponentModels.Address;
using SysCreate_Account_Address_Popup_Applet_3 = GSCrmApplication.Models.AppletModels.Create_Account_Address_Popup_Applet;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Create_Account_Address_Popup_AppletController : MainAppletController
        <SysAddress_1, SysAddress_2, SysCreate_Account_Address_Popup_Applet_3, GSAppContext, DataBUSAddressFR, BUSFactory, BUSUICreate_Account_Address_Popup_AppletFR>
	{
        public Create_Account_Address_Popup_AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {}
	}
}