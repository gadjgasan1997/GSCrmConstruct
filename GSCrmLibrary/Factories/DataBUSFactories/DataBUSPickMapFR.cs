using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSPickMapFR<TContext> : DataBUSFactory<PickMap, BUSPickMap, TContext>
        where TContext : MainContext, new()
    {
        public override BUSPickMap DataToBusiness(PickMap dataEntity, TContext context)
        {
            BUSPickMap businessEntity = base.DataToBusiness(dataEntity, context);
            
            // Field
            Field field = context.Fields
                .AsNoTracking()
                .Select(f => new
                {
                    id = f.Id,
                    busComp = f.BusComp,
                    busCompId = f.BusCompId
                })
                .Select(f => new Field
                {
                    Id = f.id,
                    BusComp = f.busComp,
                    BusCompId = f.busCompId
                })
                .FirstOrDefault(i => i.Id == dataEntity.FieldId);
            businessEntity.Field = field;
            businessEntity.FieldId = field.Id;
            businessEntity.BusComp = field.BusComp;
            businessEntity.BusCompId = field.BusCompId;

            // BusCompField
            Field busCompField = context.Fields.AsNoTracking().FirstOrDefault(i => i.Id == dataEntity.BusCompFieldId);
            if (busCompField != null)
            {
                businessEntity.BusCompField = busCompField;
                businessEntity.BusCompFieldId = busCompField.Id;
                businessEntity.BusCompFieldName = busCompField.Name;
            }

            // PickListField
            Field pickListField = context.Fields
                .AsNoTracking()
                .Select(f => new
                {
                    id = f.Id,
                    name = f.Name,
                    busComp = f.BusComp,
                    busCompId = f.BusCompId
                })
                .Select(f => new Field
                {
                    Id = f.id,
                    Name = f.name,
                    BusComp = f.busComp,
                    BusCompId = f.busCompId
                })
                .FirstOrDefault(i => i.Id == dataEntity.PickListFieldId);
            if (pickListField != null)
            {
                businessEntity.PickListField = pickListField;
                businessEntity.PickListFieldId = pickListField.Id;
                businessEntity.PickListFieldName = pickListField.Name;
                businessEntity.PickListBusComp = pickListField.BusComp;
                businessEntity.PickListBusCompId = pickListField.BusCompId;
            }

            businessEntity.Constrain = dataEntity.Constrain;
            return businessEntity;
        }
        public override PickMap BusinessToData(PickMap pickMap, BUSPickMap businessEntity, TContext context, bool NewRecord)
        {
            PickMap dataEntity = base.BusinessToData(pickMap, businessEntity, context, NewRecord);
            dataEntity.Field = businessEntity.Field;
            dataEntity.FieldId = businessEntity.FieldId;
            dataEntity.BusCompFieldId = businessEntity.BusCompFieldId;
            dataEntity.PickListFieldId = businessEntity.PickListFieldId;
            dataEntity.Constrain = businessEntity.Constrain;
            return dataEntity;
        }
    }
}
