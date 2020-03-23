using GSCrm.Data;
using GSCrm.Services.Info;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GSCrm.Factories.Default.MainFactories
{
    public class MainBUSUIFR<MainBusinessComponent, MainApplet>
        where MainBusinessComponent : Models.Default.MainEntities.MainBusinessComponent, new()
        where MainApplet : Models.Default.MainEntities.MainApplet, new()
    {
        // Преобразует бизнес уровень в UI уровень
        public virtual MainApplet BusinessToUI(MainBusinessComponent businessEntity)
        {
            return new MainApplet()
            {
                Id = businessEntity.Id,
                Name = businessEntity.Name,
                Display = businessEntity.Display,
                Header = businessEntity.Header,
                Inactive = businessEntity.Inactive,
                Required = businessEntity.Required,
                Type = businessEntity.Type,
                Sequence = businessEntity.Sequence.ToString()
            };
        }
        // Преобразует UI уровень в бизнес уровень
        public virtual MainBusinessComponent UIToBusiness(MainApplet UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            if (NewRecord)
            {
                return new MainBusinessComponent()
                {
                    Name = UIEntity.Name,
                    Display = UIEntity.Display,
                    Header = UIEntity.Header,
                    Inactive = UIEntity.Inactive,
                    Required = UIEntity.Required,
                    Type = UIEntity.Type,
                    Sequence = Convert.ToInt32(UIEntity.Sequence)
                };
            }
            else return new MainBusinessComponent()
            {
                Id = UIEntity.Id,
                Name = UIEntity.Name,
                Display = UIEntity.Display,
                Header = UIEntity.Header,
                Inactive = UIEntity.Inactive,
                Required = UIEntity.Required,
                Type = UIEntity.Type,
                Sequence = Convert.ToInt32(UIEntity.Sequence)
            };
        }
        // Инициализация
        public virtual MainBusinessComponent Init()
            => new MainBusinessComponent();
        // Валидация
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext, MainBusinessComponent businessComponent, MainApplet UIEntity)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (string.IsNullOrWhiteSpace(UIEntity.Name))
                result.Add(new ValidationResult(
                    "Name is a required field.",
                    new List<string>() { "Name" }
                    ));
            if (businessComponent.ErrorMessage != string.Empty)
                result.Add(new ValidationResult(
                    businessComponent.ErrorMessage,
                    new List<string>() { "ErrorMessage" }
                    ));
            return result;
        }
    }
}
