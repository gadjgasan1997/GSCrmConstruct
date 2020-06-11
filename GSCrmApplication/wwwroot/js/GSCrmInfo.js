//@C GSCrmUI info
class GSCrmInfo {
    //#region //@R Свойства
    static #Application = {};
    static #ApplicationInfo = {};
    static #ScreenInfo = {};
    static #ViewInfo = new Map();
    static #AppletInfo = new Map();
    static #CurrentElementsInfo = {
        "Screen": null,
        "View": null,
        "TargetApplet": null,
        "CurrentApplet": null,
        "PopupApplet": null,
        "CurrentRecord": null,
        "CurrentControl": null,
        "CurrentPopupControl": null,
        "AppletToUpdate": null,
        "RecordToUpdate" : null
    };
    static #SelectedRecords = {};

    static get Application() {
        return this.#Application;
    }

    static set Application(application) {
        this.#Application = application;
    }

    //@M Получение информации о приложении
    static get ApplicationInfo() {
        return this.#ApplicationInfo;
    }

    //@M Получение информаци об экране
    static get ScreenInfo() {
        return this.#ScreenInfo;
    }

    //@M Установка информации об экране
    static set ScreenInfo(screenInfo) {
        this.#ScreenInfo = screenInfo;
    }

    //@M Установка информации о приложении
    static set ApplicationInfo(applicationInfo) {
        this.#ApplicationInfo = applicationInfo;
    }

    //@M Получение информации обо всех представлениях
    static get ViewsInfo() {
        return this.#ViewInfo;
    }

    //@M Получение инфомрации обо всех апплетах
    static get AppletsInfo() {
        return this.#AppletInfo;
    }

    //@M Получение информации о текущих элементах
    static get CurrentElementsInfo() {
        return this.#CurrentElementsInfo;
    }
    //#endregion
    
    //#region //@R методы для работы с информацией о сущностях
    //@M Получение информации о скрине
    static GetScreenInfo(screenName) {
        return this.#ScreenInfo.get(screenName);
    }
    
    //@M Получение информации о представлении
    static GetViewInfo(viewName) {
        return this.#ViewInfo.get(viewName);
    }
    
    /**
     * @M Установка информации о представлении
    * @param viewName Название представления
    * @param viewInfo Информация о представлении
    */
    static SetViewInfo(viewName, viewInfo) {
        this.#ViewInfo.set(viewName, viewInfo);
    }

    /**
    * @M Удаляет информацию о представлении из мапы
    * @param {String} viewName Название представления
    */
    static RemoveViewInfo(viewName) {
        this.#ViewInfo.delete(viewName);
    }

    //@M Удаляет лишнюю информацию о представлениях
    static RemoveOldViewInfo() {
        this.#ViewInfo.forEach((info, viewName) => {
            if (viewName != this.#ScreenInfo['CurrentView']) {
                this.RemoveViewInfo(viewName);
            }
        });
    }

    //@M Получение информации об апплете
    static GetAppletInfo(appletName) {
        return this.#AppletInfo.get(appletName);
    }

    /**
    * @M Установка информации об апплете
    * @param appletName Название апплета
    * @param appletInfo Информация об апплете
    */
    static SetAppletInfo(appletName, appletInfo) {
        this.#AppletInfo.set(appletName, appletInfo);
    }

    /**
    * @M Удаляет информацию об апплете из мапы
    * @param {String} appletName Название апплета
    */
    static RemoveAppletInfo(appletName) {
        this.#AppletInfo.delete(appletName);
    }

    //@M Удаляет лишнюю информацию об апплетах
    static RemoveOldAppletInfo() {
        // Представление и апплеты, которые есть в нем на текущий момент
        let viewName = this.#ScreenInfo['CurrentView']['ViewName'];
        let viewItems = this.GetViewInfo(viewName)['ViewItems'];
        let applets = viewItems.map(item => item['AppletName']);

        // Апплеты, которые есть в информации о представлении(включая апплеты, подлежащие удалению)
        this.#AppletInfo.forEach((info, appletName) => {
            if (applets.indexOf(appletName) == -1) {
                this.RemoveAppletInfo(appletName);
            }
        })
    }

    //@M Получение информации о текущей выцбранной записи
    static GetSelectedRecord(appletName) {
        return this.#SelectedRecords[appletName];
    }

    //@M Установка инфомрации о текущей выцбранной записи
    static SetSelectedRecord(appletName, selectedRecords) {
        this.#SelectedRecords[appletName] = selectedRecords;
    }

    /**
    *@M Заполнение объекта с текущими выделенными элементами при нажатии на апплете
    * @param {Event} event Событие
    */
    static SetUpCurrentElements(event) {
        /**
        * Получение названия представления. Если оно не совпадает с названием представления из объекта с текущими элементами,
        * то в объекте представление заменяется новым.
        */
        let viewName = this.#ScreenInfo['CurrentView']['ViewName'];
        let view = this.#CurrentElementsInfo['View'];
        if (view == null || viewName != view.Name) {
            view = new this.#Application.View(viewName);
            this.#CurrentElementsInfo['View'] = view;
        }

        // Получение названия текущего апплета
        let currentApplet = $(event.currentTarget).closest('[data-type="applet"]');
        if (currentApplet != undefined) {
            let targetApplet = new this.#Application.Applet(currentApplet.attr('data-name'), currentApplet.attr('id'));
            let tileApplet;
            let formApplet;
            let popupApplet;
            let controlName = $(event.currentTarget).closest('[data-type="control"]').attr('data-name');
            controlName = controlName == undefined ? null : controlName;

            // В зависимости от типа текущего апплета установка разных значений для объекта
            switch (targetApplet.GetInfo()['Type']) {
                // Если тип текущего апплета - тайл
                case "Tile":
                    // Установка целевого апплета в объекте и текущей записи
                    tileApplet = new this.#Application.TileApplet(targetApplet.GetName(), targetApplet.GetId());
                    this.#CurrentElementsInfo['TargetApplet'] = tileApplet;
                    this.#CurrentElementsInfo['CurrentApplet'] = tileApplet;

                    // Если текущая запись не равна null, обновление ифнормации о ней
                    let currentRecord = tileApplet.GetRecordFromRS(event);
                    let selectedRecord = this.GetSelectedRecord(tileApplet.Name);
                    this.#CurrentElementsInfo['CurrentRecord'] = !Object.is(currentRecord, null) ? currentRecord : selectedRecord == null ? null : selectedRecord['properties'];
                    this.#CurrentElementsInfo['CurrentControl'] = controlName;
                    break;
                case "Form":
                    // Установка целевого апплета в объекте и текущей записи
                    formApplet = new this.#Application.FormApplet(targetApplet.GetName(), targetApplet.GetId());
                    this.#CurrentElementsInfo['TargetApplet'] = formApplet;
                    this.#CurrentElementsInfo['CurrentApplet'] = formApplet;
                    this.#CurrentElementsInfo['CurrentRecord'] = formApplet.GetRecordSet();
                    this.#CurrentElementsInfo['CurrentControl'] = controlName;
                    break;
                // Если тип попап
                case "Popup":
                    popupApplet = new this.#Application.PopupApplet(targetApplet.GetName(), targetApplet.GetId());
                    this.#CurrentElementsInfo['PopupApplet'] = popupApplet;
                    this.#CurrentElementsInfo['CurrentApplet'] = popupApplet;
                    this.#CurrentElementsInfo['CurrentPopupControl'] = controlName;
                    break;
            }
        }
    }

    /**
    * @M Изменяет значение выбранного элемента на новое
    * @param {String} elName Название элемента
    * @param {String} elValue Новое значение элемента
    */
    static SetElement(elName, elValue) {
        this.#CurrentElementsInfo[elName] = elValue;
    }
    //#endregion

    //@!W Эти методы подлежат удалению, нужны только для тестирования бека 
    static DisplayedRecords(componentName) {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetDisplayedRecords(componentName)
                .fail(error => console.log(error))
                .done(info => console.log(info));
        })
    }

    static BackSelectedRecords() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetSelectedRecords()
                .fail(error => console.log(error))
                .done(info => console.log(info));
        })
    }

    static BackScreenInfo() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetBackScreenInfo()
                .fail(error => {
                    console.log(error);
                    reject();
                })
                .done(info => {
                    console.log(info);
                    resolve();
                });
        });
    }

    static BackViewInfo() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetBackViewInfo()
                .fail(error => {
                    console.log(error);
                    reject();
                })
                .done(info => {
                    console.log(info);
                    resolve();
                });
        });
    }

    static BackAppletInfo() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetBackAppletInfo()
                .fail(error => {
                    console.log(error);
                    reject();
                })
                .done(info => {
                    console.log(info);
                    resolve();
                });
        });
    }
}