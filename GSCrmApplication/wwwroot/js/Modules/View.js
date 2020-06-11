import DefaultViewPR from '../PhysicalRenders/DefaultViewPR.js'

export default (function () {
    let view = function(name) {
        this.Name = name;
        this.Info = GSCrmInfo.GetViewInfo(name);
        if (this.Info != undefined)
            this.Id = this.Info['ViewId'];
    }

    //#region //@R ��������
    // �������� ���������� � ������������� �� ��� ��������
    view.prototype.GetId = function() {
        return this.Id;
    }

    // ���������� ���� ��� �������������
    view.prototype.GetName = function() {
        return this.Name;
    }

    view.prototype.GetInfo = function() {
        return this.Info;
    }
    //#endregion

    //#region //@R �������������
    // �������������� �������������
    view.prototype.Initialize = function(screen) {
        return new Promise((resolve, reject) => {
        // ����������� �������� ��������
        $('[data-type="ExpectationArea"]').removeClass('d-none');

        // ������������� �������� � �������������, ����������� ���������� � ���
        GSCrmInfo.Application.CommonRequests.InitializeView(this.Name)
            .fail(error => reject(error))
            .done(viewInfo => {
                // ��������� �������������
                let PR = new DefaultViewPR();
                let view = PR.RenderView(this);

                // ������������ id � ����������
                viewInfo['ViewId'] = $(view).attr('id');
                this.Id = $(view).attr('id');
                this.Info = viewInfo;

                // ������� ���������� � ������ ������������� � ��������� ���������� � �����
                GSCrmInfo.RemoveOldViewInfo();
                GSCrmInfo.SetViewInfo(this.Name, viewInfo);

                // ���������� ������������� � ����� � ������������� ��������
                $(screen).append(view);
                GSCrmInfo.Application.CommonRequests.UpdateContext(this.Name)
                    .catch(error => reject(error))
                    .then(() => {
                        this.InitializeItems(view, viewInfo['ViewItems'], 0)
                            .catch(error => reject(error))
                            .then(() => {
                                $(screen).trigger('OnViewLoad', []);
                                $('[data-type="ExpectationArea"]').addClass('d-none');
                                resolve(this);
                            });
                    });
            });
        });
    };

    // �������������� �������� �������������
    view.prototype.InitializeItems = function(view, applets, count) {
        return new Promise((resolve, reject) => {
            if (applets[count] != undefined) {
                let appletName = applets[count]['AppletName'];
                let appletId = "GSA_0";
                let counter = 0;

                // �� ��� ���, ���� ������� � ����� id ������������ �� ��������, ��������� 1, ����� Id ��� ����������
                while ($('#' + appletId).length > 0) {
                    appletId = appletId.split(counter).join((counter + 1));
                    counter++;
                }

                switch (applets[count]['Type']) {
                    case "Tile":
                        // ���������� ������� � ��������
                        view.append('<div data-type="applet" data-name="' + appletName + '" id="' + appletId + '"></div>');

                        // �������������
                        let tileApplet = new GSCrmInfo.Application.TileApplet(appletName, appletId);
                        tileApplet.Initialize()
                            .catch(error => reject(error))
                            .then(() => {
                                count++;

                                // ������������� ���������� ��������
                                this.InitializeItems(view, applets, count)
                                    .catch(error => reject(error))
                                    .then(() => resolve());
                            });
                        break;

                    case "Form":
                        // ���������� ������� � ��������
                        view.append('<div data-type="applet" data-name="' + appletName + '" id="' + appletId + '"></div>');

                        // �������������
                        let formApplet = new GSCrmInfo.Application.FormApplet(appletName, appletId);
                        formApplet.Initialize()
                            .catch(error => reject(error))
                            .then(() => {
                                count++;

                                // ������������� ���������� ��������
                                this.InitializeItems(view, applets, count)
                                    .catch(error => reject(error))
                                    .then(() => resolve());
                            });
                        break;
                }
            }
            else resolve();
        })
    };
    //#endregion

    //#region //@R ������ ��� ���������� ���������� � ��������� �������������
    // ����������� ���������� � ������������� � ����
    view.prototype.RequestInfo = function() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetViewInfo()
                .fail(error => reject(error))
                .done(info => {
                    GSCrmInfo.RemoveOldViewInfo();
                    GSCrmInfo.SetViewInfo(this.Name, info);
                    resolve();
                });
        });
    };

    // ��������� ��������� � �������������
    view.prototype.UpdateViewInfo = function(data) {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.UpdateViewInfo(data)
                .fail(error => reject(error))
                .done(info => {
                    GSCrmInfo.RemoveOldViewInfo();
                    GSCrmInfo.SetViewInfo(this.Name, info);
                    resolve();
                });
        });
    };

    // ������ ���������� ���������
    view.prototype.UpdateContext = function() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.UpdateContext(this.Name)
                .fail(error => reject(error))
                .done(applets => {
                    if (applets.length > 0) {
                        this.RefreshApplets(applets, 0)
                            .catch(error => reject(error))
                            .then(() => resolve());
                    }
                    else resolve();
                });
        });
    };

    // ��������� ���������� ���������
    view.prototype.PartialUpdateContext = function(applet, refreshCurrentApplet) {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.PartialUpdateContext(applet, refreshCurrentApplet)
                .fail(error => reject(error))
                .done(applets => {
                    if (applets.length > 0) {
                        this.RefreshApplets(applets, 0)
                            .catch(error => reject(error))
                            .then(() => resolve());
                    }
                    else resolve();
                });
        });
    };

    // ���������� ��������� ��� ������ ��������
    view.prototype.RefreshApplets = function(applets, count) {
        return new Promise((resolve, reject) => {
            if (applets[count] != undefined) {
                let currentApplet;
                let appletName = applets[count];
                switch (this.GetAppletByName(appletName)['Type']) {
                    case "Tile":
                        currentApplet = new GSCrmInfo.Application.TileApplet(appletName, null);
                        break;
                    case "Form":
                        currentApplet = new GSCrmInfo.Application.FormApplet(appletName, null);
                        break;
                }
                currentApplet.Initialize()
                    .catch(error => reject(error))
                    .then(() => {
                        count++;
                        this.RefreshApplets(applets, count);
                        resolve();
                    });
            }
        });
    };

    // ���������� ��������� ��� ������ �������
    view.prototype.RefreshApplet = function(appletName) {
        return new Promise((resolve, reject) => {
            let currentApplet;
            switch (this.GetAppletByName(appletName)['Type']) {
                case "Tile":
                    currentApplet = new GSCrmInfo.Application.TileApplet(appletName, null);
                    break;
                case "Form":
                    currentApplet = new GSCrmInfo.Application.FormApplet(appletName, null);
                    break;
            }
            currentApplet.Initialize()
                .catch(error => reject(error))
                .then(() => {
                    count++;
                    this.RefreshApplets(applets, count);
                    resolve();
                });
        });
    };

    // ���������� ����������� ��������
    view.prototype.RefreshViewAppletsUI = function(appletToUpdate, newRecord) {
        let appletsToUpdate = this.GetRelatedApplets(appletToUpdate['Name']);
        let appletToUpdateType = appletToUpdate.GetInfo()['Type'];
        appletsToUpdate.push(appletToUpdate);
        appletsToUpdate.map(currentApplet => {
            let currentAppletType = currentApplet.GetInfo()['Type'];
            switch(currentAppletType) {
                case "Tile":
                    // ���������� �������� ������� �� newRecord
                    if (currentApplet['Name'] == appletToUpdate['Name'])
                        currentApplet.UpdateRecordInRS(newRecord, "Id", newRecord['Id']);

                    // ��� ��������� �������� ���������� �������� �� recordSet � �������� ��
                    else {
                        let changedRecord = currentApplet.GetRecordByProperty("Id", newRecord[["Id"]])[0];
                        if (changedRecord != undefined) {
                            // ������������ ��� ��������, � ����� ������
                            for (let property in newRecord) {
                                // ��������� ���������(��������� ��� �������) �������, ����������� ��� ���������� �� �������� �������� �� recordSet-�
                                let updatedAppletElements = [];
                                if (appletToUpdateType == "Tile") {
                                    updatedAppletElements = appletToUpdate.GetColumnsByPropertyValue("Name", property);
                                }
                                else {
                                    updatedAppletElements = appletToUpdate.GetControlsByPropertyValue("Name", property);
                                }
                                updatedAppletElements.map(element => {
                                    // �������� �� undefined, ����� ��������� �� ������������ ��������
                                    if (element != undefined) {
                                        let fieldName = element['FieldName'];
                                        // ���� ���������� ��� ����� �������� ���� � ���������� �� ������� � ��� ����� �� ������
                                        if (fieldName != null && fieldName != "") {
                                            // � ������� ������� ������ ������, ���������� �� ���� �����, � �� �������� � ����� ����������� ������ ���������� ����� ���������
                                            let appletColumns = currentApplet.GetColumnsByPropertyValue("FieldName", fieldName);
                                            appletColumns.map(appletColumn => {
                                                if (appletColumn != undefined) {
                                                    changedRecord[appletColumn['Name']] = newRecord[property];
                                                }
                                            })
                                        }
                                    }
                                })
                            }
                            currentApplet.UpdateRecordInRS(changedRecord, "Id", changedRecord['Id']);
                        }
                    }
                    //console.log(currentApplet.GetRecordByProperty("Id", newRecord["Id"])[0])
                    currentApplet.RefreshTileItem(currentApplet.GetTileItemByIdInRS(newRecord['Id']));
                    break;
                case "Popup":
                case "Form":
                    // ���������� �������� ������� �� newRecord
                    if (currentApplet['Name'] == appletToUpdate['Name'])
                        currentApplet.UpdateRecordInRS(newRecord);

                    // ��� ��������� �������� ���������� �������� �� recordSet � �������� ��
                    else {
                        let changedRecord = currentApplet.GetRecordSet();
                        if (changedRecord != undefined) {
                            // ������������ ��� ��������, � ����� ������
                            for (let property in newRecord) {
                                // ��������� ���������(��������� ��� �������) �������, ����������� ��� ���������� �� �������� �������� �� recordSet-�
                                let updatedAppletElements;
                                if (appletToUpdateType == "Tile") {
                                    updatedAppletElements = appletToUpdate.GetColumnsByPropertyValue("Name", property);
                                }
                                else {
                                    updatedAppletElements = appletToUpdate.GetControlsByPropertyValue("Name", property);
                                }
                                updatedAppletElements.map(element => {
                                    if (element != undefined) {
                                        // �������� �� undefined, ����� ��������� �� ������������ ��������
                                        let fieldName = element['FieldName'];
                                        // ���� ���������� ��� ����� �������� ���� � ���������� �� ������� � ��� ����� �� ������
                                        if (fieldName != null && fieldName != "") {
                                            // � ������� ������� ������ ��������, ���������� �� ���� �����, � �� �������� � ����� ����������� ������ ���������� ����� ���������
                                            let appletControls = currentApplet.GetControlsByPropertyValue("FieldName", fieldName);
                                            appletControls.map(appletControl => {
                                                if (appletControl != undefined) {
                                                    changedRecord[appletControl['Name']] = newRecord[property];
                                                }
                                            })
                                        }
                                    }
                                })
                            }
                            currentApplet.UpdateRecordInRS(changedRecord, "Id", changedRecord['Id']);
                        }
                    }
                    //console.log(currentApplet.GetRecordSet())
                    currentApplet.RefreshControls();
                    break;
            }
        });
    }

    // ���������� ����������� ��������� � ��������� ��������
    view.prototype.RefreshAppletControlsUI = function(appletToUpdate, fieldsToUpdate, record) {
        let appletsToUpdate = this.GetRelatedApplets(appletToUpdate['Name']);
        appletsToUpdate.push(appletToUpdate);
        appletsToUpdate.map(currentApplet => {
            let changedRecord;
            let currentAppletType = currentApplet.GetInfo()['Type'];
            switch(currentAppletType) {
                case "Tile":
                    let columnsToUpdate = [];
                    changedRecord = currentApplet.GetRecordByProperty("Id", record["Id"])[0];
                    if (changedRecord != undefined) {
                        fieldsToUpdate.map(field => {
                            columnsToUpdate.push(currentApplet.GetColumnsByPropertyValue("FieldName", field).filter(item => item != undefined));
                            columnsToUpdate.map(columnToUpdate => {
                                if (columnToUpdate[0] != undefined) {
                                    changedRecord[columnToUpdate[0]['Name']] = record[columnToUpdate[0]['Name']];
                                }
                            })
                        });
                        currentApplet.UpdateRecordInRS(changedRecord, "Id", changedRecord['Id']);
                        columnsToUpdate.map(columnToUpdate => {
                            if (columnToUpdate[0] != undefined) {
                                currentApplet.RefreshCell(currentApplet.GetTileItemByIdInRS(changedRecord['Id']), columnToUpdate[0]['Name']);
                            }
                        });
                    }
                    break;
                case "Form":
                case "Popup":
                    let controlsToUpdate = [];
                    changedRecord = currentApplet.GetRecordSet();
                    if (changedRecord != undefined) {
                        fieldsToUpdate.map(field => {
                            controlsToUpdate.push(currentApplet.GetControlsByPropertyValue("FieldName", field).filter(item => item != undefined));
                            controlsToUpdate.map(controlToUpdate => {
                                if (controlToUpdate[0] != undefined) {
                                    changedRecord[controlToUpdate[0]['Name']] = record[controlToUpdate[0]['Name']];
                                }
                            })
                        });
                        currentApplet.UpdateRecordInRS(changedRecord, "Id", changedRecord['Id']);
                        controlsToUpdate.map(controlToUpdate => {
                            if (controlToUpdate[0] != undefined) {
                                currentApplet.RefreshControl(controlToUpdate[0]['Name']);
                            }
                        });
                    }
                    break;
            }
        });
    }
    //#endregion

    //#region //@R ������ ��� ������ � ��������
    // ���������� id �������, ������ �������� ��������� �������
    view.prototype.GetAppletId = function(event) {
        return $(event.currentTarget).closest('[data-type="applet"]').attr('id');
    };

    // �� ������� �� ������� ���������� ��� ��������
    view.prototype.GetAppletName = function(event) {
        return this.GetAppletNameById(this.GetAppletId(event));
    };

    // �� �������� ������� ���������� ��� id
    view.prototype.GetAppletIdByName = function(appletName) {
        let appletId;
        GSCrmInfo.AppletsInfo.forEach((applet, index) => {
            if (index == appletName) {
                appletId = applet['AppletId'];
            }
        });
        return appletId;
    };

    // �� id ������� ���������� ��� ��������
    view.prototype.GetAppletNameById = function(appletId) {
        let appletName;
        GSCrmInfo.AppletsInfo.forEach((applet, index) => {
            if (applet['AppletId'] == appletId) {
                appletName = index;
            }
        });
        return appletName;
    };

    // �� �������� ������� ���������� ���������� � ��� �� �������������
    view.prototype.GetAppletByName = function(appletName) {
        return GSCrmInfo['ViewsInfo'].get(this.Name)['ViewItems'].filter(n => n['AppletName'] == appletName)[0];
    };

    // ���������� ������ ��������� ��������
    view.prototype.GetRelatedApplets = function(appletName) {
        let relatedApplets = [];
        let appletInfo = GSCrmInfo.GetAppletInfo(appletName);
        GSCrmInfo.AppletsInfo.forEach(applet => {
            if (applet['BusCompName'] == appletInfo['BusCompName'] && applet['Name'] != appletName && applet['Initflag'] == false)
            {
                switch(applet['Type']) {
                    case "Tile":
                        relatedApplets.push(new GSCrmInfo.Application.TileApplet(applet['Name'], applet['Id']));
                        break;
                    case "Form":
                        relatedApplets.push(new GSCrmInfo.Application.FormApplet(applet['Name'], applet['Id']));
                        break;
                    case "Popup":
                        relatedApplets.push(new GSCrmInfo.Application.PopupApplet(applet['Name'], applet['Id']));
                        break;
                }
            }
        });
        return relatedApplets;
    }

    // ���������� id ����, � ������� ��� ������ ������� ������/������� � �������
    view.prototype.GetTargetId = function(event) {
        return $(event.currentTarget).closest('[data-target-id]').attr('data-target-id');
    };

    // �������� ���� � �������(��������� �� �������� � ���������)
    view.prototype.CloseSelectArea = function() {
        // �������, � �������� ��������� �������� ����
        let el = $("#" + $('[data-type="SelectArea"]').attr('data-target-id'));
        el.removeClass('gs-field-is-focused');
        if (el.find('.gs-field-input').val() != '') {
            el.addClass('gs-field-is-filled')
        }
        else {
            el.removeClass('gs-field-is-filled')
        }

        // ��������� �������� ��� �������� � recordSet-�
        let appletName = this.GetAppletNameById(el.closest('[data-type="applet"]').attr('id'));
        let applet = new GSCrmInfo.Application.Applet(appletName, null);
        applet.RecordSet[el.attr('data-name')] = el.find('.gs-field-input').val();

        // �������� ���� � �������
        $('[data-type="SelectArea"]')
            .addClass('d-none')
            .removeAttr('style')
            .removeAttr('data-target-id')
            .empty();

        // ������� ������� � ����� �������� � ����������� ���������
        $('.pick-area')
            .css('color', 'lightgrey')
            .find('.icon-chevron-thin-left')
            .removeClass('icon-chevron-thin-left')
            .addClass('icon-chevron-thin-right');
    }
    //#endregion

    return view;
})()