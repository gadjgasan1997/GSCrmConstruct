using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using static GSCrm.CommonConsts;

namespace GSCrm.DataTransformers
{
    public class OrganizationTransformer : BaseTransformer<Organization, OrganizationViewModel>
    {
        User currentUser;
        public OrganizationTransformer(ApplicationDbContext context, ResManager resManager, HttpContext httpContext = null) : base(context, resManager)
        {
            if (httpContext != null)
                currentUser = httpContext.GetCurrentUser(context);
        }

        public override Organization OnModelCreate(OrganizationViewModel orgViewModel)
        {
            Guid newOrgId = Guid.NewGuid();
            User user = context.Users.FirstOrDefault(n => n.UserName == currentUser.UserName);
            orgViewModel.OwnerId = user.Id;
            return new Organization()
            {
                Id = newOrgId,
                Name = orgViewModel.Name,
                OwnerId = orgViewModel.OwnerId,
                UserOrganizations = new List<UserOrganization>()
                {
                    new UserOrganization()
                    {
                        OrganizationId = newOrgId,
                        User = user,
                        UserId = user.Id
                    }
                }
            };
        }

        public override OrganizationViewModel DataToViewModel(Organization organization)
        {
            return new OrganizationViewModel()
            {
                Id = organization.Id,
                OwnerId = organization.OwnerId,
                Name = organization.Name
            };
        }

        public override OrganizationViewModel UpdateViewModelFromCash(OrganizationViewModel orgViewModel)
        {
            OrganizationViewModel orgViewModelCash = ModelCash<OrganizationViewModel>.GetViewModel(ORGANIZATION);
            orgViewModel.SeacrhEmployeeDivName = orgViewModelCash.SeacrhEmployeeDivName;
            orgViewModel.SeacrhPositionDivName = orgViewModelCash.SeacrhPositionDivName;
            orgViewModel.SearchDivName = orgViewModelCash.SearchDivName;
            orgViewModel.SearchEmployeeName = orgViewModelCash.SearchEmployeeName;
            orgViewModel.SearchEmployeePrimaryPosName = orgViewModelCash.SearchEmployeePrimaryPosName;
            orgViewModel.SearchParentDivName = orgViewModelCash.SearchParentDivName;
            orgViewModel.SearchParentPosName = orgViewModelCash.SearchParentPosName;
            orgViewModel.SearchPosName = orgViewModelCash.SearchPosName;
            orgViewModel.SearchPrimaryEmployeeName = orgViewModelCash.SearchPrimaryEmployeeName;
            return orgViewModel;
        }
    }
}
