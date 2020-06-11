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
using SysPosition_1 = GSCrmApplication.Models.TableModels.Position;
using SysPosition_2 = GSCrmApplication.Models.BusinessComponentModels.Position;

namespace GSCrmApplication.Factories.DataBUSFactories
{
	public class DataBUSPositionFR : DataBUSFactory<SysPosition_1, SysPosition_2, GSAppContext>
	{
        public override SysPosition_2 DataToBusiness(SysPosition_1 dataEntity, GSAppContext context)
        {
            SysPosition_2 businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.ParentPositionId = dataEntity.ParentPositionId;
            businessEntity.PrimaryEmployeeId = dataEntity.PrimaryEmployeeId;
            businessEntity.AccountId = dataEntity.AccountId;
            businessEntity.Name = dataEntity.Name;
            Employee PrimaryEmployee = context.Employee.AsNoTracking().FirstOrDefault(i => i.Id == businessEntity.PrimaryEmployeeId);
            if (PrimaryEmployee != null)
            {
                businessEntity.PrimaryEmployeeLastName = PrimaryEmployee.LastName;
                businessEntity.PrimaryEmployeeFirstName = PrimaryEmployee.FirstName;
                businessEntity.PrimaryEmployeeMiddleName = PrimaryEmployee.MiddleName;
            }
            Position ParentPosition = context.Position.AsNoTracking().FirstOrDefault(i => i.Id == businessEntity.ParentPositionId);
            if (ParentPosition != null)
            {
                businessEntity.ParentPositionName = ParentPosition.Name;
            }
            businessEntity.PrimaryEmployeeFullName = businessEntity.PrimaryEmployeeLastName + " " + businessEntity.PrimaryEmployeeFirstName?[0] + ". " + businessEntity.PrimaryEmployeeMiddleName?[0] + ".";
            return businessEntity;
        }
        public override SysPosition_1 BusinessToData(SysPosition_1 sysposition_1, SysPosition_2 businessEntity, GSAppContext context, bool NewRecord)
        {
            SysPosition_1 dataEntity = base.BusinessToData(sysposition_1, businessEntity, context, NewRecord);
            dataEntity.ParentPositionId = businessEntity.ParentPositionId;
            dataEntity.PrimaryEmployeeId = businessEntity.PrimaryEmployeeId;
            dataEntity.AccountId = businessEntity.AccountId;
            dataEntity.Name = businessEntity.Name;
            return dataEntity;
        }

        public override IEnumerable<ValidationResult> DataBUSValidate(SysPosition_1 dataEntity, SysPosition_2 businessEntity, IViewInfo viewInfo, GSAppContext context)
        {
            List<ValidationResult> result = base.DataBUSValidate(dataEntity, businessEntity, viewInfo, context).ToList();
            Applet currentApplet = viewInfo.CurrentPopupApplet ?? viewInfo.CurrentApplet;
            return result;
        }
	}
}
