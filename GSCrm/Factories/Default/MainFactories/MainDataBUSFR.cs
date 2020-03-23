using GSCrm.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GSCrm.Factories.Default.MainFactories
{
    public class MainDataBUSFR<MainTable, MainBusinessComponent>
        where MainTable : Models.Default.MainEntities.MainTable, new()
        where MainBusinessComponent : Models.Default.MainEntities.MainBusinessComponent, new()
    {
        // Преобразует уровень данных в бизнесс уровень
        public virtual MainBusinessComponent DataToBusiness(MainTable dataEntity, ApplicationContext context)
        {
            return new MainBusinessComponent()
            {
                Id = dataEntity.Id,
                Name = dataEntity.Name,
                Display = dataEntity.Display,
                Inactive = dataEntity.Inactive,
                Required = dataEntity.Required,
                Type = dataEntity.Type,
                Sequence = Convert.ToInt32(dataEntity.Sequence)
            };
        }
        // Преобразует бизнес уровень в уровень данных
        public virtual MainTable BusinessToData(MainBusinessComponent businessEntity, DbSet<MainTable> entityDBSet, bool NewRecord)
        {
            // Если создается новая запись то заполняет для нее общие свойства
            if (NewRecord)
            {
                return new MainTable()
                {
                    Name = businessEntity.Name,
                    Display = businessEntity.Display,
                    Inactive = businessEntity.Inactive,
                    Required = businessEntity.Required,
                    Type = businessEntity.Type,
                    Sequence = businessEntity.Sequence
                };
            }
            // Иначе находит эту сущность в контексте и обновляет для нее общие свойства
            else
            {
                MainTable dataEntity = entityDBSet.FirstOrDefault(i => i.Id == businessEntity.Id);
                if (dataEntity != null)
                {
                    dataEntity.Name = businessEntity.Name;
                    dataEntity.Display = businessEntity.Display;
                    dataEntity.Inactive = businessEntity.Inactive;
                    dataEntity.Required = businessEntity.Required;
                    dataEntity.Type = businessEntity.Type;
                    dataEntity.Sequence = businessEntity.Sequence;
                }
                return dataEntity;
            }
        }
        // Возникает при создании записи
        public virtual void OnRecordCreate(MainTable recordToCreate, DbSet<MainTable> entityDBSet, IWebHostEnvironment environment, ApplicationContext context)
        {
            entityDBSet.Add(recordToCreate);
            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                var error = ex.Message;
            }
        }
        // Возникает при удалении записи
        public virtual void OnRecordDelete(MainTable recordToDelete, DbSet<MainTable> entityDBSet, ApplicationContext context)
        {
            entityDBSet.Remove(recordToDelete);
            context.SaveChanges();
        }
        // Возникает при редактировании записи
        public virtual void OnRecordUpdate(MainTable recordToUpdate, DbSet<MainTable> entityDBSet, ApplicationContext context)
        {
            entityDBSet.Update(recordToUpdate);
            context.SaveChanges();
        }
    }
}
