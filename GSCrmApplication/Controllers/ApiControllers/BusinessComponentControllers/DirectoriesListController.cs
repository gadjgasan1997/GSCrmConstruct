using Microsoft.AspNetCore.Mvc;
using GSCrmApplication.Data;
using GSCrmLibrary.Services.Info;
using GSCrmApplication.Factories.MainFactories;

namespace GSCrmApplication.Controllers.ApiControllers.BusinessComponentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectoriesListController : GSCrmLibrary.Controllers.ApiControllers.BusinessComponentControllers.DirectoriesListController<GSAppContext, BUSFactory>
    {
        public DirectoriesListController(GSAppContext context, IViewInfo viewInfo)
            : base(context, viewInfo)
        { }
    }
}