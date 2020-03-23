using GSCrm.Data;
using GSCrm.Models.Default.TableModels;
using GSCrm.Models.Default.AppletModels;
using GSCrm.Models.Default.BusinessComponentModels;
using GSCrm.Services.Info;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using GSCrm.Factories.Default.MainFactories;
using System;

namespace GSCrm.Factories.Default.BUSUIFactories
{
    public class BUSUIBusCompFR : MainBUSUIFR<BUSBusComp, UIBusComp>
    {
        public override UIBusComp BusinessToUI(BUSBusComp businessEntity)
        {
            UIBusComp UIEntity = base.BusinessToUI(businessEntity);
            UIEntity.TableName = businessEntity.TableName;
            return UIEntity;
        }
        public override BUSBusComp UIToBusiness(UIBusComp UIEntity, ApplicationContext context, IViewInfo viewInfo, bool NewRecord)
        {
            BUSBusComp businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, NewRecord);

            // Если запись новая и она не уникальна, записывается ошибка
            if (NewRecord && context.BusinessComponents.FirstOrDefault(n => n.Name == UIEntity.Name) != null)
            {
                businessEntity.ErrorCode = "Unique";
                businessEntity.ErrorMessage = "Record with this name is already exists.";
            }
            else
            {
                businessEntity.TableName = UIEntity.TableName;
                Table table = context.Tables.FirstOrDefault(n => n.Name == UIEntity.TableName);
                
                if (table != null)
                {
                    businessEntity.Table = table;
                    businessEntity.TableId = table.Id;
                }

                // Если у бк удалили таблицу или ее не было, для всех полей убрать id колонки таблицы, на которой они основаны
                else
                {
                    try
                    {
                        BusComp busComp = context.BusinessComponents
                            .Include(f => f.Fields)
                            .FirstOrDefault(i => i.Id == UIEntity.Id);

                        if (busComp != null && busComp.Fields != null)
                        {
                            busComp.Fields
                                .Where(tc => tc.TableColumnId != null)
                                .ToList()
                                .ForEach(field => field.TableColumnId = null);
                        }
                    }
                    catch(Exception ex)
                    {
                        var error = ex.Message;
                    }
                }
            }
            return businessEntity;
        }
        public override BUSBusComp Init()
        {
            BUSBusComp businessEntity = base.Init();
            businessEntity.TableName = "";
            return businessEntity;
        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext, BUSBusComp businessComponent, UIBusComp UIEntity)
        {
            List<ValidationResult> result = base.Validate(validationContext, businessComponent, UIEntity).ToList();
            if (businessComponent.ErrorMessage == string.Empty)
            {
                if (businessComponent.Table == null && !string.IsNullOrWhiteSpace(UIEntity.TableName))
                    result.Add(new ValidationResult(
                        "Table with this name not found.",
                        new List<string>() { "TableName" }
                        ));
            }
            return result;
        }
    }
}
