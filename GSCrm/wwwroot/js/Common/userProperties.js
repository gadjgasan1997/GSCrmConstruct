var SetUP = elements => {

    // Для полей с UP
    $(elements)[0].querySelectorAll('[data-field-up]').forEach(el => {
        var up = el.attributes["data-field-up"].value;

        // input
        var fieldInput = $(el).find('.gs-field-input')[0];

        // Прорисовка полей
        OnFieldLoad(fieldInput, up);

        // Обработчики
        $(el)
            .off('input')
            .off('keydown')
            .on('keydown', e => {
                PreSetFieldValue(e, fieldInput, up);
            })
            .on('input', () => {
                CheckFieldValue(fieldInput, up);
            })
    })
}

// Прорисовка полей
var OnFieldLoad = (el, up) => {
    // Поле
    var field = $(el).closest('.gs-field');
    
    // Up для обязательности поля
    if (up.indexOf('Required') != -1) {
        var fieldLabel = field.find('.gs-field-label p');
        fieldLabel.text(fieldLabel.text() + " *");
    }

    // Up для ошибки под полем
    if (up.indexOf('ErrorUnderField') != - 1) {
        var wrapper = $('<div class="row mt-4 d-none"><p id="' + field.attr('id') + '_err" class="error-message"></p></div>');
        wrapper.insertAfter($(field))
    }
}

// Проверка поля перед вводом символа
var PreSetFieldValue = (e, el, up) => {
    if (up.indexOf("NotInput") != -1) {
        if (e.key != "Backspace") {
            e.preventDefault();
        }
        else {
            el.value = '';
        }
    }
}

// Проверка поля после ввода символа
var CheckFieldValue = (el, up) => {
    if (up.indexOf("SystemField") != -1) {
        el.value = el.value.replace(/[^\w]/, '');
    }
    if (up.indexOf("NumberField") != -1) {
        el.value = el.value.replace(/[^0-9]/, '');
    }
}

var isNumeric = n => !isNaN(parseFloat(n)) && isFinite(n);
