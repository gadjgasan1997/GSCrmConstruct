using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSBusObjectFR<TContext> : DataBUSFactory<BusinessObject, BUSBusinessObject, TContext>
        where TContext : MainContext, new()
    {
        public override BUSBusinessObject DataToBusiness(BusinessObject dataEntity, TContext context)
        {
            BUSBusinessObject businessEntity = base.DataToBusiness(dataEntity, context);
            BusinessComponent primaryBusComp = context.BusinessComponents.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.PrimaryBusCompId);
            if (primaryBusComp != null)
            {
                businessEntity.PrimaryBusComp = primaryBusComp;
                businessEntity.PrimaryBusCompId = primaryBusComp.Id;
                businessEntity.PrimaryBusCompName = primaryBusComp.Name;
            }
            return businessEntity;
        }
        public override BusinessObject BusinessToData(BusinessObject busObject, BUSBusinessObject businessEntity, TContext context, bool NewRecord)
        {
            BusinessObject dataEntity = base.BusinessToData(busObject, businessEntity, context, NewRecord);
            dataEntity.PrimaryBusCompId = businessEntity.PrimaryBusCompId;
            return dataEntity;
        }
    }
}
