using GSCrmLibrary.Data;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Factories.MainFactories;
using System.Linq;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSControlUPFR<TContext> : DataBUSUserPropertyFR<ControlUP, BUSControlUP, TContext>
        where TContext : MainContext, new()
    {
        public override BUSControlUP DataToBusiness(ControlUP dataEntity, TContext context)
        {
            BUSControlUP businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.ControlId = dataEntity.ControlId;
            businessEntity.Control = context.Controls.FirstOrDefault(i => i.Id == businessEntity.ControlId);
            return businessEntity;
        }
        public override ControlUP BusinessToData(ControlUP controlUP, BUSControlUP businessEntity, TContext context, bool NewRecord)
        {
            ControlUP dataEntity = base.BusinessToData(controlUP, businessEntity, context, NewRecord);
            dataEntity.Control = businessEntity.Control;
            dataEntity.ControlId = businessEntity.ControlId;
            return dataEntity;
        }
    }
}
