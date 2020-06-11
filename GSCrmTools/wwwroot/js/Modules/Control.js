import DefaultControlPR from '../PhysicalRenders/DefaultControlPR.js'
import PickList from './PickList.js'

export default (function () {
    let control = function(applet, name, id) {
        this.Id = id;
        this.Name = name;
        let properties = applet.GetControlProperties(this.Name);
        if (properties != undefined)
            this.Routing = properties['Routing'];
    }
    
    control.prototype.Initialize = function(applet) {
        let control = $("#" + this.Id);
        let properties = applet.GetControlProperties(this.Name);
        if (properties != undefined) {
            // Тип контрола и заголовок
            let type = properties['Type'];
            let PR = new DefaultControlPR();
            let recordSet = applet.GetRecordSet();
            $(control).attr('id', this.Id).addClass(properties['CssClass']);
            control = PR.RenderControl(this, applet);

            // Обработка кнопок
            if (type == 'button') {
                $(control)
                    .addClass(properties['CssClass'])
                    .off('click')
                    .on('click', event => {
                        if (!properties['Readonly']) {
                            event.stopPropagation();
                            let currentElementsInfo = GSCrmInfo.CurrentElementsInfo;
                            let appletToUpdate = currentElementsInfo['AppletToUpdate'];
                            let recordToUpdate = currentElementsInfo['RecordToUpdate'];
                            if ($(control).attr('disabled') == undefined) {
                                $('[data-type="ExpectationArea"]').removeClass('d-none');
                                    $('[data-type="control"]').attr('disabled', true);
                                    $('[data-type="applet_item"]').attr('disabled', true);
                                    if (appletToUpdate != null) {
                                        GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, recordToUpdate)
                                            .catch(error => console.log(error))
                                            .then(() => {
                                                applet.InvokeAction(event, properties);
                                            });
                                    }
                                    else applet.InvokeAction(event, properties);
                                }
                            }
                        });
                }

            // Обработка полей
            else {
                $(control)
                    .off('click')
                    .on('click', event => {
                        event.stopPropagation();
                        if (recordSet != undefined && !properties['Readonly']) {
                            GSCrmInfo.SetUpCurrentElements(event);
                            let currentElementsInfo = GSCrmInfo.CurrentElementsInfo;
                            this.UpdateRescordSet(type).then(() => {
                                $(control).trigger('ControlFocus', [{ Event: event }]);
                                switch(type) {
                                    case "checkbox":
                                        $(control).trigger('UpdateControl', [{
                                            Event: event,
                                            NewValue: recordSet[this.Name] == false ? true : false
                                        }]);
                                        if ($('[data-type="SelectArea"]').css('display') == 'block' && $('[data-type="SelectArea"]').attr('data-target-id') != $(control).attr('id'))
                                            GSCrmInfo.Application.CloseSelectArea();
                                        break;
                                    case "date":
                                        if ($('[data-type="SelectArea"]').attr('data-target-id') != $(control).attr('id')) {
                                            // Если щелкнули не по тому же полю, открытие календаря
                                            let calendar = new Calendar();
                                            calendar.Initialize(this, event);
                                        }
                                        break;
                                    case "picklist":
                                        // Если щелкнули не по тому же полю, открытие пиклиста
                                        if ($('[data-type="SelectArea"]').attr('data-target-id') != $(control).attr('id')) {
                                            var PL = new PickList();
                                            PL.Initialize(this, control).catch(error => console.log(error));
                                        }
                                        break;
                                    case "drilldown":
                                        GSCrmInfo.Application.CommonAction.Invoke("Drilldown", event, recordSet);
                                        break;
                                }
                                if ($('[data-type="SelectArea"]').css('display') == 'block' && $('[data-type="SelectArea"]').attr('data-target-id') != $(control).attr('id'))
                                    GSCrmInfo.Application.CloseSelectArea();
                            });
                        }
                    })
                    .off('focusout')
                    .on('focusout', event => {
                        // При уходе с контроле идет проверка, что с контрола действительно перешли, а не просто выбрали дочерний элемент
                        if (recordSet != undefined && $(event.relatedTarget).closest($(event.currentTarget)).length == 0)
                        {
                            if (!properties['Readonly'])
                                $(control).trigger('ControlFocusOut', [{ Event: event }])
                        }
                    })
                $(control)
                    .off('UpdateControl')
                    .on('UpdateControl', (event, args) => {
                        let newValue = args['NewValue'];
                        // Проверяется, что новое значение не равно старому и что старое не равно null а новое пустой строке одновременно
                        if (recordSet != undefined && recordSet[this.Name] != newValue && !(recordSet[this.Name] == null && newValue == '')) {
                            let currentElementsInfo = GSCrmInfo.CurrentElementsInfo;
                            let popupApplet = currentElementsInfo['PopupApplet'];
                            // Если текущий контрол является пиклистом, необходимо заново произвести пик записи, так как
                            // если выполнение дошло до этого места, значит пользователь вручную ввел значение в пиклист, а не пикнул его
                            if (type == 'picklist') {
                                let data = {
                                    IsPicked: false,
                                    Value: newValue
                                }
                                GSCrmInfo.Application.CommonRequests.SetPickListRecord(this, data)
                                    .fail(error => {
                                        GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', error['responseJSON'][0]['errorMessage']);
                                    })
                                    .done(response => {
                                        // Установка возвращенного беком значения в recordSet
                                        let newPickedValue = response['NewPickedValue'];
                                        recordSet[this.Name] = newPickedValue;
                                        $(event.currentTarget).trigger('PickListItemSelect', [{ 
                                            Event: args['Event'],
                                            NewValue: newPickedValue
                                        }]);
                                        GSCrmInfo.Application.CloseSelectArea();

                                        if (response['Status'] == "Fail") {
                                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(response['ErrorMessages']);
                                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while pick record.', errorMessage);
                                        }

                                        // Если действие произошло не в попапе
                                        if (popupApplet == null) {
                                            $(event.currentTarget).trigger("ControlChange", [{ 
                                                Event: event,
                                                NeedUpdate: true
                                            }]);
                                        }
                                        
                                        // Если действие произошло в попапе у которого не проставлена кастомная инициализация контролов
                                        else if (!popupApplet['Info']['Initflag']) {
                                            $(event.currentTarget).trigger("ControlChange", [{ 
                                                Event: event,
                                                NeedUpdate: false
                                            }]);
                                        }
                                    });
                            }

                            // Если текущий контрол не является пиклистом
                            else {
                                recordSet[this.Name] = newValue;
    
                                // Если действие произошло не в попапе
                                if (popupApplet == null)
                                {
                                    $(control).trigger("ControlChange", [{ 
                                        Event: args['Event'],
                                        NeedUpdate: true
                                    }]);
                                }
    
                                // Если действие произошло в попапе у которого не проставлена кастомная инициализация контролов
                                else if (!popupApplet['Info']['Initflag']) {
                                    $(control).trigger("ControlChange", [{ 
                                        Event: args['Event'],
                                        NeedUpdate: false
                                    }]);
                                }
                            }
                        }
                    })
                    .off('ControlChange')
                    .on('ControlChange', (event, args) => {
                        let recordSet = applet.GetRecordSet();
                        let currentElementsInfo = GSCrmInfo.CurrentElementsInfo;
                        currentElementsInfo['View'].RefreshViewAppletsUI(applet, recordSet);
                        if (recordSet != undefined && args['NeedUpdate'] && applet['Info']['Initflag'] == false) {
                            GSCrmInfo.SetElement('AppletToUpdate', applet);
                            GSCrmInfo.SetElement('RecordToUpdate', recordSet);
                        }
                    });
            }
        }
    }

    // Обновление recordSet, если это требуется и если запись для обновления не является текущей, за исключением, если была нажата кнопка или действие произошло в попапе
    control.prototype.UpdateRescordSet = function(type) {
        return new Promise((resolve, reject) => {
            let currentElementsInfo = GSCrmInfo.CurrentElementsInfo;
            let appletToUpdate = currentElementsInfo['AppletToUpdate'];
            let recordToUpdate = currentElementsInfo['RecordToUpdate'];
            let currentRecord = currentElementsInfo['CurrentRecord'];
            if (appletToUpdate != null && (currentRecord['Id'] != recordToUpdate['Id'] || type == "drilldown")) {
                GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, recordToUpdate)
                    .catch(error => reject(error))
                    .then(() => resolve());
            }
            else resolve();
        })
    }

    return control;
})();