using GSCrmApplication.Data;
using GSCrmApplication.Models.MainEntities;
using GSCrmLibrary.CodeGeneration;
using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Services.Info;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GSCrmApplication.Factories.MainFactories
{
    public class BUSUIFactory<TBusinessComponent, TApplet, TContext>
        : IBUSUIFactory<TBusinessComponent, TApplet, TContext>
        where TBusinessComponent : BUSEntity, new()
        where TApplet : UIEntity, new()
        where TContext : GSAppContext, new()
    {
        // Преобразует бизнес уровень в UI уровень
        public virtual TApplet BusinessToUI(TBusinessComponent businessEntity)
        {
            return new TApplet()
            {
                Id = businessEntity.Id,
                CreatedBy = businessEntity.CreatedBy,
                UpdatedBy = businessEntity.UpdatedBy
            };
        }
        // Преобразует UI уровень в бизнес уровень
        public virtual TBusinessComponent UIToBusiness(TApplet UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            TBusinessComponent businessEntity = new TBusinessComponent();
            if (ComponentsContext<TBusinessComponent>.TryGetComponentContext(viewInfo.CurrentApplet.BusComp?.Name, out businessEntity) | isNewRecord)
                return businessEntity;
            else return new TBusinessComponent()
            {
                Id = UIEntity.Id,
                CreatedBy = UIEntity.CreatedBy,
                UpdatedBy = UIEntity.UpdatedBy
            };
        }
        // Инициализация
        public virtual TBusinessComponent Init(TContext context)
            => new TBusinessComponent() { };
        // Валидация UI
        public virtual IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, TApplet UIEntity, bool isNewRecord)
            => new List<ValidationResult>() { };
        // Валидация бизнес уровня
        public virtual IEnumerable<ValidationResult> BUSUIValidate(TContext context, TBusinessComponent businessComponent, TApplet UIEntity)
            => new List<ValidationResult>() { };
    }
}
