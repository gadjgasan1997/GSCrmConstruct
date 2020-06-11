using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmApplication.Data;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmApplication.Factories.MainFactories;
using GSCrmApplication.Models.TableModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Data;
using SysEmployee_1 = GSCrmApplication.Models.TableModels.Employee;
using SysEmployee_2 = GSCrmApplication.Models.BusinessComponentModels.Employee;

namespace GSCrmApplication.Factories.DataBUSFactories
{
	public class DataBUSEmployeeFR : DataBUSFactory<SysEmployee_1, SysEmployee_2, GSAppContext>
	{
        public override SysEmployee_2 DataToBusiness(SysEmployee_1 dataEntity, GSAppContext context)
        {
            SysEmployee_2 businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.PrimaryPositionId = dataEntity.PrimaryPositionId;
            businessEntity.DivisionId = dataEntity.DivisionId;
            businessEntity.LastName = dataEntity.LastName;
            businessEntity.FirstName = dataEntity.FirstName;
            businessEntity.MiddleName = dataEntity.MiddleName;
            Position PrimaryPosition = context.Position.AsNoTracking().FirstOrDefault(i => i.Id == businessEntity.PrimaryPositionId);
            if (PrimaryPosition != null)
            {
                businessEntity.PrimaryPositionName = PrimaryPosition.Name;
                businessEntity.ParentPrimaryPositionId = PrimaryPosition.ParentPositionId;
                businessEntity.ParentEmployeeId = PrimaryPosition.PrimaryEmployeeId;
            }
            Employee Manager = context.Employee.AsNoTracking().FirstOrDefault(i => i.Id == businessEntity.ParentEmployeeId);
            if (Manager != null)
            {
                businessEntity.ManagerMiddleName = Manager.MiddleName;
                businessEntity.ManagerLastName = Manager.LastName;
                businessEntity.ManagerFirstName = Manager.FirstName;
            }
            businessEntity.ManagerFullName = businessEntity.ManagerLastName + " " + businessEntity.ManagerFirstName + " " + businessEntity.ManagerMiddleName?[0];
            businessEntity.FullName = businessEntity.LastName + " " + businessEntity.FirstName?[0] + ". "+ businessEntity.MiddleName?[0] + ".";
            return businessEntity;
        }
        public override SysEmployee_1 BusinessToData(SysEmployee_1 sysemployee_1, SysEmployee_2 businessEntity, GSAppContext context, bool NewRecord)
        {
            SysEmployee_1 dataEntity = base.BusinessToData(sysemployee_1, businessEntity, context, NewRecord);
            dataEntity.PrimaryPositionId = businessEntity.PrimaryPositionId;
            dataEntity.DivisionId = businessEntity.DivisionId;
            dataEntity.LastName = businessEntity.LastName;
            dataEntity.FirstName = businessEntity.FirstName;
            dataEntity.MiddleName = businessEntity.MiddleName;
            return dataEntity;
        }

        public override IEnumerable<ValidationResult> DataBUSValidate(SysEmployee_1 dataEntity, SysEmployee_2 businessEntity, IViewInfo viewInfo, GSAppContext context)
        {
            List<ValidationResult> result = base.DataBUSValidate(dataEntity, businessEntity, viewInfo, context).ToList();
            Applet currentApplet = viewInfo.CurrentPopupApplet ?? viewInfo.CurrentApplet;
            return result;
        }
	}
}
