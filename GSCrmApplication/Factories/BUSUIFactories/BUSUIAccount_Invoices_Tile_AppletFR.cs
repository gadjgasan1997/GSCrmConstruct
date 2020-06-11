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
using SysInvoice = GSCrmApplication.Models.BusinessComponentModels.Invoice;
using SysAccount_Invoices_Tile_Applet = GSCrmApplication.Models.AppletModels.Account_Invoices_Tile_Applet;

namespace GSCrmApplication.Factories.BUSUIFactories
{
	public class BUSUIAccount_Invoices_Tile_AppletFR : BUSUIFactory<SysInvoice, SysAccount_Invoices_Tile_Applet, GSAppContext>
	{
        public override SysInvoice UIToBusiness(SysAccount_Invoices_Tile_Applet UIEntity, GSAppContext context, IViewInfo viewInfo, bool NewRecord)
        {
            SysInvoice businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            businessEntity.AccountId = Guid.Parse(ComponentsRecordsInfo.GetSelectedRecord("Account"));
            businessEntity.BIC = UIEntity.BIC;
            businessEntity.BankAccount = UIEntity.BankAccount;
            return businessEntity;
        }

        public override SysAccount_Invoices_Tile_Applet BusinessToUI(SysInvoice businessEntity)
        {
            SysAccount_Invoices_Tile_Applet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.BIC = businessEntity.BIC;
            UIEntity.BankAccount = businessEntity.BankAccount;
            return UIEntity;
        }

        public override IEnumerable<ValidationResult> UIValidate(GSAppContext context, IViewInfo viewInfo, SysAccount_Invoices_Tile_Applet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            return result;
        }

        public override IEnumerable<ValidationResult> BUSUIValidate(GSAppContext context, SysInvoice businessEntity, SysAccount_Invoices_Tile_Applet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessEntity, UIEntity).ToList();
            return result;
        }
	}
}
