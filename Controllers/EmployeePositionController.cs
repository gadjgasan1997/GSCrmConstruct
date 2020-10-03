using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.DataTransformers;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using GSCrm.Repository;
using GSCrm.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using static GSCrm.CommonConsts;
using static GSCrm.Repository.EmployeeRepository;

namespace GSCrm.Controllers
{
    [Authorize]
    [Route(EMP_POSITION)]
    public class EmployeePositionController
        : MainController<EmployeePosition, EmployeePositionViewModel, EmployeePositionValidator, EmployeePositionTransformer, EmployeePositionRepository>
    {
        public EmployeePositionController(ApplicationDbContext context, IViewsInfo viewsInfo, ResManager resManager)
            : base (context, viewsInfo, resManager, new EmployeePositionTransformer(context, resManager), new EmployeePositionRepository(context, viewsInfo, resManager))
        { }

        [HttpGet("ListOfPositions/{pageNumber}")]
        public IActionResult Positions(int pageNumber)
        {
            EmployeeViewModel employeeViewModel = CurrentEmployee;
            EmployeeRepository employeeRepository = new EmployeeRepository(context, viewsInfo, resManager);
            employeeRepository.SetViewInfo(EMP_POSITIONS, pageNumber);
            employeeRepository.AttachPositions(employeeViewModel);
            return View($"{EMP_VIEWS_REL_PATH}{EMPLOYEE}.cshtml", employeeViewModel);
        }

        [HttpGet("{employeeId}")]
        public IActionResult GetPositions(string employeeId)
        {
            if (!string.IsNullOrEmpty(employeeId) && Guid.TryParse(employeeId, out Guid guid))
            {
                List<Position> allPositions = repository.GetAllPositions(guid);
                List<EmployeePosition> selectedPositions = repository.GetSelectedPositions(guid);
                List<EmployeePosition> selectedPositionsForTable = repository.GetSelectedPositionsForTable(guid);
                List<PositionViewModel> allPositionVMs = allPositions.GetViewModelsFromData
                    <Position, PositionViewModel, PositionTransformer>(new PositionTransformer(context, resManager));
                List<EmployeePositionViewModel> selectedPositionsVMs = selectedPositions.GetViewModelsFromData
                    <EmployeePosition, EmployeePositionViewModel, EmployeePositionTransformer>(transformer);
                List<EmployeePositionViewModel> selectedPositionsForTableVMs = selectedPositionsForTable.GetViewModelsFromData
                    <EmployeePosition, EmployeePositionViewModel, EmployeePositionTransformer>(transformer);
                Dictionary<string, object> result = new Dictionary<string, object>()
                {
                    { "allPositions", allPositionVMs },
                    { "selectedPositions", selectedPositionsVMs },
                    { "positionsForTable", selectedPositionsForTableVMs },
                    { "allPositionsVM", ModelCash<EmployeeViewModel>.GetViewModel(ALL_EMP_POSS) },
                    { "selectedPositionsVM", ModelCash<EmployeeViewModel>.GetViewModel(SELECTED_EMP_POSS) }
                };
                return Json(result);
            }
            else return BadRequest(resManager.GetString("PositionsExtractError"));
        }

        [HttpGet("NextAllRecords/{employeeId}")]
        public IActionResult NextAllRecords(string employeeId)
        {
            if (!string.IsNullOrEmpty(employeeId) && Guid.TryParse(employeeId, out Guid guid))
            {
                ViewInfo viewInfo = viewsInfo.Get(ALL_EMP_POSS);
                List<Position> allPositions = repository.GetAllPositions(guid, viewInfo.CurrentPageNumber + DEFAULT_PAGE_STEP);
                List<PositionViewModel> allPositionViewModels = allPositions.GetViewModelsFromData
                    <Position, PositionViewModel, PositionTransformer>(new PositionTransformer(context, resManager));
                return Json(allPositionViewModels);
            }
            return BadRequest("NextAllRecords");
        }

        [HttpGet("PreviousAllRecords/{employeeId}")]
        public IActionResult PreviousAllRecords(string employeeId)
        {
            if (!string.IsNullOrEmpty(employeeId) && Guid.TryParse(employeeId, out Guid guid))
            {
                ViewInfo viewInfo = viewsInfo.Get(ALL_EMP_POSS);
                List<Position> allPositions = repository.GetAllPositions(guid, viewInfo.CurrentPageNumber - DEFAULT_PAGE_STEP);
                List<PositionViewModel> allPositionViewModels = allPositions.GetViewModelsFromData
                    <Position, PositionViewModel, PositionTransformer>(new PositionTransformer(context, resManager));
                return Json(allPositionViewModels);
            }
            return BadRequest("PreviousAllRecords");
        }

        [HttpGet("NextSelectedRecords/{employeeId}")]
        public IActionResult NextSelectedRecords(string employeeId)
        {
            if (!string.IsNullOrEmpty(employeeId) && Guid.TryParse(employeeId, out Guid guid))
            {
                ViewInfo viewInfo = viewsInfo.Get(SELECTED_EMP_POSS);
                List<EmployeePosition> selectedPositions = repository.GetSelectedPositions(guid, viewInfo.CurrentPageNumber + DEFAULT_PAGE_STEP);
                List<EmployeePositionViewModel> selectedPositionViewModels = selectedPositions.GetViewModelsFromData
                    <EmployeePosition, EmployeePositionViewModel, EmployeePositionTransformer>(transformer);
                return Json(selectedPositionViewModels);
            }
            return BadRequest("NextSelectedRecords");
        }

        [HttpGet("PreviousSelectedRecords/{employeeId}")]
        public IActionResult PreviousSelectedRecords(string employeeId)
        {
            if (!string.IsNullOrEmpty(employeeId) && Guid.TryParse(employeeId, out Guid guid))
            {
                ViewInfo viewInfo = viewsInfo.Get(SELECTED_EMP_POSS);
                List<EmployeePosition> selectedPositions = repository.GetSelectedPositions(guid, viewInfo.CurrentPageNumber - DEFAULT_PAGE_STEP);
                List<EmployeePositionViewModel> selectedPositionViewModels = selectedPositions.GetViewModelsFromData
                    <EmployeePosition, EmployeePositionViewModel, EmployeePositionTransformer>(transformer);
                return Json(selectedPositionViewModels);
            }
            return BadRequest("PreviousSelectedRecords");
        }

        [HttpGet("ClearPositionManagementSearch")]
        public IActionResult ClearPositionManagementSearch()
        {
            repository.ClearAllPositionSearch();
            repository.ClearSelectedPositionSearch();
            return Ok();
        }

        [HttpPost("SearchAllPosition")]
        public IActionResult SearchAllPosition(EmployeeViewModel employeeViewModel)
        {
            repository.SearchAllPosition(employeeViewModel);
            return RedirectToAction("GetPositions", EMP_POSITION, new { employeeId = CurrentEmployee.Id });
        }

        [HttpGet("ClearAllPositionSearch")]
        public IActionResult ClearAllPositionSearch()
        {
            repository.ClearAllPositionSearch();
            return RedirectToAction("GetPositions", EMP_POSITION, new { employeeId = CurrentEmployee.Id });
        }

        [HttpPost("SearchSelectedPosition")]
        public IActionResult SearchSelectedPosition(EmployeeViewModel employeeViewModel)
        {
            repository.SearchSelectedPosition(employeeViewModel);
            return RedirectToAction("GetPositions", EMP_POSITION, new { employeeId = CurrentEmployee.Id });
        }

        [HttpGet("ClearSelectedPositionSearch")]
        public IActionResult ClearSelectedPositionSearch()
        {
            repository.ClearSelectedPositionSearch();
            return RedirectToAction("GetPositions", EMP_POSITION, new { employeeId = CurrentEmployee.Id });
        }

        [HttpPost("Synchronize")]
        public IActionResult Synchronize(SyncPositionsViewModel syncViewModel)
        {
            Dictionary<string, string> syncErrors = new Dictionary<string, string>();
            if (repository.TrySyncPositions(syncViewModel, syncErrors))
                return Json("");
            return BadRequest(syncErrors);
        }
    }
}
