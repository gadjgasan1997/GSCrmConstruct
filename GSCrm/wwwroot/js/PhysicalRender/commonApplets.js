var recordSet = new Map();

//@C Общий класс для апплета
class Applet {
    constructor(name, id) {
        this.Name = name == null ? 
        (function(){
            var appletName;
            Info.AppletsInfo.forEach((applet, index) => {
                if (applet['AppletId'] == id) { 
                    appletName = index;
                }
            });
            return appletName;
        })()
        : name;
        this.Id = id == null ? 
        (function(){
            var appletId;
            Info.AppletsInfo.forEach((applet, index) => {
                if (index == name) {
                    appletId = applet['AppletId'];
                }
            });
            return appletId;
        })()
        : id;
    }

    //#region //@R Свойства
    // Получает представление для текущего апплета
    get View() {
        let viewName;
        Info.ViewsInfo.forEach((info, view) => {
            info['ViewItems'].forEach(viewItem => {
                if (viewItem['AppletName'] == this.Name) {
                    viewName = view;
                }
            });
        });
        return viewName;
    }

    // Возвращает информацию об апплете из массива на фронте
    get Info() {
        return Info.GetAppletInfo(this.Name);
    }

    // Получение списка контролов из информации об апплете
    get Controls() {
        return this.Info['Controls'];
    }

    // Возвращает путь, по которому необходимо обращаться для апплета
    get Path() {
        var view = new View(this.View);
        return view.Info['Routing'][this.Name];
    }   

    // Возвращает recordSet
    get RecordSet() {
        return recordSet.get(this.Name);
    }

    // Устанавливает recordSet
    set RecordSet(data) {
        recordSet.set(this.Name, data)
    }
    //#endregion

