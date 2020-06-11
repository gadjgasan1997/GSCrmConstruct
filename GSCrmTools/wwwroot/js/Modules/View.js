import DefaultViewPR from '../PhysicalRenders/DefaultViewPR.js'

export default (function () {
    let view = function(name) {
        this.Name = name;
        this.Info = GSCrmInfo.GetViewInfo(name);
        if (this.Info != undefined)
            this.Id = this.Info['ViewId'];
    }

    //#region //@R Свойства
    // Получает информацию о представлении по его названию
    view.prototype.GetId = function() {
        return this.Id;
    }

    // Возвращает путь для представления
    view.prototype.GetName = function() {
        return this.Name;
    }

    view.prototype.GetInfo = function() {
        return this.Info;
    }
    //#endregion

    //#region //@R Инициализация
    // Инициализирует представление
    view.prototype.Initialize = function(screen) {
        return new Promise((resolve, reject) => {
        // Отображение анимации ожидания
        $('[data-type="ExpectationArea"]').removeClass('d-none');

        // Инициализация апплетов в представлении, заполненние информации о нем
        GSCrmInfo.Application.CommonRequests.InitializeView(this.Name)
            .fail(error => reject(error))
            .done(viewInfo => {
                // Рендеринг представления
                let PR = new DefaultViewPR();
                let view = PR.RenderView(this);

                // Проставление id и информации
                viewInfo['ViewId'] = $(view).attr('id');
                this.Id = $(view).attr('id');
                this.Info = viewInfo;

                // Очистка информации и старом представлении и установка информации о новом
                GSCrmInfo.RemoveOldViewInfo();
                GSCrmInfo.SetViewInfo(this.Name, viewInfo);

                // Добавление представления в экран и инициализация апплетов
                $(screen).append(view);
                GSCrmInfo.Application.CommonRequests.UpdateContext(this.Name)
                    .catch(error => reject(error))
                    .then(() => {
                        this.InitializeItems(view, viewInfo['ViewItems'], 0)
                            .catch(error => reject(error))
                            .then(() => {
                                $(screen).trigger('OnViewLoad', []);
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                resolve(this);
                            });
                    });
            });
        });
    };

    // Инициализирует элементы представления
    view.prototype.InitializeItems = function(view, applets, count) {
        return new Promise((resolve, reject) => {
            if (applets[count] != undefined) {
                let appletName = applets[count]['AppletName'];
                let appletId = "GSA_0";
                let counter = 0;

                // До тех пор, пока элемент с таким id присутствует на странице, прибавляю 1, чтобы Id был уникальным
                while ($('#' + appletId).length > 0) {
                    appletId = appletId.split(counter).join((counter + 1));
                    counter++;
                }

                switch (applets[count]['Type']) {
                    case "Tile":
                        // Добавление апплета в разметку
                        view.append('<div data-type="applet" data-name="' + appletName + '" id="' + appletId + '"></div>');

                        // Инициализация
                        let tileApplet = new GSCrmInfo.Application.TileApplet(appletName, appletId);
                        tileApplet.Initialize()
                            .catch(error => reject(error))
                            .then(() => {
                                count++;

                                // Инициализация оставшихся апплетов
                                this.InitializeItems(view, applets, count)
                                    .catch(error => reject(error))
                                    .then(() => resolve());
                            });
                        break;

                    case "Form":
                        // Добавление апплета в разметку
                        view.append('<div data-type="applet" data-name="' + appletName + '" id="' + appletId + '"></div>');

                        // Инициализация
                        let formApplet = new GSCrmInfo.Application.FormApplet(appletName, appletId);
                        formApplet.Initialize()
                            .catch(error => reject(error))
                            .then(() => {
                                count++;

                                // Инициализация оставшихся апплетов
                                this.InitializeItems(view, applets, count)
                                    .catch(error => reject(error))
                                    .then(() => resolve());
                            });
                        break;
                }
            }
            else resolve();
        })
    };
    //#endregion

    //#region //@R Методы для обновления информации и контекста представления
    // Запрашивает информацию о представлении с бека
    view.prototype.RequestInfo = function() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetViewInfo()
                .fail(error => reject(error))
                .done(info => {
                    GSCrmInfo.RemoveOldViewInfo();
                    GSCrmInfo.SetViewInfo(this.Name, info);
                    resolve();
                });
        });
    };

    // Обновляет инормацию о представлении
    view.prototype.UpdateViewInfo = function(data) {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.UpdateViewInfo(data)
                .fail(error => reject(error))
                .done(info => {
                    GSCrmInfo.RemoveOldViewInfo();
                    GSCrmInfo.SetViewInfo(this.Name, info);
                    resolve();
                });
        });
    };

    // Полное обновление контекста
    view.prototype.UpdateContext = function() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.UpdateContext(this.Name)
                .fail(error => reject(error))
                .done(applets => {
                    if (applets.length > 0) {
                        this.RefreshApplets(applets, 0)
                            .catch(error => reject(error))
                            .then(() => resolve());
                    }
                    else resolve();
                });
        });
    };

    // Частичное обновление контекста
    view.prototype.PartialUpdateContext = function(applet, refreshCurrentApplet) {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.PartialUpdateContext(applet, refreshCurrentApplet)
                .fail(error => reject(error))
                .done(applets => {
                    if (applets.length > 0) {
                        this.RefreshApplets(applets, 0)
                            .catch(error => reject(error))
                            .then(() => resolve());
                    }
                    else resolve();
                });
        });
    };

    // Обновление контекста для списка апплетов
    view.prototype.RefreshApplets = function(applets, count) {
        return new Promise((resolve, reject) => {
            if (applets[count] != undefined) {
                let currentApplet;
                let appletName = applets[count];
                switch (this.GetAppletByName(appletName)['Type']) {
                    case "Tile":
                        currentApplet = new GSCrmInfo.Application.TileApplet(appletName, null);
                        break;
                    case "Form":
                        currentApplet = new GSCrmInfo.Application.FormApplet(appletName, null);
                        break;
                }
                currentApplet.Initialize()
                    .catch(error => reject(error))
                    .then(() => {
                        count++;
                        this.RefreshApplets(applets, count);
                        resolve();
                    });
            }
        });
    };

    // Обновление контекста для одного апплета
    view.prototype.RefreshApplet = function(appletName) {
        return new Promise((resolve, reject) => {
            let currentApplet;
            switch (this.GetAppletByName(appletName)['Type']) {
                case "Tile":
                    currentApplet = new GSCrmInfo.Application.TileApplet(appletName, null);
                    break;
                case "Form":
                    currentApplet = new GSCrmInfo.Application.FormApplet(appletName, null);
                    break;
            }
            currentApplet.Initialize()
                .catch(error => reject(error))
                .then(() => {
                    count++;
                    this.RefreshApplets(applets, count);
                    resolve();
                });
        });
    };

    // Обновление отображения апплетов
    view.prototype.RefreshViewAppletsUI = function(appletToUpdate, newRecord) {
        let appletsToUpdate = this.GetRelatedApplets(appletToUpdate['Name']);
        let appletToUpdateType = appletToUpdate.GetInfo()['Type'];
        appletsToUpdate.push(appletToUpdate);
        appletsToUpdate.map(currentApplet => {
            let currentAppletType = currentApplet.GetInfo()['Type'];
            switch(currentAppletType) {
                case "Tile":
                    // Обновление текущего апплета из newRecord
                    if (currentApplet['Name'] == appletToUpdate['Name'])
                        currentApplet.UpdateRecordInRS(newRecord, "Id", newRecord['Id']);

                    // Для остальных апплетов необходимо получить их recordSet И обновить их
                    else {
                        let changedRecord = currentApplet.GetRecordByProperty("Id", newRecord[["Id"]])[0];
                        if (changedRecord != undefined) {
                            // Перебираются все свойства, в новой записи
                            for (let property in newRecord) {
                                // Получение элементов(контролов или колонок) апплета, необходимых для обновления по названию свойства из recordSet-а
                                let updatedAppletElements = [];
                                if (appletToUpdateType == "Tile") {
                                    updatedAppletElements = appletToUpdate.GetColumnsByPropertyValue("Name", property);
                                }
                                else {
                                    updatedAppletElements = appletToUpdate.GetControlsByPropertyValue("Name", property);
                                }
                                updatedAppletElements.map(element => {
                                    // Проверка на undefined, чтобы исключить не отображаемые свойства
                                    if (element != undefined) {
                                        let fieldName = element['FieldName'];
                                        // Если информация для этого элемента есть в информации об апплете и его филда не пустая
                                        if (fieldName != null && fieldName != "") {
                                            // В текущем апплете ищутся колони, основанные на этой филде, и их значения в новой формируемой записи заменяются новым значением
                                            let appletColumns = currentApplet.GetColumnsByPropertyValue("FieldName", fieldName);
                                            appletColumns.map(appletColumn => {
                                                if (appletColumn != undefined) {
                                                    changedRecord[appletColumn['Name']] = newRecord[property];
                                                }
                                            })
                                        }
                                    }
                                })
                            }
                            currentApplet.UpdateRecordInRS(changedRecord, "Id", changedRecord['Id']);
                        }
                    }
                    //console.log(currentApplet.GetRecordByProperty("Id", newRecord["Id"])[0])
                    currentApplet.RefreshTileItem(currentApplet.GetTileItemByIdInRS(newRecord['Id']));
                    break;
                case "Popup":
                case "Form":
                    // Обновление текущего апплета из newRecord
                    if (currentApplet['Name'] == appletToUpdate['Name'])
                        currentApplet.UpdateRecordInRS(newRecord);

                    // Для остальных апплетов необходимо получить их recordSet И обновить их
                    else {
                        let changedRecord = currentApplet.GetRecordSet();
                        if (changedRecord != undefined) {
                            // Перебираются все свойства, в новой записи
                            for (let property in newRecord) {
                                // Получение элементов(контролов или колонок) апплета, необходимых для обновления по названию свойства из recordSet-а
                                let updatedAppletElements;
                                if (appletToUpdateType == "Tile") {
                                    updatedAppletElements = appletToUpdate.GetColumnsByPropertyValue("Name", property);
                                }
                                else {
                                    updatedAppletElements = appletToUpdate.GetControlsByPropertyValue("Name", property);
                                }
                                updatedAppletElements.map(element => {
                                    if (element != undefined) {
                                        // Проверка на undefined, чтобы исключить не отображаемые свойства
                                        let fieldName = element['FieldName'];
                                        // Если информация для этого элемента есть в информации об апплете и его филда не пустая
                                        if (fieldName != null && fieldName != "") {
                                            // В текущем апплете ищутся контролы, основанные на этой филде, и их значения в новой формируемой записи заменяются новым значением
                                            let appletControls = currentApplet.GetControlsByPropertyValue("FieldName", fieldName);
                                            appletControls.map(appletControl => {
                                                if (appletControl != undefined) {
                                                    changedRecord[appletControl['Name']] = newRecord[property];
                                                }
                                            })
                                        }
                                    }
                                })
                            }
                            currentApplet.UpdateRecordInRS(changedRecord, "Id", changedRecord['Id']);
                        }
                    }
                    //console.log(currentApplet.GetRecordSet())
                    currentApplet.RefreshControls();
                    break;
            }
        });
    }

    // Обновление отображения контролов в связанных апплетах
    view.prototype.RefreshAppletControlsUI = function(appletToUpdate, fieldsToUpdate, record) {
        let appletsToUpdate = this.GetRelatedApplets(appletToUpdate['Name']);
        appletsToUpdate.push(appletToUpdate);
        appletsToUpdate.map(currentApplet => {
            let changedRecord;
            let currentAppletType = currentApplet.GetInfo()['Type'];
            switch(currentAppletType) {
                case "Tile":
                    let columnsToUpdate = [];
                    changedRecord = currentApplet.GetRecordByProperty("Id", record["Id"])[0];
                    if (changedRecord != undefined) {
                        fieldsToUpdate.map(field => {
                            columnsToUpdate.push(currentApplet.GetColumnsByPropertyValue("FieldName", field).filter(item => item != undefined));
                            columnsToUpdate.map(columnToUpdate => {
                                if (columnToUpdate[0] != undefined) {
                                    changedRecord[columnToUpdate[0]['Name']] = record[columnToUpdate[0]['Name']];
                                }
                            })
                        });
                        currentApplet.UpdateRecordInRS(changedRecord, "Id", changedRecord['Id']);
                        columnsToUpdate.map(columnToUpdate => {
                            if (columnToUpdate[0] != undefined) {
                                currentApplet.RefreshCell(currentApplet.GetTileItemByIdInRS(changedRecord['Id']), columnToUpdate[0]['Name']);
                            }
                        });
                    }
                    break;
                case "Form":
                case "Popup":
                    let controlsToUpdate = [];
                    changedRecord = currentApplet.GetRecordSet();
                    if (changedRecord != undefined) {
                        fieldsToUpdate.map(field => {
                            controlsToUpdate.push(currentApplet.GetControlsByPropertyValue("FieldName", field).filter(item => item != undefined));
                            controlsToUpdate.map(controlToUpdate => {
                                if (controlToUpdate[0] != undefined) {
                                    changedRecord[controlToUpdate[0]['Name']] = record[controlToUpdate[0]['Name']];
                                }
                            })
                        });
                        currentApplet.UpdateRecordInRS(changedRecord, "Id", changedRecord['Id']);
                        controlsToUpdate.map(controlToUpdate => {
                            if (controlToUpdate[0] != undefined) {
                                currentApplet.RefreshControl(controlToUpdate[0]['Name']);
                            }
                        });
                    }
                    break;
            }
        });
    }
    //#endregion

    //#region //@R Методы для работы с апплетам
    // Возвращает id апплета, внутри которого произошло событие
    view.prototype.GetAppletId = function(event) {
        return $(event.currentTarget).closest('[data-type="applet"]').attr('id');
    };

    // По событию на апплете возвращает его название
    view.prototype.GetAppletName = function(event) {
        return this.GetAppletNameById(this.GetAppletId(event));
    };

    // По названию апплета возвращает его id
    view.prototype.GetAppletIdByName = function(appletName) {
        let appletId;
        GSCrmInfo.AppletsInfo.forEach((applet, index) => {
            if (index == appletName) {
                appletId = applet['AppletId'];
            }
        });
        return appletId;
    };

    // По id апплета возвращает его название
    view.prototype.GetAppletNameById = function(appletId) {
        let appletName;
        GSCrmInfo.AppletsInfo.forEach((applet, index) => {
            if (applet['AppletId'] == appletId) {
                appletName = index;
            }
        });
        return appletName;
    };

    // По названию апплета возвращает информацию о нем из представления
    view.prototype.GetAppletByName = function(appletName) {
        return GSCrmInfo['ViewsInfo'].get(this.Name)['ViewItems'].filter(n => n['AppletName'] == appletName)[0];
    };

    // Вовзращает список связанных апплетов
    view.prototype.GetRelatedApplets = function(appletName) {
        let relatedApplets = [];
        let appletInfo = GSCrmInfo.GetAppletInfo(appletName);
        GSCrmInfo.AppletsInfo.forEach(applet => {
            if (applet['BusCompName'] == appletInfo['BusCompName'] && applet['Name'] != appletName && applet['Initflag'] == false)
            {
                switch(applet['Type']) {
                    case "Tile":
                        relatedApplets.push(new GSCrmInfo.Application.TileApplet(applet['Name'], applet['Id']));
                        break;
                    case "Form":
                        relatedApplets.push(new GSCrmInfo.Application.FormApplet(applet['Name'], applet['Id']));
                        break;
                    case "Popup":
                        relatedApplets.push(new GSCrmInfo.Application.PopupApplet(applet['Name'], applet['Id']));
                        break;
                }
            }
        });
        return relatedApplets;
    }

    // Возвращает id цели, с которой был открыт текущий апплет/область с выбором
    view.prototype.GetTargetId = function(event) {
        return $(event.currentTarget).closest('[data-target-id]').attr('data-target-id');
    };

    // Закрытие зоны с выбором(выпадушки на пиклисте и календаря)
    view.prototype.CloseSelectArea = function() {
        // Элемент, с которого произошло открытие зоны
        let el = $("#" + $('[data-type="SelectArea"]').attr('data-target-id'));
        el.removeClass('gs-field-is-focused');
        if (el.find('.gs-field-input').val() != '') {
            el.addClass('gs-field-is-filled')
        }
        else {
            el.removeClass('gs-field-is-filled')
        }

        // Установка значения для элемента в recordSet-е
        let appletName = this.GetAppletNameById(el.closest('[data-type="applet"]').attr('id'));
        let applet = new GSCrmInfo.Application.Applet(appletName, null);
        applet.RecordSet[el.attr('data-name')] = el.find('.gs-field-input').val();

        // Закрытие зоны с выбором
        $('[data-type="SelectArea"]')
            .addClass('d-none')
            .removeAttr('style')
            .removeAttr('data-target-id')
            .empty();

        // Возврат стрелки и цвета пиклиста в стандартное положение
        $('.pick-area')
            .css('color', 'lightgrey')
            .find('.icon-chevron-thin-left')
            .removeClass('icon-chevron-thin-left')
            .addClass('icon-chevron-thin-right');
    }
    //#endregion

    return view;
})()