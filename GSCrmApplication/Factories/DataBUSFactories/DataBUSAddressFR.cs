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
using SysAddress_1 = GSCrmApplication.Models.TableModels.Address;
using SysAddress_2 = GSCrmApplication.Models.BusinessComponentModels.Address;

namespace GSCrmApplication.Factories.DataBUSFactories
{
	public class DataBUSAddressFR : DataBUSFactory<SysAddress_1, SysAddress_2, GSAppContext>
	{
        public override SysAddress_2 DataToBusiness(SysAddress_1 dataEntity, GSAppContext context)
        {
            SysAddress_2 businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.AccountId = dataEntity.AccountId;
            businessEntity.Street = dataEntity.Street;
            businessEntity.City = dataEntity.City;
            businessEntity.Country = dataEntity.Country;
            businessEntity.House = dataEntity.House;
            businessEntity.FullAddress = businessEntity.Country + ", " + businessEntity.City + ", " + businessEntity.Street + ", " + businessEntity.House;
            return businessEntity;
        }
        public override SysAddress_1 BusinessToData(SysAddress_1 sysaddress_1, SysAddress_2 businessEntity, GSAppContext context, bool NewRecord)
        {
            SysAddress_1 dataEntity = base.BusinessToData(sysaddress_1, businessEntity, context, NewRecord);
            dataEntity.AccountId = businessEntity.AccountId;
            dataEntity.Street = businessEntity.Street;
            dataEntity.City = businessEntity.City;
            dataEntity.Country = businessEntity.Country;
            dataEntity.House = businessEntity.House;
            return dataEntity;
        }

        public override IEnumerable<ValidationResult> DataBUSValidate(SysAddress_1 dataEntity, SysAddress_2 businessEntity, IViewInfo viewInfo, GSAppContext context)
        {
            List<ValidationResult> result = base.DataBUSValidate(dataEntity, businessEntity, viewInfo, context).ToList();
            Applet currentApplet = viewInfo.CurrentPopupApplet ?? viewInfo.CurrentApplet;
            return result;
        }
	}
}
