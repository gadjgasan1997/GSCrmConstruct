//@C Класс для работы с информацией о сущностях
class Info {
    //#region //@R Свойства
    static #ScreenInfo = {};
    static #ViewInfo = new Map();
    static #AppletInfo = new Map();
    static #CurrentElementsInfo = {};
    static SelectedRecords = new Map();

    // Получение информаци об экране
    static get ScreenInfo() {
        return this.#ScreenInfo;
    }

    /**
     * Установка информации об экране
     * @param {String} screenName Название экрана
     * @param {{
        * String: Name Название скрина
        * String: CurrentViewName Название текущего представления
        * Array: CurrentAggregateCategory Текущая категория
        * Object: AggregateViews Список представлений текущей категории
        * }} screenInfo Информация об экране
    */
    static set ScreenInfo(screenInfo) {
        this.#ScreenInfo = screenInfo;
    }

    // Получение информации обо всех представлениях
    static get ViewsInfo() {
        return this.#ViewInfo;
    }

    // Получение инфомрации обо всех апплетах
    static get AppletsInfo() {
        return this.#AppletInfo;
    }

    // Получение информации о текущих элементах
    static get CurrentElementsInfo() {
        return this.#CurrentElementsInfo;
    }
    //#endregion

    //#region //@R методы для работы с информацией о сущностях
    //@M Получение информации о представлении
    static GetViewInfo(viewName) {
        return this.#ViewInfo.get(viewName);
    }
    
    /**
     * @M Установка информации о представлении
     * @param viewName Название представления
     * @param {{
     * View: View Представление
     * Object: Applets Список апплетов, находящихся в представлении
     * Object: ViewItems Список элементов представления
     * Object: Routing Маршрутизация для апплетов
     * }} viewInfo Информация о представлении
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
        Info.ViewsInfo.forEach((info, viewName) => {
            if (viewName != Info.ScreenInfo['CurrentView']) {
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
     * @param {{
     * String: Name Название апплета
     * String: Type Тип апплета
     * String: Header Заголовок апплета
     * Number|String: DisplayLines Количесто отоброжаемых записей
     * String: EmptyState EmptyState
     * Object: Controls Список контролов
     * Object: ControlUPs Список всех user property
     * Object: Columns Список колонок
     * String: PR Название PR файла, который генерирует разметку
     * }} appletInfo Информация об апплета
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
        let viewName = Info.ScreenInfo['CurrentView']['ViewName'];
        let viewItems = this.GetViewInfo(viewName)['ViewItems'];
        let applets = viewItems.map(item => item['AppletName']);

        // Апплеты, которые есть в информации о представлении(включая апплеты, подлежащие удалению)
        this.AppletsInfo.forEach((info, appletName) => {
            if (applets.indexOf(appletName) == -1) {
                this.RemoveAppletInfo(appletName);
            }
        })
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
        let viewName = Info.ScreenInfo['CurrentView']['ViewName'];
        let view = this.#CurrentElementsInfo['View'];
        if (view == undefined || viewName != view.Name) {
            view = new View(viewName);
            this.#CurrentElementsInfo['View'] = view;
        }
            
        // Получение название текущего апплета и создание экземпляра самого апплета.
        let currentAppletName = $(event.currentTarget).closest('[data-type="applet"]').attr('data-name');
        let currentApplet = new Applet(currentAppletName, null);
        let controlName = $(event.currentTarget).closest('[data-type="control"]').attr('data-name');

        // В зависимости от типа текущего апплета установка разных значений для объекта
        switch(currentApplet.Info['Type']) {
            // Если тип текущего апплета - тайл
            case "Tile":
                // Установка целевого апплета в объекте и текущей записи
                let targetApplet = new TileApplet(currentApplet.Name, null);
                this.#CurrentElementsInfo['TargetApplet'] = targetApplet;

                // Если текущая запись не равна null, обновление ифнормации о ней
                let record = targetApplet.GetRecordFromRS(event);
                if (!Object.is(record, null)) {
                    this.#CurrentElementsInfo['Record'] = record;
                }

                /* Если на текущем контроле висит действие ShowPopup, этот попап необходимо добавить в информацию о представлении
                   Также необходимо проверять контрол на undefined */
                if (controlName && targetApplet.GetControlProperties(controlName)['ActionName'] == 'ShowPopup') {
                    let popupName = targetApplet.Info['ControlUPs'][controlName].filter(item => item['Name'] == 'Applet')[0]['Value'];
                    this.#CurrentElementsInfo['PopupApplet'] = new PopupApplet(popupName, null);
                }
                break;
            case "Form":
                break;
            // Если тип попап
            case "Popup":
                this.#CurrentElementsInfo['PopupApplet'] = new PopupApplet(currentApplet.Name, currentApplet.Id);
                this.#CurrentElementsInfo['TargetApplet'] = new TileApplet(null, view.GetTargetId(event));
                break;
        }

        // Установка текущего контрола
        this.#CurrentElementsInfo['ControlName'] = controlName;
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

    //!W Эти методы подлежат удалению, нужны только для тестирования бека 
    static DisplayedRecords(componentName) {
        return new Promise((resolve, reject) => {
            let request = new RequestsToDB;
            request.GetDisplayedRecords(componentName)
                .fail(error => console.log(error))
                .done(info => console.log(info));
        })
    }

    static GetSelectedRecords() {
        return new Promise((resolve, reject) => {
            let request = new RequestsToDB;
            request.GetSelectedRecords()
                .fail(error => console.log(error))
                .done(info => console.log(info));
        })
    }

    static BackScreenInfo() {
        return new Promise((resolve, reject) => {
            let request = new RequestsToDB;
            request.GetBackScreenInfo()
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
            let request = new RequestsToDB;
            request.GetBackViewInfo()
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
            let request = new RequestsToDB;
            request.GetBackAppletInfo()
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