    //#region //@R Инициализация
    //@M Инициализация контролов
    InitializeControls() {
        let controls = $('#' + this.Info['AppletId'])[0].querySelectorAll('[data-type="control"]');
        controls.forEach(control => {
            // Получение контрола, его настроек и значения
            let controlName = $(control).attr('data-name');
            let properties = this.GetControlProperties(controlName);
            if (properties != undefined) {
                // Тип контрола и заголовок
                let type = properties['Type'];
                let header = properties['Header']; 
                
                // Формирование id контрола
                let counter = 0;
                let controlId = 'control_' + properties['Name'] + '_' + counter;
    
                // До тех пор, пока контрол с таким id есть на странице, добавляется 1, чтобы id был уникальным
                while ($('#' + controlId).length > 0) {
                    controlId = controlId.split(counter).join((counter + 1));
                    counter++;
                }
    
                // Если у контрола есть иконка, то она добавляется
                if (!Object.is(properties['IconName'], null)) {
                    $(control)
                        .addClass('block-center')
                        .append('<span class="' + properties['IconName'] + '" aria-hidden="true"></span>');
                }
    
                // Иначе, если тип контрола кнопка, доабвляется заголовок
                else if (type == 'button') {
                    $(control).append('<div class="block-center"><p>' + header + '</p></div>');
                }
    
                // Проставление id и класса
                $(control)
                    .attr('id', controlId)
                    .addClass(properties['CssClass']);
    
                // Инициализация контролов в зависимости от типа
                switch (type) {
                    // Инициализация полей с типом input
                    case "input":
                        // Получение ограничение на длину поля, добавление счетчика, если оно есть
                        let maxlength = properties['maxlength'];
                        if (isNumeric(maxlength)) {
                            $(control).append('<div class="gs-field-label"><p>' + header + '</p><span class="gs-field-counter d-none">0/' +
                                maxlength + '</span></div><input class="gs-field-input" maxlength="' + maxlength + '" />');
                        }
    
                        // Если его нет, контрол формируется иначе
                        else {
                            $(control).append('<div class="gs-field-label"><p>' + header + '</p></div><input class="gs-field-input" />');
                        }
    
                        $(control)
                            .off('click')
                            .off('input')
                            .on('click', () => {
                                $(control)
                                    .addClass('gs-field-is-focused')
                                    .find('.gs-field-counter')
                                    .removeClass('d-none');
                                this.SetUpClasses(controls);
                            })
                            .on('input', () => {
                                // Если на поле есть счетчик
                                event.stopPropagation();
                                let text = '';
                                let counter = $(control).find('.gs-field-counter');
                                if (counter[0] != undefined) {
                                    // Установка значения счетчика в поле
                                    let val = $(control).find('.gs-field-input').val();
                                    text = counter[0].innerText.split('/');
                                    counter[0].innerText = val.length + '/' + text[1];
    
                                    // Если количество введенных символов равно ограничению, поле подсвечивается красным
                                    if (val.length >= text[1]) {
                                        $(item)
                                            .closest('.gs-field')
                                            .addClass('gs-field-error');
                                        setTimeout(() => {
                                            $(item)
                                            .closest('.gs-field')
                                            .removeClass('gs-field-error');
                                        }, 2000)
                                    }
                                }
                            });
    
                        break;
    
                    // Инициализация полей с типом date
                    case "date":
                        $(control)
                            .addClass('field-date')
                            .attr('data-field-up', $(control).attr('data-field-up') + ';NotInput')
                            .append('<div class="gs-field-label"><p>' + header + '</p></div><input class="gs-field-input" />' +
                                '<div class="pick-area"><div class="pick-icon"><span class="icon-calendar" aria-hidden="true"></span></div></div>')
                            .off('click')
                            .on('click', event => {
                                event.stopPropagation();
                                this.SetUpClasses(controls);
    
                                // Если щелкнули не по тому же полю, открытие календаря
                                if ($('.select-area').attr('data-target-id') != $(control).attr('id')) {
                                    let calendar = new Calendar;
                                    calendar.Initialize(control, event);
                                }                                
                            });
                        break;
    
                    // Инициализация полей с типом picklist
                    case "picklist":
                        $(control)
                            .addClass('field-picklist')
                            .attr('data-field-up', $(control).attr('data-field-up') + ';NotInput')
                            .append('<div class="gs-field-label"><p>' + header + '</p></div><input class="gs-field-input" />' +
                                '<div class="pick-area"><div class="pick-icon"><span class="icon-chevron-thin-right" aria-hidden="true"></span></div></div>')
                            .off('click')
                            .on('click', event => {
                                event.stopPropagation();
                                this.SetUpClasses(controls);
    
                                // Если щелкнули не по тому же полю, открытие пиклиста
                                if ($('.select-area').attr('data-target-id') != $(control).attr('id')) {
                                    var PL = new PickList;
                                    PL.Initialize(control, event);
                                } 
                            });
                        break;
    
                    // Инициализация полей с типом checkbox
                    case "checkbox":
                        // Добавление разметки и обработчика
                        $(control)
                            .removeClass('gs-field')
                            .addClass('field-checkbox')
                            .append('<div class="checkbox-check"></div><div>' + header + '</div>')
                            .find('.checkbox-check')
                            .off('click')
                            .on('click', () => {
                                // Проставление класса
                                this.RecordSet[controlName] == false ? $(control).addClass('gs-field-is-filled') : $(control).removeClass('gs-field-is-filled');
    
                                // Проставление значения контрола в recordSet
                                this.RecordSet[controlName] = this.RecordSet[controlName] == false ? true : false;
                            });
                        break;
    
                    // Инициализация полей с типом button
                    case "button":
                        // Добавление класса и обработчика
                        $(control)
                            .addClass(properties['CssClass'])
                            .off('click')
                            .on('click', event => {
                                event.stopPropagation();
                                // Если контролы не заблокированы
                                if ($(control).attr('disabled') == undefined) {
                                    // Блокировка контролов
                                    $('[data-type="control"]').attr('disabled', true);
                                    $('[data-type="applet_item"]').attr('disabled', true);

                                    // В любом случае, перед вызовом метода, висящего на кнопке, необходимо обновить recordSet, если это требуется
                                    if (Info.CurrentElementsInfo['UpdateRecordSet']) {
                                        let action = new Action;
                                        
                                        // Вызов автообновления записи
                                        action.Invoke('AutoUpdateRecord', event, Info.CurrentElementsInfo['RecordToUpdate'])
                                            .catch(error => console.log(error))
                                            .then(() => {
                                                this.InvokeAction(event, properties);
                                            })
                                            .finally(() => {
                                                // Обнуление
                                                Info.SetElement('AppletToUpdate', null);
                                                Info.SetElement('RecordToUpdate', null);
                                                Info.SetElement('UpdateRecordSet', false);
                                            })
                                    }

                                    else this.InvokeAction(event, properties);
                                }
                            });
                        break;
                }
    
                // Обработка полей
                if (type != 'button') {
                    // Если в recordSet-е есть значение для него, происходит заполнение контрола этим значением
                    if (!Object.is(this.RecordSet[controlName], "") && !Object.is(this.RecordSet[controlName], null)) {
                        switch(type) {
                            // Если контрол это checkbox, то заполненным его надо делать только в том случае, если значение из recordSet-а - true
                            case "checkbox":
                                if (Object.is(this.RecordSet[controlName], true)) {
                                    $(control)
                                        .addClass('gs-field-is-filled')
                                        .find('.gs-field-input')
                                        .val(this.RecordSet[controlName]);
                                }
                                break;
        
                            // Иначе в любом случае
                            default:
                                $(control)
                                    .addClass('gs-field-is-filled')
                                    .find('.gs-field-input')
                                    .val(this.RecordSet[controlName]);
                                break;
                        }
                    }
    
                    // При фокусировке на контроле/уходе с контрола
                    $(control)
                        .off('focus')
                        .off('focusout')
                        .on('focus', () => {
                            $(control).find('.gs-field-input').focus();
                        })
                        .on('focusout', () => {
                            let $input = $(control).find('.gs-field-input');
                            let value = $input.val();
                        
                            // Проставление значения контрола в recordSet
                            if (type != 'checkbox') {
                                this.RecordSet[controlName] = value;
                            }
                        
                            // Если тип контрола input
                            if (type == "input") {
                                $(control).removeClass('gs-field-is-focused');
                        
                                // Если значение пустое скрытие счетчика
                                if (value === '') {
                                    $(control)
                                        .removeClass('gs-field-is-filled')
                                        .find('.gs-field-counter')
                                        .addClass('d-none');
                                }
                        
                                // Иначе проставление класса
                                else {
                                    $(control)
                                        .closest('.gs-field')
                                        .addClass('gs-field-is-filled');
                                }
                            }
                        })
                }
    
                // Установка настроек
                //SetUP('#' + appletId);
            }
        });
    }
    //#endregion

