using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using GSCrmLibrary.CodeGeneration;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Services.Info;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace GSCrmLibrary.Factories.MainFactories
{
    public class BUSUIFactory<TBusinessComponent, TApplet, TContext> 
        : IBUSUIFactory<TBusinessComponent, TApplet, TContext>
        where TBusinessComponent : BUSEntity, new()
        where TApplet : UIEntity, new()
        where TContext : MainContext, new()
    {
        // Преобразует бизнес уровень в UI уровень
        public virtual TApplet BusinessToUI(TBusinessComponent businessEntity)
        {
            return new TApplet()
            {
                Id = businessEntity.Id,
                Name = businessEntity.Name,
                Inactive = businessEntity.Inactive,
                Changed = businessEntity.Changed,
                Sequence = businessEntity.Sequence.ToString()
            };
        }
        // Преобразует UI уровень в бизнес уровень
        public virtual TBusinessComponent UIToBusiness(TApplet UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            TBusinessComponent businessEntity = new TBusinessComponent();
            if (ComponentsContext<TBusinessComponent>.TryGetComponentContext(viewInfo.CurrentApplet.BusComp?.Name, out businessEntity) | isNewRecord)
            {
                businessEntity.Name = UIEntity.Name;
                businessEntity.Inactive = UIEntity.Inactive;
                businessEntity.Sequence = Convert.ToInt32(UIEntity.Sequence);
                businessEntity.Changed = UIEntity.Changed;
                return businessEntity;
            }
            else return new TBusinessComponent()
            {
                Id = UIEntity.Id,
                Name = UIEntity.Name,
                Inactive = UIEntity.Inactive,
                Changed = UIEntity.Changed,
                Sequence = Convert.ToInt32(UIEntity.Sequence)
            };
        }
        // Инициализация
        public virtual TBusinessComponent Init(TContext context)
            => new TBusinessComponent();
        // Валидация UI
        public virtual IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, TApplet UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (!IsPermissibleName(UIEntity.Name, out string error))
                result.Add(new ValidationResult(error, new List<string>() { "Name" }));
            return result;
        }
        // Валидация бизнес уровня
        public virtual IEnumerable<ValidationResult> BUSUIValidate(TContext context, TBusinessComponent businessComponent, TApplet UIEntity)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (businessComponent.ErrorMessage != string.Empty)
                result.Add(new ValidationResult(
                    businessComponent.ErrorMessage,
                    new List<string>() { "ErrorMessage" }));
            return result;
        }
        private bool IsPermissibleName(string name, out string error)
        {
            if (typeof(TApplet).Name != "UIDirectoriesList")
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    error = "Name is a required field.";
                    return false;
                }
                if (Regex.IsMatch(name, @"[^a-zA-Z0-9_/\s]"))
                {
                    error = "Name may contain only latin letters, numbers or next special \"_ /\" symbols.";
                    return false;
                }
                if (!Regex.IsMatch(name, @"[a-zA-Z]"))
                {
                    error = "Name cannot consist entirely of numbers.";
                    return false;
                }
                if (char.IsDigit(name[0]))
                {
                    error = "Name cannot begin with a digit.";
                    return false;
                }
            }
            error = string.Empty;
            return true;
        }
    }
}
