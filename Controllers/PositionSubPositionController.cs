using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.DataTransformers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using GSCrm.Repository;
using GSCrm.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GSCrm.CommonConsts;
using static GSCrm.Repository.PositionRepository;

namespace GSCrm.Controllers
{
    [Authorize]
    [Route(POS_SUB_POS)]
    public class PositionSubPositionController
        : MainController<Position, PositionViewModel, PositionValidator, PositionTransformer, PositionRepository>
    {
        public PositionSubPositionController(ApplicationDbContext context, IViewsInfo viewsInfo, ResManager resManager)
            : base(context, viewsInfo, resManager, new PositionTransformer(context, resManager), new PositionRepository(context, viewsInfo, resManager))
        { }

        [HttpGet("ListOfSubPositions/{pageNumber}")]
        public IActionResult PositionSubPositions(int pageNumber)
        {
            PositionViewModel positionViewModel = CurrentPosition;
            repository.SetViewInfo(POS_EMPLOYEES, pageNumber);
            repository.AttachSubPositions(positionViewModel);
            return View($"{POS_VIEWS_REL_PATH}{POSITION}.cshtml", positionViewModel);
        }
    }
}
