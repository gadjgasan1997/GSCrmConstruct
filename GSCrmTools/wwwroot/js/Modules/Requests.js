export default class Requests {
    //@M Запрос на инициализацию приложения
    static InitializeApplication() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Application/InitializeApplication/"
        });
    }

    //@M Запрос на получение информации о приложении
    static GetApplicationInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Application/ApplicationInfo"
        });
    }

    //@M Запрос на обновление приложения
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

    //@M Запрос на инициализацию экрана
    static InitializeScreen(data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Screen/InitializeScreen/",
            data: JSON.stringify(data)
        });
    }

    //@M Запрос на получение информации об экране
    static GetScreenInfo(Name) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Screen/ScreenInfo/" + Name
        });
    }

    //@M Запрос на обновление информации о скрине
    static UpdateScreenInfo(data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Screen/UpdateScreenInfo/",
            data: JSON.stringify(data),
        });
    }

    //@M Запрос на инициализацию представления
    static InitializeView(name) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/View/InitializeView/" + name,
        });
    }

    //@M Получает информацию о представлении
    static GetViewInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/View/ViewInfo",
        });
    }

    //@M Запрос на обновление информации о представлении
    static UpdateViewInfo(data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            url: "/api/View/UpdateViewInfo",
            data: JSON.stringify(data),
        });
    }

    //@M Запрос на полное обновление контекста в представлении
    static UpdateContext() {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: "/api/View/UpdateContext"
        });
    }

    //@M Запрос на частичное обновление контекста в представлении
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

    //@M Запрос на инициализацию апплета
    static InitializeApplet(name) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Applet/InitializeApplet/" + name,
        });
    }

    //@M Получает информауию об апплете
    static GetAppletInfo(applet) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Applet/AppletInfo"
        });
    }

    //@M Запрос на получение записей для апплета
    static GetRecords(appletInfo) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: appletInfo["Routing"] + "GetRecords/" + appletInfo["Name"]
        });
    }

    //@M Запрос на получение одной записи
    static GetRecord(appletInfo) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            url: appletInfo["Routing"] + "GetRecord/" + appletInfo['Name'],
            dataType: "json",
        });
    }

    //@M Запрос на добавление записи
    static NewRecord(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            dataType: "json",
            url: applet['Info']["Routing"] + "NewRecord",
            data: JSON.stringify(data),
        });
    }

    //@M Запрос на обновление записи
    static UpdateRecord(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: applet['Info']["Routing"] + "UpdateRecord",
            dataType: "json",
            data: JSON.stringify(data),
        });
    }

    //@M Запрос на обновление записи
    static UndoUpdate(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: applet['Info']["Routing"] + "UndoUpdate",
            dataType: "json",
            data: JSON.stringify(data),
        });
    }

    //@M Запрос на удаление записи
    static DeleteRecord(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: applet['Info']["Routing"] + "DeleteRecord",
            dataType: "json",
            data: JSON.stringify(data),
        });
    }

    //@C Копирование записи
    static CopyRecord(control) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: control["Routing"] + "CopyRecord",
        });
    }

    //@M Запрос на apply таблицы
    static ApplyTable(applet) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: applet['Info']["BusCompRouting"] + "ApplyTable",
        });
    }

    //@M Запрос на генерацию кода
    static Publish(applet) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: applet['Info']["BusCompRouting"] + "Publish",
        });
    }

    //@M Запрос на получение записей для пиклиста
    static GetPickListRecords(control) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            dataType: "json",
            url: control["Routing"] + "PickListRecords",
        });
    }
    
    //@M Запрос на выбор записи из пиклиста
    static SetPickListRecord(control, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: control["Routing"] + "Pick",
            data: JSON.stringify(data)
        });
    }

    //@C Запрос на фильтрацию
    static ExecuteQuery(applet, data) {
        return $.ajax({
            type: "Post",
            contentType: "application/json",
            url: applet['Info']["BusCompRouting"] + "ExecuteQuery",
            data: JSON.stringify(data)
        });
    }

    //@C Запрос на отмену фильтрации
    static CancelQuery(applet) {
        return $.ajax({
            type: "Get",
            contentType: "application/json",
            url: applet['Info']["BusCompRouting"] + "CancelQuery",
        });
    }



    //@!W Эти методы подлежат удалению, нужны только для тестирования бека 
    //@M Запрос на получение отоброжаемых записей компоненты
    static GetDisplayedRecords(componentName) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetDisplayedRecords/" + componentName
        });
    }

    //@M Запрос на получение текущих выделенных записей на бизнес компонентах
    static GetSelectedRecords() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetSelectedRecords/"
        });
    }

    //@M Запрос на получение информации о скрине, которая хранится на беке
    static GetBackScreenInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetScreenInfo/"
        });
    }

    //@M Запрос на получение информации о представлении, которая хранится на беке
    static GetBackViewInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetViewInfo/"
        });
    }

    //@M Запрос на получение информации о представлении, которая хранится на беке
    static GetBackAppletInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetAppletInfo/"
        });
    }
}