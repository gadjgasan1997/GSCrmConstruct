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
using SysEmployee_1 = GSCrmApplication.Models.TableModels.Employee;
using SysEmployee_2 = GSCrmApplication.Models.BusinessComponentModels.Employee;
using SysEmployee_Tile_Applet_3 = GSCrmApplication.Models.AppletModels.Employee_Tile_Applet;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Employee_Tile_AppletController : MainAppletController
        <SysEmployee_1, SysEmployee_2, SysEmployee_Tile_Applet_3, GSAppContext, DataBUSEmployeeFR, BUSFactory, BUSUIEmployee_Tile_AppletFR>
	{
        public Employee_Tile_AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {}
	}
}