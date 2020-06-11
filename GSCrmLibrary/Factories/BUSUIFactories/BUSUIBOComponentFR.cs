using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIBOComponentFR<TContext> : BUSUIFactory<BUSBusinessObjectComponent, UIBusinessObjectComponent, TContext>
        where TContext : MainContext, new()
    {
        public override UIBusinessObjectComponent BusinessToUI(BUSBusinessObjectComponent businessEntity)
        {
            UIBusinessObjectComponent UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.BusCompName = businessEntity.BusCompName;
            UIEntity.LinkName = businessEntity.LinkName;
            return UIEntity;
        }
        public override BUSBusinessObjectComponent UIToBusiness(UIBusinessObjectComponent UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSBusinessObjectComponent businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            BusinessObject busObject = context.BusinessObjects
                .AsNoTracking()
                .Select(bo => new
                {
                    id = bo.Id,
                    name = bo.Name,
                    components = bo.BusObjectComponents.Select(boc => new 
                    {
                        id = boc.Id,
                        name = boc.Name
                    })
                })
                .Select(bo => new BusinessObject
                {
                    Id = bo.id,
                    Name = bo.name,
                    BusObjectComponents = bo.components.Select(boc => new BusinessObjectComponent
                    {
                        Id = boc.id,
                        Name = boc.name
                    }).ToList()
                })
                .FirstOrDefault(n => n.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Business Object"));
            if (busObject == null)
                businessEntity.ErrorMessage = "First you need create business object.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                BusinessObjectComponent businessObjectComponent = busObject.BusObjectComponents?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (businessObjectComponent != null && businessObjectComponent.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Business object component with this name is already exists in business object {busObject.Name}.";
                else
                {
                    // BusObject
                    businessEntity.BusObject = busObject;
                    businessEntity.BusObjectId = busObject.Id;
                    businessEntity.BusObjectName = busObject.Name;

                    // BusComp
                    BusinessComponent busComp = context.BusinessComponents.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.BusCompName);
                    if (busComp != null)
                    {
                        businessEntity.BusComp = busComp;
                        businessEntity.BusCompId = busComp.Id;
                        businessEntity.BusCompName = busComp.Name;
                    }

                    // Link
                    Link link = context.Links.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.LinkName);
                    if (link != null)
                    {
                        businessEntity.Link = link;
                        businessEntity.LinkId = link.Id;
                        businessEntity.LinkName = link.Name;
                    }
                }
            }

            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIBusinessObjectComponent UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            if (string.IsNullOrWhiteSpace(UIEntity.BusCompName))
                result.Add(new ValidationResult(
                    "Business component name is a required field",
                    new List<string>() { "BusCompName" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSBusinessObjectComponent businessComponent, UIBusinessObjectComponent UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (businessComponent.BusComp == null)
                    result.Add(new ValidationResult(
                        "Business component with this name not found",
                        new List<string>() { "BusCompName" }));
                if (businessComponent.Link == null && !string.IsNullOrWhiteSpace(UIEntity.LinkName))
                    result.Add(new ValidationResult(
                        "Link with this name not found",
                        new List<string>() { "LinkName" }));
            }
            return result;
        }
    }
}
