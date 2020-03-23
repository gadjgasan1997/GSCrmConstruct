using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSControlUPFR : MainUPDataBUSFR<ControlUP, BUSControlUP>
    {
        public override BUSControlUP DataToBusiness(ControlUP dataEntity, ApplicationContext context)
        {
            BUSControlUP businessEntity = base.DataToBusiness(dataEntity, context);
            businessEntity.ControlId = dataEntity.ControlId;
            businessEntity.Control = context.Controls.FirstOrDefault(i => i.Id == businessEntity.ControlId);
            return businessEntity;
        }
        public override ControlUP BusinessToData(BUSControlUP businessEntity, DbSet<ControlUP> entityDBSet, bool NewRecord)
        {
            ControlUP dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.Control = businessEntity.Control;
            dataEntity.ControlId = businessEntity.ControlId;
            return dataEntity;
        }
    }
}
