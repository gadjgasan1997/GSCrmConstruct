using GSCrmApplication.Data;
using GSCrmApplication.Models.MainEntities;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Services.Info;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GSCrmApplication.Factories.MainFactories
{
    public class DataBUSFactory<TTable, TBusinessComponent, TContext>
        : IDataBUSFactory<TTable, TBusinessComponent, TContext>
        where TTable : DataEntity, new()
        where TBusinessComponent : BUSEntity, new()
        where TContext : GSAppContext, new()
    {
        // Преобразует уровень данных в бизнесс уровень
        public virtual TBusinessComponent DataToBusiness(TTable dataEntity, TContext context)
        {
            return new TBusinessComponent()
            {
                Id = dataEntity.Id,
                CreatedBy = dataEntity.CreatedBy,
                UpdatedBy = dataEntity.UpdatedBy
            };
        }
        // Преобразует бизнес уровень в уровень данных
        public virtual TTable BusinessToData(TTable dataEntity, TBusinessComponent businessEntity, TContext context, bool NewRecord)
        {
            // Если создается новая запись то заполняет для нее общие свойства
            if (NewRecord)
                return new TTable() { };
            // Иначе находит эту сущность в контексте и обновляет для нее общие свойства
            else return dataEntity;
        }
        // Возникает при создании записи
        public virtual void OnRecordCreate(TTable recordToCreate, DbSet<TTable> entities, TContext context)
        {
            entities.Add(recordToCreate);
            context.SaveChanges();
        }
        // Возникает при удалении записи
        public virtual void OnRecordDelete(TTable recordToDelete, DbSet<TTable> entities, TContext context)
        {
            entities.Remove(recordToDelete);
            context.SaveChanges();
        }
        // Возникает при редактировании записи
        public virtual void OnRecordUpdate(TTable oldRecord, TTable changedRecord, DbSet<TTable> entities, TContext context)
        {
            entities.Update(changedRecord);
            context.SaveChanges();
        }
        // Валидация бизнес уровня
        public virtual IEnumerable<ValidationResult> DataBUSValidate(TTable dataEntity, TBusinessComponent businessEntity, IViewInfo viewInfo, TContext context)
            => new List<ValidationResult>();
    }
}
