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
using SysContact_1 = GSCrmApplication.Models.TableModels.Contact;
using SysContact_2 = GSCrmApplication.Models.BusinessComponentModels.Contact;

namespace GSCrmApplication.Factories.DataBUSFactories
{
	public class DataBUSContactFR : DataBUSFactory<SysContact_1, SysContact_2, GSAppContext>
	{
        public override SysContact_2 DataToBusiness(SysContact_1 dataEntity, GSAppContext context)
        {
            SysContact_2 businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.HomeNumber = dataEntity.HomeNumber;
            businessEntity.AccountId = dataEntity.AccountId;
            businessEntity.PersonId = dataEntity.PersonId;
            businessEntity.CellNumber = dataEntity.CellNumber;
            businessEntity.Email = dataEntity.Email;
            businessEntity.WorkNumber = dataEntity.WorkNumber;
            Employee Employee = context.Employee.AsNoTracking().FirstOrDefault(i => i.Id == businessEntity.PersonId);
            if (Employee != null)
            {
                businessEntity.MiddleName = Employee.MiddleName;
                businessEntity.LastName = Employee.LastName;
                businessEntity.FirstName = Employee.FirstName;
            }
            Account Account = context.Account.AsNoTracking().FirstOrDefault(i => i.Id == businessEntity.AccountId);
            if (Account != null)
            {
                businessEntity.AccountName = Account.Name;
            }
            return businessEntity;
        }
        public override SysContact_1 BusinessToData(SysContact_1 syscontact_1, SysContact_2 businessEntity, GSAppContext context, bool NewRecord)
        {
            SysContact_1 dataEntity = base.BusinessToData(syscontact_1, businessEntity, context, NewRecord);
            dataEntity.HomeNumber = businessEntity.HomeNumber;
            dataEntity.AccountId = businessEntity.AccountId;
            dataEntity.PersonId = businessEntity.PersonId;
            dataEntity.CellNumber = businessEntity.CellNumber;
            dataEntity.Email = businessEntity.Email;
            dataEntity.WorkNumber = businessEntity.WorkNumber;
            return dataEntity;
        }

        public override IEnumerable<ValidationResult> DataBUSValidate(SysContact_1 dataEntity, SysContact_2 businessEntity, IViewInfo viewInfo, GSAppContext context)
        {
            List<ValidationResult> result = base.DataBUSValidate(dataEntity, businessEntity, viewInfo, context).ToList();
            Applet currentApplet = viewInfo.CurrentPopupApplet ?? viewInfo.CurrentApplet;
            return result;
        }
	}
}
