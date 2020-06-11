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
using SysPosition_1 = GSCrmApplication.Models.TableModels.Position;
using SysPosition_2 = GSCrmApplication.Models.BusinessComponentModels.Position;
using SysCreate_Account_Position_Popup_Applet_3 = GSCrmApplication.Models.AppletModels.Create_Account_Position_Popup_Applet;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Create_Account_Position_Popup_AppletController : MainAppletController
        <SysPosition_1, SysPosition_2, SysCreate_Account_Position_Popup_Applet_3, GSAppContext, DataBUSPositionFR, BUSFactory, BUSUICreate_Account_Position_Popup_AppletFR>
	{
        public Create_Account_Position_Popup_AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {}
	}
}