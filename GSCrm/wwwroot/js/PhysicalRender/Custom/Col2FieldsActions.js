class Col2FieldsActions {
    RenderApplet(applet) {
        var $applet = $('#' + applet.Id);
        var controls = applet.Controls;
    
        // Тело
        var body = $('<div class="row justify-content-around">' +
            '<div class="col-md">' +
            '<div class="row justify-content-center"><div class="block-center"><p class="label-md">Fill in the data</p></div></div>' +
            '<div class="divider-mr-md"></div>' +
            '<div class="row justify-content-center" id="Fields_' + applet.Id + '"></div></div>' +
            '<div class="col-md">' +
            '<div class="row justify-content-center"><div class="block-center"><p class="label-md">Actions</p></div></div>' +
            '<div class="divider-mr-md"></div>' +
            '<div class="d-flex flex-row flex-md-column flex-lg-row flex-wrap" id="Actions_' + applet.Id + '"></div>' +
            '</div></div>');
    
        // Контролы
        controls.forEach((control, index) => {
            let name = control['Name'];
            switch(control['Type']) {
                case "button":
                    body
                        .find('#Actions' + '_' + applet.Id)
                        .append('<div class="col-auto ml-auto mr-auto mb-4"><div data-type="control" data-name="'
                        + name + '" tabindex="' + index + '"></div></div>');
                    break;
                case "checkbox":
                    // Разметка
                    let checkbox = $('<div class="col-auto" style="width: 80%; margin: auto; height: 45px">' +
                    '<div class="gs-field" data-type="control" data-name="' + name + '" style="margin: auto" tabindex="' + index + '"></div></div>');
    
                    // Над контролами с типом checkbox нужны меньшие отступы
                    if (controls[index + 1] && controls[index + 1]['Type'] == 'checkbox')
                        checkbox.addClass('mb-1');
                    else checkbox.addClass('mb-3');
    
                    // Добавление разметки
                    body
                        .find('#Fields' + '_' + applet.Id)
                        .append(checkbox);  
                    break;
                default:
                    // Разметка
                    let el = $('<div class="col-auto" style="width: 80%; margin: auto">' +
                    '<div class="gs-field" data-type="control" data-name="' + name + '" style="margin: auto" tabindex="' + index + '"></div></div>');
    
                    // Над контролами с типом checkbox нужны меньшие отступы
                    if (controls[index + 1] && controls[index + 1]['Type'] != 'checkbox')
                        el.addClass('mb-4');
                    else el.addClass('mb-3');
    
                    // Добавление разметки
                    body
                        .find('#Fields' + '_' + applet.Id)
                        .append(el);                        
                    break;
            }
        });
    
        $applet
            .find('.p-modal-body')
            .append(body);
    }
}