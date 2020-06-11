//@C GSCrmUI info
class GSCrmInfo {
    //#region //@R ��������
    static #Application = {};
    static #ApplicationInfo = {};
    static #ScreenInfo = {};
    static #ViewInfo = new Map();
    static #AppletInfo = new Map();
    static #CurrentElementsInfo = {
        "Screen": null,
        "View": null,
        "TargetApplet": null,
        "CurrentApplet": null,
        "PopupApplet": null,
        "CurrentRecord": null,
        "CurrentControl": null,
        "CurrentPopupControl": null,
        "AppletToUpdate": null,
        "RecordToUpdate" : null
    };
    static #SelectedRecords = {};

    static get Application() {
        return this.#Application;
    }

    static set Application(application) {
        this.#Application = application;
    }

    //@M ��������� ���������� � ����������
    static get ApplicationInfo() {
        return this.#ApplicationInfo;
    }

    //@M ��������� ��������� �� ������
    static get ScreenInfo() {
        return this.#ScreenInfo;
    }

    //@M ��������� ���������� �� ������
    static set ScreenInfo(screenInfo) {
        this.#ScreenInfo = screenInfo;
    }

    //@M ��������� ���������� � ����������
    static set ApplicationInfo(applicationInfo) {
        this.#ApplicationInfo = applicationInfo;
    }

    //@M ��������� ���������� ��� ���� ��������������
    static get ViewsInfo() {
        return this.#ViewInfo;
    }

    //@M ��������� ���������� ��� ���� ��������
    static get AppletsInfo() {
        return this.#AppletInfo;
    }

    //@M ��������� ���������� � ������� ���������
    static get CurrentElementsInfo() {
        return this.#CurrentElementsInfo;
    }
    //#endregion
    
    //#region //@R ������ ��� ������ � ����������� � ���������
    //@M ��������� ���������� � ������
    static GetScreenInfo(screenName) {
        return this.#ScreenInfo.get(screenName);
    }
    
    //@M ��������� ���������� � �������������
    static GetViewInfo(viewName) {
        return this.#ViewInfo.get(viewName);
    }
    
    /**
     * @M ��������� ���������� � �������������
    * @param viewName �������� �������������
    * @param viewInfo ���������� � �������������
    */
    static SetViewInfo(viewName, viewInfo) {
        this.#ViewInfo.set(viewName, viewInfo);
    }

    /**
    * @M ������� ���������� � ������������� �� ����
    * @param {String} viewName �������� �������������
    */
    static RemoveViewInfo(viewName) {
        this.#ViewInfo.delete(viewName);
    }

    //@M ������� ������ ���������� � ��������������
    static RemoveOldViewInfo() {
        this.#ViewInfo.forEach((info, viewName) => {
            if (viewName != this.#ScreenInfo['CurrentView']) {
                this.RemoveViewInfo(viewName);
            }
        });
    }

    //@M ��������� ���������� �� �������
    static GetAppletInfo(appletName) {
        return this.#AppletInfo.get(appletName);
    }

    /**
    * @M ��������� ���������� �� �������
    * @param appletName �������� �������
    * @param appletInfo ���������� �� �������
    */
    static SetAppletInfo(appletName, appletInfo) {
        this.#AppletInfo.set(appletName, appletInfo);
    }

    /**
    * @M ������� ���������� �� ������� �� ����
    * @param {String} appletName �������� �������
    */
    static RemoveAppletInfo(appletName) {
        this.#AppletInfo.delete(appletName);
    }

    //@M ������� ������ ���������� �� ��������
    static RemoveOldAppletInfo() {
        // ������������� � �������, ������� ���� � ��� �� ������� ������
        let viewName = this.#ScreenInfo['CurrentView']['ViewName'];
        let viewItems = this.GetViewInfo(viewName)['ViewItems'];
        let applets = viewItems.map(item => item['AppletName']);

        // �������, ������� ���� � ���������� � �������������(������� �������, ���������� ��������)
        this.#AppletInfo.forEach((info, appletName) => {
            if (applets.indexOf(appletName) == -1) {
                this.RemoveAppletInfo(appletName);
            }
        })
    }

    //@M ��������� ���������� � ������� ���������� ������
    static GetSelectedRecord(appletName) {
        return this.#SelectedRecords[appletName];
    }

    //@M ��������� ���������� � ������� ���������� ������
    static SetSelectedRecord(appletName, selectedRecords) {
        this.#SelectedRecords[appletName] = selectedRecords;
    }

    /**
    *@M ���������� ������� � �������� ����������� ���������� ��� ������� �� �������
    * @param {Event} event �������
    */
    static SetUpCurrentElements(event) {
        /**
        * ��������� �������� �������������. ���� ��� �� ��������� � ��������� ������������� �� ������� � �������� ����������,
        * �� � ������� ������������� ���������� �����.
        */
        let viewName = this.#ScreenInfo['CurrentView']['ViewName'];
        let view = this.#CurrentElementsInfo['View'];
        if (view == null || viewName != view.Name) {
            view = new this.#Application.View(viewName);
            this.#CurrentElementsInfo['View'] = view;
        }

        // ��������� �������� �������� �������
        let currentApplet = $(event.currentTarget).closest('[data-type="applet"]');
        if (currentApplet != undefined) {
            let targetApplet = new this.#Application.Applet(currentApplet.attr('data-name'), currentApplet.attr('id'));
            let tileApplet;
            let formApplet;
            let popupApplet;
            let controlName = $(event.currentTarget).closest('[data-type="control"]').attr('data-name');
            controlName = controlName == undefined ? null : controlName;

            // � ����������� �� ���� �������� ������� ��������� ������ �������� ��� �������
            switch (targetApplet.GetInfo()['Type']) {
                // ���� ��� �������� ������� - ����
                case "Tile":
                    // ��������� �������� ������� � ������� � ������� ������
                    tileApplet = new this.#Application.TileApplet(targetApplet.GetName(), targetApplet.GetId());
                    this.#CurrentElementsInfo['TargetApplet'] = tileApplet;
                    this.#CurrentElementsInfo['CurrentApplet'] = tileApplet;

                    // ���� ������� ������ �� ����� null, ���������� ���������� � ���
                    let currentRecord = tileApplet.GetRecordFromRS(event);
                    let selectedRecord = this.GetSelectedRecord(tileApplet.Name);
                    this.#CurrentElementsInfo['CurrentRecord'] = !Object.is(currentRecord, null) ? currentRecord : selectedRecord == null ? null : selectedRecord['properties'];
                    this.#CurrentElementsInfo['CurrentControl'] = controlName;
                    break;
                case "Form":
                    // ��������� �������� ������� � ������� � ������� ������
                    formApplet = new this.#Application.FormApplet(targetApplet.GetName(), targetApplet.GetId());
                    this.#CurrentElementsInfo['TargetApplet'] = formApplet;
                    this.#CurrentElementsInfo['CurrentApplet'] = formApplet;
                    this.#CurrentElementsInfo['CurrentRecord'] = formApplet.GetRecordSet();
                    this.#CurrentElementsInfo['CurrentControl'] = controlName;
                    break;
                // ���� ��� �����
                case "Popup":
                    popupApplet = new this.#Application.PopupApplet(targetApplet.GetName(), targetApplet.GetId());
                    this.#CurrentElementsInfo['PopupApplet'] = popupApplet;
                    this.#CurrentElementsInfo['CurrentApplet'] = popupApplet;
                    this.#CurrentElementsInfo['CurrentPopupControl'] = controlName;
                    break;
            }
        }
    }

    /**
    * @M �������� �������� ���������� �������� �� �����
    * @param {String} elName �������� ��������
    * @param {String} elValue ����� �������� ��������
    */
    static SetElement(elName, elValue) {
        this.#CurrentElementsInfo[elName] = elValue;
    }
    //#endregion

    //@!W ��� ������ �������� ��������, ����� ������ ��� ������������ ���� 
    static DisplayedRecords(componentName) {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetDisplayedRecords(componentName)
                .fail(error => console.log(error))
                .done(info => console.log(info));
        })
    }

    static BackSelectedRecords() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetSelectedRecords()
                .fail(error => console.log(error))
                .done(info => console.log(info));
        })
    }

    static BackScreenInfo() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetBackScreenInfo()
                .fail(error => {
                    console.log(error);
                    reject();
                })
                .done(info => {
                    console.log(info);
                    resolve();
                });
        });
    }

    static BackViewInfo() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetBackViewInfo()
                .fail(error => {
                    console.log(error);
                    reject();
                })
                .done(info => {
                    console.log(info);
                    resolve();
                });
        });
    }

    static BackAppletInfo() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetBackAppletInfo()
                .fail(error => {
                    console.log(error);
                    reject();
                })
                .done(info => {
                    console.log(info);
                    resolve();
                });
        });
    }
}