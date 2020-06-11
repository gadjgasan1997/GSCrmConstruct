using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.TableModels;

namespace GSCrmLibrary.DataMapping
{
    public class AppletMap<TContext> : IDataMapping<TContext, Applet, Applet>
        where TContext : MainContext
    {
        public IEnumerable<Applet> Map(IEnumerable<Applet> records, TContext context)
        {
            foreach (Applet record in records)
            {
                // Получение нового id и названия записи
                Guid newAppletId = Guid.NewGuid();
                string recordName = record.Name;
                Applet sameNameComponent = context.Applets
                    .AsNoTracking()
                    .Select(bc => new { id = bc.Id, name = bc.Name })
                    .Select(bc => new Applet { Id = bc.id, Name = bc.name })
                    .FirstOrDefault(n => n.Name == recordName);
                while (sameNameComponent != null)
                {
                    recordName = $"Copy_{recordName}";
                    sameNameComponent = context.Applets
                        .AsNoTracking()
                        .Select(bc => new { id = bc.Id, name = bc.Name })
                        .Select(bc => new Applet { Id = bc.id, Name = bc.name })
                        .FirstOrDefault(n => n.Name == recordName);
                }

                // Select из базы
                List<Control> controls = new List<Control>();
                List<Column> columns = new List<Column>();

                Applet applet = context.Applets
                    .Include(p => p.PhysicalRender)
                    .Include(bc => bc.BusComp)
                    .Include(c => c.Columns)
                        .ThenInclude(up => up.ColumnUPs)
                    .Include(c => c.Columns)
                        .ThenInclude(f => f.Field)
                    .Include(c => c.Controls)
                        .ThenInclude(up => up.ControlUPs)
                    .Include(c => c.Controls)
                        .ThenInclude(f => f.Field)
                    .FirstOrDefault(i => i.Id == record.Id);

                applet.Columns.ForEach(c =>
                {
                    Guid columnId = Guid.NewGuid();
                    Column column = new Column
                    {
                        Id = columnId,
                        AppletId = newAppletId,
                        Changed = c.Changed,
                        Created = DateTime.Now,
                        CreatedBy = c.CreatedBy,
                        Field = c.Field,
                        FieldId = c.FieldId,
                        Header = c.Header,
                        Icon = c.Icon,
                        IconId = c.IconId,
                        Inactive = c.Inactive,
                        LastUpdated = DateTime.Now,
                        Name = c.Name,
                        Readonly = c.Readonly,
                        Required = c.Required,
                        Sequence = c.Sequence,
                        Type = c.Type,
                        UpdatedBy = c.UpdatedBy
                    };
                    c.ColumnUPs.ForEach(up =>
                    {
                        column.ColumnUPs.Add(new ColumnUP
                        {
                            Id = Guid.NewGuid(),
                            Column = c,
                            ColumnId = columnId,
                            Changed = up.Changed,
                            Created = DateTime.Now,
                            CreatedBy = up.CreatedBy,
                            Inactive = up.Inactive,
                            LastUpdated = DateTime.Now,
                            Name = up.Name,
                            Sequence = up.Sequence,
                            UpdatedBy = up.UpdatedBy,
                            Value = up.Value
                        });
                    });
                    columns.Add(column);
                });

                applet.Controls.ForEach(c =>
                {
                    Guid controlId = Guid.NewGuid();
                    Control control = new Control
                    {
                        Id = controlId,
                        AppletId = newAppletId,
                        Changed = c.Changed,
                        Created = DateTime.Now,
                        CreatedBy = c.CreatedBy,
                        Field = c.Field,
                        FieldId = c.FieldId,
                        Header = c.Header,
                        Icon = c.Icon,
                        IconId = c.IconId,
                        Inactive = c.Inactive,
                        LastUpdated = DateTime.Now,
                        Name = c.Name,
                        Readonly = c.Readonly,
                        Required = c.Required,
                        Sequence = c.Sequence,
                        Type = c.Type,
                        UpdatedBy = c.UpdatedBy,
                        CssClass = c.CssClass
                    };
                    c.ControlUPs.ForEach(up =>
                    {
                        control.ControlUPs.Add(new ControlUP
                        {
                            Id = Guid.NewGuid(),
                            Control = c,
                            ControlId = controlId,
                            Changed = up.Changed,
                            Created = DateTime.Now,
                            CreatedBy = up.CreatedBy,
                            Inactive = up.Inactive,
                            LastUpdated = DateTime.Now,
                            Name = up.Name,
                            Sequence = up.Sequence,
                            UpdatedBy = up.UpdatedBy,
                            Value = up.Value
                        });
                    });
                    controls.Add(control);
                });

                // Возврат нового апплета
                yield return new Applet
                {
                    Id = newAppletId,
                    BusComp = applet.BusComp,
                    BusCompId = applet.BusCompId,
                    Changed = applet.Changed,
                    Columns = columns,
                    Controls = controls,
                    Created = DateTime.Now,
                    CreatedBy = applet.CreatedBy,
                    DisplayLines = applet.DisplayLines,
                    EmptyState = applet.EmptyState,
                    Header = applet.Header,
                    Inactive = applet.Inactive,
                    Initflag = applet.Initflag,
                    LastUpdated = DateTime.Now,
                    Name = recordName,
                    PhysicalRender = applet.PhysicalRender,
                    PhysicalRenderId = applet.PhysicalRenderId,
                    Routing = $"/api/{Utils.GetPermissibleName(recordName)}/",
                    Sequence = applet.Sequence,
                    Type = applet.Type,
                    UpdatedBy = applet.UpdatedBy
                };
            }
        }
    }
}
