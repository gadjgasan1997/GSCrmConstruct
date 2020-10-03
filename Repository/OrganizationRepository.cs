using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.DataTransformers;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using GSCrm.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using static GSCrm.CommonConsts;
using static GSCrm.Utils.CollectionsUtils;

namespace GSCrm.Repository
{
    public class OrganizationRepository : GenericRepository<Organization, OrganizationViewModel, OrganizationValidatior, OrganizationTransformer>
    {
        private readonly User currentUser;
        public static OrganizationViewModel CurrentOrganization { get; set; }
        public OrganizationRepository(ApplicationDbContext context, IViewsInfo viewsInfo, ResManager resManager, HttpContext httpContext = null)
            : base(context, viewsInfo, resManager, new OrganizationValidatior(context, resManager), new OrganizationTransformer(context, resManager, httpContext))
        {
            if (httpContext != null)
                currentUser = httpContext.GetCurrentUser(context);
        }

        #region Searching
        /// <summary>
        /// Метод устанавливает значения для поиска по организациям
        /// </summary>
        /// <param name="orgsViewModel"></param>
        /// <returns></returns>
        public void Search(OrganizationsViewModel orgsViewModel)
        {
            viewsInfo.Reset(ORGANIZATIONS);
            OrganizationsViewModel orgsViewModelCash = ModelCash<OrganizationsViewModel>.GetViewModel(ORGANIZATIONS);
            orgsViewModelCash.SearchName = orgsViewModel.SearchName?.ToLower().TrimStartAndEnd();
            ModelCash<OrganizationsViewModel>.SetViewModel(ORGANIZATIONS, orgsViewModelCash);
        }

        /// <summary>
        /// Метод очищает поиск по организациям
        /// </summary>
        public void ClearSearch()
        {
            OrganizationsViewModel orgsViewModelCash = ModelCash<OrganizationsViewModel>.GetViewModel(ORGANIZATIONS);
            orgsViewModelCash.SearchName = default;
            ModelCash<OrganizationsViewModel>.SetViewModel(ORGANIZATIONS, orgsViewModelCash);
        }

        /// <summary>
        /// Метод для поиска подразделения
        /// </summary>
        /// <param name="orgViewModel"></param>
        public void SearchDivision(OrganizationViewModel orgViewModel)
        {
            viewsInfo.Reset(DIVISIONS);
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(DIVISIONS);
            orgViewModelCash.Id = orgViewModel.Id;
            orgViewModelCash.SearchDivName = orgViewModel.SearchDivName?.ToLower().TrimStartAndEnd();
            orgViewModelCash.SearchParentDivName = orgViewModel.SearchParentDivName?.ToLower().TrimStartAndEnd();
            ModelCash<OrganizationViewModel>.SetViewModel(DIVISIONS, orgViewModelCash);
        }

        /// <summary>
        /// Метод очищает кеш для модели с подразделениями
        /// </summary>
        public void ClearDivisionSearch()
        {
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(DIVISIONS);
            orgViewModelCash.SearchDivName = default;
            orgViewModelCash.SearchParentDivName = default;
            ModelCash<OrganizationViewModel>.SetViewModel(DIVISIONS, orgViewModelCash);
        }

        /// <summary>
        /// Метод для поиска должности
        /// </summary>
        /// <param name="orgViewModel"></param>
        public void SearchPosition(OrganizationViewModel orgViewModel)
        {
            viewsInfo.Reset(POSITIONS);
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(POSITIONS);
            orgViewModelCash.Id = orgViewModel.Id;
            orgViewModelCash.SearchPosName = orgViewModel.SearchPosName?.ToLower().TrimStartAndEnd();
            orgViewModelCash.SeacrhPositionDivName = orgViewModel.SeacrhPositionDivName?.ToLower().TrimStartAndEnd();
            orgViewModelCash.SearchPrimaryEmployeeName = orgViewModel.SearchPrimaryEmployeeName?.ToLower().TrimStartAndEnd();
            orgViewModelCash.SearchParentPosName = orgViewModel.SearchParentPosName?.ToLower().TrimStartAndEnd();
            ModelCash<OrganizationViewModel>.SetViewModel(POSITIONS, orgViewModelCash);
        }

        /// <summary>
        /// Метод очищает кеш для модели с должностями
        /// </summary>
        public void ClearPositionSearch()
        {
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(POSITIONS);
            orgViewModelCash.SearchPosName = default;
            orgViewModelCash.SeacrhPositionDivName = default;
            orgViewModelCash.SearchPrimaryEmployeeName = default;
            orgViewModelCash.SearchParentPosName = default;
            ModelCash<OrganizationViewModel>.SetViewModel(POSITIONS, orgViewModelCash);
        }

