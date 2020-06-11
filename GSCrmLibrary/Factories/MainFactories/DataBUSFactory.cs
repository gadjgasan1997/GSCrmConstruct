using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Services.Info;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.MainFactories
{
    public class DataBUSFactory<TTable, TBusinessComponent, TContext> 
        : IDataBUSFactory<TTable, TBusinessComponent, TContext>
        where TTable : DataEntity, new()
        where TBusinessComponent : BUSEntity, new()
        where TContext : MainContext, new()
    {
        // Преобразует уровень данных в бизнесс уровень
        public virtual TBusinessComponent DataToBusiness(TTable dataEntity, TContext context)
        {
            return new TBusinessComponent()
            {
                Id = dataEntity.Id,
                Name = dataEntity.Name,
                Inactive = dataEntity.Inactive,
                Changed = dataEntity.Changed,
                Sequence = Convert.ToInt32(dataEntity.Sequence)
            };
        }
        // Преобразует бизнес уровень в уровень данных
        public virtual TTable BusinessToData(TTable dataEntity, TBusinessComponent businessEntity, TContext context, bool NewRecord)
        {
            // Если создается новая запись то заполняет для нее общие свойства
            if (NewRecord)
            {
                return new TTable()
                {
                    Name = businessEntity.Name,
                    Inactive = businessEntity.Inactive,
                    Changed = businessEntity.Changed,
                    Sequence = businessEntity.Sequence
                };
            }
            // Иначе находит эту сущность в контексте и обновляет для нее общие свойства
            else if (dataEntity != null)
            {
                dataEntity.Name = businessEntity.Name;
                dataEntity.LastUpdated = DateTime.Now;
                dataEntity.Inactive = businessEntity.Inactive;
                dataEntity.Changed = businessEntity.Changed;
                dataEntity.Sequence = businessEntity.Sequence;
            }
            return dataEntity;
        }
        // Возникает при создании записи
        public virtual void OnRecordCreate(TTable recordToCreate, DbSet<TTable> entities, TContext context)
        {
            recordToCreate.Name?.TrimStart()?.TrimEnd();
            entities.AsNoTracking().ToList().Add(recordToCreate);
            context.Entry(recordToCreate).State = EntityState.Added;
            context.SaveChanges();
        }
        // Возникает при удалении записи
        public virtual void OnRecordDelete(TTable recordToDelete, DbSet<TTable> entities, TContext context)
        {
            entities.AsNoTracking().ToList().Remove(recordToDelete);
            context.Entry(recordToDelete).State = EntityState.Deleted;
            context.SaveChanges();
        }
        // Возникает при редактировании записи
        public virtual void OnRecordUpdate(TTable oldRecord, TTable changedRecord, DbSet<TTable> entities, TContext context)
        {
            changedRecord.Name?.TrimStart()?.TrimEnd();
            context.Entry(changedRecord).State = EntityState.Modified;
            context.SaveChanges();
        }
        // Валидация бизнес уровня
        public virtual IEnumerable<ValidationResult> DataBUSValidate(TTable dataEntity, TBusinessComponent businessEntity, IViewInfo viewInfo, TContext context)
            => new List<ValidationResult>();
    }
}