    //#region //@R Методы для работы с контролами
    /**
     *@M Получение контрола, на котором произошло действие
     * @param {Event} event Событие нажатия на контрол
     */
    GetCurrentControl(event) {
        return $(event.currentTarget).closest('[data-type="control"]');
    }

    /**
     *@M Получение выбранного свойства всех контролов по названию
     * @param {String} propertyName Название свойства
     */
    GetControlsProperty(propertyName) {
        return this.Controls.map(control => control[propertyName]);
    }

    /**
     *@M Получение всех свойств выбранного контрола по названию
     * @param {String} controlName Название контрола
     */
    GetControlProperties(controlName) {
        return this.Controls.filter(control => control['Name'] == controlName)[0];
    }

    /**
     *@M Получение всех user property контролов
     * @param {String} controlName Название контрола
     */
    GetControlUPs(controlName) {
        return this.Info['ControlUPs'];
    }

    /**
     *@M Полученеи контролов у которых свойство popertyName равно popertyValue
     * @param {String} popertyName Название свойства
     * @param {String|Number|Boolean|null} popertyValue Значение свойства
     */
    GetControlsByPropertyValue(popertyName, popertyValue) {
        return this.Controls.filter(control => control[popertyName] == popertyValue)[0];
    }

    //#endregion

    //#region //@R Методы для работы с recordSet-ом
    //@M Ззапрос на получение текущей записи с бека
    GetRecord() {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.GetRecord(this)
                .fail(error => reject(error))
                .done(data => resolve(data));
        });
    } 

    /**
     *@M Получение значения свойства текущего апплета из recordSet-а для всех записей
     * @param {String} propertyName Название свойства
     */
    GetRecordsProperty(propertyName) {
        return this.RecordSet.map(item => item[propertyName]);
    }

    /**
     *@M Получение записи для текущего апплета из recordSet-а по значению свойства
     * @param {String} propName Название свойства
     * @param {String} propValue Значение свойства
     */
    GetRecordByProperty(propName, propValue) {
        return this.RecordSet.filter(item => item[propName] == propValue);
    }
    //#endregion

    //#region //@R Вспомогательные методы для работы с апплетами
    //@M Возвращает информацию об апплете, выполняя запрос на бек
    RequestInfo() {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.GetAppletInfo(this)
                .fail(error => reject(error))
                .done(info => {
                    resolve(info);
                });
        });
    }

    /**
     *@M Определяет, является ли действие системным
     * @param {String} actionName Название действия
     */
    IsSystemAction(actionName) {
        if (['NextRecords',
            'PreviousRecords',
            'ShowPopup',
            'NewRecord',
            'UpdateRecord',
            'DeleteRecord',
            'UndoRecord',
            'UndoUpdate',
            'AppletConfg'].indexOf(actionName) != -1)
            return true;
        return false;
    }

    /**
     * @M Вызов действия, висящего на контроле
     * @param {Event} event События нажатия на контрол
     * @param {Object} properties Свойства контрола
     */
    InvokeAction(event, properties) {
        // Если действие - промотка вперед или назад, тогда заменить название действия на Navigation
        let actionName;
        if (properties['ActionName'] == "NextRecords" || properties['ActionName'] == "PreviousRecords") {
            actionName = "Navigation";
        }
        else actionName = properties['ActionName'];
        
        // Заполнение информации о текущих выделенных элементах
        Info.SetUpCurrentElements(event);

        // Вызов действия
        let action = new Action;
        let func = action.Invoke(actionName, event, this.RecordSet);

        // Если действие возвращает promise
        if (func != undefined) {
            func != undefined && func
                .catch(error => {
                    console.log(error);
                })
                .then()
                .finally(() => {
                    // Снятие блокировки с контролов
                    $('[data-type="control"]').attr('disabled', false);
    
                    // Снятие блокировки с элементов списка
                    $('[data-type="applet_item"]').attr('disabled', false);
                });
        }

        // Иначе просто разблокировка контролов
        else {
            // Снятие блокировки с контролов
            $('[data-type="control"]').attr('disabled', false);

            // Снятие блокировки с элементов списка
            $('[data-type="applet_item"]').attr('disabled', false); 
        }
    }
    
    /**
     * @M Проставление классов для контролов при переходе по ним
     * @param {Array} controls 
     */
    SetUpClasses(controls) {
        controls.forEach(item => {
            let currentItemName = $(item).attr('data-name');
            switch (this.GetControlProperties(currentItemName)['Type']) {
                case "checkbox":
                    // В том случае, если контрол заполнен, добавление соответствующего класса
                    this.RecordSet[currentItemName] == false ? $(item).removeClass('gs-field-is-filled') 
                    : $(item).addClass('gs-field-is-filled');
                    break;

                case "picklist":
                    // В том случае, если контрол заполнен, добавление соответствующего класса
                    this.RecordSet[currentItemName] == '' ? $(item).removeClass('gs-field-is-filled')
                    : $(item).addClass('gs-field-is-filled');

                    // Проставление стрелок на пиклистах в стандартное положение
                    $(item)
                        .find('.icon-chevron-thin-left')
                        .removeClass('icon-chevron-thin-left')
                        .addClass('icon-chevron-thin-right');
                    break;

                default:
                    // В том случае, если контрол заполнен, добавление соответствующего класса
                    Object.is(this.RecordSet[currentItemName], null) ||
                    Object.is(this.RecordSet[currentItemName], "") ||
                    this.RecordSet[currentItemName] == undefined
                    ? $(item).removeClass('gs-field-is-filled')
                    : $(item).addClass('gs-field-is-filled');
                    break;
            }
        });
    }
    //#endregion
}

