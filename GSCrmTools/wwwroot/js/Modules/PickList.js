import DefaultPickListPR from '../PhysicalRenders/DefaultPickListPR.js'

export default class PickList {
    Initialize(control, controlElement) {
        return new Promise((resolve, reject) => {
            let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
            let view = currentElementsInfo['View'];
            let targetApplet = currentElementsInfo['TargetApplet'];
            let popupApplet = currentElementsInfo['PopupApplet'];
            let currentApplet = currentElementsInfo['CurrentApplet'];
            let currentRecord = currentElementsInfo['CurrentRecord'];
            let recordId = currentRecord == null ? null : currentRecord['Id'];
            let currentControl = currentElementsInfo['CurrentControl'];
            let currentPopupControl = currentElementsInfo['CurrentPopupControl'];
            let recordSet = currentApplet.GetRecordSet();

            // Обновление информации о представлении
            let data = {
                ActionType: 'OpenPickList',
                TargetApplet: targetApplet['Name'],
                CurrentRecord: recordId,
                CurrentControl: currentControl,
                OpenPopup: false,
                ClosePopup: false,
                PopupApplet: popupApplet == null ? null : popupApplet['Name'],
                CurrentPopupControl: currentPopupControl,
            }

            view.UpdateViewInfo(data)
                .catch(error => reject(error))
                .then(() => {
                    GSCrmInfo.Application.CommonRequests.GetPickListRecords(control)
                        .fail(error => reject(error))
                        .done(result => {
                            if (result != undefined) {
                                GSCrmInfo.SetElement("Picklist", result);
                                let PR = new DefaultPickListPR();
                                let counter = 0;
    
                                // Рендеринг пиклиста
                                PR.RenderPickList(
                                    controlElement, 
                                    result['DisplayedPickListRecords'].map(item => {
                                        item['PickListItemId'] = 'PickListItem_' + counter++;
                                        return item['Value'];
                                    }));
    
                                // Проставление id элементам
                                counter = 0;
                                $('[data-type="SelectArea"]')
                                    .find('[data-type="PickListItem"]')
                                    .map((index, item) => $(item).attr('id', 'PickListItem_' + counter++));
    
                                // Установка текущей записи пиклиста
                                let currentRecord = result['DisplayedPickListRecords'].filter(item => {
                                    return item['Id'] == result['PickListInfo']['CurrentRecordId'];
                                })[0];
                                currentRecord != undefined && PR.SetCurrentElement(currentRecord['PickListItemId']);
    
                                // Добавление обработчика выбора элемента из пиклиста
                                $('[data-type="SelectArea"]')
                                    .find('[data-type="PickListItem"]')
                                    .off('click')
                                    .on('click', event => {
                                        let pickedRecord = result['DisplayedPickListRecords'].filter(item => {
                                            return item['PickListItemId'] == $(event.currentTarget).attr('id')
                                        })[0];
                                        data = {
                                            IsPicked: true,
                                            PickedRecord: pickedRecord['Id']
                                        }

                                        // Пик записи
                                        GSCrmInfo.Application.CommonRequests.SetPickListRecord(control, data)
                                            .fail(error => reject(error))
                                            .done(response => {
                                                // Установка возвращенного беком значения в recordSet
                                                let newPickedValue = response['NewPickedValue'];
                                                recordSet[control['Name']] = newPickedValue;
                                                $(controlElement).trigger('PickListItemSelect', [{ 
                                                    Event: event, 
                                                    NewValue: newPickedValue
                                                }]);
                                                GSCrmInfo.Application.CloseSelectArea();

                                                if (response['Status'] == "Fail") {
                                                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(response['ErrorMessages']);
                                                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while pick record.', errorMessage);
                                                }

                                                // Если действие произошло не в попапе
                                                if (popupApplet == null) {
                                                    $(controlElement).trigger("ControlChange", [{ 
                                                        Event: event,
                                                        NeedUpdate: true
                                                    }]);
                                                }
                                                
                                                // Если действие произошло в попапе у которого не проставлена кастомная инициализация контролов
                                                else if (!popupApplet['Info']['Initflag']) {
                                                    $(controlElement).trigger("ControlChange", [{ 
                                                        Event: event,
                                                        NeedUpdate: false
                                                    }]);
                                                }
                                                resolve();
                                            });
                                    })
                            }

                            else GSCrmInfo.Application.CloseSelectArea();
                        })
                })
        })
    }
}