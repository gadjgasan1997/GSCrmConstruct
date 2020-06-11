using System.Linq;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSPickListFR<TContext> : DataBUSFactory<PickList, BUSPickList, TContext>
        where TContext : MainContext, new()
    {
        public override BUSPickList DataToBusiness(PickList dataEntity, TContext context)
        {
            BUSPickList businessEntity = base.DataToBusiness(dataEntity, context);
            BusinessComponent busComp = context.BusinessComponents.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.BusCompId);
            if (busComp != null)
            {
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp.Id;
                businessEntity.BusCompName = busComp.Name;
            }

            businessEntity.SearchSpecification = dataEntity.SearchSpecification;
            businessEntity.Bounded = dataEntity.Bounded;
            return businessEntity;
        }
        public override PickList BusinessToData(PickList pL, BUSPickList businessEntity, TContext context, bool NewRecord)
        {
            PickList dataEntity = base.BusinessToData(pL, businessEntity, context, NewRecord);
            dataEntity.BusCompId = businessEntity.BusCompId;
            dataEntity.Bounded = businessEntity.Bounded;
            dataEntity.SearchSpecification = businessEntity.SearchSpecification;
            return dataEntity;
        }
    }
}