//!C Класс для тайл апплета
class TileApplet extends Applet {
    constructor(name, id) {
        super(name, id);
    }

    //#region //@R Свойства
    // При фокусировке на элементе списка
    set Focus(tileItem) {
        // Именно такая последовательность, так как чтобы обработчик события сработал правильно, необходимо сначала установить текущую запись
        // 0 шаг
        // Заполняю объект с выделенными записями
        Info.SelectedRecords.set(this.Name, {
            "tileItem": tileItem,
            "properties": this.GetRecordFromRSByIndex(this.GetSelection(tileItem))
        });

        // 1 шаг
        // Прикрепляю событие
        tileItem[0].dispatchEvent(new CustomEvent('FocusAppletItem', {
            detail: {}
        }));
    }

    // Возвращает записи, на которых в данный момент в апплетах находится фокус
    get SelectedRecords() {
        var records = {};
        Info.SelectedRecords.forEach((item, applet) => {
            records[applet] = item;
        });
        return records;
    }

    // Возвращает первую запись в recordSet-е
    get FirstRecord() {
        return super.RecordSet[0];
    }

    // Возвращает последнюю запись в recordSet-е
    get LastRecord() {
        return super.RecordSet[super.RecordSet.length - 1];
    }

    // Возвращает все свойства всех колонок
    get Columns() {
        return super.Info['Columns'];
    }

    // Возвращает записи из тайла
    get TileItems() {
        return $('#' + super.Info['AppletId']).find('.' + super.Info['itemClass']);
    }
    //#endregion

