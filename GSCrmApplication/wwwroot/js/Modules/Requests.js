export default class Requests {
    //@M ������ �� ������������� ����������
    static InitializeApplication() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Application/InitializeApplication/"
        });
    }

    //@M ������ �� ��������� ���������� � ����������
    static GetApplicationInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Application/ApplicationInfo"
        });
    }

    //@M ������ �� ���������� ����������
    static UpdateApplicationInfo(data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Application/UpdateApplicationInfo/",
            data: JSON.stringify(data)
        });
    }

    //@M Drilldown
    static Drilldown() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            url: "/api/Application/Drilldown/"
        });
    }

    //@M ������ �� ������������� ������
    static InitializeScreen(data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Screen/InitializeScreen/",
            data: JSON.stringify(data)
        });
    }

    //@M ������ �� ��������� ���������� �� ������
    static GetScreenInfo(Name) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Screen/ScreenInfo/" + Name
        });
    }

    //@M ������ �� ���������� ���������� � ������
    static UpdateScreenInfo(data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Screen/UpdateScreenInfo/",
            data: JSON.stringify(data),
        });
    }

    //@M ������ �� ������������� �������������
    static InitializeView(name) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/View/InitializeView/" + name,
        });
    }

    //@M �������� ���������� � �������������
    static GetViewInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/View/ViewInfo",
        });
    }

    //@M ������ �� ���������� ���������� � �������������
    static UpdateViewInfo(data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            url: "/api/View/UpdateViewInfo",
            data: JSON.stringify(data),
        });
    }

    //@M ������ �� ������ ���������� ��������� � �������������
    static UpdateContext() {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: "/api/View/UpdateContext"
        });
    }

    //@M ������ �� ��������� ���������� ��������� � �������������
    static PartialUpdateContext(appletInfo, refreshCurrentApplet) {
        let data = {
            appletName: appletInfo["Name"],
            refreshCurrentApplet: refreshCurrentApplet
        }

        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: "/api/View/PartialUpdateContext",
            dataType: "json",
            data: JSON.stringify(data),
        });
    }

    //@M ������ �� ������������� �������
    static InitializeApplet(name) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Applet/InitializeApplet/" + name,
        });
    }

    //@M �������� ���������� �� �������
    static GetAppletInfo(applet) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Applet/AppletInfo"
        });
    }

    //@M ������ �� ��������� ������� ��� �������
    static GetRecords(appletInfo) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: appletInfo["Routing"] + "GetRecords/" + appletInfo["Name"]
        });
    }

    //@M ������ �� ��������� ����� ������
    static GetRecord(appletInfo) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            url: appletInfo["Routing"] + "GetRecord/" + appletInfo['Name'],
            dataType: "json",
        });
    }

    //@M ������ �� ���������� ������
    static NewRecord(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            url: applet['Info']["Routing"] + "NewRecord",
            data: JSON.stringify(data),
        });
    }

    //@M ������ �� ���������� ������
    static UpdateRecord(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: applet['Info']["Routing"] + "UpdateRecord",
            dataType: "json",
            data: JSON.stringify(data),
        });
    }

    //@M ������ �� ���������� ������
    static UndoUpdate(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: applet['Info']["Routing"] + "UndoUpdate",
            dataType: "json",
            data: JSON.stringify(data),
        });
    }

    //@M ������ �� �������� ������
    static DeleteRecord(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: applet['Info']["Routing"] + "DeleteRecord",
            dataType: "json",
            data: JSON.stringify(data),
        });
    }

    //@C ����������� ������
    static CopyRecord(control) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: control["Routing"] + "CopyRecord",
        });
    }

    //@M ������ �� apply �������
    static ApplyTable(applet) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: applet['Info']["BusCompRouting"] + "ApplyTable",
        });
    }

    //@M ������ �� ��������� ����
    static Publish(applet) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: applet['Info']["BusCompRouting"] + "Publish",
        });
    }

    //@M ������ �� ��������� ������� ��� ��������
    static GetPickListRecords(control) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            dataType: "json",
            url: control["Routing"] + "PickListRecords",
        });
    }
    
    //@M ������ �� ����� ������ �� ��������
    static SetPickListRecord(control, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: control["Routing"] + "Pick",
            data: JSON.stringify(data)
        });
    }

    //@C ������ �� ����������
    static ExecuteQuery(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: applet['Info']["BusCompRouting"] + "ExecuteQuery",
            data: JSON.stringify(data)
        });
    }

    //@C ������ �� ������ ����������
    static CancelQuery(applet) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: applet['Info']["BusCompRouting"] + "CancelQuery",
        });
    }



    //@!W ��� ������ �������� ��������, ����� ������ ��� ������������ ���� 
    //@M ������ �� ��������� ������������ ������� ����������
    static GetDisplayedRecords(componentName) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetDisplayedRecords/" + componentName
        });
    }

    //@M ������ �� ��������� ������� ���������� ������� �� ������ �����������
    static GetSelectedRecords() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetSelectedRecords/"
        });
    }

    //@M ������ �� ��������� ���������� � ������, ������� �������� �� ����
    static GetBackScreenInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetScreenInfo/"
        });
    }

    //@M ������ �� ��������� ���������� � �������������, ������� �������� �� ����
    static GetBackViewInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetViewInfo/"
        });
    }

    //@M ������ �� ��������� ���������� � �������������, ������� �������� �� ����
    static GetBackAppletInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetAppletInfo/"
        });
    }
}