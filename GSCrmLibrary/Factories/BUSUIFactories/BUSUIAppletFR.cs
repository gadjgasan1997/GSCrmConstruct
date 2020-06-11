using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIAppletFR<TContext> : BUSUIFactory<BUSApplet, UIApplet, TContext>
        where TContext : MainContext, new()
    {
        public override UIApplet BusinessToUI(BUSApplet businessEntity)
        {
            UIApplet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.InitFlag = businessEntity.Initflag;
            UIEntity.BusCompName = businessEntity.BusCompName;
            UIEntity.DisplayLines = businessEntity.DisplayLines.ToString();
            UIEntity.EmptyState = businessEntity.EmptyState;
            UIEntity.PhysicalRenderName = businessEntity.PhysicalRenderName;
            UIEntity.Header = businessEntity.Header;
            UIEntity.Type = businessEntity.Type;
            UIEntity.Virtual = businessEntity.Virtual;
            return UIEntity;
        }
        public override BUSApplet UIToBusiness(UIApplet UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSApplet businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);

            // BusComp
            BusinessComponent busComp = context.BusinessComponents.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.BusCompName);
            if (busComp != null)
            {
                businessEntity.BusComp = busComp;
                businessEntity.BusCompId = busComp.Id;
                businessEntity.BusCompName = busComp.Name;
            }

            // PhysicalRender
            PhysicalRender PR = context.PhysicalRenders.FirstOrDefault(n => n.Name == UIEntity.PhysicalRenderName);
            if (PR != null)
            {
                businessEntity.PhysicalRender = PR;
                businessEntity.PhysicalRenderId = PR.Id;
                businessEntity.PhysicalRenderName = PR.Name;
            }

            businessEntity.Virtual = UIEntity.Virtual;
            businessEntity.Header = UIEntity.Header;
            businessEntity.Type = UIEntity.Type;
            businessEntity.Initflag = UIEntity.InitFlag;
            businessEntity.DisplayLines = Convert.ToInt32(UIEntity.DisplayLines);
            businessEntity.EmptyState = UIEntity.EmptyState;
            return businessEntity;
        }
        public override BUSApplet Init(TContext context)
        {
            BUSApplet businessEntity = base.Init(context);
            businessEntity.DisplayLines = 1;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIApplet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            Applet applet = context.Applets.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (applet != null && applet.Id != UIEntity.Id)
                result.Add(new ValidationResult("Applet with this name is already exists.", new List<string>() { "Name" }));
            return result;
        }
        public override IEnumerable<ValidationResult> BUSUIValidate(TContext context, BUSApplet businessComponent, UIApplet UIEntity)
        {
            List<ValidationResult> result = base.BUSUIValidate(context, businessComponent, UIEntity).ToList();
            if (string.IsNullOrWhiteSpace(businessComponent.ErrorMessage))
            {
                if (!string.IsNullOrWhiteSpace(UIEntity.BusCompName) && businessComponent.BusComp == null)
                    result.Add(new ValidationResult(
                        "Business component with this name not found.",
                        new List<string>() { "BusCompName" }));

                if (!string.IsNullOrWhiteSpace(UIEntity.PhysicalRenderName) && businessComponent.PhysicalRender == null)
                    result.Add(new ValidationResult(
                        "Physical render with this name not found.",
                        new List<string>() { "PhysicalRenderName" }));
            }
            return result;
        }
    }
}