    //#region //@R Инициализация
    //@M Инициализация апплета
    Initialize() {
        return new Promise((resolve, reject) => {
            super.RequestInfo()
                .catch(error => reject(error))
                .then(info => {
                    info['AppletId'] = this.Id;
                    Info.RemoveOldAppletInfo(this.Name);
                    Info.SetAppletInfo(this.Name, info);
    
                    // Прикрепление обработчика
                    $('#' +  this.Id)[0].dispatchEvent(new CustomEvent('OnAppletReady', {
                        detail: {}
                    }));
    
                    resolve();
                });
        });
    }

    //@M инициализация строк
    InitializeRows() {
        // Для каждой строки в апплете
        $('#' + this.Id + ' [data-type="applet_item"]')
            .each((selection, row) => {
                // Текущая строка
                $(row)
                    // Проставление id и класса
                    .attr('id', 'tile_' + this.Id + '_item_' + selection)
                    .addClass(super.Info['itemClass'])
                    // Обработчики событий
                    .off('mouseover')
                    .off('mouseout')
                    .off('click')
                    .off('TileItemSelect')
                    .on('mouseover', event => {
                        event.currentTarget.dispatchEvent(new CustomEvent('AppletItemMouseOver', {
                            detail: {}
                        }));
                    })
                    .on('mouseout', event => {
                        event.currentTarget.dispatchEvent(new CustomEvent('AppletItemMouseOut', {
                            detail: {}
                        }));
                    })
                    .on('click', event => {
                        $(event.currentTarget).trigger('TileItemSelect', []);
                    })
                    .on('TileItemSelect', event => {
                        this.SelectTileItem(event);
                    })
                    // Маппинг ячеек
                    .find('[data-type="cell"]')
                    .map((index, cell) => {
                        let columnName = cell.getAttribute('data-name');
                        let columnType = this.GetColumnProperties(columnName)[0]['Type'];
                        let cellValue = super.RecordSet[selection][columnName];
                        var PR = new DefaultTileAppletPR;
                        PR.RenderCell(cell, cellValue, columnName, columnType);
                        $(cell)
                            .off('CellCnange')
                            .on('CellCnange', (event, args) => {
                                /* Перед тем, как менять recordSet, необходимо убедиться, что в нем остуствуют изменения, 
                                иначе надо вначале обновить последнюю измененную запись
                                Также добавлена проверка на то, что строка для обновления не должна быть текущей */
                                args['Event'].stopPropagation();
                                let record = recordSet.get(this.Name)[selection];
                                if (Info.CurrentElementsInfo['UpdateRecordSet'] && Info.CurrentElementsInfo['RecordToUpdate']['Id'] != record['Id']) {
                                    let action = new Action;
                                    
                                    // Обновление информации о текущих выбранных элементах
                                    Info.SetUpCurrentElements(event);
                                    
                                    // Вызов автообновления записи
                                    action.Invoke('AutoUpdateRecord', event, Info.CurrentElementsInfo['RecordToUpdate'])
                                        .catch(error => console.log(error))
                                        .finally(() => {
                                            Info.SetElement('AppletToUpdate', this);
                                            Info.SetElement('UpdateRecordSet', true);
                                            Info.SetElement('RecordToUpdate', record);
                                            this.SelectTileItem(event);
                                        });
                                }
                                else {
                                    Info.SetElement('AppletToUpdate', this);
                                    Info.SetElement('UpdateRecordSet', true);
                                    Info.SetElement('RecordToUpdate', record);
                                    this.SelectTileItem(event);
                                }
                                
                                // Обновление recordSet-а и проставление сведений о записи, необходимой для обновления
                                recordSet.get(this.Name)[selection][columnName] = args['CellNewValue'];
                            });
                    });
            });
    }
    //#endregion

