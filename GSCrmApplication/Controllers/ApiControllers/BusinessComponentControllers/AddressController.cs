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
using SysAddress_1 = GSCrmApplication.Models.TableModels.Address;
using SysAddress_2 = GSCrmApplication.Models.BusinessComponentModels.Address;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AddressController : MainBusinessComponentController
        <SysAddress_1, SysAddress_2, GSAppContext, DataBUSAddressFR, BUSFactory>
	{
        public AddressController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        {}
	}
}