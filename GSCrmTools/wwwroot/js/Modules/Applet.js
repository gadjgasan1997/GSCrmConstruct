var recordSet = new Map();
export default (function () {
    let applet = function(name, id) {
        this.Name = name;
        this.Id = id;
        this.Name = name == null ?
            (function () {
                let appletName;
                GSCrmInfo.AppletsInfo.forEach((applet, index) => {
                    if (applet['AppletId'] == id) {
                        appletName = index;
                    }
                });
                return appletName;
            })()
            : name;
        this.Id = id == null ?
            (function () {
                let appletId;
                GSCrmInfo.AppletsInfo.forEach((applet, index) => {
                    if (index == name) {
                        appletId = applet['AppletId'];
                    }
                });
                return appletId;
            })()
            : id;
        let viewName;
        GSCrmInfo.ViewsInfo.forEach((info, view) => {
            info['ViewItems'].forEach(viewItem => {
                if (viewItem['AppletName'] == this.Name) {
                    viewName = view;
                }
            });
        });
        this.Info = GSCrmInfo.GetAppletInfo(this.Name);
        if (this.Info != undefined && viewName != undefined)
            this.Info['View'] = viewName;
    };
        
    //#region //@R Свойства
    // Возвращает Id апплета
    applet.prototype.GetId = function () {
        return this.Id;
    };

    // Возвращает название апплета
    applet.prototype.GetName = function () {
        return this.Name;
    };

    // Возвращает информацию об апплете из массива на фронте
    applet.prototype.GetInfo = function() {
        return this.Info;
    };
    //#endregion
    
    //#region //@R Инициализация
    //@M Инициализация апплета
    applet.prototype.InitializeApplet = function () {
        return new Promise((resolve, reject) => {
            if (GSCrmInfo.GetAppletInfo(this.Name) == undefined) {
                GSCrmInfo.Application.CommonRequests.InitializeApplet(this.Name)
                    .fail(error => reject(error))
                    .done(appletInfo => {
                        appletInfo['AppletId'] = this.Id;
                        appletInfo['TileItems'] = [];
                        GSCrmInfo.RemoveOldAppletInfo(this.Name);
                        GSCrmInfo.SetAppletInfo(this.Name, appletInfo);
                        this.Info = appletInfo;
                        resolve(appletInfo);
                    });
            }
            else {
                this.Info['AppletId'] = this.Id;
                resolve();
            }
        });
    };
    
    //@M Инициализация контрола
    applet.prototype.InitializeControl = function(item) {
        // Формирование id контрола
        let controlName = $(item).attr('data-name');
        let counter = 0;
        let controlId = 'control_' + controlName + '_' + counter;

        // До тех пор, пока контрол с таким id есть на странице, добавляется 1, чтобы id был уникальным
        while ($('#' + controlId).length > 0) {
            controlId = controlId.split(counter).join((counter + 1));
            counter++;
        }

        $(item).attr("id", controlId);
            
        let control = new GSCrmInfo.Application.Control(this, controlName, controlId);
        control.Initialize(this);
    };

    //@M Инициализация контролов
    applet.prototype.InitializeControls = function() {
        $('#' + this.Info['AppletId'])[0]
            .querySelectorAll('[data-type="control"]')
            .forEach(item => this.InitializeControl(item));
    };
    //#endregion

    //#region //@R Методы для работы с контролами
    /**
    *@M Получение контрола, на котором произошло действие
    * @param {Event} event Событие нажатия на контрол
    */
    applet.prototype.GetCurrentControl = function (event) {
        return $(event.currentTarget).closest('[data-type="control"]');
    };

    /**
    *@M Получение выбранного свойства всех контролов по названию
    * @param {String} propertyName Название свойства
    */
    applet.prototype.GetControlsProperty = function (propertyName) {
        return this.Info['Controls'].map(control => control[propertyName]);
    };

    /**
    *@M Получение всех свойств выбранного контрола по названию
    * @param {String} controlName Название контрола
    */
    applet.prototype.GetControlProperties = function (controlName) {
        return this.Info['Controls'].filter(control => control['Name'] == controlName)[0];
    };

    /**
    *@M Получение контролов у которых свойство popertyName равно popertyValue
    * @param {String} popertyName Название свойства
    * @param {String|Number|Boolean|null} popertyValue Значение свойства
    */
    applet.prototype.GetControlsByPropertyValue = function (popertyName, popertyValue) {
        return this.Info['Controls'].filter(control => control[popertyName] == popertyValue);
    };

    /**
    *@M Получение всех user property контролов
    * @param {String} controlName Название контрола
    */
    applet.prototype.GetControlUPs = function (controlName) {
        return this.Info['ControlUPs'];
    };

    //#endregion

    //#region //@R Методы для работы с recordSet-ом
    //@M Возвращает recordSet
    applet.prototype.GetRecordSet = function() {
        return recordSet.get(this.Name);
    };

    //@M Устанавливает recordSet
    applet.prototype.SetRecordSet = function (data) {
        recordSet.set(this.Name, data)
    };

    //@M Удаляет recordSet
    applet.prototype.DeleteRecordSet = function() {
        recordSet.delete(this.Name);
    }

    //@M Ззапрос на получение текущей записи с бека
    applet.prototype.GetAppletRecord = function() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetRecord(this.Info)
                .fail(error => reject(error))
                .done(data => resolve(data));
        });
    };

    /**
    *@M Получение значения свойства текущего апплета из recordSet-а для всех записей
    * @param {String} propertyName Название свойства
    */
    applet.prototype.GetRecordsProperty = function (propertyName) {
        return this.GetRecordSet().map(item => item[propertyName]);
    };

    /**
    *@M Получение записи для текущего апплета из recordSet-а по значению свойства
    * @param {String} propName Название свойства
    * @param {String} propValue Значение свойства
    */
    applet.prototype.GetRecordByProperty = function (popertyName, popertyValue) {
        return this.GetRecordSet().filter(item => item[popertyName] == popertyValue);
    };

    /**
    *@M Изменение записи для текущего апплета
    * @param {String} propName Название свойства
    * @param {String} propValue Значение свойства
    * @param {String} newRecord Новая запись
    */
    applet.prototype.UpdateRecordInRS = function (newRecord, propertyName, propertyValue) {
        switch(this.Info['Type']) {
            case "Tile":
                this.SetRecordSet(this.GetRecordSet().map(item => {
                    if (item[propertyName] == propertyValue)
                        item = newRecord;
                    return item;
                }));
                break;
            case "Form":
                this.SetRecordSet(newRecord);
                break;
        }
    };
    //#endregion

    //#region //@R Вспомогательные методы для работы с апплетами
    /**
    *@M Определяет, является ли действие системным
    * @param {String} actionType Название действия
    */
    applet.prototype.IsSystemAction = function (actionType) {
        if (['NextRecords',
            'PreviousRecords',
            'ShowPopup',
            'NewRecord',
            'UpdateRecord',
            'DeleteRecord',
            'UndoRecord',
            'UndoUpdate',
            'AppletConfg'].indexOf(actionType) != -1)
            return true;
        return false;
    };

    /**
    * @M Вызов действия, висящего на контроле
    * @param {Event} event События нажатия на контрол
    * @param {Object} properties Свойства контрола
    */
    applet.prototype.InvokeAction = function (event, properties) {
        // Если действие - промотка вперед или назад, тогда заменить название действия на Navigation
        let actionType;
        if (properties['ActionType'] == "NextRecords" || properties['ActionType'] == "PreviousRecords") {
            actionType = "Navigation";
        }
        else actionType = properties['ActionType'];

        // Заполнение информации о текущих выделенных элементах
        GSCrmInfo.SetUpCurrentElements(event);

        // Вызов действия
        GSCrmInfo.Application.CommonAction.Invoke(actionType, event, this.GetRecordSet());
    };

    return applet;
})()