    //#region //@R Методы для работы со списком
    /**
     *@M Событие выбора записи из списка, обновляет информацию о представлении и его контекст
     * @param {Event} event Событие выбора записи
     */
    SelectTileItem(event) {
        let tileItem = $(event.currentTarget).closest('[data-type="applet_item"]');
        if (tileItem.attr('disabled') == undefined) {
            let currentRecord = Info.SelectedRecords.get(this.Name);
    
            // В случае, если кликнули не по той же записи, обновление инфорамции о представлении и контекста
            if (currentRecord == undefined || currentRecord['tileItem'][0] != tileItem[0]) {

                // Фокусировка
                this.Focus = tileItem;
    
                // Установка блокировки для контролов
                $('[data-type="control"]').attr('disabled', true);
    
                // Установка блокировки для элементов списка
                $('[data-type="applet_item"]').attr('disabled', true);
                
                // Отображение анимации ожидания
                $('.cssload-wrapper').removeClass('d-none');
    
                // Установка текущих элементов представления
                let recordId = this.GetRecordFromRS(event)['Id'];
                let data = {
                    ViewName: this.View,
                    CurrentApplet: this.Name,
                    CurrentRecord: recordId,
                    CurrentControl: null,
                    Action: 'SelectTileItem'
                }

                // Обновление информации о текущих выбранных элементах
                Info.SetUpCurrentElements(event);          

                /* В любом случае, перед вызовом метода, висящего на кнопке, необходимо обновить recordSet, если это требуется.
                    Если есть записб для обновления, то вначале надо обновить ее, и только затем информацию о представлении.
                    Если обновление записи не требуется, можно сразу обновить информацию о представлении.
                    Также добавлена проверка на то, что строка для обновления не должна быть текущей */
                if (Info.CurrentElementsInfo['UpdateRecordSet'] && Info.CurrentElementsInfo['RecordToUpdate']['Id'] != this.GetRecordFromRS(event)['Id']) {
                    let action = new Action;

                    // Вызов автообновления записи
                    action.Invoke('AutoUpdateRecord', event, Info.CurrentElementsInfo['RecordToUpdate'])
                        .catch(error => console.log(error))
                        .then(() => {
                            let view = new View(this.View);
                            view.UpdateViewInfo(data)
                                .catch(error => {
                                    console.log(error);
                                })
                                .then(() => {
                                    // Обновление контекста
                                    view.PartialUpdateContext(this, super.RecordSet[this.GetSelection(tileItem)]['Id'], false)
                                        .catch(error => {
                                            console.log(error);
                                        });
                                })
                                .finally(() => {
                                    // Снятие блокировки с контролов
                                    $('[data-type="control"]').attr('disabled', false);
                
                                    // Снятие блокировки с элементов списка
                                    $('[data-type="applet_item"]').attr('disabled', false);
                                    
                                    // Скрытие анимации ожидания
                                    $('.cssload-wrapper').addClass('d-none');
                                    
                                    // Обнуление
                                    Info.SetElement('AppletToUpdate', null);
                                    Info.SetElement('RecordToUpdate', null);
                                    Info.SetElement('UpdateRecordSet', false);
                                });
                        });
                }
                else {
                    let view = new View(this.View);
                    view.UpdateViewInfo(data)
                        .catch(error => {
                            console.log(error);
                        })
                        .then(() => {
                            // Обновление контекста
                            view.PartialUpdateContext(this, super.RecordSet[this.GetSelection(tileItem)]['Id'], false)
                                .catch(error => {
                                    console.log(error);
                                });
                        })
                        .finally(() => {
                            // Снятие блокировки с контролов
                            $('[data-type="control"]').attr('disabled', false);
        
                            // Снятие блокировки с элементов списка
                            $('[data-type="applet_item"]').attr('disabled', false);
                            
                            // Скрытие анимации ожидания
                            $('.cssload-wrapper').addClass('d-none');
                        });
                }
            }
        }
    }

    /**
     *@M Получение элемента апплета по его id из recordSet-а
     * @param {String} recordId Id записи из recordSet-а
     */
    GetTileItemByIdInRS(recordId) {
        var sequence = 0;
        super.RecordSet.forEach((item, index) => {
            if (item['Id'] == recordId)
                sequence = index;
        });
        return $('#' + this.Id).find('[data-type="applet_item"]').eq(sequence);
    }

    /**
     *@M Получение элемента апплета по его индексу
     * @param {Number} sequence Индекс элемента апплета
     */
    GetTileItemBySequence(sequence) {
        return $($('#' + super.Info['AppletId']).find('[data-type="applet_item"]')[sequence]);
    }

    /**
     *@M Получение selection-а элемента апплета
     * @param {HTMLElement} target Элемент, selection которого необходимо получить
     */
    GetSelection(target) {
        var index = 0;
        target
            .closest('[data-type="applet"]')
            .find('[data-type="applet_item"]')
            .filter((i, item) => {
                if ($(item)[0] == target.closest('[data-type="applet_item"]')[0])
                    index = i;
            });
        return index;
    }
    //#endregion

    //#region //@R Методы для работы с колонками
    /**
     *@M Получение выбранного свойства всех колонок по названию
     * @param {String} propertyName Название свойства
     */
    GetColumnsProperty(propertyName) {
        return this.Columns.map(column => column[propertyName]);
    }

    /**
     *@M Получение всех свойств выбранной колонки по названию
     * @param {String} columnName Название колонки
     */
    GetColumnProperties(columnName) {
        return this.Columns.filter(column => column['Name'] == columnName);
    }

