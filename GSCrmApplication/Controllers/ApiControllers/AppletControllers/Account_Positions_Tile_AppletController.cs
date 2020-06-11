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
using SysAccount_Positions_Tile_Applet_3 = GSCrmApplication.Models.AppletModels.Account_Positions_Tile_Applet;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Account_Positions_Tile_AppletController : MainAppletController
        <SysPosition_1, SysPosition_2, SysAccount_Positions_Tile_Applet_3, GSAppContext, DataBUSPositionFR, BUSFactory, BUSUIAccount_Positions_Tile_AppletFR>
	{
        public Account_Positions_Tile_AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {}
	}
}