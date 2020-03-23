// Инициализирует поля в апплете, запоняет их значениями
var InitializeFields = appletName => {
    var appletId = AppletInfo(appletName)['AppletId'];
    $('#' + appletId)[0].querySelectorAll('[data-type="field"]').forEach(field => {
        var fieldName = $(field).attr('data-name');
        var properties = GetControlProperties(appletName, fieldName);
        var header = properties['Header'];

        // Формирую id
        var counter = 0;
        var fieldId = 'field_' + '_' + properties['RecordName'] + '_' + counter;

        // До тех пор, пока такой id еще есть в документе, прибавляю 1, чтобы id  был уникальным. Костыль, в дальнейшем лучше заменить на что-то нормальное
        while ($('#' + fieldId).length > 0) {
            fieldId = fieldId.split(counter).join((counter + 1));
            counter++;
        }

        // Проставление класса и атрибутов
        $(field)
            .addClass('gs-field')
            .attr('id', fieldId)
            .attr('data-field-type', properties['Type'])
            .attr('data-field-label', header)
            .attr('data-field-up', properties['UP']);

        // Проставление up required
        if (properties['required']) {
            $(field).attr('data-field-up', $(field).attr('data-field-up') + ';Required');
        }

        switch (properties['Type']) {

            // Инициализация полей для ввода
            case "input":

                // Если поле ограничено по длине
                var maxlength = properties['maxlength'];
                if (isNumeric(maxlength)) {
                    $(field).append('<div class="gs-field-label"><p>' + header + '</p><span class="gs-field-counter d-none">0/' +
                        maxlength + '</span></div><input class="gs-field-input" maxlength="' + maxlength + '" />');
                }

                // Если нет
                else {
                    $(field).append('<div class="gs-field-label"><p>' + header + '</p></div><input class="gs-field-input" />');
                }

                break;

            // Инциализация полей с типом дата, добавление класса, проставление up NotInput
            case "date":
                $(field)
                    .addClass('field-date')
                    .attr('data-field-up', $(field).attr('data-field-up') + ';NotInput')
                    .append('<div class="gs-field-label"><p>' + header + '</p></div><input class="gs-field-input" />' +
                        '<div class="pick-area"><div class="pick-icon"><span class="icon-calendar" aria-hidden="true"></span></div></div>');
                break;

            // Инициализация полей с типом пиклиста
            case "picklist":
                $(field)
                    .addClass('field-picklist')
                    .attr('data-field-up', $(field).attr('data-field-up') + ';NotInput')
                    .append('<div class="gs-field-label"><p>' + header + '</p></div><input class="gs-field-input" />' +
                        '<div class="pick-area"><div class="pick-icon"><span class="icon-chevron-thin-right" aria-hidden="true"></span></div></div>');
                break;

            // Инициализация полей с типом checkbox
            case "checkbox":
                console.log(":wqerty")
                $(field)
                    .removeClass('gs-field')
                    .addClass('field-checkbox');
                break;
        }
    });

    // Установка настроек
    SetUP('#' + appletId);
}

// Скрытие выпадушки и снятие фокусировки с элемента, с которого она была вызвана, обновление RecordSet-а
var CloseSelectArea = () => {
    // Убираю фокус с других элементов, проставление класса
    var el = $("#" + $('.select-area').attr('data-target-id'));
    el.removeClass('gs-field-is-focused');
    if (el.find('.gs-field-input').val() != '') {
        el.addClass('gs-field-is-filled')
    }
    else {
        el.removeClass('gs-field-is-filled')
    }

    // Обновление recordSet-а
    var appletName = GetAppletNameById(el.closest('[data-type="applet"]').attr('id'));
    RecordSet(appletName).set(el.attr('data-name'), el.find('.gs-field-input').val());

    // Скрытие выпадушки
    $('.select-area')
        .addClass('d-none')
        .removeAttr('style')
        .removeAttr('data-target-id')
        .empty();

    // Возврат стрелки пиклиста в прежнее положение
    $('.pick-area')
        .find('.icon-chevron-thin-left')
        .removeClass('icon-chevron-thin-left')
        .addClass('icon-chevron-thin-right');

    // Стили кнопки открывающей выпадшуку
    $('.pick-area').css('color', 'lightgrey');
}

