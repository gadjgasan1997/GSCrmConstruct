using System.Collections.Generic;

namespace GSCrm.Services.Info
{
    public class EntitiesInfo : IEntitiesInfo
    {
        private Dictionary<string, string> viewRouting
        {
            get => new Dictionary<string, string>()
            {
                { "Applet View", "/api/Applet/" },
                { "Applet Control View", "/api/Applet/" },
                { "Control UP View", "/api/Applet/" },
                { "Applet Column View", "/api/Applet/" },
                { "Business Component View", "/api/BusinessComponent/" },
                { "Business Component Field View", "/api/BusinessComponent/" },
                { "Business Component Join View", "/api/BusinessComponent/" },
                { "Join Specification View", "/api/BusinessComponent/" },
                { "Business Object View", "/api/BusinessObject/" },
                { "Business Object Component View", "/api/BusinessObject/" },
                { "Link View", "/api/Link/" },
                { "Screen View", "/api/Screen/" },
                { "Screen Item View", "/api/Screen/" },
                { "Views View", "/api/Views/" },
                { "View Items View", "/api/Views/" },
                { "PickList View", "/api/PickList/" },
                { "Physical Render View", "/api/PhysicalRender/" },
                { "Icon View", "/api/Icon/" },
                { "Action View", "/api/Action/" },
                { "Action UP View", "/api/Action/" },
                { "Table View", "/api/Table/" },
                { "Table Column View", "/api/Table/" }
            };
        }
        private Dictionary<string, string> appletRoting
        {
            get => new Dictionary<string, string>()
            {
                { "Applet Tile Applet", "/api/Applet/" },
                { "Create Applet Popup Applet", "/api/Applet/" },
                { "Update Applet Popup Applet", "/api/Applet/" },
                { "Applet Item Tile Applet", "/api/AppletItem/" },
                { "Create Applet Item Popup Applet", "/api/AppletItem/" },
                { "Update Applet Item Popup Applet", "/api/AppletItem/" },
                { "Column Tile Applet", "/api/Column/" },
                { "Create Column Popup Applet", "/api/Column/" },
                { "Update Column Popup Applet", "/api/Column/" },
                { "Control Tile Applet", "/api/Control/" },
                { "Create Control Popup Applet", "/api/Control/" },
                { "Update Control Popup Applet", "/api/Control/" },
                { "Control UP Tile Applet", "/api/ControlUP/" },
                { "Create Control UP Popup Applet", "/api/ControlUP/" },
                { "Update Control UP Popup Applet", "/api/ControlUP/" },
                { "Business Object Tile Applet", "/api/BusObject/" },
                { "Create Business Object Popup Applet", "/api/BusObject/" },
                { "Update Business Object Popup Applet", "/api/BusObject/" },
                { "Business Component Tile Applet", "/api/BusComp/" },
                { "Create Business Component Popup Applet", "/api/BusComp/" },
                { "Update Business Component Popup Applet", "/api/BusComp/" },
                { "Fields Tile Applet", "/api/Field/" },
                { "Create Field Popup Applet", "/api/Field/" },
                { "Update Field Popup Applet", "/api/Field/" },
                { "Join Tile Applet", "/api/Join/" },
                { "Create Join Popup Applet", "/api/Join/" },
                { "Update Join Popup Applet", "/api/Join/" },
                { "Join Specification Tile Applet", "/api/JoinSpecification/" },
                { "Create Join Specification Popup Applet", "/api/JoinSpecification/" },
                { "Update Join Specification Popup Applet", "/api/JoinSpecification/" },
                { "Business Object Component Tile Applet", "/api/BOComponent/" },
                { "Create Business Object Component Popup Applet", "/api/BOComponent/" },
                { "Update Business Object Component Popup Applet", "/api/BOComponent/" },
                { "Link Tile Applet", "/api/Link/" },
                { "Create Link Popup Applet", "/api/Link/" },
                { "Update Link Popup Applet", "/api/Link/" },
                { "PickList Tile Applet", "/api/PL/" },
                { "Create PickList Popup Applet", "/api/PL/" },
                { "Update PickList Popup Applet", "/api/PL/" },
                { "Screen Tile Applet", "/api/Screen/" },
                { "Create Screen Popup Applet", "/api/Screen/" },
                { "Update Screen Popup Applet", "/api/Screen/" },
                { "Screen Item Tile Applet", "/api/ScreenItem/" },
                { "Create Screen Item Popup Applet", "/api/ScreenItem/" },
                { "Update Screen Item Popup Applet", "/api/ScreenItem/" },
                { "View Tile Applet", "/api/View/" },
                { "Create View Popup Applet", "/api/View/" },
                { "Update View Popup Applet", "/api/View/" },
                { "View Item Tile Applet", "/api/ViewItem/" },
                { "Create View Item Popup Applet", "/api/ViewItem/" },
                { "Update View Item Popup Applet", "/api/ViewItem/" },
                { "Physical Render Tile Applet", "/api/PR/" },
                { "Create Physical Render Popup Applet", "/api/PR/" },
                { "Update Physical Render Popup Applet", "/api/PR/" },
                { "Icon Tile Applet", "/api/Icon/" },
                { "Create Icon Popup Applet", "/api/Icon/" },
                { "Update Icon Popup Applet", "/api/Icon/" },
                { "Action Tile Applet", "/api/Action/" },
                { "Create Action Popup Applet", "/api/Action/" },
                { "Update Action Popup Applet", "/api/Action/" },
                { "Action UP Tile Applet", "/api/ActionUP/" },
                { "Create Action UP Popup Applet", "/api/ActionUP/" },
                { "Update Action UP Popup Applet", "/api/ActionUP/" },
                { "Table Tile Applet", "/api/Table/" },
                { "Create Table Popup Applet", "/api/Table/" },
                { "Update Table Popup Applet", "/api/Table/" },
                { "Table Column Tile Applet", "/api/TableColumn/" },
                { "Create Table Column Popup Applet", "/api/TableColumn/" },
                { "Update Table Column Popup Applet", "/api/TableColumn/" },
            };
        }
        public Dictionary<string, string> ViewRouting => viewRouting;

        public Dictionary<string, string> AppletRouting => appletRoting;
    }
}
