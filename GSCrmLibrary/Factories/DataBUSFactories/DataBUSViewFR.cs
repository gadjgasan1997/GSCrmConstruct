using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GSCrmLibrary.Models.AppletModels;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSViewFR<TContext> : DataBUSFactory<View, BUSView, TContext>
        where TContext : MainContext, new()
    {
        public override BUSView DataToBusiness(View dataEntity, TContext context)
        {
            BUSView businessEntity = base.DataToBusiness(dataEntity, context);
            BusinessObject busObject = context.BusinessObjects.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.BusObjectId);
            businessEntity.BusObject = busObject;
            if (busObject != null)
            {
                businessEntity.BusObjectId = busObject.Id;
                businessEntity.BusObjectName = busObject.Name;
            }
            return businessEntity;
        }
        public override View BusinessToData(View view, BUSView businessEntity, TContext context, bool NewRecord)
        {
            View dataEntity = base.BusinessToData(view, businessEntity, context, NewRecord);
            dataEntity.BusObjectId = businessEntity.BusObjectId;
            return dataEntity;
        }
    }
}