    /**
     *@M Возвращает колонки у которых свойство popertyName равно popertyValue
     * @param {String} popertyName Название свойства
     * @param {String|Number|Boolean|null} popertyValue Значение свойства
     */
    GetColumnsByPropertyValue(popertyName, propertyValue) {
        return this.Columns.filter(column => column[popertyName] == propertyValue)[0];
    }
    //#endregion

    //#region //@R Методы для работы с recordSet-ом
    //@M Запрос на получение записей для апплета с бека
    GetRecords() {
        return new Promise((resolve, reject) => {
            let request = new RequestsToDB;
            request.GetRecords(this)
                .fail(error => reject(error['responseJSON']))
                .done(result => {
                    let data = result;
                    let displayedRecords = data['DisplayedRecords'];
    
                    // Заполнение recordSet-а
                    recordSet.set(this.Name, displayedRecords);

                    // Если количество полученных записей 0
                    if (displayedRecords.length == 0) {
                        // При инициализации представления и, также, если в апплете до этого были записи надо добавить empty state
                        let body = $('#' + this.Id + ' .def-tile-body');
                        if (this.TileItems.length != 0 || body.length == 0) {
                            let PR = new DefaultTileAppletPR;
                            PR.RenderApplet(this.Name, displayedRecords.length);
                            super.InitializeControls();
                        }
                    }
    
                    // Иначе
                    else {
                        // Перерисовываю апплет, если количество полученных записей не совпадает с количеством строк в апплете
                        if (displayedRecords.length != this.TileItems.length) {
                            let PR = new DefaultTileAppletPR;
                            PR.RenderApplet(this.Name, displayedRecords.length);
                            super.InitializeControls();
                        }
    
                        // Создание строк
                        this.InitializeRows();
                    }
    
                    // Обновление сведений о текущих выбранных записях
                    let recordId = data['SelectedRecords'][this.Name];
                    let tileItem = this.GetTileItemByIdInRS(recordId);
                    let properties = super.GetRecordByProperty('Id', recordId);
                    if (properties.length != 0) {
                        let selectRecordInfo = {};
                        selectRecordInfo['properties'] = properties;
                        selectRecordInfo['tileItem'] = tileItem;
                        Info.SelectedRecords.set(this.Name, selectRecordInfo);
                    }
                    else {
                        Info.SelectedRecords.set(this.Name, null);
                    }
    
                    // Фокусировка
                    if (displayedRecords.length > 0) {
                        this.Focus = tileItem;
                    }
    
                    resolve(data);
                });
        });
    }

    /**
     *@M Возвращает свойство для выбранных записей во всех аплетах
     * @param {String} propertyName Название свойства для получения
     */
    GetSelectedRecordsProperty(propertyName) {
        let records = {};
        Info.SelectedRecords.forEach((item, applet) => {
            records[applet] = item['properties'][propertyName];
        });
        return records;
    }

    /**
     *@M Возвращает значение свойства по имени для выбранной записи в текущем апплете
     * @param {String} propertyName Название свойства для получения
     */
    GetSelectedRecordProperty(propertyName) {
        return Info.SelectedRecords.get(this.Name)['properties'][propertyName];
    }

    /**
     * Получает запись из recordSet-а по событию
     * @param {Event} event Событие выбора записи
     */
    GetRecordFromRS(event) {
        let tileItem = $(event.currentTarget).closest('[data-type="applet_item"]');
        return tileItem.length == 0 ? null : super.RecordSet[this.GetSelection(tileItem)];
    }

    /**
     * Получает запись из recordSet-а по индексу
     * @param {Number} index Номер записи из recordSet-а
     */
    GetRecordFromRSByIndex(index) {
        return this.RecordSet.filter((item, i) => i == index)[0];
    }
    //#endregion
}

//!C Класс для попап апплета
class PopupApplet extends Applet {
    constructor(name, id) {
        super(name, id);
    }
    
