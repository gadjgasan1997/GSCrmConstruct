using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIBOComponentFR : MainBUSUIFR<BUSBOComponent, UIBOComponent>
    {
        public override UIBOComponent BusinessToUI(BUSBOComponent businessEntity)
        {
            UIBOComponent UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.BusCompName = businessEntity.BusCompName;
            UIEntity.LinkName = businessEntity.LinkName;
            return UIEntity;
        }
        public override BUSBOComponent UIToBusiness(UIBOComponent UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BusObject busObject = context.BusinessObjects
                .Include(boc => boc.BusObjectComponents)
                .FirstOrDefault(n => n.Id.ToString() == ComponentContext.GetSelectedRecord("Business Object"));
            BUSBOComponent businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && busObject != null && busObject.BusObjectComponents != null &&
                busObject.BusObjectComponents.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Business object component with this name is already exists in business object " + busObject.Name + ".";
            }
            else
            {
                businessEntity.BusCompName = UIEntity.BusCompName;
                businessEntity.LinkName = UIEntity.LinkName;
                BusComp busComp = context.BusinessComponents.FirstOrDefault(n => n.Name == UIEntity.BusCompName);
                if (busComp != null)
                {
                    businessEntity.BusComp = busComp;
                    businessEntity.BusCompId = busComp.Id;
                }
                if (busObject != null)
                {
                    businessEntity.BusObject = busObject;
                    businessEntity.BusObjectId = busObject.Id;
                    businessEntity.BusObjectName = busObject.Name;
                }
                Link link = context.Links.FirstOrDefault(n => n.Name == UIEntity.LinkName);
                if (link != null)
                {
                    businessEntity.Link = link;
                    businessEntity.LinkId = link.Id;
                }
            }
            return businessEntity;
        }
        public override BUSBOComponent Init()
        {
            BUSBOComponent businessEntity = base.Init();
            businessEntity.BusCompName = "";
            businessEntity.LinkName = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSBOComponent businessComponent, UIBOComponent UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.ErrorMessage == string.Empty)
            {
                if (string.IsNullOrWhiteSpace(UIEntity.BusCompName))
                    result.Add(new ValidationResult(
                        "Business component name is a required field",
                        new List<string>() { "BusCompName" }
                        ));
                if (businessComponent.BusComp == null && !string.IsNullOrWhiteSpace(UIEntity.BusCompName))
                    result.Add(new ValidationResult(
                        "Business component with this name not found",
                        new List<string>() { "BusCompName" }
                        ));
                if (businessComponent.Link == null && !string.IsNullOrWhiteSpace(UIEntity.LinkName))
                    result.Add(new ValidationResult(
                        "Link with this name not found",
                        new List<string>() { "LinkName" }
                        ));
            }
            return result;
        }
    }
}
