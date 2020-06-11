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
using SysContact_1 = GSCrmApplication.Models.TableModels.Contact;
using SysContact_2 = GSCrmApplication.Models.BusinessComponentModels.Contact;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContactController : MainBusinessComponentController
        <SysContact_1, SysContact_2, GSAppContext, DataBUSContactFR, BUSFactory>
	{
        public ContactController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        {}
	}
}