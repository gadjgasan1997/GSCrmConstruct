using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Factories.Default.MainFactories;
using GSCrm.Models.Default.TableModels;
using GSCrm.Data;
using GSCrm.Services.Info;
using System.Linq;
using System.Collections.Generic;
using System;
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIAppletFR : MainBUSUIFR<BUSApplet, UIApplet>
    {
        public override BUSApplet UIToBusiness(UIApplet UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            Applet applet = context.Applets.FirstOrDefault(i => i.Id.ToString() == ComponentContext.GetSelectedRecord("Action"));
            BUSApplet businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && applet != null && context.Applets.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Applet with this name is already exists.";
            }

            else
            {
                // BusComp
                BusComp busComp = context.BusinessComponents.FirstOrDefault(n => n.Name == UIEntity.BusCompName);
                if (busComp != null)
                {
                    businessEntity.BusComp = busComp;
                    businessEntity.BusCompId = busComp.Id;
                    businessEntity.BusCompName = busComp.Name;
                }

                // PhysicalRender
                PR PR = context.PhysicalRenders.FirstOrDefault(n => n.Name == UIEntity.PhysicalRenderName);
                if (PR != null)
                {
                    businessEntity.PhysicalRender = PR;
                    businessEntity.PhysicalRenderId = PR.Id;
                    businessEntity.PhysicalRenderName = PR.Name;
                }

                businessEntity.DisplayLines = Convert.ToInt32(UIEntity.DisplayLines);
                businessEntity.EmptyState = UIEntity.EmptyState;
                businessEntity.Header = UIEntity.Header;
            }
            return businessEntity;
        }
        public override BUSApplet Init()
        {
            BUSApplet businessEntity = base.Init();
            businessEntity.Header = "";
            businessEntity.BusCompName = "";
            businessEntity.DisplayLines = 1;
            businessEntity.EmptyState = "";
            businessEntity.PhysicalRenderName = "";
            return businessEntity;
        }
        public override UIApplet BusinessToUI(BUSApplet businessEntity)
        {
            UIApplet UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.Header = businessEntity.Header;
            UIEntity.BusCompName = businessEntity.BusCompName;
            UIEntity.DisplayLines = businessEntity.DisplayLines.ToString();
            UIEntity.EmptyState = businessEntity.EmptyState;
            UIEntity.PhysicalRenderName = businessEntity.PhysicalRenderName;
            return UIEntity;
        }
    }
}
