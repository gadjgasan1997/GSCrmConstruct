using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using static GSCrm.CommonConsts;

namespace GSCrm.DataTransformers
{
    public class EmployeeTransformer : BaseTransformer<Employee, EmployeeViewModel>
    {
        public EmployeeTransformer(ApplicationDbContext context, ResManager resManager) : base (context, resManager) { }

        public override Employee OnModelCreate(EmployeeViewModel employeeViewModel)
        {
            Organization currentOrganization = employeeViewModel.GetOrganization(context);
            Division division = employeeViewModel.GetDivision(currentOrganization);
            Position primaryPosition = employeeViewModel.GetPrimaryPosition(division);

            Employee employee = new Employee()
            {
                FirstName = employeeViewModel.FirstName,
                LastName = employeeViewModel.LastName,
                MiddleName = employeeViewModel.MiddleName,
                PrimaryPositionId = primaryPosition.Id,
                DivisionId = division.Id,
                UserId = Guid.Parse(employeeViewModel.UserId)
            };

            EmployeePosition employeePosition = new EmployeePosition()
            {
                Employee = employee,
                EmployeeId = employee.Id,
                Position = primaryPosition,
                PositionId = primaryPosition.Id
            };
            context.Entry(employeePosition).State = EntityState.Added;
            employee.EmployeePositions.Add(employeePosition);
            return employee;
        }

        public override Employee OnModelUpdate(EmployeeViewModel employeeViewModel)
        {
            Employee oldEmployee = context.Employees
                .Include(empPos => empPos.EmployeePositions)
                .Include(empCon => empCon.EmployeeContacts)
                .FirstOrDefault(i => i.Id == employeeViewModel.Id);
            oldEmployee.FirstName = employeeViewModel.FirstName;
            oldEmployee.LastName = employeeViewModel.LastName;
            oldEmployee.MiddleName = employeeViewModel.MiddleName;
            return oldEmployee;
        }

        public override EmployeeViewModel DataToViewModel(Employee employee)
        {
            Position primaryPosition = employee.GetPrimaryPosition(context);
            Position parentPosition = primaryPosition == null ? null : primaryPosition.GetParentPosition(context);
            Employee supervisor = parentPosition == null ? null : context.Employees.FirstOrDefault(i => i.Id == parentPosition.PrimaryEmployeeId);
            Division division = employee.GetDivision(context);
            Organization organization = division.GetOrganization(context);
            return new EmployeeViewModel()
            {
                Id = employee.Id,
                OrganizationId = organization.Id,
                OrganizationName = organization.Name,
                DivisionId = division.Id,
                DivisionName = division.Name,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                FullName = employee.GetFullName(),
                FullInitialName = employee.GetIntialsFullName(),
                PrimaryPositionId = primaryPosition?.Id,
                PrimaryPositionName = primaryPosition?.Name,
                EmployeeStatus = employee.EmployeeStatus.ToString(),
                SupervisorId = supervisor?.Id.ToString(),
                SupervisorInitialName = supervisor?.GetIntialsFullName()
            };
        }

        public override EmployeeViewModel UpdateViewModelFromCash(EmployeeViewModel employeeViewModel)
        {
            EmployeeViewModel empPosViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_POSITIONS);
            EmployeeViewModel empContactViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_CONTACTS);
            employeeViewModel.SearchPosName = empPosViewModelCash.SearchPosName;
            employeeViewModel.SearchParentPosName = empPosViewModelCash.SearchParentPosName;
            employeeViewModel.SearchContactType = empContactViewModelCash.SearchContactType;
            employeeViewModel.SearchContactPhone = empContactViewModelCash.SearchContactPhone;
            employeeViewModel.SearchContactEmail = empContactViewModelCash.SearchContactEmail;
            return employeeViewModel;
        }
    }
}
