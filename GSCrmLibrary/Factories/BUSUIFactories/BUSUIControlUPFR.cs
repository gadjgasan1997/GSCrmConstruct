using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIControlUPFR<TContext> : BUSUIUserPropertyFactory<BUSControlUP, UIControlUP, TContext>
        where TContext : MainContext, new()
    {
        public override BUSControlUP UIToBusiness(UIControlUP UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSControlUP businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Control control = context.Controls
                .AsNoTracking()
                .Select(a => new
                {
                    id = a.Id,
                    name = a.Name,
                    controlUPs = a.ControlUPs.Select(controlUP => new
                    {
                        id = controlUP.Id,
                        name = controlUP.Name
                    })
                })
                .Select(a => new Control
                {
                    Id = a.id,
                    Name = a.name,
                    ControlUPs = a.controlUPs.Select(controlUP => new ControlUP
                    {
                        Id = controlUP.id,
                        Name = controlUP.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Control"));
            if (control == null)
                businessEntity.ErrorMessage = "First you need create control.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                ControlUP controlUP = control.ControlUPs?.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (controlUP != null && controlUP.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Control user property with this name is already exists in control {control.Name}.";
                else
                {
                    businessEntity.Control = control;
                    businessEntity.ControlId = control.Id;
                }
            }
            return businessEntity;
        }
    }
}
