using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.AppletModels;
using GSCrmLibrary.Factories.DataBUSFactories;
using GSCrmLibrary.Factories.BUSUIFactories;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Services.Info
{
    public class AppletInfoUI : IAppletInfoUI
    {
        private readonly IAppletInfo appletInfo;
        public AppletInfoUI(IAppletInfo appletInfo)
        {
            this.appletInfo = appletInfo;
        }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("BusCompName")]
        public string BusCompName { get; set; }
        [JsonPropertyName("Type")]
        public string Type { get; set; }
        [JsonPropertyName("Header")]
        public string Header { get; set; }
        [JsonPropertyName("DisplayLines")]
        public int DisplayLines { get; set; }
        [JsonPropertyName("EmptyState")]
        public string EmptyState { get; set; }
        [JsonPropertyName("Controls")]
        public List<UIControl> Controls { get; set; }
        [JsonPropertyName("ControlUPs")]
        public Dictionary<string, List<UIControlUP>> ControlUPs { get; set; }
        [JsonPropertyName("Columns")]
        public List<UIColumn> Columns { get; set; }
        [JsonPropertyName("PR")]
        public string PR { get; set; }
        [JsonPropertyName("Routing")]
        public string Routing { get; set; }
        [JsonPropertyName("BusCompRouting")]
        public string BusCompRouting { get; set; }
        [JsonPropertyName("Initflag")]
        public bool Initflag { get; set; }
        public void Initialize<TContext>(TContext context)
            where TContext : MainContext, new()
        {
            Name = appletInfo.Name;
            BusCompName = appletInfo.BusCompName;
            Type = appletInfo.Type;
            Header = appletInfo.Header;
            DisplayLines = appletInfo.DisplayLines;
            EmptyState = appletInfo.EmptyState;
            Controls = new List<UIControl>();
            ControlUPs = new Dictionary<string, List<UIControlUP>>();
            Columns = new List<UIColumn>();
            DataBUSControlFR<TContext> dataBUSControlFactory = new DataBUSControlFR<TContext>();
            BUSUIControlFR<TContext> busUIControlFactory = new BUSUIControlFR<TContext>();
            DataBUSControlUPFR<TContext> dataBUSControlUPFactory = new DataBUSControlUPFR<TContext>();
            BUSUIControlUPFR<TContext> busUIControlUPFactory = new BUSUIControlUPFR<TContext>();
            DataBUSColumnFR<TContext> dataBUSColumnFactory = new DataBUSColumnFR<TContext>();
            BUSUIColumnFR<TContext> busUIColumnFactory = new BUSUIColumnFR<TContext>();

            appletInfo.Controls
                .OrderBy(s => s.Sequence)
                .ToList()
                .ForEach(control =>
                {
                    UIControl UIEntity = busUIControlFactory.BusinessToUI(dataBUSControlFactory.DataToBusiness(control, context));
                    if (control.Type == "picklist" && control.Field != null)
                    {
                        PickList pl = context.PickLists
                            .Include(b => b.BusComp)
                            .FirstOrDefault(n => n.Id == control.Field.PickListId);
                        UIEntity.Routing = pl?.BusComp?.Routing;
                    }
                    if (control.ActionType == ActionType.CopyRecord)
                    {
                        ControlUP controlUP = control.ControlUPs.FirstOrDefault(n => n.Name == "Data Map");
                        if (controlUP != null)
                        {
                            DataMap dataMap = context.DataMaps.FirstOrDefault(n => n.Name == controlUP.Value);
                            if (dataMap != null) UIEntity.Routing = dataMap.Routing;
                        }
                    }
                    Controls.Add(UIEntity);
                    ControlUPs.Add(control.Name, new List<UIControlUP>());
                    control.ControlUPs.ForEach(controlUP =>
                    {
                        ControlUPs.GetValueOrDefault(control.Name)
                            .Add(busUIControlUPFactory.BusinessToUI(dataBUSControlUPFactory.DataToBusiness(controlUP, context)));
                    });
                });

            appletInfo.Columns
                .OrderBy(s => s.Sequence)
                .ToList()
                .ForEach(column => {
                    UIColumn UIEntity = busUIColumnFactory.BusinessToUI(dataBUSColumnFactory.DataToBusiness(column, context));
                    Columns.Add(UIEntity);
                });

            PR = appletInfo.PR;
            Routing = appletInfo.Routing;
            BusCompRouting = appletInfo.BusCompRouting;
            Initflag = appletInfo.Initflag;
        }
        public object Serialize() => new JsonResult(this).Value;
    }
}
