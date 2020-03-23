class DefaultTileAppletPR {
    // Рендеринг апплета
    RenderApplet (appletName, displayLines) {
        // Получение апплета, информации о нем и необхдимых контролов
        var tileApplet = new TileApplet(appletName, null);
        var info = tileApplet.Info;
        var applet = $("#" + info['AppletId']);
        var columns = tileApplet.GetColumnsProperty('Name');
        var headers = tileApplet.GetColumnsProperty('Header');
        var newRecord = tileApplet.GetControlProperties('NewRecord');
        var appletConfg = tileApplet.GetControlProperties('AppletConfg');
        var updateRecord = tileApplet.GetControlProperties('UpdateRecord');
        var deleteRecord = tileApplet.GetControlProperties('DeleteRecord');
        var nextNav = tileApplet.GetControlProperties('NextRecords');
        var prevNav = tileApplet.GetControlProperties('PreviousRecords');
    
        info['class'] = 'def-tile';
        info['itemClass'] = 'def-tile-item';
        
        // Очистка апплета
        applet
            .find('.def-wrapper')
            .remove();
        applet
            .find('.nav-records')
            .remove();
    
        // Обработка событий
        applet
            .off('scroll')
            .off('OnAppletReady')
            .off('ClearAppletItemsFocus')
            .on('ClearAppletItemsFocus', event => {
                $(event.currentTarget)
                    .closest('[data-type="applet"]')
                    .find('.def-tile-item-static')
                    .removeClass('def-tile-item-static');
            })
            .on('scroll', event => {
                $(event.currentTarget)
                    .find('.header-items .row-actions')
                    .css('right', '0px')
            });
    
        // Если отсутствуют записи, добавление emptyState-а
        var body = $('<div class="def-tile-body"></div>');
        if (displayLines == 0) {
            applet.append('<div class="def-wrapper"><div class="applet-header"><p>' + info['Header'] + '</p></div><div class="def-tile-empty"><div class="def-tile-header">' +
                '<div class="row no-gutters header-items"></div></div></div></div>');
    
            // Добавление заголовков
            headers.forEach(item => {
                applet
                    .find('.header-items')
                    .append('<div class="col"><div class="block-center"><p class="label-fat-sm">' + item + '</p></div></div>');
            });
    
            // Добавление кнопок создания записи/конфигурации апплета
            applet
                .find('.header-items')
                .append('<div class="row-actions"><div class="row action-md"><div class="tile-create" data-type="control" data-name="' + newRecord['Name'] + '"></div>' +
                    '<div class="tile-confg" data-type="control" data-name="AppletConfg"></div></div></div>');
    
            // EmptyState
            applet
                .find('.def-tile-empty')
                .append('<div class="row def-tile-empty-state"><div class="col-auto"><img src="/img/default-empty.svg" /></div>' +
                    '<div class="col-auto"><div class="block-center"><h3>' + info['EmptyState'] + '</h3></div></div></div>');
        }
    
        else {
            applet.append('<div class="def-wrapper"><div class="applet-header"><p>' + info['Header'] + '</p></div><div class="def-tile"><div class="def-tile-header">' +
                '<div class="header-items"></div></div></div></div>')
    
            // Добавление заголовков
            headers.forEach(item => {
                applet
                    .find('.header-items')
                    .append('<div class="label-fat-sm">' + item + '</div>');
            });
    
            // Добавление кнопок создания записи/конфигурации апплета
            applet
                .find('.header-items')
                .append('<div class="row-actions"><div class="row action-md"><div class="tile-create" data-type="control" data-name="' + newRecord['Name'] + '"></div>' +
                    '<div class="tile-confg" data-type="control" data-name="AppletConfg"></div></div></div>');
    
            // Добавление строк
            for (var index = 0; index < displayLines; index++) {
    
                // Строка
                var appletItem = $('<div class="def-tile-item" data-type="applet_item"></div>');
    
                // Обработчики событий
                appletItem
                    .off('FocusAppletItem')
                    .off('AppletItemMouseOver')
                    .off('AppletItemMouseOut')
    
                    // При фокусировке на строке проставление классов
                    .on('FocusAppletItem', event => {
                        $(event.currentTarget)
                            .closest('[data-type="applet"]')
                            .find('.def-tile-item-static')
                            .removeClass('def-tile-item-static');
    
                        tileApplet.SelectedRecords[appletName]['tileItem'].addClass('def-tile-item-static');
                    })
                    .on('AppletItemMouseOver', event => $(event.currentTarget).addClass('def-tile-item-focused'))
                    .on('AppletItemMouseOut', event => $(event.currentTarget).removeClass('def-tile-item-focused'));
    
                body.append(appletItem);
    
                // Добавление колонок в строку
                columns.forEach(column => {
                    let $column = $('<div data-type="cell" data-name="' + column + '"></div>');
                    body
                        .find('[data-type="applet_item"]')
                        .last()
                        .append($column);
                })
    
                // Добавление кнопок удаления/редактирования записи
                body
                    .find('[data-type="applet_item"]')
                    .last()
                    .append('<div class="row-actions"><div class="row action-md"><div class="tile-edit" data-type="control" data-name="' + updateRecord['Name'] + '"></div>' +
                    '<div class="tile-delete" data-type="control" data-name="' + deleteRecord['Name'] + '"></div></div></div>');
            }
    
            applet
                .find('.def-tile')
                .append(body);
    
            // Навигация
            var footer = $('<div class="row nav-records">' +
                '<div class="col-auto"><div class="previous-nav"><div data-type="control" data-name="' + prevNav['Name'] + '"></div></div></div>' +
                '<div class="col-auto"><div class="next-nav"><div data-type="control" data-name="' + nextNav['Name'] + '"></div></div></div></div>');
    
            applet
                .find('.def-wrapper')
                .append(footer);
        }
    }

    // Ренедеринг ячеек тайла
    RenderCell(cell, cellValue, columnName, columnType) {
        switch (columnType) {
            case "checkbox":
                let el = $('<div class="field-checkbox-tile"><div class="checkbox-check"></div></div>');
                cellValue == true ? el.addClass('gs-field-is-filled') : el.removeClass('gs-field-is-filled');
                $(cell)
                    .empty()
                    .append(el)
                el
                    .off('click')
                    .on('click', event => {
                        if (el.hasClass('gs-field-is-filled')) {
                            el.removeClass('gs-field-is-filled');
                        }
                        else {
                            el.addClass('gs-field-is-filled');
                        }
                        $(cell).trigger("CellCnange", [{
                            Event: event,
                            CellNewValue: el.hasClass('gs-field-is-filled') ? true : false
                        }]);
                    });
                break;
            default:
                $(cell).text(cellValue);
                break;
        }
    }
}