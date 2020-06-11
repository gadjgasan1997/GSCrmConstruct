import DefaultErrorPR from '../PhysicalRenders/DefaultErrorPR.js'

export default class Action {
    /**
        * @M ����� �������� �� �����, ��������� ��������� ��������:
        * @param {String} actionType ��� ��������, ������� ���������� �������
        * @param {Event} event ������� ������� �� �������
        * @param {Object} currentElementsInfo ������ � �������� ���������� ����������
        * @param {Object} recordSet RecordSet
        */
    static Invoke(actionType, event, recordSet, payload) {
        let actions = new {
            //@C ���������
            Navigation: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet) {
                    return new Promise((resolve, reject) => {
                        // �������������, ������ �������� ��������� ��������, ������ � �������
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let view = currentElementsInfo['View'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let currentControl = currentElementsInfo['CurrentControl'];

                        // ��������� ������� ��������� ���������
                        let recordId = targetApplet.GetSelectedRecordProperty('Id');
                        let data = {
                            ActionType: currentControl,
                            TargetApplet: targetApplet['Name'],
                            CurrentRecord: recordId,
                            CurrentControl: currentControl,
                            OpenPopup: false,
                            ClosePopup: false,
                            PopupApplet: null,
                            CurrentPopupControl: null
                        }

                        // ���������� ���������� � ������������� �� ����
                        view.UpdateViewInfo(data)
                            .catch(error => {
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view info.', errorMessage);
                                reject(error);
                            })
                            .then(() => {
                                // ��������� ����� ������� ��� �������
                                targetApplet.Initialize()
                                    .catch(error => {
                                        $('[data-type="ExpectationArea"]').addClass('d-none');
                                        $('[data-type="control"]').attr('disabled', false);
                                        $('[data-type="applet_item"]').attr('disabled', false);
                                        GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', 'An error occured while initialize applet records.');
                                        reject(error);
                                    })
                                    .then(() => {
                                        GSCrmInfo.SetElement("CurrentRecord", GSCrmInfo.GetSelectedRecord(targetApplet['Name'])['properties'])
                                        // ��������� ���������� ��������� �������������
                                        view.PartialUpdateContext(targetApplet.GetInfo(), false)
                                            .catch(error => {
                                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                                reject(error);
                                            })
                                            .then(() => resolve())
                                            .finally(() => {
                                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                                $('[data-type="control"]').attr('disabled', false);
                                                $('[data-type="applet_item"]').attr('disabled', false);
                                            });
                                    });
                            });
                    });
                }
            },

            //@C �������� ������
            ShowPopup: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet) {
                    return new Promise((resolve, reject) => {
                        event.stopPropagation();

                        // ������ � ������� � �������� ��������� ������ ������
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let view = currentElementsInfo['View'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let currentControl = currentElementsInfo['CurrentControl'];
                        let record = currentElementsInfo['CurrentRecord'];
                        let recordId = record == null ? null : record['Id'];

                        // ����� ������
                        // ��������� �������� ������ �� user property �������, � �������� �� �����������
                        let up = targetApplet.GetInfo()['ControlUPs'];
                        let popupAppletName = up[currentControl].filter(item => item['Name'] == 'Applet')[0]['Value'];

                        // ������������ id ������
                        let popupAppletId = targetApplet.GetInfo()["AppletId"] + "_Popup_0";
                        let counter = 0;

                        // �� ��� ���, ���� ������� � ����� id ������������ � ���������, ����������� 1, ����� Id ��� ����������
                        while ($('#' + popupAppletId).length > 0) {
                            popupAppletId = popupAppletId.split(counter).join((counter + 1));
                            counter++;
                        }
                        
                        let data = {
                            ActionType: 'ShowPopup',
                            TargetApplet: targetApplet.GetName(),
                            CurrentRecord: recordId,
                            CurrentControl: currentControl,
                            OpenPopup: true,
                            ClosePopup: false,
                            PopupApplet: null,
                            CurrentPopupControl: null
                        }

                        // ���������� ���������� � �������������
                        view.UpdateViewInfo(data)
                            .catch(error => {
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view info.', errorMessage);
                                reject(error);
                            })
                            .then(() => {
                                // ������������� ������
                                let popupApplet = new GSCrmInfo.Application.PopupApplet(popupAppletName, popupAppletId);
                                popupApplet.Initialize(event)
                                    .catch(error => {
                                        let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                        GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', 'An error occured while initialize popup applet.');
                                        reject(error);
                                    })
                                    .then(() => resolve())
                                    .finally(() => {
                                        $('[data-type="ExpectationArea"]').addClass('d-none');
                                        $('[data-type="control"]').attr('disabled', false);
                                        $('[data-type="applet_item"]').attr('disabled', false);
                                    });
                            });
                    });
                }
            },

