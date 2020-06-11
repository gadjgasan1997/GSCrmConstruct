using System.Linq;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.BusinessComponentModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Factories.BUSUIFactories
{
    public class BUSUIColumnUPFR<TContext> : BUSUIUserPropertyFactory<BUSColumnUP, UIColumnUP, TContext>
        where TContext : MainContext, new()
    {
        public override BUSColumnUP UIToBusiness(UIColumnUP UIEntity, TContext context, IViewInfo viewInfo, bool isNewRecord)
        {
            BUSColumnUP businessEntity = base.UIToBusiness(UIEntity, context, viewInfo, isNewRecord);
            Column column = context.Columns
                .AsNoTracking()
                .Select(a => new 
                {
                    id = a.Id,
                    name = a.Name,
                    columnUPs = a.ColumnUPs.Select(columnUP => new 
                    {
                        id = columnUP.Id,
                        name = columnUP.Name
                    })
                })
                .Select(a => new Column
                {
                    Id = a.id,
                    Name = a.name,
                    ColumnUPs = a.columnUPs.Select(columnUP => new ColumnUP
                    {
                        Id = columnUP.id,
                        Name = columnUP.name
                    }).ToList()
                })
                .FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord("Column"));
            if (column == null)
                businessEntity.ErrorMessage = "First you need create column.";
            else
            {
                // Если запись новая и она не уникальна, записывается ошибка
                ColumnUP columnUP = column.ColumnUPs.FirstOrDefault(n => n.Name == UIEntity.Name);
                if (columnUP != null && columnUP.Id != UIEntity.Id)
                    businessEntity.ErrorMessage = $"Column user property with this name is already exists in column {column.Name}.";
                else
                {
                    businessEntity.Column = column;
                    businessEntity.ColumnId = column.Id;
                }
            }
            return businessEntity;
        }
    }
}
