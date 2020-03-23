using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Factories.Default.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GSCrm.Factories.Default.DataBUSFactories
{
    public class DataBUSLinkFR : MainDataBUSFR<Link, BUSLink>
    {
        public override BUSLink DataToBusiness(Link dataEntity, ApplicationContext context)
        {
            BUSLink businessEntity = base.DataToBusiness(dataEntity, context);
            // Parent bc
            BusComp parentBusComp = context.BusinessComponents
                .Include(field => field.Fields)
                .FirstOrDefault(i => i.Id == dataEntity.ParentBCId);
            if (parentBusComp != null)
            {
                businessEntity.ParentBusComp = parentBusComp;
                businessEntity.ParentBCId = parentBusComp.Id;
                businessEntity.ParentBCName = parentBusComp.Name;
            }

            // Parent field
            Field parentField = parentBusComp.Fields.FirstOrDefault(i => i.Id == dataEntity.ParentFieldId);
            if (parentField != null)
            {
                businessEntity.ParentField = parentField;
                businessEntity.ParentFieldId = parentField.Id;
                businessEntity.ParentFieldName = parentField.Name;
            }

            // Child bc
            BusComp childBusComp = context.BusinessComponents
                .Include(field => field.Fields)
                .FirstOrDefault(i => i.Id == dataEntity.ChildBCId);
            if (childBusComp != null)
            {
                businessEntity.ChildBusComp = childBusComp;
                businessEntity.ChildBCId = childBusComp.Id;
                businessEntity.ChildBCName = childBusComp.Name;
            }

            // Child field
            Field childField = childBusComp.Fields.FirstOrDefault(i => i.Id == dataEntity.ChildFieldId);
            if (childField != null)
            {
                businessEntity.ChildField = childField;
                businessEntity.ChildFieldId = childField.Id;
                businessEntity.ChildFieldName = childField.Name;
            }
            return businessEntity;
        }
        public override Link BusinessToData(BUSLink businessEntity, DbSet<Link> entityDBSet, bool NewRecord)
        {
            Link dataEntity = base.BusinessToData(businessEntity, entityDBSet, NewRecord);
            dataEntity.ParentBCId = businessEntity.ParentBCId;
            dataEntity.ParentFieldId = businessEntity.ParentFieldId;
            dataEntity.ChildBCId = businessEntity.ChildBCId;
            dataEntity.ChildFieldId = businessEntity.ChildFieldId;
            return dataEntity;
        }
    }
}
