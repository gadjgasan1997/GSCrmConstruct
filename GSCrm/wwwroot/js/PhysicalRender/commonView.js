class View {
    constructor(name) {
        this.Name = name;
    }

    //#region //@R Свойства
    // Получает информацию о представлении по его названию
    get Info() {
        return Info.GetViewInfo(this.Name);
    }

    // Возвращает путь для представления
    get Path() {
    return Info.ScreenInfo['Routing'][this.Name];
    }
    //#endregion
    
    //#region //@R Инициализация
    // Инициализирует представление
    Initialize() {
        return new Promise((resolve, reject) => {
            // Очистка контейнера
            $('.large-container main').empty();

            // Представление
            var $view = $('<div data-type="view" data-name="' + this.Name + '"></div>');

            // Id представления
            var viewId = "GSV_0";
            var counter = 0;

            // До тех пор, пока элемент с таким id присутствует на странице, прибавляю 1, чтобы Id был уникальным
            while ($('#' + viewId).length > 0) {
                viewId = viewId.split(counter).join((counter + 1));
                counter++;
            }

            // Проставление id представлению
            this.Id = viewId;
            $view.attr('id', this.Id);

            // Добавление представления в разметку
            $('.large-container main').append($view);

            // Отображение анимации ожидания
            $('.cssload-wrapper').removeClass('d-none');

            // Инициализация апплетов в представлении, заполненние информации о нем
            var request = new RequestsToDB;
            
            request.InitializeView(this)
                .fail(error => reject(error))
                .done(info => {
                    info['ViewId'] = viewId;
                    Info.RemoveOldViewInfo();
                    Info.SetViewInfo(this.Name, info);

                    var count = 0;
                    this.InitializeItems($view, info['ViewItems'], count)
                        .catch(error => reject(error))
                        .then(() => resolve());
                });
        });
    }

    // Инициализирует элементы представления
    InitializeItems($view, applets, count) {
        return new Promise((resolve, reject) => {
            if (applets[count] != undefined) {
                var appletName = applets[count]['AppletName'];
                var appletId = "GSA_0";
                var counter = 0;
    
                // До тех пор, пока элемент с таким id присутствует на странице, прибавляю 1, чтобы Id был уникальным
                while ($('#' + appletId).length > 0) {
                    appletId = appletId.split(counter).join((counter + 1));
                    counter++;
                }
    
                switch (applets[count]['Type']) {
                    case "Tile":
                        // Добавляю апплет в разметку
                        $view.append('<div data-type="applet" data-name="' + appletName + '" id="' + appletId + '"></div>');
    
                        var tileApplet = new TileApplet(appletName, appletId);
                        // Инициализирую его
                        tileApplet.Initialize()
                            .catch(error => reject(error))
                            .then(() => {
                                count++;
    
                                // Инициализирую оставшиеся апплеты
                                this.InitializeItems($view, applets, count)
                                    .catch(error => reject(error))
                                    .then(() => resolve());
                            });
                        break;
    
                    default:
                        count++;
                        this.InitializeItems($view, applets, count)
                            .catch(error => reject(error))
                            .then(() => resolve());
                        break;
                }
            }
            else this.UpdateContext()
                .catch(error => reject(error))
                .then(() => resolve())
                .finally(() => {
                    // Скрытие анимации ожидания
                    $('.cssload-wrapper').addClass('d-none');
                });
        })
    }
    //#endregion

    //#region //@R Методы для обновления информации и контекста представления
    // Запрашивает информацию о представлении с бека
    RequestInfo() {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.GetViewInfo(this)
                .fail(error => reject(error))
                .done(info => {
                    Info.RemoveOldViewInfo();
                    Info.SetViewInfo(this.Name, info);
                    resolve();
                });
        });
    }

    // Обновляет инормацию о представлении
    UpdateViewInfo(data) {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.UpdateViewInfo(this, data)
                .fail(error => reject(error))
                .done(info => {
                    Info.RemoveOldViewInfo();
                    Info.SetViewInfo(this.Name, info);
                    resolve();
                });
        });
    }

    // Полное обновление контекста
    UpdateContext() {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.UpdateContext(this)
                .fail(error => reject(error))
                .done(applets => {
                    if (applets.length > 0) {
                        var count = 0;
                        this.RefreshApplets(applets, count)
                            .catch(error => reject(error))
                            .then(() => resolve());
                    }
                    else resolve();
                });
        });
    }

    // Частичное обновление контекста
    PartialUpdateContext(applet, recordId, refreshCurrentApplet) {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.PartialUpdateContext(applet, recordId, refreshCurrentApplet)
                .fail(error => reject(error))
                .done(applets => {
                    if (applets.length > 0) {
                        var count = 0;
                        this.RefreshApplets(applets, count)
                            .catch(error => reject(error))
                            .then(() => resolve());
                    }
                    else resolve();
                });
        });
    }

    // Обновление контекста для списка апплетов
    RefreshApplets(applets, count) {
        return new Promise((resolve, reject) => {
            if (applets[count] != undefined) {
                let applet = new Applet(applets[count], null);
                switch(applet.Info['Type']) {
                    case "Tile":
                        let tileApplet = new TileApplet(applet.Name);
                        tileApplet.GetRecords()
                            .catch(error => reject(error))
                            .then(() => {
                                count++;
                                this.RefreshApplets(applets, count);
                                resolve();
                            });
                        break;
                }
            }
        });
    }

    // Обновление контекста для одного апплета
    RefreshApplet(appletName) {
        return new Promise((resolve, reject) => {
            var tileApplet = new TileApplet(appletName);
            tileApplet.GetRecords()
                .catch(error => reject(error))
                .then(() => resolve());
        });
    }
    //#endregion

    //#region //@R Методы для работы с апплетам
    // Возвращает id апплета, внутри которого произошло событие
    GetAppletId(event) {
        return $(event.currentTarget).closest('[data-type="applet"]').attr('id');
    }

    // По событию на апплете возвращает его название
    GetAppletName(event) {
        return this.GetAppletNameById(this.GetAppletId(event));
    }

    // По названию апплета возвращает его id
    GetAppletIdByName(appletName) {
        var appletId;
        Info.AppletsInfo.forEach((applet, index) => {
            if (index == appletName) {
                appletId = applet['AppletId'];
            }
        });
        return appletId;
    }

    // По id апплета возвращает его название
    GetAppletNameById(appletId) {
        var appletName;
        Info.AppletsInfo.forEach((applet, index) => {
            if (applet['AppletId'] == appletId) {
                appletName = index;
            }
        });
        return appletName;
    }

    // Возвращает id цели, с которой был открыт текущий апплет/область с выбором
    GetTargetId(event) {
        return $(event.currentTarget).closest('[data-target-id]').attr('data-target-id');
    }

    // Закрытие зоны с выбором(выпадушки на пиклисте и календаря)
    CloseSelectArea() {
        // Элемент, с которого произошло открытие зоны
        let el = $("#" + $('.select-area').attr('data-target-id'));
        el.removeClass('gs-field-is-focused');
        if (el.find('.gs-field-input').val() != '') {
            el.addClass('gs-field-is-filled')
        }
        else {
            el.removeClass('gs-field-is-filled')
        }

        // Установка значения для элемента в recordSet-е
        let appletName = GetAppletNameById(el.closest('[data-type="applet"]').attr('id'));
        let applet = new Applet(appletName);
        applet.RecordSet[el.attr('data-name')] = el.find('.gs-field-input').val();

        // Закрытие зоны с выбором
        $('.select-area')
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
}

// Получает представление, внутри которого произошло событие
var GetActiveView = event => {
    return $(event.currentTarget).closest('[data-type="view"]').attr('data-name');
}