        /// <summary>
        /// Метод для поиска сотрудника
        /// </summary>
        /// <param name="orgViewModel"></param>
        public void SearchEmployee(OrganizationViewModel orgViewModel)
        {
            viewsInfo.Reset(EMPLOYEES);
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(EMPLOYEES);
            orgViewModelCash.Id = orgViewModel.Id;
            orgViewModelCash.SearchEmployeeName = orgViewModel.SearchEmployeeName?.ToLower().TrimStartAndEnd();
            orgViewModelCash.SearchEmployeePrimaryPosName = orgViewModel.SearchEmployeePrimaryPosName?.ToLower().TrimStartAndEnd();
            orgViewModelCash.SeacrhEmployeeDivName = orgViewModel.SeacrhEmployeeDivName?.ToLower().TrimStartAndEnd();
            ModelCash<OrganizationViewModel>.SetViewModel(EMPLOYEES, orgViewModelCash);
        }

        /// <summary>
        /// Метод очищает кеш для модели с сотрудниками
        /// </summary>
        public void ClearEmployeeSearch()
        {
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(EMPLOYEES);
            orgViewModelCash.SearchEmployeeName = default;
            orgViewModelCash.SearchEmployeePrimaryPosName = default;
            orgViewModelCash.SeacrhEmployeeDivName = default;
            ModelCash<OrganizationViewModel>.SetViewModel(EMPLOYEES, orgViewModelCash);
        }
        #endregion

        #region Attaching Organizations
        /// <summary>
        /// Метод добавляет список моделей представления организаций
        /// </summary>
        /// <param name="orgsViewModel"></param>
        public void AttachOrganizations(ref OrganizationsViewModel orgsViewModel)
        {
            orgsViewModel.OrganizationViewModels = context.GetOrganizations(currentUser)
                .TransformToViewModels<Organization, OrganizationViewModel, OrganizationTransformer>(
                    transformer: new OrganizationTransformer(context, resManager),
                    limitingFunc: GetLimitedList);
        }

        /// <summary>
        /// Метод ограничивает список организаций по существующему фильтру
        /// </summary>
        /// <param name="organizations"></param>
        /// <returns></returns>
        private List<Organization> GetLimitedList(List<Organization> organizations)
        {
            List<Organization> limitedOrgs = GetLimitedOrgsList(organizations);
            LimitListByPageNumber(ORGANIZATIONS, ref limitedOrgs);
            return limitedOrgs;
        }

        private List<Organization> GetLimitedOrgsList(List<Organization> orgsToLimit)
        {
            OrganizationsViewModel orgsViewModelCash = ModelCash<OrganizationsViewModel>.GetViewModel(ORGANIZATIONS);
            if (!string.IsNullOrEmpty(orgsViewModelCash.SearchName))
                orgsToLimit = orgsToLimit.Where(n => n.Name.ToLower().Contains(orgsViewModelCash.SearchName)).ToList();
            return orgsToLimit;
        }
        #endregion

        #region Attaching Divions
        /// <summary>
        /// Добавляет подразделения к организации, преобразую данные в отображение для выбранной страницы
        /// </summary>
        /// <returns></returns>
        public void AttachDivisions(OrganizationViewModel orgViewModel)
        {
            orgViewModel.Divisions = orgViewModel.GetDivisions(context)
                .TransformToViewModels<Division, DivisionViewModel, DivisionTransformer>(
                    transformer: new DivisionTransformer(context, resManager),
                    limitingFunc: GetLimitedDivisionsList);
        }

        private List<Division> GetLimitedDivisionsList(List<Division> divisions)
        {
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(DIVISIONS);
            List<Division> limitedDivisions = divisions;
            LimitDivByName(orgViewModelCash, ref limitedDivisions);
            LimitDivByParent(orgViewModelCash, divisions, ref limitedDivisions);
            LimitListByPageNumber(DIVISIONS, ref limitedDivisions);
            return limitedDivisions;
        }

        /// <summary>
        /// Ограничение списка подразделений по названию
        /// </summary>
        /// <param name="orgViewModelCash"></param>
        /// <param name="divisionsToLimit"></param>
        private void LimitDivByName(OrganizationViewModel orgViewModelCash, ref List<Division> divisionsToLimit)
        {
            if (!string.IsNullOrEmpty(orgViewModelCash.SearchDivName))
                divisionsToLimit = divisionsToLimit.Where(n => n.Name.ToLower().Contains(orgViewModelCash.SearchDivName)).ToList();
        }

