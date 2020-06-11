using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.DataMapping
{
    public class BusinessComponentMap<TContext> : IDataMapping<TContext, BusinessComponent, BusinessComponent>
        where TContext : MainContext
    {
        public IEnumerable<BusinessComponent> Map(IEnumerable<BusinessComponent> records, TContext context)
        {
            foreach (BusinessComponent record in records)
            {
                // Получение нового id и названия записи
                Guid newBCId = Guid.NewGuid();
                string recordName = record.Name;
                BusinessComponent sameNameComponent = context.BusinessComponents
                    .AsNoTracking()
                    .Select(bc => new { id = bc.Id, name = bc.Name })
                    .Select(bc => new BusinessComponent { Id = bc.id, Name = bc.name })
                    .FirstOrDefault(n => n.Name == recordName);
                while (sameNameComponent != null)
                {
                    recordName = $"Copy_{recordName}";
                    sameNameComponent = context.BusinessComponents
                        .AsNoTracking()
                        .Select(bc => new { id = bc.Id, name = bc.Name })
                        .Select(bc => new BusinessComponent { Id = bc.id, Name = bc.name })
                        .FirstOrDefault(n => n.Name == recordName);
                }

                // Select из базы
                BusinessComponent businessComponent = context.BusinessComponents
                    .AsNoTracking()
                    .Include(f => f.Fields)
                        .ThenInclude(p => p.PickList)
                            .ThenInclude(d => d.BusComp)
                    .Include(f => f.Fields)
                        .ThenInclude(p => p.PickMaps)
                    .Include(f => f.Fields)
                        .ThenInclude(p => p.Join)
                    .Include(f => f.Fields)
                        .ThenInclude(p => p.JoinColumn)
                    .Include(f => f.Fields)
                        .ThenInclude(p => p.TableColumn)
                    .Include(f => f.Joins)
                        .ThenInclude(p => p.Table)
                    .Include(f => f.Joins)
                        .ThenInclude(p => p.JoinSpecifications)
                    .FirstOrDefault(i => i.Id == record.Id);

                // Маппинг
                List<JoinSpecification> joinSpecifications = new List<JoinSpecification>();
                List<Join> joins = new List<Join>();
                List<PickMap> pickMaps = new List<PickMap>();
                List<Field> fields = new List<Field>();

                // Поля
                businessComponent.Fields.ForEach(f =>
                {
                    Field field = new Field
                    {
                        Id = Guid.NewGuid(),
                        BusCompId = newBCId,
                        CalculatedValue = f.CalculatedValue,
                        Changed = f.Changed,
                        Created = DateTime.Now,
                        CreatedBy = f.CreatedBy,
                        Inactive = f.Inactive,
                        IsCalculate = f.IsCalculate,
                        Join = f.Join,
                        JoinId = f.JoinId,
                        JoinColumn = f.JoinColumn,
                        JoinColumnId = f.JoinColumnId,
                        LastUpdated = DateTime.Now,
                        Name = f.Name,
                        PickList = f.PickList,
                        PickListId = f.PickListId,
                        Readonly = f.Readonly,
                        Required = f.Required,
                        Sequence = f.Sequence,
                        TableColumn = f.TableColumn,
                        TableColumnId = f.TableColumnId,
                        Type = f.Type,
                        UpdatedBy = f.UpdatedBy
                    };
                    f.PickMaps.ForEach(pickMap =>
                    {
                        field.PickMaps.Add(new PickMap
                        {
                            Id = Guid.NewGuid(),
                            Field = field,
                            FieldId = field.Id,
                            Changed = pickMap.Changed,
                            Constrain = pickMap.Constrain,
                            Created = DateTime.Now,
                            CreatedBy = pickMap.CreatedBy,
                            Inactive = pickMap.Inactive,
                            LastUpdated = DateTime.Now,
                            Name = pickMap.Name,
                            Sequence = pickMap.Sequence,
                            UpdatedBy = pickMap.UpdatedBy,
                            BusCompFieldId = pickMap.BusCompFieldId,
                            PickListFieldId = pickMap.PickListFieldId
                        });
                    });
                    fields.Add(field);
                });

                // Замена id полей с целевой бизнес компоненты в пикмапах на новые
                fields.Where(p => p.PickMaps?.Count > 0).ToList().ForEach(field =>
                {
                    field.PickMaps.ForEach(pickMap =>
                    {
                        // Получение через название поля его нового id, так как id полей меняется при маппинге
                        Field busCompField = businessComponent.Fields.FirstOrDefault(i => i.Id == pickMap.BusCompFieldId);
                        Field newBusCompField = busCompField == null ? null : fields.FirstOrDefault(n => n.Name == busCompField.Name);
                        if (newBusCompField != null)
                            pickMap.BusCompFieldId = newBusCompField.Id;
                    });
                });

                // Joins
                businessComponent.Joins.ForEach(j =>
                {
                    Join join = new Join
                    {
                        Id = Guid.NewGuid(),
                        Name = j.Name,
                        BusCompId = newBCId,
                        Changed = j.Changed,
                        Created = DateTime.Now,
                        CreatedBy = j.CreatedBy,
                        Inactive = j.Inactive,
                        LastUpdated = DateTime.Now,
                        Sequence = j.Sequence,
                        Table = j.Table,
                        TableId = j.TableId,
                        UpdatedBy = j.UpdatedBy
                    };
                    j.JoinSpecifications.ForEach(joinSpecification =>
                    {
                        Field sourceField = businessComponent.Fields.FirstOrDefault(i => i.Id == joinSpecification.SourceFieldId);
                        Field newSourceField = sourceField == null ? null : fields.FirstOrDefault(n => n.Name == sourceField.Name);
                        join.JoinSpecifications.Add(new JoinSpecification
                        {
                            Id = Guid.NewGuid(),
                            Changed = joinSpecification.Changed,
                            Created = DateTime.Now,
                            CreatedBy = joinSpecification.CreatedBy,
                            DestinationColumn = joinSpecification.DestinationColumn,
                            DestinationColumnId = joinSpecification.DestinationColumnId,
                            Inactive = joinSpecification.Inactive,
                            Join = join,
                            JoinId = join.Id,
                            LastUpdated = DateTime.Now,
                            Name = joinSpecification.Name,
                            Sequence = joinSpecification.Sequence,
                            SourceField = newSourceField,
                            SourceFieldId = newSourceField == null ? Guid.Empty : newSourceField.Id,
                            UpdatedBy = joinSpecification.UpdatedBy
                        });
                    });
                    joins.Add(join);
                });

                // Замена id полей целевых бизнес компонент на спецификациях join-ов на новые
                fields.Where(j => j.Join != null).ToList().ForEach(f =>
                {
                    Join newJoin = joins.FirstOrDefault(n => n.Name == f.Join.Name);
                    f.Join = newJoin;
                    f.JoinId = newJoin.Id;
                });

                // Возврат новой компоненты
                yield return new BusinessComponent
                {
                    Id = newBCId,
                    Name = recordName,
                    Changed = businessComponent.Changed,
                    Created = DateTime.Now,
                    CreatedBy = businessComponent.CreatedBy,
                    Fields = fields,
                    Inactive = businessComponent.Inactive,
                    Joins = joins,
                    LastUpdated = DateTime.Now,
                    Routing = $"/api/{Utils.GetPermissibleName(recordName)}/",
                    Sequence = businessComponent.Sequence,
                    TableId = businessComponent.TableId,
                    UpdatedBy = businessComponent.UpdatedBy
                };
            }
        }
    }
}