// При нажатии на поле
$(document).on('focus', '.gs-field', e => {
    var target = e.currentTarget;
    $(target).addClass('gs-field-is-focused');
    $(target)
        .find('.gs-field-counter')
        .removeClass('d-none');
    $(e.target)
        .find('.gs-field-input')
        .focus();
    $(e.target)
        .find('.icon-chevron-thin-right')
        .removeClass('icon-chevron-thin-right')
        .addClass('icon-chevron-thin-left');
})

// При фокусировке на поле с датой, формирую календарь
$(document).on('click', '.field-date', e => {
    e.stopPropagation();
    var field = $(e.currentTarget)[0];

    // Убираю фокус с других элементов, проставление класса
    document.querySelectorAll('[data-type="field"]').forEach(item => {
        if (field != $(item)[0]) {
            $(item).removeClass('gs-field-is-focused');
        }
        if ($(item).find('.gs-field-input').val() != '') {
            $(item).addClass('gs-field-is-filled')
        }
        else {
            $(item).removeClass('gs-field-is-filled')
        }
    })

    // Проверка что не кликнули по одному и тому же полю
    if ($('.select-area').attr('data-target-id') != $(e.currentTarget).attr('id')) {
        CreateCalendar(field, e);
    }
})

// При фокусировке на поле с пиклистом, формирую выпадушку
$(document).on('click', '.field-picklist', e => {
    e.stopPropagation();
    var field = $(e.currentTarget)[0];

    // Убираю фокус с других элементов, проставление класса
    document.querySelectorAll('[data-type="field"]').forEach(item => {
        if (field != $(item)[0]) {
            $(item).removeClass('gs-field-is-focused');

            // Возврат стрелки пиклиста в прежнее положение
            $(item)
                .find('.icon-chevron-thin-left')
                .removeClass('icon-chevron-thin-left')
                .addClass('icon-chevron-thin-right');
        }
        if ($(item).find('.gs-field-input').val() != '') {
            $(item).addClass('gs-field-is-filled');
        }
        else {
            $(item).removeClass('gs-field-is-filled');
        }
    })

    // Проверка что не кликнули по одному и тому же полю
    if ($('.select-area').attr('data-target-id') != $(e.currentTarget).attr('id')) {
        CreatePickList($(e.currentTarget), e);
    }
})

// Проверка на длину при вводе в поле
$(document).on('input', '.gs-field', e => {
    // Цель
    var target = e.currentTarget;

    // Поле
    var gsField = $(target).closest('.gs-field');

    // Счетчик
    var text = '';
    var counter = $(target).find('.gs-field-counter');
    if (counter[0] != undefined) {
        text = counter[0].innerText.split('/');
    }

    // Ввод
    var input = $(target).find('.gs-field-input');
    var value = input.val();
    if (counter[0] !== undefined) {
        counter[0].innerText = value.length + '/' + text[1];
    }

    // Если ввели максимальное количество символов
    if (value.length >= text[1]) {
        gsField.addClass('gs-field-error');
        setTimeout(() => {
            gsField.removeClass('gs-field-error');
        }, 2000)
    }
})

// При уходе с поля установка, удаление классов, обновление RecordSet-а
$(document).on('blur', '.gs-field', e => {
    // Цель, поле для ввода, значение, текущий апплет
    var $target = $(e.currentTarget);
    var $input = $target.find('.gs-field-input');
    var value = $input.val();

    // Обновление RecordSet-а
    RecordSet(GetAppletName(e)).set($target.attr('data-name'), value);

    // Если тип поля - input
    if ($target.attr('data-field-type') == "input") {
        $target.removeClass('gs-field-is-focused');

        // Если поле пустое убираем класс, скрываем счетчик
        if (value === '') {
            $target
                .removeClass('gs-field-is-filled')
                .find('.gs-field-counter')
                .addClass('d-none');
        }

        // Если значение в поле не пустое добаляем класс
        else {
            $target
                .closest('.gs-field')
                .addClass('gs-field-is-filled');
        }
    }
})

// Останавливаю всплытие при нажатии на выпадушку
$(document).on('click', '.select-area', e => e.stopPropagation());

// При клике по документу закрываю календарь
$(document).on('click', () => {
    if (!$('.select-area').hasClass('d-none')) {
        CloseSelectArea();
    }
})