            // �������� ������
            ClosePopup : class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet) {
                    return new Promise((resolve, reject) => {
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let view = currentElementsInfo['View'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let popupApplet = currentElementsInfo['PopupApplet'];
                        let currentControl = currentElementsInfo['CurrentControl'];
                        let record = currentElementsInfo['CurrentRecord'];
                        if (popupApplet != null) {
                            GSCrmInfo.Application.CommonAction.DisposePopupApplet('ClosePopup', record, currentControl)
                                .catch(error => {
                                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while close popup applet.', errorMessage);
                                    reject(error);
                                })
                                .then(() => resolve())
                                .finally(() => {
                                    view.RefreshViewAppletsUI(targetApplet, record);
                                    $('[data-type="ExpectationArea"]').addClass('d-none');
                                    $('[data-type="control"]').attr('disabled', false);
                                    $('[data-type="applet_item"]').attr('disabled', false);
                                });
                        }
                    });
                }
            },


            //@C  �������� ����� ������
            NewRecord: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet) {
                    return new Promise((resolve, reject) => {
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let view = currentElementsInfo['View'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let popupApplet = currentElementsInfo['PopupApplet'];
                        let currentApplet = currentElementsInfo['CurrentApplet'];
                        let currentControl = currentElementsInfo['CurrentControl'];

                        // ������ �� �������� ����� ������
                        GSCrmInfo.Application.CommonRequests.NewRecord(currentApplet, recordSet)
                            .fail(response => {
                                response = response['responseJSON'];
                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(response['ErrorMessages']);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', errorMessage);
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                reject(response['ErrorMessages']);
                            })
                            .done(response => {
                                let newRecord = response['NewRecord'];
                                GSCrmInfo.SetElement("CurrentRecord", newRecord);
                                if (!Object.is(popupApplet, null)) {
                                    GSCrmInfo.Application.CommonAction.DisposePopupApplet('NewRecord', newRecord, currentControl)
                                        .catch(error => {
                                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while close popup applet.', errorMessage);
                                            reject(error);
                                        })
                                        .then(() => {
                                            view.PartialUpdateContext(targetApplet, true)
                                                .catch(error => {
                                                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                                    reject(error);
                                                })
                                                .then(() => resolve())
                                        })
                                        .finally(() => {
                                            $('[data-type="ExpectationArea"]').addClass('d-none');
                                            $('[data-type="control"]').attr('disabled', false);
                                            $('[data-type="applet_item"]').attr('disabled', false);
                                        });
                                }
                                else {
                                    view.PartialUpdateContext(targetApplet, true)
                                        .catch(error => {
                                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                            reject(error);
                                        })
                                        .then(() => resolve())
                                        .finally(() => {
                                            $('[data-type="ExpectationArea"]').addClass('d-none');
                                            $('[data-type="control"]').attr('disabled', false);
                                            $('[data-type="applet_item"]').attr('disabled', false);
                                        });
                                }
                            });
                    });
                }
            },

            //@C ������ �������� ������
            UndoRecord: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet) {
                    return new Promise((resolve, reject) => {
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let popupApplet = currentElementsInfo['PopupApplet'];
                        let record = currentElementsInfo['CurrentRecord'];
                        let currentControl = currentElementsInfo['CurrentControl'];
                        if (popupApplet != null) {
                            GSCrmInfo.Application.CommonAction.DisposePopupApplet('UndoRecord', record, currentControl)
                                .catch(error => {
                                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while close popup applet.', errorMessage);
                                    reject(error);
                                })
                                .then(() => resolve())
                                .finally(() => {
                                    $('[data-type="ExpectationArea"]').addClass('d-none');
                                    $('[data-type="control"]').attr('disabled', false);
                                    $('[data-type="applet_item"]').attr('disabled', false);
                                });
                        }
                    })
                }
            },

            //@C ���������� ������
            UpdateRecord: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet) {
                    return new Promise((resolve, reject) => {
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let view = currentElementsInfo['View'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let popupApplet = currentElementsInfo['PopupApplet'];
                        let currentApplet = currentElementsInfo['CurrentApplet'];
                        let currentControl = currentElementsInfo['CurrentControl'];

                        // ���������� ������
                        GSCrmInfo.Application.CommonRequests.UpdateRecord(currentApplet, recordSet)
                            .fail(response => {
                                response = response['responseJSON'];
                                let changedRecord = response['ChangedRecord'];
                                GSCrmInfo.SetElement("CurrentRecord", changedRecord);
                                view.RefreshAppletControlsUI(currentApplet, response['FieldsToUpdate'], changedRecord);
                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(response['ErrorMessages']);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', errorMessage);
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                reject(response['ErrorMessages']);
                            })
                            .done(response => {
                                let changedRecord = response['ChangedRecord'];
                                GSCrmInfo.SetElement("CurrentRecord", changedRecord);
                                if (!Object.is(popupApplet, null)) {
                                    GSCrmInfo.Application.CommonAction.DisposePopupApplet('UpdateRecord', changedRecord, currentControl)
                                        .catch(error => {
                                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while close popup applet.', errorMessage);
                                            reject(error);
                                        })
                                        .then(() => {
                                            view.PartialUpdateContext(targetApplet, true)
                                                .catch(error => {
                                                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                                    reject(error);
                                                })
                                                .then(() => resolve())
                                        })
                                        .finally(() => {
                                            // ������� �������� ��������
                                            $('[data-type="ExpectationArea"]').addClass('d-none');
                                            $('[data-type="control"]').attr('disabled', false);
                                            $('[data-type="applet_item"]').attr('disabled', false);
                                        });
                                }
                                else {
                                    view.PartialUpdateContext(targetApplet, true)
                                        .catch(error => {
                                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                            reject(error);
                                        })
                                        .then(() => resolve())
                                }
                            });
                    });
                }
            },

            //@C �������������� ���������� ������ ��� �������� �� ��������� ������
            AutoUpdateRecord: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet, updateChangedRecord) {
                    return new Promise((resolve, reject) => {
                        // ������, ������� ���� �������� � ������ � ���
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let view = currentElementsInfo['View'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let appletToUpdate = currentElementsInfo['AppletToUpdate'];
                        let updatedRecord = currentElementsInfo['RecordToUpdate'];
                        let currentControl = currentElementsInfo['CurrentControl'];
                        let recordId = updatedRecord['Id'];

                        // �������, ���� �� ��������� ������ � ���������� � ������� ��������� ����� ����������
                        if (updateChangedRecord == undefined)
                            updateChangedRecord = true;

                        let data = {
                            ActionType: 'UpdateRecord',
                            TargetApplet: appletToUpdate['Name'],
                            CurrentRecord: recordId,
                            CurrentControl: currentControl,
                            OpenPopup: false,
                            ClosePopup: false,
                            PopupApplet: null,
                            CurrentPopupControl: null
                        }

                        view.UpdateViewInfo(data)
                            .catch(error => {
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                GSCrmInfo.SetElement('AppletToUpdate', null);
                                GSCrmInfo.SetElement('RecordToUpdate', null);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view info.', errorMessage);
                                reject(error);
                            })
                            .then(() => {
                                // ���������� ������
                                GSCrmInfo.Application.CommonRequests.UpdateRecord(appletToUpdate, updatedRecord)
                                    .fail(response => {
                                        response = response['responseJSON'];
                                        // ���� ���������� ��������������� �� ���������� ������
                                        if (updateChangedRecord) {
                                            let changedRecord = response['ChangedRecord'];
                                            GSCrmInfo.SetElement("CurrentRecord", changedRecord);
                                            view.RefreshAppletControlsUI(appletToUpdate, response['FieldsToUpdate'], changedRecord);
                                        }
                                        let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(response['ErrorMessages']);
                                        GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', errorMessage);
                                        reject(response['ErrorMessages']);
                                    })
                                    .done(response  => {;
                                        // ���� ���������� ��������������� �� ���������� ������
                                        if (updateChangedRecord) {
                                            let changedRecord = response['ChangedRecord'];
                                            GSCrmInfo.SetElement("CurrentRecord", changedRecord);
                                            view.RefreshViewAppletsUI(appletToUpdate, changedRecord);
                                        }
                                        resolve();
                                    })
                                    .always(() => {
                                        $('[data-type="ExpectationArea"]').addClass('d-none');
                                        GSCrmInfo.SetElement('AppletToUpdate', null);
                                        GSCrmInfo.SetElement('RecordToUpdate', null);
                                        $('[data-type="control"]').attr('disabled', false);
                                        $('[data-type="applet_item"]').attr('disabled', false);
                                    });
                            });
                    });
                }
            },

            //@C ������ ���������� ������
            UndoUpdate: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet) {
                    return new Promise((resolve, reject) => {
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let view = currentElementsInfo['View'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let popupApplet = currentElementsInfo['PopupApplet'];
                        let currentApplet = currentElementsInfo['CurrentApplet'];
                        let currentRecord = currentElementsInfo['CurrentRecord'];
                        let currentControl = currentElementsInfo['CurrentControl'];

                        // ������ ������
                        GSCrmInfo.Application.CommonRequests.UndoUpdate(currentApplet, currentRecord)
                            .fail(error => {
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', 'An error occured while canceling updating record.');
                                reject(error);
                            })
                            .done(changedRecord => {
                                GSCrmInfo.SetElement("CurrentRecord", changedRecord);
                                if (popupApplet != null) {
                                    GSCrmInfo.Application.CommonAction.DisposePopupApplet('UndoUpdate', changedRecord, currentControl)
                                        .catch(error => {
                                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while dispose popup applet.', errorMessage);
                                            reject(error);
                                        })
                                        .then(() => resolve())
                                        .finally(() => {
                                            // ������� �������� ��������
                                            view.RefreshViewAppletsUI(targetApplet, changedRecord);
                                            $('[data-type="ExpectationArea"]').addClass('d-none');
                                            $('[data-type="control"]').attr('disabled', false);
                                            $('[data-type="applet_item"]').attr('disabled', false);
                                        });
                                }
                            })
                    })
                };
            },

            //@C �������� ������
            DeleteRecord: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet) {
                    return new Promise((resolve, reject) => {
                        // ������������ � ������, �� ������� ��������� �������
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let view = currentElementsInfo['View'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let popupApplet = currentElementsInfo['PopupApplet'];
                        let currentApplet = currentElementsInfo['CurrentApplet'];
                        let recordId = currentElementsInfo['CurrentRecord']['Id'];
                        let currentControl = currentElementsInfo['CurrentControl'];

                        let data = {
                            Id: recordId,
                            AppletName: targetApplet.GetName()
                        };
                        
                        GSCrmInfo.Application.CommonRequests.DeleteRecord(currentApplet, data)
                            .fail(error => {
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', error['responseJSON'][0]);
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                reject(error['responseJSON']);
                            })
                            .done(record => {
                                GSCrmInfo.SetElement("CurrentRecord", record);
                                // ��������� �������� ���������� ������� � �������������
                                recordId = record == null ? null : record['Id'];
                                let data = {
                                    ActionType: 'DeleteRecord',
                                    TargetApplet: targetApplet.GetName(),
                                    CurrentRecord: recordId,
                                    CurrentControl: currentControl,
                                    OpenPopup: false,
                                    ClosePopup: false,
                                    PopupApplet: null,
                                    CurrentPopupControl: null
                                }
                                
                                // ���������� ���������� � �������������
                                view.UpdateViewInfo(data)
                                    .catch(error => {
                                        $('[data-type="ExpectationArea"]').addClass('d-none');
                                        $('[data-type="control"]').attr('disabled', false);
                                        $('[data-type="applet_item"]').attr('disabled', false);
                                        let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                        GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view info.', errorMessage);
                                        reject(error);
                                    })
                                    .then(() => {
                                        // ��������� ���������� ��������� ������������� � ����������� �������� �������
                                        view.PartialUpdateContext(targetApplet, true)
                                            .catch(error => {
                                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                                reject(error);
                                            })
                                            .then(() => resolve())
                                            .finally(() => {
                                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                                $('[data-type="control"]').attr('disabled', false);
                                                $('[data-type="applet_item"]').attr('disabled', false);
                                            })
                                    });
                            });
                    });
                }
            },

            //@C Apply �������
            ApplyTable: class extends GSCrmInfo.Application.CommonAction {
                Initialize() {
                    return new Promise((resolve, reject) => {
                        let targetApplet = GSCrmInfo['CurrentElementsInfo']['TargetApplet'];
                        GSCrmInfo.Application.CommonRequests.ApplyTable(targetApplet)
                            .fail(error => {
                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while applying a table.', errorMessage);
                                reject(error);
                            })
                            .done(() =>resolve())
                            .always(() => {
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                            });
                    });
                }
            },

            //@C ��������� ���� ��� ��������
            Publish: class extends GSCrmInfo.Application.CommonAction {
                Initialize() {
                    return new Promise((resolve, reject) => {
                        let targetApplet = GSCrmInfo['CurrentElementsInfo']['TargetApplet'];
                        GSCrmInfo.Application.CommonRequests.Publish(targetApplet)
                            .fail(error => {
                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while publishing.', errorMessage);
                                reject(error);
                            })
                            .done(() =>resolve())
                            .always(() => {
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                            });
                    });
                }
            },

            //@C ����������
            ExecuteQuery: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event, recordSet) {
                    return new Promise((resolve, reject) => {
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let view = currentElementsInfo['View'];
                        GSCrmInfo.Application.CommonRequests.ExecuteQuery(targetApplet, recordSet)
                            .fail(error => {
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while querying.', errorMessage);
                                reject(error);
                            })
                            .done(() => {
                                // ��������� ���������� ��������� �������������
                                view.PartialUpdateContext(targetApplet.GetInfo(), false)
                                    .catch(error => {
                                        let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                        GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                        reject(error);
                                    })
                                    .then(() => resolve())
                                    .finally(() => {
                                        $('[data-type="ExpectationArea"]').addClass('d-none');
                                        $('[data-type="control"]').attr('disabled', false);
                                        $('[data-type="applet_item"]').attr('disabled', false);
                                    });
                            });
                    });
                }
            },

            //@C ������ ����������
            CancelQuery: class extends GSCrmInfo.Application.CommonAction {
                Initialize() {
                    return new Promise((resolve, reject) => {
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let view = currentElementsInfo['View'];
                        GSCrmInfo.Application.CommonRequests.CancelQuery(targetApplet)
                            .fail(error => {
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while querying.', errorMessage);
                                reject(error);
                            })
                            .done(() => {
                                // ��������� ���������� ��������� �������������
                                view.PartialUpdateContext(targetApplet.GetInfo(), false)
                                    .catch(error => {
                                        let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                        GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                        reject(error);
                                    })
                                    .then(() => resolve())
                                    .finally(() => {
                                        $('[data-type="ExpectationArea"]').addClass('d-none');
                                        $('[data-type="control"]').attr('disabled', false);
                                        $('[data-type="applet_item"]').attr('disabled', false);
                                    });
                            });
                    });
                }
            },

            //@C ����������� ������
            CopyRecord: class extends GSCrmInfo.Application.CommonAction {
                Initialize() {
                    return new Promise((resolve, reject) => {
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let view = currentElementsInfo['View'];
                        let currentApplet = currentElementsInfo['CurrentApplet'];
                        let targetApplet = currentElementsInfo['TargetApplet'];
                        let popupApplet = currentElementsInfo['PopupApplet'];
                        let currentControl = currentElementsInfo['CurrentControl'];
                        let currentPopupControl = currentElementsInfo['CurrentPopupControl'];
                        let controlProperties = currentApplet.GetControlProperties(currentControl);
                        GSCrmInfo.Application.CommonRequests.CopyRecord(controlProperties)
                            .fail(response => {
                                if (response["status"] != undefined) {
                                    switch(response["status"]) {
                                        case 404:
                                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(["Invalid address for map data."]);
                                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while coping a record: ', errorMessage);
                                            break;
                                    }
                                }
                                else {
                                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(response['ErrorMessages']);
                                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while coping a record: ', errorMessage);
                                    reject(response['ErrorMessages']);
                                }
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                            })
                            .done(response => {
                                let record = response['NewRecord'];
                                GSCrmInfo.SetElement("CurrentRecord", record);
                                GSCrmInfo.SetElement('AppletToUpdate', targetApplet);
                                GSCrmInfo.SetElement('RecordToUpdate', record);
                                if (popupApplet != null) {
                                    GSCrmInfo.Application.CommonAction.DisposePopupApplet('CopyRecord', record, currentControl)
                                        .catch(error => {
                                            $('[data-type="ExpectationArea"]').addClass('d-none');
                                            $('[data-type="control"]').attr('disabled', false);
                                            $('[data-type="applet_item"]').attr('disabled', false);
                                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while close popup applet.', errorMessage);
                                            reject(error);
                                        })
                                        .then(() => {
                                            view.PartialUpdateContext(targetApplet, true)
                                                .catch(error => {
                                                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                                    reject(error);
                                                })
                                                .then(() => resolve())
                                                .finally(() => {
                                                    $('[data-type="ExpectationArea"]').addClass('d-none');
                                                    $('[data-type="control"]').attr('disabled', false);
                                                    $('[data-type="applet_item"]').attr('disabled', false);
                                                });
                                        });
                                }
                                else {
                                    view.PartialUpdateContext(targetApplet, true)
                                        .catch(error => {
                                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view context.', errorMessage);
                                            reject(error);
                                        })
                                        .then(() => resolve())
                                        .finally(() => {
                                            $('[data-type="ExpectationArea"]').addClass('d-none');
                                            $('[data-type="control"]').attr('disabled', false);
                                            $('[data-type="applet_item"]').attr('disabled', false);
                                        });
                                }
                            });
                    });
                }
            },

            // ������� �� drilldown
            Drilldown: class extends GSCrmInfo.Application.CommonAction {
                Initialize(event) {
                    return new Promise((resolve, reject) => {
                        let currentElementsInfo = GSCrmInfo.CurrentElementsInfo;
                        let screen =  currentElementsInfo['Screen'];
                        let view = currentElementsInfo['View'];
                        let currentApplet = currentElementsInfo['CurrentApplet'];
                        let popupApplet = currentElementsInfo['PopupApplet'];
                        let currentRecord =  currentElementsInfo['CurrentRecord'];
                        let recordId = currentRecord == null ? null : currentRecord['Id'];
                        let currentControl = currentElementsInfo['CurrentControl'];
                        let currentPopupControl = currentElementsInfo['CurrentPopupControl'];
                        let currentColumn = $(event.currentTarget).closest('[data-type="cell"]');
                        let currentColumnName = currentColumn == undefined ? null : currentColumn.attr('data-name');

                        let data = {
                            ActionType: 'Drilldown',
                            TargetApplet: currentApplet['Name'],
                            CurrentRecord: recordId,
                            CurrentControl: currentControl,
                            CurrentColumn: currentColumnName,
                            OpenPopup: false,
                            ClosePopup: false,
                            PopupApplet: popupApplet,
                            CurrentPopupControl: currentPopupControl
                        }

                        view.UpdateViewInfo(data)
                            .catch(error => {
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                $('[data-type="control"]').attr('disabled', false);
                                $('[data-type="applet_item"]').attr('disabled', false);
                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view info.', errorMessage);
                            })
                            .then(() => {
                                GSCrmInfo.Application.CommonRequests.Drilldown()
                                    .fail(error => {
                                        let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                        GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view info.', errorMessage);
                                    })
                                    .done(() => document.location.reload());
                            });
                        
                        resolve();
                    });
                }
            }
        }[actionType];
        return actions.Initialize(event, recordSet, payload);
    }

    static DisposePopupApplet(action, record, control) {
        return new Promise((resolve, reject) => {
            let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
            let view = currentElementsInfo['View'];
            let targetApplet = currentElementsInfo['TargetApplet'];
            let popupApplet = currentElementsInfo['PopupApplet'];
            let currentControl = currentElementsInfo['currentControl'];
            let recordId = record == null ? null : record['Id'];
            if (!Object.is(popupApplet, null)) {
                // ����������� ������
                popupApplet.Dispose();
                GSCrmInfo.SetElement("PopupApplet", null);
                GSCrmInfo.SetElement('CurrentApplet', targetApplet);

                // ���������� ���������� � �������������
                let data = {
                    ActionType: action,
                    TargetApplet: targetApplet.GetName(),
                    CurrentRecord: recordId,
                    CurrentControl: currentControl,
                    OpenPopup: false,
                    ClosePopup: true,
                    PopupApplet: null,
                    CurrentPopupControl: control
                }

                view.UpdateViewInfo(data)
                    .catch(error => {
                        let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                        GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating view info.', errorMessage);
                        reject(error);
                    })
                    .then(() => resolve());
            }
        })
    }

    static RaiseErrorText(title, errorText) {
        let PR = new DefaultErrorPR();
        PR.RaiserError(title, errorText);
    }

    static RenderError(errorMessages) {
        let PR = new DefaultErrorPR();
        return PR.RenderError(errorMessages);
    }
}