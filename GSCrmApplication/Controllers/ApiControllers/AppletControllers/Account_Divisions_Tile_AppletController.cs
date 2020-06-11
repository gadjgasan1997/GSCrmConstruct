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
using SysDivision_1 = GSCrmApplication.Models.TableModels.Division;
using SysDivision_2 = GSCrmApplication.Models.BusinessComponentModels.Division;
using SysAccount_Divisions_Tile_Applet_3 = GSCrmApplication.Models.AppletModels.Account_Divisions_Tile_Applet;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Account_Divisions_Tile_AppletController : MainAppletController
        <SysDivision_1, SysDivision_2, SysAccount_Divisions_Tile_Applet_3, GSAppContext, DataBUSDivisionFR, BUSFactory, BUSUIAccount_Divisions_Tile_AppletFR>
	{
        public Account_Divisions_Tile_AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {}
	}
}