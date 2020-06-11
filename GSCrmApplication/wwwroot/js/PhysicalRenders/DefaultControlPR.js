export default class DefaultControlPR {
    RenderControl(control, applet) {
        let controlElement = $('#' + control['Id']);
        let controlName = control['Name'];
        let properties = applet.GetControlProperties(controlName);
        if (properties != undefined) {
            let type = properties['Type'];
            let icon = properties['IconName'];
            let header = properties['Header'];
            let recordSet = applet.GetRecordSet();
            
            // ���� � �������� ���� ������, �� ��� �����������
            if (icon != null) {
                $(controlElement).addClass('block-center').append('<span class="' + icon + '" aria-hidden="true"></span>');
            }

            // �����, ���� ��� �������� ������, ����������� ���������
            else if (type == 'button') {
                $(controlElement).append('<div class="block-center"><p>' + header + '</p></div>');
            }
            
            // ������������� ��������� � ����������� �� ����
            switch (type) {
                // ������������� ����� � ����� input
                case "input":
                    $(controlElement)
                        .addClass('field-input')
                        .append('<div class="gs-field-label"><p>' + header + '</p></div><input class="gs-field-input" />')
                        .off('ControlFocus')
                        .on('ControlFocus', (event, args) => {
                            $(controlElement).find('.gs-field-input')[0].focus();
                            $(controlElement).addClass('gs-field-is-focused').find('.gs-field-counter').removeClass('d-none');
                        })
                        .off('input')
                        .on('input', () => {
                            event.stopPropagation();
                        });
                    //$(controlElement).find('.gs-field-input').off('focus').on('focus', event => event.stopPropagation());
                    break;
    
                // ������������� ����� � ����� date
                case "date":
                    $(controlElement)
                        .addClass('field-date')
                        .attr('data-field-up', $(controlElement).attr('data-field-up') + ';NotInput')
                        .append('<div class="gs-field-label"><p>' + header + '</p></div><input class="gs-field-input" />' +
                            '<div class="pick-divider"></div><div class="pick-area block-center">' +
                            '<span class="icon-calendar" aria-hidden="true"></span></div>')
                        .off('ControlFocus')
                        .on('ControlFocus', (event, args) => {
                            $(controlElement).addClass('gs-field-is-focused');
                        });
                    break;
    
                // ������������� ����� � ����� picklist
                case "picklist":
                    $(controlElement)
                        .addClass('field-picklist')
                        .attr('data-field-up', $(controlElement).attr('data-field-up') + ';NotInput')
                        .append('<div class="gs-field-label"><p>' + header + '</p></div><input class="gs-field-input" />' +
                            '<div class="pick-divider"></div><div class="pick-area block-center">' +
                            '<span class="icon-chevron-thin-right" aria-hidden="true"></span></div>')
                        .off('ControlFocus')
                        .on('ControlFocus', (event, args) => {
                            $(controlElement).find('.gs-field-input')[0].focus();
                            $(controlElement).addClass('gs-field-is-focused');
                        });
                    //$(controlElement).find('.gs-field-input').off('focusin').on('focusin', event => event.stopPropagation());
                    break;
    
                // ������������� ����� � ����� checkbox
                case "checkbox":
                    $(controlElement)
                        .removeClass('gs-field')
                        .addClass('field-checkbox')
                        .append('<div class="checkbox-check"></div><div>' + header + '</div>')
                        .off('ControlFocus')
                        .on('ControlFocus', (event, args) => {
                            if (!$(controlElement).hasClass('gs-field-is-filled'))
                                $(controlElement).addClass('gs-field-is-filled')
                            else $(controlElement).removeClass('gs-field-is-filled');
                        });
                        if(properties['Readonly'])
                            $(controlElement).addClass("gs-field-readonly");
                    break;

                // ������������� drilldown-��
                case "drilldown":
                    $(controlElement)
                        .removeClass('gs-field')
                        .addClass('field-drilldown')
                        .append('<p class="label-sm">' + header + '</p>')
                        .off('ControlFocus')
                        .on('ControlFocus', (event, args) => {
                            
                        });
                        if(properties['Readonly'])
                            $(controlElement).addClass("gs-field-readonly");
                    break;
            }

            if(properties['Readonly'])
            {
                $(controlElement)
                    .addClass("gs-field-readonly")
                    .find('.gs-field-input')
                    .attr('tabindex', '-1')
                    .attr('readonly', 'true');
            }

            if (type != 'button') {
                // ���� � recordSet-� ���� �������� ��� ����, ���������� ���������� �������� ���� ���������
                if (recordSet != undefined) {
                    if (recordSet[controlName] != "" && recordSet[controlName] != null) {
                        switch (type) {
                            // ���� ������� ��� checkbox, �� ����������� ��� ���� ������ ������ � ��� ������, ���� �������� �� recordSet-� - true
                            case "checkbox":
                                if (recordSet[controlName])
                                    $(controlElement).addClass('gs-field-is-filled');
                                else $(controlElement).removeClass('gs-field-is-filled')
                                break;
    
                            // ����� � ����� ������
                            default:
                                if (recordSet[controlName] != undefined)
                                    $(controlElement).addClass('gs-field-is-filled').find('.gs-field-input').val(recordSet[controlName]);
                                break;
                        }
                    }
    
                    else $(controlElement).removeClass('gs-field-is-filled');
                }
            }
            
            $(controlElement)
                .off('ControlFocusOut')
                .on('ControlFocusOut', (event, args) => {
                    let newValue = type == 'checkbox' 
                    ? $(controlElement).hasClass('gs-field-is-filled') ? true : false
                    : $(controlElement).find('.gs-field-input').val();
                    $(controlElement).removeClass('gs-field-is-focused');

                    if (type == 'picklist') {
                        $(controlElement).removeClass('gs-field-is-focused');
                        $(controlElement)
                            .find('.icon-chevron-thin-left')
                            .removeClass('icon-chevron-thin-left')
                            .addClass('icon-chevron-thin-right');
                        if (newValue != '') $(controlElement).addClass('gs-field-is-filled')
                        else $(controlElement).removeClass('gs-field-is-filled')
                    }

                    if (type == "input") {
                        // ���� �������� ������ ������� ��������
                        if (newValue == '') {
                            $(controlElement)
                                .removeClass('gs-field-is-filled')
                                .find('.gs-field-counter')
                                .addClass('d-none');
                        }

                        // ����� ������������ ������
                        else $(controlElement).closest('.gs-field').addClass('gs-field-is-filled');
                    }

                    $(controlElement).trigger('UpdateControl', [{
                        Event: args['Event'],
                        NewValue: newValue
                    }]);
                })
        }
        return controlElement;
    }
}