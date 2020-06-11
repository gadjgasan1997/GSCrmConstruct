using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIDirectoriesListFR<TContext> : BUSUIFactory<BUSDirectoriesList, UIDirectoriesList, TContext>
        where TContext : MainContext, new()
    {
        public override UIDirectoriesList BusinessToUI(BUSDirectoriesList businessEntity)
        {
            UIDirectoriesList UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.DirectoryType = businessEntity.DirectoryType;
            UIEntity.DisplayValue = businessEntity.DisplayValue;
            UIEntity.Language = businessEntity.Language;
            UIEntity.LIC = businessEntity.LIC;
            return UIEntity;
        }
        public override BUSDirectoriesList UIToBusiness(UIDirectoriesList UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSDirectoriesList businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            businessEntity.DirectoryType = UIEntity.DirectoryType;
            businessEntity.DisplayValue = UIEntity.DisplayValue;
            businessEntity.LIC = UIEntity.LIC;
            businessEntity.Language = UIEntity.Language;
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UIDirectoriesList UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            DirectoriesList directoryList = context.DirectoriesList.AsNoTracking().FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Directories List"));
            DirectoriesList directoriesList = context.DirectoriesList.AsNoTracking()
                .FirstOrDefault(n => n.DirectoryType == UIEntity.DirectoryType && n.LIC == UIEntity.LIC && n.Language == UIEntity.Language);
            if (directoriesList != null && directoriesList.Id != UIEntity.Id)
            {
                result.Add(new ValidationResult(
                    $"Directory list with this language independent code and language is already exists in directory type {UIEntity.DirectoryType}.",
                    new List<string>() { "DirectoryType" }));
            }
            if (string.IsNullOrWhiteSpace(UIEntity.DirectoryType))
                result.Add(new ValidationResult(
                    "Display type is a required field.",
                    new List<string>() { "DirectoryType" }));
            if (string.IsNullOrWhiteSpace(UIEntity.DisplayValue))
                result.Add(new ValidationResult(
                    "Display value is a required field.",
                    new List<string>() { "DisplayValue" }));
            if (string.IsNullOrWhiteSpace(UIEntity.LIC))
                result.Add(new ValidationResult(
                    "Language independent code is a required field.",
                    new List<string>() { "LIC" }));
            if (string.IsNullOrWhiteSpace(UIEntity.Language))
                result.Add(new ValidationResult(
                    "Language is a required field.",
                    new List<string>() { "DisplayValue" }));
            return result;
        }
    }
}
