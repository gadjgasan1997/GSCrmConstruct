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
using SysInvoice_1 = GSCrmApplication.Models.TableModels.Invoice;
using SysInvoice_2 = GSCrmApplication.Models.BusinessComponentModels.Invoice;

namespace GSCrmApplication.Factories.DataBUSFactories
{
	public class DataBUSInvoiceFR : DataBUSFactory<SysInvoice_1, SysInvoice_2, GSAppContext>
	{
        public override SysInvoice_2 DataToBusiness(SysInvoice_1 dataEntity, GSAppContext context)
        {
            SysInvoice_2 businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.BIC = dataEntity.BIC;
            businessEntity.BankAccount = dataEntity.BankAccount;
            businessEntity.AccountId = dataEntity.AccountId;
            return businessEntity;
        }
        public override SysInvoice_1 BusinessToData(SysInvoice_1 sysinvoice_1, SysInvoice_2 businessEntity, GSAppContext context, bool NewRecord)
        {
            SysInvoice_1 dataEntity = base.BusinessToData(sysinvoice_1, businessEntity, context, NewRecord);
            dataEntity.BIC = businessEntity.BIC;
            dataEntity.BankAccount = businessEntity.BankAccount;
            dataEntity.AccountId = businessEntity.AccountId;
            return dataEntity;
        }

        public override IEnumerable<ValidationResult> DataBUSValidate(SysInvoice_1 dataEntity, SysInvoice_2 businessEntity, IViewInfo viewInfo, GSAppContext context)
        {
            List<ValidationResult> result = base.DataBUSValidate(dataEntity, businessEntity, viewInfo, context).ToList();
            Applet currentApplet = viewInfo.CurrentPopupApplet ?? viewInfo.CurrentApplet;
            return result;
        }
	}
}
