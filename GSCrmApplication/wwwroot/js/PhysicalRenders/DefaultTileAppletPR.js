export default class DefaultTileAppletPR {
    // Рендеринг апплета
    RenderApplet (appletName, displayLines) {
        // Получение апплета, информации о нем и необхдимых контролов
        let info = GSCrmInfo.GetAppletInfo(appletName);
        let applet = new GSCrmInfo.Application.TileApplet(appletName, info['AppletId']);
        let $applet = $("#" + info['AppletId']);
        let columns = applet.GetColumnsProperty('Name');
        let headers = applet.GetColumnsProperty('Header');
        let newRecord = applet.GetControlProperties('NewRecord');
        let updateRecord = applet.GetControlProperties('UpdateRecord');
        let deleteRecord = applet.GetControlProperties('DeleteRecord');
        let copyRecord = applet.GetControlProperties('CopyRecord');
        let nextNav = applet.GetControlProperties('NextRecords');
        let prevNav = applet.GetControlProperties('PreviousRecords');
        
        // Очистка апплета
        $applet.find('.gs-wrapper').remove();
        $applet.find('.nav-records').remove();
    
        // Обработка событий
        $applet
            .addClass('gs-applet')
            .off('scroll')
            .off('OnAppletReady')
            .off('ClearAppletItemsFocus')
            .on('ClearAppletItemsFocus', event => {
                $(event.currentTarget)
                    .closest('[data-type="applet"]')
                    .find('.gs-tile-item-static')
                    .removeClass('gs-tile-item-static');
            })
            .on('scroll', event => {
                $(event.currentTarget).find('.header-items .row-actions').css('right', '0px')
            });

        // Получение ширины колонки с действиями в зависимости от их количества
        let counter = 3;
        let actionsCssClass = "";
        if (updateRecord != undefined) counter--;
        if (deleteRecord != undefined) counter--;
        if (copyRecord != undefined) counter--;
        switch(counter) {
            case 0:
                actionsCssClass = "row-actions-lg";
                break;
            case 1:
                actionsCssClass = "row-actions-md";
                break;
            case 2:
                actionsCssClass = "row-actions-sm";
                break;
            default:
                    break;
        }
    
        // Если отсутствуют записи, добавление emptyState-а
        let body = $('<div class="gs-tile-body"></div>');
        if (displayLines == 0) {
            $applet.append('<div class="gs-wrapper"><div class="applet-header"><p>' + info['Header'] + '</p></div><div class="gs-tile-empty"><div class="gs-tile-header">' +
                '<div class="row no-gutters header-items"></div></div></div></div>');
    
            // Добавление заголовков
            headers.forEach(item => {
                $applet.find('.header-items')
                    .append('<div class="col"><div class="block-center"><p class="label-fat-sm">' + item + '</p></div></div>');
            });
    
            // Добавление кнопки создания записи
            $applet.find('.header-items')
                .append('<div class="row row-actions ' + actionsCssClass + '"><div class="tile-create" data-type="control" data-name="' + newRecord['Name'] + '"></div></div>');
    
            // EmptyState
            $applet.find('.gs-tile-empty')
                .append('<div class="row gs-tile-empty-state"><div class="col-auto"><img src="/img/default-empty.svg" /></div>' +
                    '<div class="col-auto"><div class="block-center"><h3>' + info['EmptyState'] + '</h3></div></div></div>');
        }
    
        else {
            $applet.append('<div class="gs-wrapper"><div class="applet-header"><p>' + info['Header'] + '</p></div><div class="gs-tile"><div class="gs-tile-header">' +
                '<div class="header-items"></div></div></div></div>')
    
            // Добавление заголовков
            headers.forEach((item, headerIndex) => {
                headerIndex < headers.length - 1
                ? $applet.find('.header-items').append('<div class="label-fat-sm">' + item + '</div><div class="expand-column-btn"></div>')
                : $applet.find('.header-items').append('<div class="label-fat-sm">' + item + '</div>');
            });
            
            // Добавление кнопки создания записи
            $applet.find('.header-items')
                .append('<div class="row-actions ' + actionsCssClass + '"><div class="row justify-content-around">' +
                '<div class="tile-create" data-type="control" data-name="' + newRecord['Name'] + '"></div></div></div>');

            // Растягивание колонки
            $applet.find('.expand-column-btn').off('click').on('click', event => {
                let columnIndex = $(event.currentTarget).index();
                $applet.find('.gs-tile-body .gs-tile-item').each((index, item) => {
                    let children = $(item).children().eq(columnIndex - 1);
                    if (!$(children).hasClass('expand-column')) $(children).addClass('expand-column');
                    else $(children).removeClass('expand-column');
                })
            }); 

            // Добавление строк
            for (let index = 0; index < displayLines; index++) {
    
                // Строка
                let appletItem = $('<div class="gs-tile-item" data-type="applet_item"></div>');
    
                // Обработчики событий
                appletItem
                    .off('FocusAppletItem')
                    .off('AppletItemMouseOver')
                    .off('AppletItemMouseOut')
    
                    // При фокусировке на строке проставление классов
                    .on('FocusAppletItem', event => {
                        $(event.currentTarget)
                            .closest('[data-type="applet"]')
                            .find('.gs-tile-item-static')
                            .removeClass('gs-tile-item-static');
    
                            GSCrmInfo.GetSelectedRecord(appletName)['record'].addClass('gs-tile-item-static');
                    })
                    .on('AppletItemMouseOver', event => $(event.currentTarget).addClass('gs-tile-item-focused'))
                    .on('AppletItemMouseOut', event => $(event.currentTarget).removeClass('gs-tile-item-focused'));
    
                body.append(appletItem);
    
                // Добавление колонок в строку
                columns.forEach((column, index) => {
                    let $column;
                    index < columns.length - 1
                    ? $column = $('<div data-type="cell" data-name="' + column + '"></div><div></div>')
                    : $column = $('<div data-type="cell" data-name="' + column + '"></div>');
                    body.find('[data-type="applet_item"]').last().append($column);
                })
    
                // Добавление кнопок удаления/редактирования/копирования записи
                body.find('[data-type="applet_item"]').last().append('<div class="row-actions ' + actionsCssClass + '"><div class="row justify-content-around"></div></div>')
                if (updateRecord != undefined) {
                    body.find('[data-type="applet_item"]').last()
                        .find('.justify-content-around').append('<div class="tile-edit" data-type="control" data-name="' + updateRecord['Name'] + '"></div>');
                }
                if (deleteRecord != undefined) {
                    body.find('[data-type="applet_item"]').last()
                        .find('.justify-content-around').append('<div class="tile-delete" data-type="control" data-name="' + deleteRecord['Name'] + '"></div>');
                }
                if (copyRecord != undefined) {
                    body.find('[data-type="applet_item"]').last()
                        .find('.justify-content-around').append('<div class="tile-copy" data-type="control" data-name="' + copyRecord['Name'] + '"></div>');
                }
            }
    
            $applet.find('.gs-tile').append(body);
    
            // Навигация
            let footer = $('<div class="row nav-records">' +
                '<div class="col-auto"><div class="previous-nav"><div data-type="control" data-name="' + prevNav['Name'] + '"></div></div></div>' +
                '<div class="col-auto"><div class="next-nav"><div data-type="control" data-name="' + nextNav['Name'] + '"></div></div></div></div>');
    
            $applet.find('.gs-wrapper').append(footer);
        }
    }

    // Ренедеринг ячеек тайла
    RenderCell(cell, cellValue, properties) {
        let el;
        switch (properties['Type']) {
            case "checkbox":
                el = $('<div class="field-checkbox-tile"><div class="checkbox-check"></div></div>');
                if(properties['Readonly'])
                    el.addClass('gs-field-readonly');
                cellValue == true ? el.addClass('gs-field-is-filled') : el.removeClass('gs-field-is-filled');
                $(cell).empty().append(el);
                el.off('click').on('click', event => {
                    if (!properties['Readonly']) {
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
                    }
                });
                break;
            case "drilldown":
                if (!Object.is(cellValue, null)) {
                    el = $('<div class="field-drilldown-tile"><p>' + cellValue + '</p></div>');
                    if(properties['Readonly'])
                        el.addClass('gs-field-readonly');
                    el.find('p').off('click').on('click', event => {
                        $(cell).trigger("Drilldown", [{
                            Event: event
                        }]);
                    });
                    $(cell).empty().append(el);
                }
                break;
            default:
                $(cell).text(cellValue);
                break;
        }
    }
}