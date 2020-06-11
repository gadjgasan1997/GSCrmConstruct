var recordSet = new Map();
export default (function () {
    let applet = function(name, id) {
        this.Name = name;
        this.Id = id;
        this.Name = name == null ?
            (function () {
                let appletName;
                GSCrmInfo.AppletsInfo.forEach((applet, index) => {
                    if (applet['AppletId'] == id) {
                        appletName = index;
                    }
                });
                return appletName;
            })()
            : name;
        this.Id = id == null ?
            (function () {
                let appletId;
                GSCrmInfo.AppletsInfo.forEach((applet, index) => {
                    if (index == name) {
                        appletId = applet['AppletId'];
                    }
                });
                return appletId;
            })()
            : id;
        let viewName;
        GSCrmInfo.ViewsInfo.forEach((info, view) => {
            info['ViewItems'].forEach(viewItem => {
                if (viewItem['AppletName'] == this.Name) {
                    viewName = view;
                }
            });
        });
        this.Info = GSCrmInfo.GetAppletInfo(this.Name);
        if (this.Info != undefined && viewName != undefined)
            this.Info['View'] = viewName;
    };
        
    //#region //@R ��������
    // ���������� Id �������
    applet.prototype.GetId = function () {
        return this.Id;
    };

    // ���������� �������� �������
    applet.prototype.GetName = function () {
        return this.Name;
    };

    // ���������� ���������� �� ������� �� ������� �� ������
    applet.prototype.GetInfo = function() {
        return this.Info;
    };
    //#endregion
    
    //#region //@R �������������
    //@M ������������� �������
    applet.prototype.InitializeApplet = function () {
        return new Promise((resolve, reject) => {
            if (GSCrmInfo.GetAppletInfo(this.Name) == undefined) {
                GSCrmInfo.Application.CommonRequests.InitializeApplet(this.Name)
                    .fail(error => reject(error))
                    .done(appletInfo => {
                        appletInfo['AppletId'] = this.Id;
                        appletInfo['TileItems'] = [];
                        GSCrmInfo.RemoveOldAppletInfo(this.Name);
                        GSCrmInfo.SetAppletInfo(this.Name, appletInfo);
                        this.Info = appletInfo;
                        resolve(appletInfo);
                    });
            }
            else {
                this.Info['AppletId'] = this.Id;
                resolve();
            }
        });
    };
    
    //@M ������������� ��������
    applet.prototype.InitializeControl = function(item) {
        // ������������ id ��������
        let controlName = $(item).attr('data-name');
        let counter = 0;
        let controlId = 'control_' + controlName + '_' + counter;

        // �� ��� ���, ���� ������� � ����� id ���� �� ��������, ����������� 1, ����� id ��� ����������
        while ($('#' + controlId).length > 0) {
            controlId = controlId.split(counter).join((counter + 1));
            counter++;
        }

        $(item).attr("id", controlId);
            
        let control = new GSCrmInfo.Application.Control(this, controlName, controlId);
        control.Initialize(this);
    };

    //@M ������������� ���������
    applet.prototype.InitializeControls = function() {
        $('#' + this.Info['AppletId'])[0]
            .querySelectorAll('[data-type="control"]')
            .forEach(item => this.InitializeControl(item));
    };
    //#endregion

    //#region //@R ������ ��� ������ � ����������
    /**
    *@M ��������� ��������, �� ������� ��������� ��������
    * @param {Event} event ������� ������� �� �������
    */
    applet.prototype.GetCurrentControl = function (event) {
        return $(event.currentTarget).closest('[data-type="control"]');
    };

    /**
    *@M ��������� ���������� �������� ���� ��������� �� ��������
    * @param {String} propertyName �������� ��������
    */
    applet.prototype.GetControlsProperty = function (propertyName) {
        return this.Info['Controls'].map(control => control[propertyName]);
    };

    /**
    *@M ��������� ���� ������� ���������� �������� �� ��������
    * @param {String} controlName �������� ��������
    */
    applet.prototype.GetControlProperties = function (controlName) {
        return this.Info['Controls'].filter(control => control['Name'] == controlName)[0];
    };

    /**
    *@M ��������� ��������� � ������� �������� popertyName ����� popertyValue
    * @param {String} popertyName �������� ��������
    * @param {String|Number|Boolean|null} popertyValue �������� ��������
    */
    applet.prototype.GetControlsByPropertyValue = function (popertyName, popertyValue) {
        return this.Info['Controls'].filter(control => control[popertyName] == popertyValue);
    };

    /**
    *@M ��������� ���� user property ���������
    * @param {String} controlName �������� ��������
    */
    applet.prototype.GetControlUPs = function (controlName) {
        return this.Info['ControlUPs'];
    };

    //#endregion

    //#region //@R ������ ��� ������ � recordSet-��
    //@M ���������� recordSet
    applet.prototype.GetRecordSet = function() {
        return recordSet.get(this.Name);
    };

    //@M ������������� recordSet
    applet.prototype.SetRecordSet = function (data) {
        recordSet.set(this.Name, data)
    };

    //@M ������� recordSet
    applet.prototype.DeleteRecordSet = function() {
        recordSet.delete(this.Name);
    }

    //@M ������� �� ��������� ������� ������ � ����
    applet.prototype.GetAppletRecord = function() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetRecord(this.Info)
                .fail(error => reject(error))
                .done(data => resolve(data));
        });
    };

    /**
    *@M ��������� �������� �������� �������� ������� �� recordSet-� ��� ���� �������
    * @param {String} propertyName �������� ��������
    */
    applet.prototype.GetRecordsProperty = function (propertyName) {
        return this.GetRecordSet().map(item => item[propertyName]);
    };

    /**
    *@M ��������� ������ ��� �������� ������� �� recordSet-� �� �������� ��������
    * @param {String} propName �������� ��������
    * @param {String} propValue �������� ��������
    */
    applet.prototype.GetRecordByProperty = function (popertyName, popertyValue) {
        return this.GetRecordSet().filter(item => item[popertyName] == popertyValue);
    };

    /**
    *@M ��������� ������ ��� �������� �������
    * @param {String} propName �������� ��������
    * @param {String} propValue �������� ��������
    * @param {String} newRecord ����� ������
    */
    applet.prototype.UpdateRecordInRS = function (newRecord, propertyName, propertyValue) {
        switch(this.Info['Type']) {
            case "Tile":
                this.SetRecordSet(this.GetRecordSet().map(item => {
                    if (item[propertyName] == propertyValue)
                        item = newRecord;
                    return item;
                }));
                break;
            case "Form":
                this.SetRecordSet(newRecord);
                break;
        }
    };
    //#endregion

    //#region //@R ��������������� ������ ��� ������ � ���������
    /**
    *@M ����������, �������� �� �������� ���������
    * @param {String} actionType �������� ��������
    */
    applet.prototype.IsSystemAction = function (actionType) {
        if (['NextRecords',
            'PreviousRecords',
            'ShowPopup',
            'NewRecord',
            'UpdateRecord',
            'DeleteRecord',
            'UndoRecord',
            'UndoUpdate',
            'AppletConfg'].indexOf(actionType) != -1)
            return true;
        return false;
    };

    /**
    * @M ����� ��������, �������� �� ��������
    * @param {Event} event ������� ������� �� �������
    * @param {Object} properties �������� ��������
    */
    applet.prototype.InvokeAction = function (event, properties) {
        // ���� �������� - �������� ������ ��� �����, ����� �������� �������� �������� �� Navigation
        let actionType;
        if (properties['ActionType'] == "NextRecords" || properties['ActionType'] == "PreviousRecords") {
            actionType = "Navigation";
        }
        else actionType = properties['ActionType'];

        // ���������� ���������� � ������� ���������� ���������
        GSCrmInfo.SetUpCurrentElements(event);

        // ����� ��������
        GSCrmInfo.Application.CommonAction.Invoke(actionType, event, this.GetRecordSet());
    };

    return applet;
})()