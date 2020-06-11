using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GSCrmLibrary.Factories.DataBUSFactories
{
    public class DataBUSLinkFR<TContext> : DataBUSFactory<Link, BUSLink, TContext>
        where TContext : MainContext, new()
    {
        public override BUSLink DataToBusiness(Link dataEntity, TContext context)
        {
            BUSLink businessEntity = base.DataToBusiness(dataEntity, context);
            // Parent bc
            BusinessComponent parentBusComp = context.BusinessComponents
                .AsNoTracking()
                .Select(bc => new
                {
                    id = bc.Id,
                    name = bc.Name,
                    fields = bc.Fields.Select(field => new
                    {
                        id = field.Id,
                        name = field.Name,
                    })
                })
                .Select(bc => new BusinessComponent
                {
                    Id = bc.id,
                    Name = bc.name,
                    Fields = bc.fields.Select(field => new Field
                    {
                        Id = field.id,
                        Name = field.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == dataEntity.ParentBCId);
            if (parentBusComp != null)
            {
                businessEntity.ParentBusComp = parentBusComp;
                businessEntity.ParentBCId = parentBusComp.Id;
                businessEntity.ParentBCName = parentBusComp.Name;

                // Parent field
                Field parentField = parentBusComp.Fields.FirstOrDefault(i => i.Id == dataEntity.ParentFieldId);
                if (parentField != null)
                {
                    businessEntity.ParentField = parentField;
                    businessEntity.ParentFieldId = parentField.Id;
                    businessEntity.ParentFieldName = parentField.Name;
                }
            }


            // Child bc
            BusinessComponent childBusComp = context.BusinessComponents
                .AsNoTracking()
                .Select(bc => new
                {
                    id = bc.Id,
                    name = bc.Name,
                    fields = bc.Fields.Select(field => new
                    {
                        id = field.Id,
                        name = field.Name,
                    })
                })
                .Select(bc => new BusinessComponent
                {
                    Id = bc.id,
                    Name = bc.name,
                    Fields = bc.fields.Select(field => new Field
                    {
                        Id = field.id,
                        Name = field.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id == dataEntity.ChildBCId);
            if (childBusComp != null)
            {
                businessEntity.ChildBusComp = childBusComp;
                businessEntity.ChildBCId = childBusComp.Id;
                businessEntity.ChildBCName = childBusComp.Name;

                // Child field
                Field childField = childBusComp.Fields.FirstOrDefault(i => i.Id == dataEntity.ChildFieldId);
                if (childField != null)
                {
                    businessEntity.ChildField = childField;
                    businessEntity.ChildFieldId = childField.Id;
                    businessEntity.ChildFieldName = childField.Name;
                }
            }

            return businessEntity;
        }
        public override Link BusinessToData(Link link, BUSLink businessEntity, TContext context, bool NewRecord)
        {
            Link dataEntity = base.BusinessToData(link, businessEntity, context, NewRecord);
            dataEntity.ParentBCId = businessEntity.ParentBCId;
            dataEntity.ParentFieldId = businessEntity.ParentFieldId;
            dataEntity.ChildBCId = businessEntity.ChildBCId;
            dataEntity.ChildFieldId = businessEntity.ChildFieldId;
            return dataEntity;
        }
    }
}
