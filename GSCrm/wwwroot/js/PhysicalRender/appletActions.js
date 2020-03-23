//@C Действие
class Action {
    /**
     * @M Вызов действия по имени, принимает следующие аргменты:
     * @param {String} actionName Название действия, которое необходимо вызвать
     * @param {Event} event Событие нажатия на контрол
     * @param {{
        * View: View,
        * TargetApplet: Applet,
        * PopupApplet: Applet,
        * ControlName: String
        * Record: Object
     * }} currentElements Объект с текущими выбранными элементами
     * @param {Object} recordsList RecordSet
     */
    Invoke(actionName, event, recordsList, payload) {
        let actions = new {
            //@C Навигация
            Navigation: class extends Action {
                Initialize(event, recordsList) {
                    return new Promise((resolve, reject) => {
                        // Отображение анимации ожидания
                        $('.cssload-wrapper').removeClass('d-none');
                
                        // Представление, внутри которого произошло действие, апплет и контрол
                        let currentElements = Info.CurrentElementsInfo;
                        let view = currentElements.View;
                        let targetApplet = currentElements.TargetApplet;
                        let controlName = currentElements.ControlName;
                        
                        // Установка текущих выбранных элементов
                        let recordId = targetApplet.GetSelectedRecordProperty('Id');
                        let data = {
                            ViewName: view.Name,
                            CurrentApplet: targetApplet.Name,
                            CurrentRecord: recordId,
                            CurrentControl: controlName,
                            Action: controlName
                        }
                
                        // Обновление информации о представлении на беке
                        view.UpdateViewInfo(data)
                            .catch(error => {
                                // Скрытие анимации ожидания
                                $('.cssload-wrapper').addClass('d-none');
                                reject(error);
                            })
                            .then(() => { 
                                // Получение новых записей для апплета
                                targetApplet.GetRecords()
                                    .catch(error => reject(error))
                                    .then(() => {
                                        recordId = targetApplet.GetSelectedRecordProperty('Id');
                                        
                                        // Частичное обновление контекста представления
                                        view.PartialUpdateContext(targetApplet, recordId, false)
                                            .catch(error => reject(error))
                                            .then(() => {
                                                resolve();
                                            })
                                            .finally(() => {
                                                // Скрытие анимации ожидания
                                                $('.cssload-wrapper').addClass('d-none');
                                            });
                                        });
                            });
                        });
                }
            },

            //@C Открытие попапа
            ShowPopup: class extends Action {
                Initialize(event, recordsList) {
                    return new Promise((resolve, reject) => {
                        event.stopPropagation();
                
                        // Отображение анимации ожидания
                        $('.cssload-wrapper').removeClass('d-none');

                        // Апплет и контрол с которого произошел запуск попапа
                        let currentElements = Info.CurrentElementsInfo;
                        let targetApplet = currentElements.TargetApplet;
                        let controlName = currentElements.ControlName;

                        // Попап апплет
                        // Получение названия попапа из user property апплета, с которого он открывается
                        let up = targetApplet.Info['ControlUPs'];
                        let popupAppletName = up[controlName].filter(item => item['Name'] == 'Applet')[0]['Value'];

                        // Формирование id попапа
                        let popupAppletId = targetApplet.Id + "_Popup_0";
                        let counter = 0;
            
                        // До тех пор, пока элемент с таким id присутствует в документе, прибавление 1, чтобы Id был уникальным
                        while ($('#' + popupAppletId).length > 0) {
                            popupAppletId = popupAppletId.split(counter).join((counter + 1));
                            counter++;
                        }

                        // Инициализация попапа
                        let popupApplet = new PopupApplet(popupAppletName, popupAppletId);
                        popupApplet.Initialize(event)
                            .catch(error => {                
                                // Скрытие анимации ожидания
                                $('.cssload-wrapper').addClass('d-none');
                                reject(error);
                            })
                            .then(() => resolve())
                            .finally(() => {
                                // Скрытие анимации ожидания
                                $('.cssload-wrapper').addClass('d-none');
                            });
                    });
                }
            },

            //@C  Создание новой записи
            NewRecord: class extends Action {
                Initialize(event, recordsList) {
                    return new Promise((resolve, reject) => {
                        // Отображение анимации ожидания
                        $('.cssload-wrapper').removeClass('d-none');
                
                        let data = {};
                        let currentElements = Info.CurrentElementsInfo;
                        let view = currentElements.View;
                        let targetApplet = currentElements.TargetApplet;
                        let popupApplet = currentElements.PopupApplet;
                        let properties = targetApplet.GetControlProperties('NewRecord');

                        // Преобразование recordSet-а в объект, который правильно примет бек
                        for (let item in recordsList) {
                            data[item] = recordsList[item];
                        }
                
                        // Запрос на создание новой записи
                        var request = new RequestsToDB;
                        request.NewRecord(targetApplet, data)
                            .fail(error => {
                                // Скрытие анимации ожидания
                                $('.cssload-wrapper').addClass('d-none');            
                                reject(error['responseJSON']);
                            })
                            .done(recordId => {
                                if(!Object.is(popupApplet, null)) {
                                    // Установка информации о текущих выбранных элементах
                                    let data = {
                                        ViewName: view.Name,
                                        CurrentApplet: targetApplet.Name,
                                        CurrentRecord: recordId,
                                        CurrentPopupControl: currentElements.ControlName,
                                        Action: 'NewRecord'
                                    }
                    
                                    // Обновлении информации о представлении
                                    view.UpdateViewInfo(data)
                                        .catch(error => {
                                            // Скрытие анимации ожидания
                                            $('.cssload-wrapper').addClass('d-none');
                                            reject(error);
                                        })
                                        .then(() => {
                                            // Частичное оьновление контекста представления с обновлением текущего апплета
                                            view.PartialUpdateContext(targetApplet, recordId, true)
                                                .catch(error => reject(error))
                                                .then(() => {
                                                    // Если на контроле висит событие showPopup
                                                    if (properties['ActionName'] == 'ShowPopup') {
                                                        popupApplet.Dispose();
                                                        resolve();
                                                    }
                                                })
                                                .finally(() => {
                                                    // Скрытие анимации ожидания
                                                    $('.cssload-wrapper').addClass('d-none');
                                                });
                                        });
                                }
                            });
                    });
                }
            },

            //@C Отмена создания записи
            UndoRecord: class extends Action {
                Initialize(event, recordsList) {
                    return new Promise((resolve, reject) => {
                        let currentElements = Info.CurrentElementsInfo;
                        let popupApplet = currentElements.PopupApplet;
                        if (!Object.is(popupApplet, null)) {
                            // Уничтожение попапа
                            popupApplet.Dispose();
    
                            // Обновление информации о представлении
                            let view = Info.CurrentElementsInfo['View'];
                            let data = {
                                ViewName: view.Name,
                                CurrentApplet: Info.CurrentElementsInfo['TargetApplet'].Name,
                                ClosePopup: true,
                                Action: 'UndoRecord'
                            }
                            view.UpdateViewInfo(data)
                                .catch(error => reject(error))
                                .then(() => resolve());
                        }
                    });
                }
            },

            //@C Обновление записи
            UpdateRecord: class extends Action {
                Initialize(event, recordsList) {
                    return new Promise((resolve, reject) => {
                        // Отображение анимации ожидания
                        $('.cssload-wrapper').removeClass('d-none');
                
                        var data = {};
                        Info.SetUpCurrentElements(event);
                        let currentElements = Info.CurrentElementsInfo;
                        let view = currentElements.View;
                        let targetApplet = currentElements.TargetApplet;
                        let popupApplet = currentElements.PopupApplet;

                        // Преобразование входящей мапы в объект
                        for (var item in recordsList) {
                            data[item] = recordsList[item];
                        }
                
                        // Обновление записи
                        let request = new RequestsToDB;
                        request.UpdateRecord(targetApplet, data)
                            .fail(error => {
                                // Скрытие анимации ожидания
                                $('.cssload-wrapper').addClass('d-none');
                                reject(error);
                            })
                            .done(recordId => {
                                // Текущие выбранные элементы
                                var data = {
                                    ViewName: view.Name,
                                    CurrentApplet: targetApplet.Name,
                                    CurrentRecord: recordId,
                                    CurrentPopupControl: currentElements.ControlName,
                                    Action: 'UpdateRecord'
                                }
                
                                // Обновление представления
                                view.UpdateViewInfo(data)
                                    .catch(error => {
                                        // Скрытие анимации ожидания
                                        $('.cssload-wrapper').addClass('d-none');
                                        reject(error);
                                    })
                                    .then(() => {
                                        view.PartialUpdateContext(targetApplet, recordId, true)
                                            .catch(error => reject(error))
                                            .then(() => {
                                                popupApplet.Dispose();
                                                resolve();
                                            })
                                            .finally(() => {
                                                // Скрытие анимации ожидания
                                                $('.cssload-wrapper').addClass('d-none');
                                            });
                                    });
                            });
                    });
                }
            },

            //@C Автоматическое обновление записи при переходе по элементам списка
            AutoUpdateRecord: class extends Action {
                Initialize(event, recordsList) {
                    return new Promise((resolve, reject) => {
                        // Отображение анимации ожидания
                        $('.cssload-wrapper').removeClass('d-none');
                
                        // Преобразование входящей мапы в объект
                        var data = {};
                        for (var item in recordsList) {
                            data[item] = recordsList[item];
                        }
                
                        // Обновление записи
                        let request = new RequestsToDB;
                        request.UpdateRecord(Info.CurrentElementsInfo['AppletToUpdate'], data)
                            .fail(error => {
                                // Скрытие анимации ожидания
                                reject(error);
                            })
                            .done(() => resolve())
                            .always(() => {
                                $('.cssload-wrapper').addClass('d-none');
                            });
                    });
                }
            },

            //@C Отмена обновления записи
            UndoUpdate: class extends Action {
                Initialize(event, recordsList) {
                    return new Promise((resolve, reject) => {
                        let currentElements = Info.CurrentElementsInfo;
                        let popupApplet = currentElements.PopupApplet;
                        if (!Object.is(popupApplet, null)) {    
                            // Уничтожение попапа
                            popupApplet.Dispose();
    
                            // Обновление информации о представлении
                            let view = Info.CurrentElementsInfo['View'];
                            let data = {
                                ViewName: view.Name,
                                CurrentApplet: Info.CurrentElementsInfo['TargetApplet'].Name,
                                ClosePopup: true,
                                Action: 'UndoUpdate'
                            }
                            view.UpdateViewInfo(data)
                                .catch(error => reject(error))
                                .then(() => resolve());
                        }
                    });
                }
            },

            //@C Удаление записи
            DeleteRecord: class extends Action {
                Initialize(event, recordsList) {
                    return new Promise((resolve, reject) => {
                        // Отображение анимации ожидания
                        $('.cssload-wrapper').removeClass('d-none');
                
                        // Представлние и апплет, на котором произошло событие
                        let currentElements = Info.CurrentElementsInfo;
                        let view = currentElements.View;
                        let targetApplet = currentElements.TargetApplet;
                        let recordId = currentElements.Record['Id'];
                        let controlName = currentElements.ControlName;
                
                        // Выполняю запрос
                        let request = new RequestsToDB;
                        request.DeleteRecord(targetApplet, recordId)
                            .fail(error => {
                                // Скрытие анимации ожидания
                                $('.cssload-wrapper').addClass('d-none');
                                reject(error['responseJSON']);
                            })
                            .done(recordId => {
                                // Установка текущего выбранного апплета и представления
                                var data = {
                                    ViewName: view.Name,
                                    CurrentApplet: targetApplet.Name,
                                    CurrentRecord: recordId,
                                    CurrentControl: controlName,
                                    Action: 'DeleteRecord'
                                }
                
                                // Обновление информации о представлении
                                view.UpdateViewInfo(data)
                                    .catch(error => {
                                        // Скрытие анимации ожидания
                                        $('.cssload-wrapper').addClass('d-none');
                                        reject(error);
                                    })
                                    .then(() => {
                                        // Частичное обновление контекста представления с обновлением текущего апплета
                                        view.PartialUpdateContext(targetApplet, recordId, true)
                                            .catch(error => reject(error))
                                            .then(() => {
                                                resolve();
                                            })
                                            .finally(() => {
                                                // Скрытие анимации ожидания
                                                $('.cssload-wrapper').addClass('d-none');
                                            });
                                    });
                            });
                    });
                }
            }
        }[actionName];
        return actions.Initialize(event, recordsList, payload);
    }
}