using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using GSCrmLibrary.Data;
using GSCrmLibrary.Factories.MainFactories;
using GSCrmLibrary.Models.MainEntities;
using GSCrmLibrary.Models.RequestModels;
using GSCrmLibrary.Models.TableModels;
using Microsoft.EntityFrameworkCore;

namespace GSCrmLibrary
{
    public static class EntitiesUtils<TTable, TBusinessComponent, TDataBUSFactory, TContext>
        where TTable : class, IDataEntity, new()
        where TBusinessComponent : IBUSEntity, new()
        where TContext : MainContext, new()
        where TDataBUSFactory : IDataBUSFactory<TTable, TBusinessComponent, TContext>, new()
    {
        public static List<TTable> FilterEntities(TContext context, FilterEntitiesModel model, IEnumerable<TTable> dataEntities)
        {
            if (model.Link != null)
            {
                TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
                BusinessObjectComponent objectComponent = model.BOComponents.FirstOrDefault(bcId => bcId.BusCompId == model.BusComp.Id);

                // Родительская БК и филды
                BusinessComponent parentBusComp = context.BusinessComponents
                    .AsNoTracking()
                    .Select(bc => new
                    {
                        id = bc.Id,
                        name = bc.Name,
                        table = bc.Table,
                        fields = bc.Fields.Select(field => new { id = field.Id, name = field.Name })
                    })
                    .Select(bc => new BusinessComponent
                    {
                        Id = bc.id,
                        Name = bc.name,
                        Table = bc.table,
                        Fields = bc.fields.Select(field => new Field { Id = field.id, Name = field.name }).ToList()
                    })
                    .FirstOrDefault(i => i.Id == model.Link.ParentBCId);
                Field parentField = parentBusComp.Fields.FirstOrDefault(i => i.Id == model.Link.ParentFieldId);
                Field childField = model.BusComp.Fields.FirstOrDefault(i => i.Id == model.Link.ChildFieldId);

                // Родительская запись
                var recordId = ComponentsRecordsInfo.GetSelectedRecord(parentBusComp.Name);
                IEnumerable<dynamic> parentRecords = (IEnumerable<dynamic>)(context.GetType().GetProperty(parentBusComp.Table.Name).GetValue(context));
                dynamic parentRecord = parentRecords.FirstOrDefault(i => i.Id.ToString() == recordId);

                // Значение родитеского поля из родительской БКО по которому будет фильтроваться дочерняя(текущая БКО)
                string parentFieldValue = parentRecord == null ? string.Empty : parentRecord.GetType().GetProperty(parentField.Name).GetValue(parentRecord).ToString();

                /* Установка search spec-а по дочерней(текущей) БКО
                   Если на родительской бк есть записи, надо фильтровать дочернюю бк */
                if (parentFieldValue != string.Empty)
                {
                    string searchSpecificationByParent = $"{childField.Name} = \"{parentFieldValue}\"";
                    ComponentsRecordsInfo.SetSearchSpecification(objectComponent.Name, SearchSpecTypes.SearchSpecificationByParent, searchSpecificationByParent);
                    dataEntities = dataEntities.AsQueryable().Where(searchSpecificationByParent).ToList();
                }

                // Если записей нет, значит надо очистить дочернюю бк
                else dataEntities = new List<TTable>();

                return dataEntities.Take(10).ToList();
            }
            else
            {
                var selectedRecordId = ComponentsRecordsInfo.GetSelectedRecord(model.BusComp.Name);
                if (selectedRecordId != null)
                    return dataEntities.SkipWhile(i => i.Id.ToString() != selectedRecordId).Take(10).ToList();
                else return dataEntities.Take(10).ToList();
            }
        }
    }
}
