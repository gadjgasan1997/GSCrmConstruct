/* Инициализирует форм апплет, получая на вход:
 * - название представления
 * - название апплета */
var InitializeFormApplet = (viewName, appletName, recordId) => {
    $.getJSON('/confgs/AppletsDefinition/appletsDefinition.json').then(json => {
        var data = json[viewName]['AppletList'][appletName];

        // Инициализация объекта с информацией об апплете
        var appletId = appletName.split(' ').join('_');
        var info = new Map();
        var fields = data.fields.map(field => field);
        var controls = data.controls.map(control => control);
        info.set('PR', data.PR);
        info.set('fields', fields);
        info.set('controls', controls);
        info.set('payload', data.payload);
        appletInfo.set(appletName, info);

        // Установка recordSet-а
        var rs = new Map();
        recordSet.set(appletName, rs);

        // Добавление recordSet-а
        fields.forEach(field => rs.set(field['name'], ''));

        if (!Object.is(recordId, null)) {
            rs.set('Id', recordId);
        }

        // Получение PR апплета
        var PR = GetPR(appletName);

        // Подключение скриптов
        if (data.PR != '') {
            //$('body').append('<script src="/js/AppletsPR/' + PR + '.js"></script>');
        }

        if (fields.length != 0) {
            //$('body').append('<script src="/js/Blocks/fields.js"></script>');
        }

        // Создание апплета
        window[PR](appletName);

        // Инициализация полей
        InitializeFields(appletName);

        // Инициализация контролов
        InitializeControls(appletName);
    });
}

// Возвращает id апплета, внутри которого произошло событие
var GetFormId = event => $(event.currentTarget).closest('.form-container')[0].firstElementChild['id'];

// Возвращает список всех полей
var GetFields = appletName => AppletInfo(appletName)['Controls'].filter(item => item['Type'] != 'button');

// Возвращает выбранное свойство всех полей
var GetFieldsProperty = (appletName, propertyName) => GetFields(appletName).map(field => field[propertyName]);

// Возвращает все свойства выбранного поля
var GetFieldProperties = (appletName, fieldName) => GetFields(appletName).filter(field => field['RecordName'] == fieldName)[0];

// Возвращает поля по значению свойства
var GetFieldsByPropertyValue = (appletName, propertyName, propertyValue) => {
    return GetFields(appletName).filter(field => field[propertyName] == propertyValue)[0];
}