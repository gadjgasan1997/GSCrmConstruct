using GSCrmLibrary.Data;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Services.Info;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GSCrmLibrary.Factories.MainFactories
{
    public interface IDataBUSFactory<TTable, TBusinessComponent, TContext>
        where TTable : class, IDataEntity, new()
        where TBusinessComponent : IBUSEntity, new()
        where TContext : MainContext
    {
        // Преобразует уровень данных в бизнесс уровень
        TBusinessComponent DataToBusiness(TTable dataEntity, TContext context);
        // Преобразует бизнес уровень в уровень данных
        TTable BusinessToData(TTable dataEntity, TBusinessComponent businessEntity, TContext context, bool NewRecord);
        // Возникает при создании записи
        void OnRecordCreate(TTable recordToCreate, DbSet<TTable> entities, TContext context);
        // Возникает при удалении записи
        void OnRecordDelete(TTable recordToDelete, DbSet<TTable> entities, TContext context);
        // Возникает при редактировании записи
        void OnRecordUpdate(TTable oldRecord, TTable changedRecord, DbSet<TTable> entities, TContext context);
        // Валидация бизнес уровня
        IEnumerable<ValidationResult> DataBUSValidate(TTable dataEntity, TBusinessComponent businessEntity, IViewInfo viewInfo, TContext context);
    }
}
