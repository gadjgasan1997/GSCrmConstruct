using GSCrm.Data;
using GSCrm.Models.Default.AppletModels;
using System;
using System.Collections.Generic;
using System.Linq;
using GSCrm.Factories.Default.DataBUSFactories;
using GSCrm.Factories.Default.BUSUIFactories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace GSCrm.Services.Info
{
    public class AppletInfoUI : IAppletInfoUI
    {
        private readonly IAppletInfo appletInfo;
        private readonly IEntitiesInfo entitiesInfo;
        public AppletInfoUI(IAppletInfo appletInfo, IEntitiesInfo entitiesInfo)
        {
            this.appletInfo = appletInfo;
            this.entitiesInfo = entitiesInfo;
        }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
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
        [JsonPropertyName("Path")]
        public string Path { get; set; }
        private void IntializeAppletUIInfo(ApplicationContext context)
        {
            Name = appletInfo.Name;
            Type = appletInfo.Type;
            Header = appletInfo.Header;
            DisplayLines = appletInfo.DisplayLines;
            EmptyState = appletInfo.EmptyState;
            Controls = new List<UIControl>();
            ControlUPs = new Dictionary<string, List<UIControlUP>>();
            Columns = new List<UIColumn>();
            DataBUSControlFR dataBUSControlFactory = new DataBUSControlFR();
            BUSUIControlFR busUIControlFactory = new BUSUIControlFR();
            DataBUSControlUPFR dataBUSControlUPFactory = new DataBUSControlUPFR();
            BUSUIControlUPFR busUIControlUPFactory = new BUSUIControlUPFR();
            DataBUSColumnFR dataBUSColumnFactory = new DataBUSColumnFR();
            BUSUIColumnFR busUIColumnFactory = new BUSUIColumnFR();

            appletInfo.Controls
                .OrderBy(s => s.Sequence)
                .ToList()
                .ForEach(control =>
                {
                    Controls.Add(busUIControlFactory.BusinessToUI(dataBUSControlFactory.DataToBusiness(control, context)));
                    ControlUPs.Add(control.Name, new List<UIControlUP>());
                    control.ControlUPs.ForEach(controlUP =>
                    {
                        ControlUPs
                            .GetValueOrDefault(control.Name)
                            .Add(busUIControlUPFactory.BusinessToUI(dataBUSControlUPFactory.DataToBusiness(controlUP, context)));
                    });
                });

            appletInfo.Columns
                .OrderBy(s => s.Sequence)
                .ToList()
                .ForEach(column => Columns.Add(
                    busUIColumnFactory.BusinessToUI(dataBUSColumnFactory.DataToBusiness(column, context))
                ));

            PR = appletInfo.PR;
            Path = entitiesInfo.AppletRouting.GetValueOrDefault(context.BusinessComponents.FirstOrDefault(i => i.Id == appletInfo.Applet.BusCompId).Name);
        }
        public object Serialize(ApplicationContext context)
        {
            IntializeAppletUIInfo(context);
            return new JsonResult(this).Value;
        }
    }
}
