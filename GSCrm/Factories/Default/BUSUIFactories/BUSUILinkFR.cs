using GSCrm.Data;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using GSCrm.Factories.Default.MainFactories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrm.Models.Default.TableModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUILinkFR : MainBUSUIFR<BUSLink, UILink>
    {
        public override BUSLink UIToBusiness(UILink UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSLink businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);
            // Parent bc
            BusComp parentBusComp = context.BusinessComponents
                .Include(field => field.Fields)
                .FirstOrDefault(n => n.Name == UIEntity.ParentBCName);
            if (parentBusComp != null)
            {
                businessEntity.ParentBusComp = parentBusComp;
                businessEntity.ParentBCId = parentBusComp.Id;
                businessEntity.ParentBCName = parentBusComp.Name;
            }

            // Parent field
            Field parentField = parentBusComp.Fields.FirstOrDefault(n => n.Name == UIEntity.ParentFieldName);
            if (parentField != null)
            {
                businessEntity.ParentField = parentField;
                businessEntity.ParentFieldId = parentField.Id;
                businessEntity.ParentFieldName = parentField.Name;
            }

            // Child bc
            BusComp childBusComp = context.BusinessComponents
                .Include(field => field.Fields)
                .FirstOrDefault(n => n.Name == UIEntity.ChildBCName);
            if (childBusComp != null)
            {
                businessEntity.ChildBusComp = childBusComp;
                businessEntity.ChildBCId = childBusComp.Id;
                businessEntity.ChildBCName = childBusComp.Name;
            }

            // Child field
            Field childField = childBusComp.Fields.FirstOrDefault(n => n.Name == UIEntity.ChildFieldName);
            if (childField != null)
            {
                businessEntity.ChildField = childField;
                businessEntity.ChildFieldId = childField.Id;
                businessEntity.ChildFieldName = childField.Name;
            }

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && context.Links.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Link with this name is already exists.";
            }
            return businessEntity;
        }
        public override UILink BusinessToUI(BUSLink businessEntity)
        {
            UILink UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.ChildBCName = businessEntity.ChildBCName;
            UIEntity.ChildFieldName = businessEntity.ChildFieldName;
            UIEntity.ParentBCName = businessEntity.ParentBCName;
            UIEntity.ParentFieldName = businessEntity.ParentFieldName;
            return UIEntity;
        }
        public override BUSLink Init()
        {
            BUSLink businessEntity = base.Init();
            businessEntity.ChildBCName = "";
            businessEntity.ChildFieldName = "";
            businessEntity.ParentBCName = "";
            businessEntity.ParentFieldName = "";
            return businessEntity;
        }
    }
}
