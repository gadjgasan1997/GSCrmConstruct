using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Services.Info;

namespace GSCrmLibrary.Factories.MainFactories
{
    public interface IBUSUIFactory<TBusinessComponent, TApplet, TContext>
        where TBusinessComponent : IBUSEntity, new()
        where TApplet : IUIEntity, new()
        where TContext : MainContext, new()
    {
        // Преобразует бизнес уровень в UI уровень
        TApplet BusinessToUI(TBusinessComponent businessEntity);
        // Преобразует UI уровень в бизнес уровень
        TBusinessComponent UIToBusiness(TApplet UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord);
        // Инициализация
        TBusinessComponent Init(TContext context);
        // Валидация UI
        IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, TApplet UIEntity, bool isNewRecord);
        // Валидация бизнес уровня
        IEnumerable<ValidationResult> BUSUIValidate(TContext context, TBusinessComponent businessComponent, TApplet UIEntity);
    }
}