        /// <summary>
        /// Ограничение списка подразделений по названию родительского
        /// </summary>
        /// <param name="orgViewModelCash"></param>
        /// <param name="allDivisions">Список всех подразделений организации</param>
        /// <param name="divisionsToLimit"></param>
        private void LimitDivByParent(OrganizationViewModel orgViewModelCash, List<Division> allDivisions, ref List<Division> divisionsToLimit)
        {
            if (!string.IsNullOrEmpty(orgViewModelCash.SearchParentDivName))
            {
                TransformCollection(
                    collectionToLimit: ref divisionsToLimit,
                    limitingCollection: allDivisions.Where(n => n.Name.Contains(orgViewModelCash.SearchParentDivName)).ToList(),
                    limitCondition: n => n.Name.ToLower().Contains(orgViewModelCash.SearchParentDivName),
                    selectCondition: i => i.Id,
                    removeCondition: (parentDivisionIdList, division) => division.ParentDivisionId == null || !parentDivisionIdList.Contains((Guid)division.ParentDivisionId));
            }
        }
        #endregion

        #region Attaching Positions
        /// <summary>
        /// Добавляет должности к организации, преобразую данные в отображение для выбранной страницы
        /// </summary>
        /// <returns></returns>
        public void AttachPositions(OrganizationViewModel orgViewModel)
        {
            orgViewModel.Positions = orgViewModel.GetPositions(context)
                .TransformToViewModels<Position, PositionViewModel, PositionTransformer>(
                    transformer: new PositionTransformer(context, resManager),
                    limitingFunc: GetLimitedPositionsList);
        }

        private List<Position> GetLimitedPositionsList(List<Position> positions)
        {
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(POSITIONS);
            List<Position> limitedPositions = positions;
            LimitPosByName(orgViewModelCash, ref limitedPositions);
            LimitPosByDivision(orgViewModelCash, ref limitedPositions);
            LimitPosByPrimaryEmployee(orgViewModelCash, ref limitedPositions);
            LimitPosByParent(orgViewModelCash, ref limitedPositions);
            LimitListByPageNumber(POSITIONS, ref limitedPositions);
            return limitedPositions;
        }

        /// <summary>
        /// Ограничение списка должностей по названию
        /// </summary>
        /// <param name="orgViewModelCash"></param>
        /// <param name="positionsToLimit"></param>
        private void LimitPosByName(OrganizationViewModel orgViewModelCash, ref List<Position> positionsToLimit)
        {
            if (!string.IsNullOrEmpty(orgViewModelCash.SearchPosName))
                positionsToLimit = positionsToLimit.Where(n => n.Name.ToLower().Contains(orgViewModelCash.SearchPosName)).ToList();
        }

        /// <summary>
        /// Ограничение списка должностей по названию подразделения
        /// </summary>
        /// <param name="orgViewModelCash"></param>
        /// <param name="positionsToLimit"></param>
        private void LimitPosByDivision(OrganizationViewModel orgViewModelCash, ref List<Position> positionsToLimit)
        {
            if (!string.IsNullOrEmpty(orgViewModelCash.SeacrhPositionDivName))
            {
                TransformCollection(
                    collectionToLimit: ref positionsToLimit,
                    limitingCollection: context.GetOrgDivisions(orgViewModelCash.Id),
                    limitCondition: n => n.Name.ToLower().Contains(orgViewModelCash.SeacrhPositionDivName),
                    selectCondition: i => i.Id,
                    removeCondition: (divisionIdList, position) => !divisionIdList.Contains(position.DivisionId));
            }
        }

        /// <summary>
        /// Ограничение списка должностей по фио основного сотрудника
        /// </summary>
        /// <param name="orgViewModelCash"></param>
        /// <param name="positionsToLimit"></param>
        private void LimitPosByPrimaryEmployee(OrganizationViewModel orgViewModelCash, ref List<Position> positionsToLimit)
        {
            if (!string.IsNullOrEmpty(orgViewModelCash.SearchPrimaryEmployeeName))
            {
                TransformCollection(
                    collectionToLimit: ref positionsToLimit,
                    limitingCollection: context.GetOrgEmployees(orgViewModelCash.Id),
                    limitCondition: n => n.GetFullName().ToLower().Contains(orgViewModelCash.SearchPrimaryEmployeeName),
                    selectCondition: i => i.Id,
                    removeCondition: (employeeIdList, position) => position.PrimaryEmployeeId == null || !employeeIdList.Contains((Guid)position.PrimaryEmployeeId));
            }
        }

        /// <summary>
        /// Ограничение списка должностей по названию родительской
        /// </summary>
        /// <param name="orgViewModelCash"></param>
        /// <param name="positionsToLimit"></param>
        private void LimitPosByParent(OrganizationViewModel orgViewModelCash, ref List<Position> positionsToLimit)
        {
            if (!string.IsNullOrEmpty(orgViewModelCash.SearchParentPosName))
            {
                TransformCollection(
                    collectionToLimit: ref positionsToLimit,
                    limitingCollection: context.GetOrgPositions(orgViewModelCash.Id),
                    limitCondition: n => n.Name.ToLower().Contains(orgViewModelCash.SearchParentPosName),
                    selectCondition: i => i.Id,
                    removeCondition: (positionIdList, position) => position.ParentPositionId == null || !positionIdList.Contains((Guid)position.ParentPositionId));
            }
        }
        #endregion

