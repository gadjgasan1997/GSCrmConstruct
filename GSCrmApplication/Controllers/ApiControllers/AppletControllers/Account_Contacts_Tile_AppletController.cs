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
using SysContact_1 = GSCrmApplication.Models.TableModels.Contact;
using SysContact_2 = GSCrmApplication.Models.BusinessComponentModels.Contact;
using SysAccount_Contacts_Tile_Applet_3 = GSCrmApplication.Models.AppletModels.Account_Contacts_Tile_Applet;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Account_Contacts_Tile_AppletController : MainAppletController
        <SysContact_1, SysContact_2, SysAccount_Contacts_Tile_Applet_3, GSAppContext, DataBUSContactFR, BUSFactory, BUSUIAccount_Contacts_Tile_AppletFR>
	{
        public Account_Contacts_Tile_AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {}
	}
}