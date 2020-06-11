import DefaultTileAppletPR from '../PhysicalRenders/DefaultTileAppletPR.js'

export default function (application) {
    let tileApplet = function(name, id) {
        application.prototype.Applet.apply(this, [name, id]);
    }
    
    tileApplet.prototype = Object.create(application.prototype.Applet.prototype);

    //#region //@R Свойства
    // При фокусировке на элементе списка
    tileApplet.prototype.Focus = function (tileItem) {
        // Именно такая последовательность, так как чтобы обработчик события сработал правильно, необходимо сначала установить текущую запись
        // 0 шаг
        // Заполняю объект с выделенными записями
        GSCrmInfo.SetSelectedRecord(this.Name, {
            "record": tileItem,
            "properties": this.GetRecordFromRSByIndex(this.GetSelection(tileItem))
        });

        // 1 шаг
        // Прикрепляю событие
        tileItem[0].dispatchEvent(new CustomEvent('FocusAppletItem', {
            detail: {}
        }));
    };

    // Возвращает записи, на которых в данный момент в апплетах находится фокус
    tileApplet.prototype.SelectedRecords = function () {
        var records = {};
        GSCrmInfo.SelectedRecords.forEach((item, applet) => {
            records[applet] = item;
        });
        return records;
    };

    // Возвращает первую запись в recordSet-е
    tileApplet.prototype.FirstRecord = function () {
        return this.GetRecordSet()[0];
    };

    // Возвращает последнюю запись в recordSet-е
    tileApplet.prototype.LastRecord = function () {
        return this.GetRecordSet()[this.GetRecordSet().length - 1];
    };

    // Возвращает все свойства всех колонок
    tileApplet.prototype.Columns = function () {
        return this.Info['Columns'];
    };

    // Возвращает записи из тайла
    tileApplet.prototype.TileItems = function () {
        return $('#' + this.Info['AppletId']).find('.' + this.Info['itemClass']);
    };
    //#endregion

    //#region //@R Инициализация
    //@M Инициализация тайл апплета
    tileApplet.prototype.Initialize = function () {
        return new Promise((resolve, reject) => {
            this.InitializeApplet(this.Name)
                .catch(error => reject(error))
                .then(() => {
                    GSCrmInfo.Application.CommonRequests.GetRecords(this.Info)
                        .fail(error => reject(error['responseJSON']))
                        .done(recordsInfo => {
                            let displayedRecords = recordsInfo['DisplayedRecords'];
                            let selectedRecords = recordsInfo['SelectedRecords'];

                            // Заполнение recordSet-а
                            this.SetRecordSet(displayedRecords);
                            
                            // Если количество полученных записей 0
                            if (displayedRecords.length == 0) {
                                // При инициализации представления и, также, если в апплете до этого были записи надо добавить empty state
                                let PR = new DefaultTileAppletPR;
                                PR.RenderApplet(this.Name, displayedRecords.length);
                                this.InitializeControls();
                            }
        
                            // Иначе
                            else {
                                // Перерисовываю апплет, если количество полученных записей не совпадает с количеством строк в апплете
                                if (displayedRecords.length != this.Info['TileItems'].length) {
                                    let PR = new DefaultTileAppletPR;
                                    PR.RenderApplet(this.Name, displayedRecords.length);
                                    this.InitializeControls();
                                }
                                this.InitializeRows();
                            }
        
                            // Обновление сведений о текущих выбранных записях
                            let recordId = selectedRecords[this.Name];
                            let tileItem = this.GetTileItemByIdInRS(recordId);
                            let properties = this.GetRecordByProperty('Id', recordId);
                            if (properties.length != 0) {
                                let selectRecordInfo = {};
                                selectRecordInfo['properties'] = properties;
                                selectRecordInfo['record'] = tileItem;
                                GSCrmInfo.SetSelectedRecord(this.Name, selectRecordInfo);
                            }
                            else GSCrmInfo.SetSelectedRecord(this.Name, null);
                            
                            // Фокусировка
                            if (displayedRecords.length > 0) {
                                this.Focus(tileItem);
                            }

                            //this.Info['TileItems'] = $('#' + this.Id).find('[data-type="applet_item"]');
        
                            resolve(recordsInfo);
                        });
                });
        });
    };

    //@M инициализация строк
    tileApplet.prototype.InitializeRows = function () {
        // Для каждой строки в апплете
        $('#' + this.Id + ' [data-type="applet_item"]')
            .each((selection, row) => {
                // Текущая строка
                $(row)
                    // Проставление id и класса
                    .attr('id', 'tile_' + this.Id + '_item_' + selection)
                    .addClass(this.Info['itemClass'])
                    // Обработчики событий
                    .off('mouseover')
                    .off('mouseout')
                    .off('click')
                    .off('TileItemSelect')
                    .on('mouseover', event => {
                        $(event.currentTarget).trigger('AppletItemMouseOver', []);
                    })
                    .on('mouseout', event => {
                        $(event.currentTarget).trigger('AppletItemMouseOut', []);
                    })
                    .on('click', event => {
                        $(event.currentTarget).trigger('TileItemSelect', []);
                    })
                    .on('TileItemSelect', event => {
                        GSCrmInfo.SetUpCurrentElements(event);
                        GSCrmInfo['CurrentElementsInfo']['TargetApplet'].SelectTileItem(event);
                    })
                    // Маппинг ячеек
                    .find('[data-type="cell"]')
                    .map((index, cell) => {
                        let columnName = cell.getAttribute('data-name');
                        let properties = this.GetColumnProperties(columnName)[0];
                        let cellValue = this.GetRecordSet()[selection][columnName];
                        let PR = new DefaultTileAppletPR();
                        PR.RenderCell(cell, cellValue, properties);
                        $(cell)
                            .off('CellCnange')
                            .on('CellCnange', (event, args) => {
                                if (!properties['Readonly']) {
                                    args['Event'].stopPropagation();

                                    /* Перед тем, как менять recordSet, необходимо убедиться, что в нем остуствуют изменения, 
                                    иначе надо вначале обновить последнюю измененную запись
                                    Также добавлена проверка на то, что строка для обновления не должна быть текущей */
                                    let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                                    let view = currentElementsInfo['View'];
                                    let targetApplet = new GSCrmInfo.Application.TileApplet(view.GetAppletName(event), view.GetAppletId(event));
                                    let currentRecord = this.GetRecordSet()[selection];
                                    let appletToUpdate = currentElementsInfo['AppletToUpdate'];
                                    let recordToUpdate = currentElementsInfo['RecordToUpdate'];

                                    if (appletToUpdate != null && recordToUpdate['Id'] != currentRecord['Id']) {
                                        // Обновление информации о текущих выбранных элементах
                                        GSCrmInfo.SetUpCurrentElements(event);
                                        
                                        // Вызов автообновления записи
                                        GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, recordToUpdate)
                                            .catch(error => console.log(error))
                                            .then(() => {
                                                // Элемента ддя обновления устанаваливаются заново, так как в методе AutoUpdateRecord происходит их обнуление
                                                if (this.Info['Initflag'] == false) {
                                                    GSCrmInfo.SetElement('AppletToUpdate', this);
                                                    GSCrmInfo.SetElement('RecordToUpdate', currentRecord);
                                                }
                                                targetApplet.SelectTileItem(event)
                                                    .catch(error => console.log(error))
                                                    .then(() => view.RefreshViewAppletsUI(targetApplet, targetApplet.GetRecordSet()[
                                                        this.GetSelection($(event.currentTarget).closest('[data-type="applet_item"]'))
                                                    ]));
                                        });
                                    }
                                    
                                    else {
                                        if (this.Info['Initflag'] == false) {
                                            GSCrmInfo.SetElement('AppletToUpdate', this);
                                            GSCrmInfo.SetElement('RecordToUpdate', currentRecord);
                                        }
                                        targetApplet.SelectTileItem(event)
                                            .catch(error => console.log(error))
                                            .then(() => view.RefreshViewAppletsUI(targetApplet, targetApplet.GetRecordSet()[
                                                this.GetSelection($(event.currentTarget).closest('[data-type="applet_item"]'))
                                            ]));
                                    }
                                    
                                    // Обновление recordSet-а и проставление сведений о записи, необходимой для обновления
                                    this.GetRecordSet()[selection][columnName] = args['CellNewValue'];
                                }
                            })
                        .off('Drilldown')
                        .on('Drilldown', (event, args) => {
                            args['Event'].stopPropagation();

                            /* Перед тем, как менять recordSet, необходимо убедиться, что в нем остуствуют изменения, 
                            иначе надо вначале обновить последнюю измененную запись
                            Также добавлена проверка на то, что строка для обновления не должна быть текущей */
                            let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                            let currentRecord = this.GetRecordSet()[selection];
                            let appletToUpdate = currentElementsInfo['AppletToUpdate'];
                            let recordToUpdate = currentElementsInfo['RecordToUpdate'];
                            let currentColumn = $(event.currentTarget).closest('[data-type="cell"]');
                            let columnType;
                            if (currentColumn != undefined) {
                                let columnName = currentColumn.attr('data-name');
                                let columnProperties = this.GetColumnProperties(columnName)[0];
                                columnType = columnProperties == undefined ? null : columnProperties['Type'];
                            }
                            
                            // Обновление информации о текущих выбранных элементах
                            GSCrmInfo.SetUpCurrentElements(event);

                            if (appletToUpdate != null && (recordToUpdate['Id'] != currentRecord['Id'] || columnType == "drilldown")) {
                                
                                // Вызов автообновления записи
                                GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, recordToUpdate, false)
                                    .catch(error => console.log(error))
                                    .then(() => GSCrmInfo.Application.CommonAction.Invoke("Drilldown", event, currentRecord));
                            }
                            
                            else GSCrmInfo.Application.CommonAction.Invoke("Drilldown", event, currentRecord, false);
                        });
                    });
            });
    };
    //#endregion

    //#region //@R Методы для работы со списком
   /**
    *@M Обновление строки в апплете из recordSet-а
    * @param {String} tileItem Строка в апплете
    */
    tileApplet.prototype.RefreshTileItem = function (tileItem) {
        tileItem.find('[data-type="cell"]').map((index, cell) => {
            let columnName = cell.getAttribute('data-name');
            let cellValue = this.GetRecordSet()[this.GetSelection(tileItem)][columnName];
            var PR = new DefaultTileAppletPR();
            PR.RenderCell(cell, cellValue, this.GetColumnProperties(columnName)[0]);
            return cell;
        });
    };

    /**
     *@M Обновление ячейки строки в апплете из recordSet-а
     * @param {String} tileItem Строка в апплете
     */
     tileApplet.prototype.RefreshCell = function (tileItem, columnName) {
         tileItem.find('[data-type="cell"]').filter('[data-name="' + columnName + '"]').map((index, cell) => {
             let cellValue = this.GetRecordSet()[this.GetSelection(tileItem)][columnName];
             var PR = new DefaultTileAppletPR();
             PR.RenderCell(cell, cellValue, this.GetColumnProperties(columnName)[0]);
             return cell;
         });
     };

    /**
    *@M Событие выбора записи из списка, обновляет информацию о представлении и его контекст
    * @param {Event} event Событие выбора записи
    */
    tileApplet.prototype.SelectTileItem = function (event) {
        return new Promise((resolve, reject) => {
            let tileItem = $(event.currentTarget).closest('[data-type="applet_item"]');
            if (tileItem.attr('disabled') == undefined) {
                let currentRecord = GSCrmInfo.GetSelectedRecord(this.Name);
    
                // В случае, если кликнули не по той же записи, обновление инфорамции о представлении и контекста
                if (currentRecord == undefined || currentRecord['record'][0] != tileItem[0]) {
                    $('[data-type="control"]').attr('disabled', true);
                    $('[data-type="applet_item"]').attr('disabled', true);
                    $('[data-type="ExpectationArea"]').removeClass('d-none');
    
                    // Обновление информации о представлении
                    let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                    let view = currentElementsInfo['View'];
                    let appletToUpdate = currentElementsInfo['AppletToUpdate'];
                    let recordToUpdate = currentElementsInfo['RecordToUpdate'];
                    let currentControl = currentElementsInfo['CurrentControl'];
                    let recordId = this.GetRecordFromRS(event)['Id'];
                    
                    let data = {
                        ActionType: 'SelectTileItem',
                        TargetApplet: this.Name,
                        CurrentRecord: recordId,
                        CurrentControl: currentControl,
                        OpenPopup: false,
                        ClosePopup: false,
                        PopupApplet: null,
                        CurrentPopupControl: null
                    }

                    /*  Если есть запись для обновления, то вначале надо обновить ее, и только затем информацию о представлении.
                        Если обновление записи не требуется, можно сразу обновить информацию о представлении.
                        Также добавлена проверка на то, что строка для обновления не должна быть текущей */
                    if (appletToUpdate != null && recordToUpdate['Id'] != this.GetRecordFromRS(event)['Id']) {
                        // Вызов автообновления записи
                        let errorMessage;
                        GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, recordToUpdate)
                            .catch(error => {
                                errorMessage = error;
                                reject(error);
                            })
                            .then(() => {
                                if (errorMessage == undefined) {
                                    // Фокусировка
                                    this.Focus(tileItem);
                                    view.UpdateViewInfo(data)
                                        .catch(error => reject(error))
                                        .then(() => {
                                            // Обновление контекста
                                            view.PartialUpdateContext(this.Info, false)
                                                .catch(error => reject(error))
                                                .then(() => {
                                                    $('[data-type="control"]').attr('disabled', false);
                                                    $('[data-type="applet_item"]').attr('disabled', false);
                                                    $('[data-type="ExpectationArea"]').addClass('d-none');
                                                    resolve();
                                                });
                                        });
                                }
                            });
                    }
                    else {
                        // Фокусировка
                        this.Focus(tileItem);
                        view.UpdateViewInfo(data)
                            .catch(error => reject(error))
                            .then(() => {
                                // Обновление контекста
                                view.PartialUpdateContext(this.Info, false)
                                    .catch(error => reject(error))
                                    .then(() => {
                                        $('[data-type="control"]').attr('disabled', false);
                                        $('[data-type="applet_item"]').attr('disabled', false);
                                        $('[data-type="ExpectationArea"]').addClass('d-none');
                                        resolve();
                                    });
                            });
                    }
                }
                else resolve();
            }
            else resolve();
        })
    };

    /**
    *@M Получение элемента апплета по его id из recordSet-а
    * @param {String} recordId Id записи из recordSet-а
    */
    tileApplet.prototype.GetTileItemByIdInRS = function (recordId) {
        let sequence = 0;
        this.GetRecordSet().forEach((item, index) => {
            if (item['Id'] == recordId)
                sequence = index;
        });
        return $('#' + this.Id).find('[data-type="applet_item"]').eq(sequence);
    };

    /**
    *@M Получение элемента апплета по его индексу
    * @param {Number} sequence Индекс элемента апплета
    */
    tileApplet.prototype.GetTileItemBySequence = function (sequence) {
        return $($('#' + this.Info['AppletId']).find('[data-type="applet_item"]')[sequence]);
    };

    /**
    *@M Получение selection-а элемента апплета
    * @param {HTMLElement} target Элемент, selection которого необходимо получить
    */
    tileApplet.prototype.GetSelection = function (target) {
        let index = 0;
        target
            .closest('[data-type="applet"]')
            .find('[data-type="applet_item"]')
            .filter((i, item) => {
                if ($(item)[0] == target.closest('[data-type="applet_item"]')[0])
                    index = i;
            });
        return index;
    };
    //#endregion

    //#region //@R Методы для работы с колонками
    /**
    *@M Получение выбранного свойства всех колонок по названию
    * @param {String} propertyName Название свойства
    */
    tileApplet.prototype.GetColumnsProperty = function (propertyName) {
        return this.Info['Columns'].map(column => column[propertyName]);
    };

    /**
    *@M Получение всех свойств выбранной колонки по названию
    * @param {String} columnName Название колонки
    */
    tileApplet.prototype.GetColumnProperties = function (columnName) {
        return this.Info['Columns'].filter(column => column['Name'] == columnName);
    };

    /**
    *@M Возвращает колонки у которых свойство popertyName равно popertyValue
    * @param {String} popertyName Название свойства
    * @param {String|Number|Boolean|null} popertyValue Значение свойства
    */
    tileApplet.prototype.GetColumnsByPropertyValue = function (popertyName, propertyValue) {
        return this.Info['Columns'].filter(column => column[popertyName] == propertyValue);
    };
    //#endregion

    //#region //@R Методы для работы с recordSet-ом
    /**
        *@M Возвращает свойство для выбранных записей во всех аплетах
        * @param {String} propertyName Название свойства для получения
        */
    tileApplet.prototype.GetSelectedRecordsProperty = function (propertyName) {
        let records = {};
        GSCrmInfo.SelectedRecords.forEach((item, applet) => {
            records[applet] = item['properties'][propertyName];
        });
        return records;
    };

    /**
        *@M Возвращает значение свойства по имени для выбранной записи в текущем апплете
        * @param {String} propertyName Название свойства для получения
        */
    tileApplet.prototype.GetSelectedRecordProperty = function (propertyName) {
        return GSCrmInfo.GetSelectedRecord(this.Name)['properties'][propertyName];
    };

    /**
        * Получает запись из recordSet-а по событию
        * @param {Event} event Событие выбора записи
        */
    tileApplet.prototype.GetRecordFromRS = function (event) {
        let tileItem = $(event.currentTarget).closest('[data-type="applet_item"]');
        return tileItem.length == 0 ? null : this.GetRecordSet()[this.GetSelection(tileItem)];
    };

    /**
        * Получает запись из recordSet-а по индексу
        * @param {Number} index Номер записи из recordSet-а
        */
    tileApplet.prototype.GetRecordFromRSByIndex = function (index) {
        return this.GetRecordSet().filter((item, i) => i == index)[0];
    }
    //#endregion

    return tileApplet;
}