        #region Attaching Employees
        /// <summary>
        /// Добавляет сотрудников к организации, преобразую данные в отображение для выбранной страницы
        /// </summary>
        /// <returns></returns>
        public void AttachEmployees(OrganizationViewModel orgViewModel)
        {
            orgViewModel.Employees = orgViewModel.GetEmployees(context)
                .TransformToViewModels<Employee, EmployeeViewModel, EmployeeTransformer>(
                    transformer: new EmployeeTransformer(context, resManager),
                    limitingFunc: GetLimitedEmployeesList);
        }

        private List<Employee> GetLimitedEmployeesList(List<Employee> employees)
        {
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(EMPLOYEES);
            List<Employee> limitedEmployees = employees;
            LimitEmpByName(orgViewModelCash, ref limitedEmployees);
            LimitEmpByPrimaryPosition(orgViewModelCash, ref limitedEmployees);
            LimitEmpByDivision(orgViewModelCash, ref limitedEmployees);
            LimitListByPageNumber(EMPLOYEES, ref limitedEmployees);
            return limitedEmployees;
        }

        /// <summary>
        /// Ограничение списка сотрудников по фио
        /// </summary>
        /// <param name="orgViewModelCash"></param>
        /// <param name="employeesToLimit"></param>
        private void LimitEmpByName(OrganizationViewModel orgViewModelCash, ref List<Employee> employeesToLimit)
        {
            if (!string.IsNullOrEmpty(orgViewModelCash.SearchEmployeeName))
                employeesToLimit = employeesToLimit.Where(n => n.GetFullName().ToLower().Contains(orgViewModelCash.SearchEmployeeName)).ToList();
        }

        /// <summary>
        /// Ограничение списка сотрудников по названию основной должности
        /// </summary>
        /// <param name="orgViewModelCash"></param>
        /// <param name="employeesToLimit"></param>
        private void LimitEmpByPrimaryPosition(OrganizationViewModel orgViewModelCash, ref List<Employee> employeesToLimit)
        {
            if (!string.IsNullOrEmpty(orgViewModelCash.SearchEmployeePrimaryPosName))
            {
                TransformCollection(
                    collectionToLimit: ref employeesToLimit,
                    limitingCollection: context.GetOrgPositions(orgViewModelCash.Id),
                    limitCondition: n => n.Name.ToLower().Contains(orgViewModelCash.SearchEmployeePrimaryPosName),
                    selectCondition: i => i.Id,
                    removeCondition: (positionIdList, employee) => employee.PrimaryPositionId == null || !positionIdList.Contains((Guid)employee.PrimaryPositionId));
            }
        }

        /// <summary>
        /// Ограничение списка сотрудников по названию подразделения
        /// </summary>
        /// <param name="orgViewModelCash"></param>
        /// <param name="employeesToLimit"></param>
        private void LimitEmpByDivision(OrganizationViewModel orgViewModelCash, ref List<Employee> employeesToLimit)
        {
            if (!string.IsNullOrEmpty(orgViewModelCash.SeacrhEmployeeDivName))
            {
                TransformCollection(
                    collectionToLimit: ref employeesToLimit,
                    limitingCollection: context.GetOrgDivisions(orgViewModelCash.Id),
                    limitCondition: n => n.Name.ToLower().Contains(orgViewModelCash.SeacrhEmployeeDivName),
                    selectCondition: i => i.Id,
                    removeCondition: (divisionIdList, employee) => !divisionIdList.Contains(employee.DivisionId));
            }
        }
        #endregion

        #region Other Methods
        /// <summary>
        /// Метод возвращает список моделей представления сотрудника, ограниченнный по id организации и имени
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="employeePart"></param>
        /// <returns></returns>
        public List<EmployeeViewModel> GetOrgEmployeeViewModels(Guid organizationId, string employeePart)
        {
            List<EmployeeViewModel> employeeViewModels = context.GetOrgEmployees(organizationId)
                .TransformToViewModels<Employee, EmployeeViewModel, EmployeeTransformer>(
                    transformer: new EmployeeTransformer(context, resManager),
                    limitingFunc: n => n.GetFullName().ToLower().Contains(employeePart.ToLower().TrimStartAndEnd()) && n.EmployeeStatus != EmployeeStatus.Lock);
            return employeeViewModels;
        }
        #endregion
    }
}
