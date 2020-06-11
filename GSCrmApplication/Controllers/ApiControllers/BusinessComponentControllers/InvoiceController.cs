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
using SysInvoice_1 = GSCrmApplication.Models.TableModels.Invoice;
using SysInvoice_2 = GSCrmApplication.Models.BusinessComponentModels.Invoice;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InvoiceController : MainBusinessComponentController
        <SysInvoice_1, SysInvoice_2, GSAppContext, DataBUSInvoiceFR, BUSFactory>
	{
        public InvoiceController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        {}
	}
}