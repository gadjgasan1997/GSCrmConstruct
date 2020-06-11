import DefaultTileAppletPR from '../PhysicalRenders/DefaultTileAppletPR.js'

export default function (application) {
    let tileApplet = function(name, id) {
        application.prototype.Applet.apply(this, [name, id]);
    }
    
    tileApplet.prototype = Object.create(application.prototype.Applet.prototype);

    //#region //@R ��������
    // ��� ����������� �� �������� ������
    tileApplet.prototype.Focus = function (tileItem) {
        // ������ ����� ������������������, ��� ��� ����� ���������� ������� �������� ���������, ���������� ������� ���������� ������� ������
        // 0 ���
        // �������� ������ � ����������� ��������
        GSCrmInfo.SetSelectedRecord(this.Name, {
            "record": tileItem,
            "properties": this.GetRecordFromRSByIndex(this.GetSelection(tileItem))
        });

        // 1 ���
        // ���������� �������
        tileItem[0].dispatchEvent(new CustomEvent('FocusAppletItem', {
            detail: {}
        }));
    };

    // ���������� ������, �� ������� � ������ ������ � �������� ��������� �����
    tileApplet.prototype.SelectedRecords = function () {
        var records = {};
        GSCrmInfo.SelectedRecords.forEach((item, applet) => {
            records[applet] = item;
        });
        return records;
    };

    // ���������� ������ ������ � recordSet-�
    tileApplet.prototype.FirstRecord = function () {
        return this.GetRecordSet()[0];
    };

    // ���������� ��������� ������ � recordSet-�
    tileApplet.prototype.LastRecord = function () {
        return this.GetRecordSet()[this.GetRecordSet().length - 1];
    };

    // ���������� ��� �������� ���� �������
    tileApplet.prototype.Columns = function () {
        return this.Info['Columns'];
    };

    // ���������� ������ �� �����
    tileApplet.prototype.TileItems = function () {
        return $('#' + this.Info['AppletId']).find('.' + this.Info['itemClass']);
    };
    //#endregion

    //#region //@R �������������
    //@M ������������� ���� �������
    tileApplet.prototype.Initialize = function () {
        return new Promise((resolve, reject) => {
            this.InitializeApplet(this.Name)
                .catch(error => reject(error))
                .then(() => {
                    GSCrmInfo.Application.CommonRequests.GetRecords(this.Info)
                        .fail(error => reject(error['responseJSON']))
                        .done(recordsInfo => {
                            let displayedRecords = recordsInfo['DisplayedRecords'];
                            let selectedRecords = recordsInfo['SelectedRecords'];

                            // ���������� recordSet-�
                            this.SetRecordSet(displayedRecords);
                            
                            // ���� ���������� ���������� ������� 0
                            if (displayedRecords.length == 0) {
                                // ��� ������������� ������������� �, �����, ���� � ������� �� ����� ���� ������ ���� �������� empty state
                                let PR = new DefaultTileAppletPR;
                                PR.RenderApplet(this.Name, displayedRecords.length);
                                this.InitializeControls();
                            }
        
                            // �����
                            else {
                                // ������������� ������, ���� ���������� ���������� ������� �� ��������� � ����������� ����� � �������
                                if (displayedRecords.length != this.Info['TileItems'].length) {
                                    let PR = new DefaultTileAppletPR;
                                    PR.RenderApplet(this.Name, displayedRecords.length);
                                    this.InitializeControls();
                                }
                                this.InitializeRows();
                            }
        
                            // ���������� �������� � ������� ��������� �������
                            let recordId = selectedRecords[this.Name];
                            let tileItem = this.GetTileItemByIdInRS(recordId);
                            let properties = this.GetRecordByProperty('Id', recordId);
                            if (properties.length != 0) {
                                let selectRecordInfo = {};
                                selectRecordInfo['properties'] = properties;
                                selectRecordInfo['record'] = tileItem;
                                GSCrmInfo.SetSelectedRecord(this.Name, selectRecordInfo);
                            }
                            else GSCrmInfo.SetSelectedRecord(this.Name, null);
                            
                            // �����������
                            if (displayedRecords.length > 0) {
                                this.Focus(tileItem);
                            }

                            //this.Info['TileItems'] = $('#' + this.Id).find('[data-type="applet_item"]');
        
                            resolve(recordsInfo);
                        });
                });
        });
    };

    //@M ������������� �����
    tileApplet.prototype.InitializeRows = function () {
        // ��� ������ ������ � �������
        $('#' + this.Id + ' [data-type="applet_item"]')
            .each((selection, row) => {
                // ������� ������
                $(row)
                    // ������������ id � ������
                    .attr('id', 'tile_' + this.Id + '_item_' + selection)
                    .addClass(this.Info['itemClass'])
                    // ����������� �������
                    .off('mouseover')
                    .off('mouseout')
                    .off('click')
                    .off('TileItemSelect')
                    .on('mouseover', event => {
                        $(event.currentTarget).trigger('AppletItemMouseOver', []);
                    })
                    .on('mouseout', event => {
                        $(event.currentTarget).trigger('AppletItemMouseOut', []);
                    })
                    .on('click', event => {
                        $(event.currentTarget).trigger('TileItemSelect', []);
                    })
                    .on('TileItemSelect', event => {
                        GSCrmInfo.SetUpCurrentElements(event);
                        GSCrmInfo['CurrentElementsInfo']['TargetApplet'].SelectTileItem(event);
                    })
                    // ������� �����
                    .find('[data-type="cell"]')
                    .map((index, cell) => {
                        let columnName = cell.getAttribute('data-name');
                        let properties = this.GetColumnProperties(columnName)[0];
                        let cellValue = this.GetRecordSet()[selection][columnName];
                        let PR = new DefaultTileAppletPR();
                        PR.RenderCell(cell, cellValue, properties);
                        $(cell)
                            .off('CellCnange')
                            .on('CellCnange', (event, args) => {
                                if (!properties['Readonly']) {
                                    args['Event'].stopPropagation();

                                    /* ����� ���, ��� ������ recordSet, ���������� ���������, ��� � ��� ���������� ���������, 
                                    ����� ���� ������� �������� ��������� ���������� ������
                                    ����� ��������� �������� �� ��, ��� ������ ��� ���������� �� ������ ���� ������� */
                                    let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                                    let view = currentElementsInfo['View'];
                                    let targetApplet = new GSCrmInfo.Application.TileApplet(view.GetAppletName(event), view.GetAppletId(event));
                                    let currentRecord = this.GetRecordSet()[selection];
                                    let appletToUpdate = currentElementsInfo['AppletToUpdate'];
                                    let recordToUpdate = currentElementsInfo['RecordToUpdate'];

                                    if (appletToUpdate != null && recordToUpdate['Id'] != currentRecord['Id']) {
                                        // ���������� ���������� � ������� ��������� ���������
                                        GSCrmInfo.SetUpCurrentElements(event);
                                        
                                        // ����� �������������� ������
                                        GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, recordToUpdate)
                                            .catch(error => console.log(error))
                                            .then(() => {
                                                // �������� ��� ���������� ���������������� ������, ��� ��� � ������ AutoUpdateRecord ���������� �� ���������
                                                if (this.Info['Initflag'] == false) {
                                                    GSCrmInfo.SetElement('AppletToUpdate', this);
                                                    GSCrmInfo.SetElement('RecordToUpdate', currentRecord);
                                                }
                                                targetApplet.SelectTileItem(event)
                                                    .catch(error => console.log(error))
                                                    .then(() => view.RefreshViewAppletsUI(targetApplet, targetApplet.GetRecordSet()[
                                                        this.GetSelection($(event.currentTarget).closest('[data-type="applet_item"]'))
                                                    ]));
                                        });
                                    }
                                    
                                    else {
                                        if (this.Info['Initflag'] == false) {
                                            GSCrmInfo.SetElement('AppletToUpdate', this);
                                            GSCrmInfo.SetElement('RecordToUpdate', currentRecord);
                                        }
                                        targetApplet.SelectTileItem(event)
                                            .catch(error => console.log(error))
                                            .then(() => view.RefreshViewAppletsUI(targetApplet, targetApplet.GetRecordSet()[
                                                this.GetSelection($(event.currentTarget).closest('[data-type="applet_item"]'))
                                            ]));
                                    }
                                    
                                    // ���������� recordSet-� � ������������ �������� � ������, ����������� ��� ����������
                                    this.GetRecordSet()[selection][columnName] = args['CellNewValue'];
                                }
                            })
                        .off('Drilldown')
                        .on('Drilldown', (event, args) => {
                            args['Event'].stopPropagation();

                            /* ����� ���, ��� ������ recordSet, ���������� ���������, ��� � ��� ���������� ���������, 
                            ����� ���� ������� �������� ��������� ���������� ������
                            ����� ��������� �������� �� ��, ��� ������ ��� ���������� �� ������ ���� ������� */
                            let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                            let currentRecord = this.GetRecordSet()[selection];
                            let appletToUpdate = currentElementsInfo['AppletToUpdate'];
                            let recordToUpdate = currentElementsInfo['RecordToUpdate'];
                            let currentColumn = $(event.currentTarget).closest('[data-type="cell"]');
                            let columnType;
                            if (currentColumn != undefined) {
                                let columnName = currentColumn.attr('data-name');
                                let columnProperties = this.GetColumnProperties(columnName)[0];
                                columnType = columnProperties == undefined ? null : columnProperties['Type'];
                            }
                            
                            // ���������� ���������� � ������� ��������� ���������
                            GSCrmInfo.SetUpCurrentElements(event);

                            if (appletToUpdate != null && (recordToUpdate['Id'] != currentRecord['Id'] || columnType == "drilldown")) {
                                
                                // ����� �������������� ������
                                GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, recordToUpdate, false)
                                    .catch(error => console.log(error))
                                    .then(() => GSCrmInfo.Application.CommonAction.Invoke("Drilldown", event, currentRecord));
                            }
                            
                            else GSCrmInfo.Application.CommonAction.Invoke("Drilldown", event, currentRecord, false);
                        });
                    });
            });
    };
    //#endregion

    //#region //@R ������ ��� ������ �� �������
   /**
    *@M ���������� ������ � ������� �� recordSet-�
    * @param {String} tileItem ������ � �������
    */
    tileApplet.prototype.RefreshTileItem = function (tileItem) {
        tileItem.find('[data-type="cell"]').map((index, cell) => {
            let columnName = cell.getAttribute('data-name');
            let cellValue = this.GetRecordSet()[this.GetSelection(tileItem)][columnName];
            var PR = new DefaultTileAppletPR();
            PR.RenderCell(cell, cellValue, this.GetColumnProperties(columnName)[0]);
            return cell;
        });
    };

    /**
     *@M ���������� ������ ������ � ������� �� recordSet-�
     * @param {String} tileItem ������ � �������
     */
     tileApplet.prototype.RefreshCell = function (tileItem, columnName) {
         tileItem.find('[data-type="cell"]').filter('[data-name="' + columnName + '"]').map((index, cell) => {
             let cellValue = this.GetRecordSet()[this.GetSelection(tileItem)][columnName];
             var PR = new DefaultTileAppletPR();
             PR.RenderCell(cell, cellValue, this.GetColumnProperties(columnName)[0]);
             return cell;
         });
     };

    /**
    *@M ������� ������ ������ �� ������, ��������� ���������� � ������������� � ��� ��������
    * @param {Event} event ������� ������ ������
    */
    tileApplet.prototype.SelectTileItem = function (event) {
        return new Promise((resolve, reject) => {
            let tileItem = $(event.currentTarget).closest('[data-type="applet_item"]');
            if (tileItem.attr('disabled') == undefined) {
                let currentRecord = GSCrmInfo.GetSelectedRecord(this.Name);
    
                // � ������, ���� �������� �� �� ��� �� ������, ���������� ���������� � ������������� � ���������
                if (currentRecord == undefined || currentRecord['record'][0] != tileItem[0]) {
                    $('[data-type="control"]').attr('disabled', true);
                    $('[data-type="applet_item"]').attr('disabled', true);
                    $('[data-type="ExpectationArea"]').removeClass('d-none');
    
                    // ���������� ���������� � �������������
                    let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                    let view = currentElementsInfo['View'];
                    let appletToUpdate = currentElementsInfo['AppletToUpdate'];
                    let recordToUpdate = currentElementsInfo['RecordToUpdate'];
                    let currentControl = currentElementsInfo['CurrentControl'];
                    let recordId = this.GetRecordFromRS(event)['Id'];
                    
                    let data = {
                        ActionType: 'SelectTileItem',
                        TargetApplet: this.Name,
                        CurrentRecord: recordId,
                        CurrentControl: currentControl,
                        OpenPopup: false,
                        ClosePopup: false,
                        PopupApplet: null,
                        CurrentPopupControl: null
                    }

                    /*  ���� ���� ������ ��� ����������, �� ������� ���� �������� ��, � ������ ����� ���������� � �������������.
                        ���� ���������� ������ �� ���������, ����� ����� �������� ���������� � �������������.
                        ����� ��������� �������� �� ��, ��� ������ ��� ���������� �� ������ ���� ������� */
                    if (appletToUpdate != null && recordToUpdate['Id'] != this.GetRecordFromRS(event)['Id']) {
                        // ����� �������������� ������
                        let errorMessage;
                        GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, recordToUpdate)
                            .catch(error => {
                                errorMessage = error;
                                reject(error);
                            })
                            .then(() => {
                                if (errorMessage == undefined) {
                                    // �����������
                                    this.Focus(tileItem);
                                    view.UpdateViewInfo(data)
                                        .catch(error => reject(error))
                                        .then(() => {
                                            // ���������� ���������
                                            view.PartialUpdateContext(this.Info, false)
                                                .catch(error => reject(error))
                                                .then(() => {
                                                    $('[data-type="control"]').attr('disabled', false);
                                                    $('[data-type="applet_item"]').attr('disabled', false);
                                                    $('[data-type="ExpectationArea"]').addClass('d-none');
                                                    resolve();
                                                });
                                        });
                                }
                            });
                    }
                    else {
                        // �����������
                        this.Focus(tileItem);
                        view.UpdateViewInfo(data)
                            .catch(error => reject(error))
                            .then(() => {
                                // ���������� ���������
                                view.PartialUpdateContext(this.Info, false)
                                    .catch(error => reject(error))
                                    .then(() => {
                                        $('[data-type="control"]').attr('disabled', false);
                                        $('[data-type="applet_item"]').attr('disabled', false);
                                        $('[data-type="ExpectationArea"]').addClass('d-none');
                                        resolve();
                                    });
                            });
                    }
                }
                else resolve();
            }
            else resolve();
        })
    };

    /**
    *@M ��������� �������� ������� �� ��� id �� recordSet-�
    * @param {String} recordId Id ������ �� recordSet-�
    */
    tileApplet.prototype.GetTileItemByIdInRS = function (recordId) {
        let sequence = 0;
        this.GetRecordSet().forEach((item, index) => {
            if (item['Id'] == recordId)
                sequence = index;
        });
        return $('#' + this.Id).find('[data-type="applet_item"]').eq(sequence);
    };

    /**
    *@M ��������� �������� ������� �� ��� �������
    * @param {Number} sequence ������ �������� �������
    */
    tileApplet.prototype.GetTileItemBySequence = function (sequence) {
        return $($('#' + this.Info['AppletId']).find('[data-type="applet_item"]')[sequence]);
    };

    /**
    *@M ��������� selection-� �������� �������
    * @param {HTMLElement} target �������, selection �������� ���������� ��������
    */
    tileApplet.prototype.GetSelection = function (target) {
        let index = 0;
        target
            .closest('[data-type="applet"]')
            .find('[data-type="applet_item"]')
            .filter((i, item) => {
                if ($(item)[0] == target.closest('[data-type="applet_item"]')[0])
                    index = i;
            });
        return index;
    };
    //#endregion

    //#region //@R ������ ��� ������ � ���������
    /**
    *@M ��������� ���������� �������� ���� ������� �� ��������
    * @param {String} propertyName �������� ��������
    */
    tileApplet.prototype.GetColumnsProperty = function (propertyName) {
        return this.Info['Columns'].map(column => column[propertyName]);
    };

    /**
    *@M ��������� ���� ������� ��������� ������� �� ��������
    * @param {String} columnName �������� �������
    */
    tileApplet.prototype.GetColumnProperties = function (columnName) {
        return this.Info['Columns'].filter(column => column['Name'] == columnName);
    };

    /**
    *@M ���������� ������� � ������� �������� popertyName ����� popertyValue
    * @param {String} popertyName �������� ��������
    * @param {String|Number|Boolean|null} popertyValue �������� ��������
    */
    tileApplet.prototype.GetColumnsByPropertyValue = function (popertyName, propertyValue) {
        return this.Info['Columns'].filter(column => column[popertyName] == propertyValue);
    };
    //#endregion

    //#region //@R ������ ��� ������ � recordSet-��
    /**
        *@M ���������� �������� ��� ��������� ������� �� ���� �������
        * @param {String} propertyName �������� �������� ��� ���������
        */
    tileApplet.prototype.GetSelectedRecordsProperty = function (propertyName) {
        let records = {};
        GSCrmInfo.SelectedRecords.forEach((item, applet) => {
            records[applet] = item['properties'][propertyName];
        });
        return records;
    };

    /**
        *@M ���������� �������� �������� �� ����� ��� ��������� ������ � ������� �������
        * @param {String} propertyName �������� �������� ��� ���������
        */
    tileApplet.prototype.GetSelectedRecordProperty = function (propertyName) {
        return GSCrmInfo.GetSelectedRecord(this.Name)['properties'][propertyName];
    };

    /**
        * �������� ������ �� recordSet-� �� �������
        * @param {Event} event ������� ������ ������
        */
    tileApplet.prototype.GetRecordFromRS = function (event) {
        let tileItem = $(event.currentTarget).closest('[data-type="applet_item"]');
        return tileItem.length == 0 ? null : this.GetRecordSet()[this.GetSelection(tileItem)];
    };

    /**
        * �������� ������ �� recordSet-� �� �������
        * @param {Number} index ����� ������ �� recordSet-�
        */
    tileApplet.prototype.GetRecordFromRSByIndex = function (index) {
        return this.GetRecordSet().filter((item, i) => i == index)[0];
    }
    //#endregion

    return tileApplet;
}