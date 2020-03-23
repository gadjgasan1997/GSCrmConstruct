//@C Запросы на бек
class RequestsToDB {
    //@M Запрос на получение одной записи
    GetRecord(applet) {
        return $.ajax({
            method: "GET",
            contentType: "application/json;charset=utf-8",
            url: applet.Path + "GetRecord",
            dataType: "json",
        });
    }

    //@M Запрос на получение записей для апплета
    GetRecords(applet) {
        var data = {
            AppletName: applet.Name,
        };
        return $.ajax({
            method: "Post",
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            url: applet.Path + "GetRecords",
            dataType: "json",
        });
    }

    //@M Запрос на полное обновление контекста в представлении
    UpdateContext(view) {
        var inputsObj = {
            ViewName: view.Name
        }
    
        return $.ajax({
            method: "Post",
            data: JSON.stringify(inputsObj),
            contentType: "application/json;charset=utf-8",
            url: view.Path + "UpdateContext",
            dataType: "json",
        });
    }

    //@M Запрос на частичное обновление контекста в представлении
    PartialUpdateContext(applet, recordId, refreshCurrentApplet) {
        let view = new View(applet.View);
        var inputsObj = {};
    
        inputsObj['viewName'] = view.Name;
        inputsObj['appletName'] = applet.Name;
        inputsObj['id'] = recordId;
        inputsObj['refreshCurrentApplet'] = refreshCurrentApplet;
    
        return $.ajax({
            method: "Post",
            data: JSON.stringify(inputsObj),
            contentType: "application/json;charset=utf-8",
            url: view.Path + "PartialUpdateContext",
            dataType: "json",
        });
    }
    
    //@M Запрос на добавление записи
    NewRecord(applet, data) {
        return $.ajax({
            method: "Post",
            contentType: "application/json",
            url: applet.Path + "NewRecord",
            data: JSON.stringify(data)
        });
    }

    //@M Запрос на обновление записи
    UpdateRecord(applet, data) {
        return $.ajax({
            method: "Post",
            contentType: "application/json",
            url: applet.Path + "UpdateRecord",
            data: JSON.stringify(data)
        }); 
    }

    //@M Запрос на удаление записи
    DeleteRecord(applet, recordId) {
        var data = {
            id: recordId,
            appletName: applet.Name
        };
    
        return $.ajax({
            method: "Post",
            contentType: "application/json",
            url: applet.Path + "DeleteRecord",
            data: JSON.stringify(data)
        });
    }

    //@M Запрос на инициализацию представления
    InitializeView(view) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: view.Path + "InitializeView/" + view.Name,
        });
    }

    //@M Получает информацию о представлении
    GetViewInfo(view) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: view.Path + "ViewInfo",
        });
    }

    //@M Запрос на обновление информации о представлении
    UpdateViewInfo(view, data) {
        return $.ajax({
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            dataType: "json",
            url: view.Path + "UpdateViewInfo",
        });
    }

    //@M Получает информауию об апплете
    GetAppletInfo(applet) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: applet.Path + "AppletInfo/" + applet.Name
        });
    }

    //@M Запрос на получение записей для пиклиста
    GetPickListRecords(applet, recordName) {
        var data = {
            recordName: recordName,
            appletName: applet.Name
        }
    
        return $.ajax({
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            dataType: "json",
            url: applet.Path + "PickListRecords",
        });
    }

    //@M Запрос на инициализацию экрана
    InitializeScreen(screen) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: screen.Path + "InitializeScreen/" + screen.Name
        });
    }
    
    //@M Запрос на получение информации об экране
    GetScreenInfo(screen) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: screen.Path + "ScreenInfo"
        });
    }

    //@M Запрос на обновление информации о скрине
    UpdateScreenInfo(screen, data) {
        return $.ajax({
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            dataType: "json",
            url: screen.Path + "UpdateScreenInfo",
        }); 
    }

    //!W Эти методы подлежат удалению, нужны только для тестирования бека 
    //@M Запрос на получение отоброжаемых записей компоненты
    GetDisplayedRecords(componentName) {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetDisplayedRecords/" + componentName
        });
    }

    //@M Запрос на получение текущих выделенных записей на бизнес компонентах
    GetSelectedRecords() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetSelectedRecords/"
        });
    }

    //@M Запрос на получение информации о скрине, которая хранится на беке
    GetBackScreenInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetScreenInfo/"
        });
    }

    //@M Запрос на получение информации о представлении, которая хранится на беке
    GetBackViewInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetViewInfo/"
        });
    }

    //@M Запрос на получение информации о представлении, которая хранится на беке
    GetBackAppletInfo() {
        return $.ajax({
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            url: "/api/Test/GetAppletInfo/"
        });
    }
}