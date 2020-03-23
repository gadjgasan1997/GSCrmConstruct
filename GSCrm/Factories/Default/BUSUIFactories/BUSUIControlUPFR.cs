using GSCrm.Data;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Services.Info;
using Microsoft.EntityFrameworkCore;
using GSCrm.Factories.Default.MainFactories;
using System.Collections.Generic;
using System.Linq;
using GSCrm.Data.Context;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIControlUPFR : MainUPBUSUIFR<BUSControlUP, UIControlUP>
    {
        public override BUSControlUP UIToBusiness(UIControlUP UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            Control control = context.Controls
                .Include(cntrUp => cntrUp.ControlUPs)
                .FirstOrDefault(i => i.Id.ToString() == ComponentContext.GetSelectedRecord("Control"));
            BUSControlUP businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && control != null && control.ControlUPs != null && 
                control.ControlUPs.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Control user property with this name is already exists in control " + control.Name + ".";
            }
            else
            {
                businessEntity.Control = control;
                businessEntity.ControlId = control.Id;
            }
            return businessEntity;
        }
    }
}
