﻿using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Data;
using GSCrmLibrary.Services.Info;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUITableFR<TContext> : BUSUIFactory<BUSTable, UITable, TContext>
        where TContext : MainContext, new()
    {
        public override IEnumerable<ValidationResult> UIValidate(TContext context, IViewInfo viewInfo, UITable UIEntity, bool isNewRecord)
        {
            List<ValidationResult> result = base.UIValidate(context, viewInfo, UIEntity, isNewRecord).ToList();
            Table table = context.Tables?.AsNoTracking().FirstOrDefault(n => n.Name == UIEntity.Name);
            if (table != null && table.Id != UIEntity.Id)
                result.Add(new ValidationResult("Table with this name is already exists.", new List<string>() { "Name" }));
            return result;
        }
    }
}