using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GSCrm.Models.ViewModels
{
    public class PositionViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public bool IsPrimary { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public Guid DivisionId { get; set; }
        public string DivisionName { get; set; }
        public Guid? ParentPositionId { get; set; }
        public string ParentPositionName { get; set; }
        public Guid? PrimaryEmployeeId { get; set; }
        public string PrimaryEmployeeInitialName { get; set; }
        public string SearchEmployeeInitialName { get; set; }
        public string SearchSubPositionName { get; set; }
        public string SearchSubPositionPrimaryEmployee { get; set; }
        public List<PositionViewModel> PositionsHierarchy { get; set; } = new List<PositionViewModel>();
        public List<EmployeeViewModel> PositionEmployees { get; set; } = new List<EmployeeViewModel>();
        public List<PositionViewModel> SubPositions { get; set; } = new List<PositionViewModel>();
    }
}
