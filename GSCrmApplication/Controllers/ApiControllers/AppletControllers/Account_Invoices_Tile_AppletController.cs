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
using SysInvoice_1 = GSCrmApplication.Models.TableModels.Invoice;
using SysInvoice_2 = GSCrmApplication.Models.BusinessComponentModels.Invoice;
using SysAccount_Invoices_Tile_Applet_3 = GSCrmApplication.Models.AppletModels.Account_Invoices_Tile_Applet;

namespace GSCrmApplication.Controllers.ApiControllers.AppletControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class Account_Invoices_Tile_AppletController : MainAppletController
        <SysInvoice_1, SysInvoice_2, SysAccount_Invoices_Tile_Applet_3, GSAppContext, DataBUSInvoiceFR, BUSFactory, BUSUIAccount_Invoices_Tile_AppletFR>
	{
        public Account_Invoices_Tile_AppletController(GSAppContext context, IScreenInfo screenInfo, IViewInfo viewInfo)
            : base(context, screenInfo, viewInfo)
        {}
	}
}