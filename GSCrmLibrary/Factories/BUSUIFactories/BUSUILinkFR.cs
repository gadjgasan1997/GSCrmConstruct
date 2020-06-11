using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;
using System;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUILinkFR<TContext> : BUSUIFactory<BUSLink, UILink, TContext>
        where TContext : MainContext, new()
    {
        public override BUSLink UIToBusiness(UILink UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSLink businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);

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
        public override BUSLink Init(TContext context)
        {
            BUSLink businessEntity = base.Init(context);
            businessEntity.ParentBCId = Guid.Empty;
            businessEntity.ChildBCId = Guid.Empty;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UILink UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            Link link = context.Links.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (link != null && link.Id != UIEntity.Id)
                result.Add(new ValidationResult("Link with this name is already exists.", new List<string>() { "Name" }));
            if (string.IsNullOrWhiteSpace(UIEntity.ParentBCName))
                result.Add(new ValidationResult(
                    "Parent business component name is a required field.",
                    new List<string>() { "ParentBCName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.ParentFieldName))
                result.Add(new ValidationResult(
                    "Parent field name is a required field.",
                    new List<string>() { "ParentFieldName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.ChildBCName))
                result.Add(new ValidationResult(
                    "Child business component name is a required field.",
                    new List<string>() { "ChildBCName" }));
            if (string.IsNullOrWhiteSpace(UIEntity.ChildFieldName))
                result.Add(new ValidationResult(
                    "Child field name is a required field.",
                    new List<string>() { "ChildFieldName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSLink businessComponent, UILink UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.ParentBCName == null)
                    result.Add(new ValidationResult(
                        "Business component with this name not found.",
                        new List<string>() { "ParentBCName" }));
                else if (businessComponent.ParentFieldName == null)
                    result.Add(new ValidationResult(
                        "Field with this name not found.",
                        new List<string>() { "ParentFieldName" }));
                if (businessComponent.ChildBCName == null)
                    result.Add(new ValidationResult(
                        "Business component with this name not found.",
                        new List<string>() { "ChildBCName" }));
                else if (businessComponent.ChildFieldName == null)
                    result.Add(new ValidationResult(
                        "Field with this name not found.",
                        new List<string>() { "ChildFieldName" }));
            }
            return result;
        }
    }
}