    //#region //@R Инициализация и уничтожение попапа
        /**
     *@M Инициализация попап апплета, на вход принимается события нажатия на контрол, по которому открывается попап
     * @param {Event} event Событие нажатия на контрол
     */
    Initialize(event) {
        return new Promise((resolve, reject) => {
            let info = Info.CurrentElementsInfo;
            let controlName = Info.CurrentElementsInfo['ControlName'];
            let targetAppletName = Info.CurrentElementsInfo['TargetApplet'].Name;
            let record = Info.CurrentElementsInfo['Record'];
            let recordId = record == undefined ? null : record['Id'];
            let appletItem = $(event.currentTarget).closest('[data-type="applet_item"]');

            let inputsObj = {
                ViewName: Info.CurrentElementsInfo['Name'],
                CurrentApplet: targetAppletName,
                CurrentRecord: recordId,
                CurrentControl: controlName,
                OpenPopup: true,
                Action: 'ShowPopup'
            }

            // Обновление информации о представлении
            info['View'].UpdateViewInfo(inputsObj)
                .catch(error => reject(error))
                .then(() => {
                    // Получение информации о попапе
                    super.RequestInfo()
                        .catch(error => reject(error))
                        .then(appletInfo => {                
                            // Установка информации об апплете
                            appletInfo['AppletId'] = this.Id;
                            Info.RemoveOldAppletInfo(this.Name);
                            Info.SetAppletInfo(this.Name, appletInfo);
                            let targetApplet = new Applet(targetAppletName);

                            // Получение размера попапа из user property апплета, с которого он был открыт
                            let size = targetApplet.Info['ControlUPs'][controlName].filter(item => item['Name'] == 'Size')[0]['Value'];

                            // Получение сведений о записи, отоброжаемой в попапе
                            super.GetRecord()
                                .catch(error => reject(error))
                                .then(data => {
                                    // Установка recordSet-а
                                    recordSet.set(this.Name, data);

                                    // Создание модального окна
                                    (() => {
                                        // Скрытие области с выбором, обязательно вначале, так как проставляется record-set
                                        if (!$('.select-area').hasClass('d-none')) {
                                            view.CloseSelectArea();
                                        }

                                        // Тело
                                        $('.all-popup-modal')
                                            .empty()
                                            .append('<div class="p-modal-' + size + '">' +
                                                '<div class="popup-wrap">' +
                                                '<div class="row p-modal-head">' +
                                                '<div class="p-modal-dummy"><div></div></div>' +
                                                '<div><h4>' + appletInfo['Header'] + '</h4></div>' +
                                                '<div class="p-modal-close"><div></div></div>' +
                                                '</div>' +
                                                '<div class="divider-mr-none"></div>' +
                                                '<div class="p-modal-body"></div>' +
                                                '<div class="divider-mr-none"></div>' +
                                                '<div class="p-modal-footer"></div>' +
                                                '</div>' +
                                                '</div>');

                                        // Отображение
                                        $('.all-popup').removeClass('d-none');
                                        $('.mask-all-screen').removeClass('d-none');

                                        // При открытии модального скрывать скроллы (если модальное окно раскрыто на весь экран)
                                        if (window.innerWidth < 992) {
                                            $('html').css('overflow', 'hidden')
                                        }
                                    })();

                                    // Проставление атрибутов
                                    $('.popup-wrap')
                                        .attr('id', this.Id)
                                        .attr('data-name', this.Name)
                                        .attr('data-type', 'applet')
                                        .attr('data-target-id', targetApplet.Id);

                                    // Инициализация попап апплета
                                    let PR = new Col2FieldsActions;
                                    PR.RenderApplet(this);

                                    // Инициализация контролов
                                    super.InitializeControls(this.Name);

                                    /* Если поднятие попапа произошло на записи тайл апплета, необходимо обновить контекст дочерних сущностей
                                       и сфокусироваться на выделенной */
                                    if (targetApplet.Info['Type'] == 'Tile' && appletItem.length > 0 && recordId != null) {
                                        targetApplet = new TileApplet(targetApplet.Name, targetApplet.Id);
                                        targetApplet.Focus = appletItem;
                                        info['View'].PartialUpdateContext(targetApplet, recordId, false)
                                            .catch(error => reject(error))
                                            .then(() => resolve());
                                    }
                                    else resolve();
                                });
                        });
                });
        });
    }

    /**
     *@M Закрытие попапа, в зависимости от типа операции выполняя либо обновление информации о представлении, либо ее запрос с бека
     * @param {String} operationType Тип операции
     */
    Dispose() {
        // Удаление данных из recrodSet-а
        recordSet.delete(this.Name);

        // Удаление данных из appletInfo
        Info.RemoveAppletInfo(this.Name);

        // Удаление данных из CurrentElementsInfo
        Info.SetElement('PopupApplet', null);
        Info.SetElement('ControlName', null);

        // Скрытие попапа
        $('.all-popup')
            .addClass('d-none')
            .find('.all-popup-modal')
            .empty();

        // Скрываю маску
        $('.mask-all-screen').addClass('d-none');

        // Отоброжаю скроллы
        $('html').css('overflow', 'auto');
    }
    //#endregion
}