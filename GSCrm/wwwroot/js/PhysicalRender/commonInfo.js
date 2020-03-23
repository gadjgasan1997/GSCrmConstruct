//@C ����� ��� ������ � ����������� � ���������
class Info {
    //#region //@R ��������
    static #ScreenInfo = {};
    static #ViewInfo = new Map();
    static #AppletInfo = new Map();
    static #CurrentElementsInfo = {};
    static SelectedRecords = new Map();

    // ��������� ��������� �� ������
    static get ScreenInfo() {
        return this.#ScreenInfo;
    }

    /**
     * ��������� ���������� �� ������
     * @param {String} screenName �������� ������
     * @param {{
        * String: Name �������� ������
        * String: CurrentViewName �������� �������� �������������
        * Array: CurrentAggregateCategory ������� ���������
        * Object: AggregateViews ������ ������������� ������� ���������
        * }} screenInfo ���������� �� ������
    */
    static set ScreenInfo(screenInfo) {
        this.#ScreenInfo = screenInfo;
    }

    // ��������� ���������� ��� ���� ��������������
    static get ViewsInfo() {
        return this.#ViewInfo;
    }

    // ��������� ���������� ��� ���� ��������
    static get AppletsInfo() {
        return this.#AppletInfo;
    }

    // ��������� ���������� � ������� ���������
    static get CurrentElementsInfo() {
        return this.#CurrentElementsInfo;
    }
    //#endregion

    //#region //@R ������ ��� ������ � ����������� � ���������
    //@M ��������� ���������� � �������������
    static GetViewInfo(viewName) {
        return this.#ViewInfo.get(viewName);
    }
    
    /**
     * @M ��������� ���������� � �������������
     * @param viewName �������� �������������
     * @param {{
     * View: View �������������
     * Object: Applets ������ ��������, ����������� � �������������
     * Object: ViewItems ������ ��������� �������������
     * Object: Routing ������������� ��� ��������
     * }} viewInfo ���������� � �������������
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
        Info.ViewsInfo.forEach((info, viewName) => {
            if (viewName != Info.ScreenInfo['CurrentView']) {
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
     * @param {{
     * String: Name �������� �������
     * String: Type ��� �������
     * String: Header ��������� �������
     * Number|String: DisplayLines ��������� ������������ �������
     * String: EmptyState EmptyState
     * Object: Controls ������ ���������
     * Object: ControlUPs ������ ���� user property
     * Object: Columns ������ �������
     * String: PR �������� PR �����, ������� ���������� ��������
     * }} appletInfo ���������� �� �������
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
        let viewName = Info.ScreenInfo['CurrentView']['ViewName'];
        let viewItems = this.GetViewInfo(viewName)['ViewItems'];
        let applets = viewItems.map(item => item['AppletName']);

        // �������, ������� ���� � ���������� � �������������(������� �������, ���������� ��������)
        this.AppletsInfo.forEach((info, appletName) => {
            if (applets.indexOf(appletName) == -1) {
                this.RemoveAppletInfo(appletName);
            }
        })
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
        let viewName = Info.ScreenInfo['CurrentView']['ViewName'];
        let view = this.#CurrentElementsInfo['View'];
        if (view == undefined || viewName != view.Name) {
            view = new View(viewName);
            this.#CurrentElementsInfo['View'] = view;
        }
            
        // ��������� �������� �������� ������� � �������� ���������� ������ �������.
        let currentAppletName = $(event.currentTarget).closest('[data-type="applet"]').attr('data-name');
        let currentApplet = new Applet(currentAppletName, null);
        let controlName = $(event.currentTarget).closest('[data-type="control"]').attr('data-name');

        // � ����������� �� ���� �������� ������� ��������� ������ �������� ��� �������
        switch(currentApplet.Info['Type']) {
            // ���� ��� �������� ������� - ����
            case "Tile":
                // ��������� �������� ������� � ������� � ������� ������
                let targetApplet = new TileApplet(currentApplet.Name, null);
                this.#CurrentElementsInfo['TargetApplet'] = targetApplet;

                // ���� ������� ������ �� ����� null, ���������� ���������� � ���
                let record = targetApplet.GetRecordFromRS(event);
                if (!Object.is(record, null)) {
                    this.#CurrentElementsInfo['Record'] = record;
                }

                /* ���� �� ������� �������� ����� �������� ShowPopup, ���� ����� ���������� �������� � ���������� � �������������
                   ����� ���������� ��������� ������� �� undefined */
                if (controlName && targetApplet.GetControlProperties(controlName)['ActionName'] == 'ShowPopup') {
                    let popupName = targetApplet.Info['ControlUPs'][controlName].filter(item => item['Name'] == 'Applet')[0]['Value'];
                    this.#CurrentElementsInfo['PopupApplet'] = new PopupApplet(popupName, null);
                }
                break;
            case "Form":
                break;
            // ���� ��� �����
            case "Popup":
                this.#CurrentElementsInfo['PopupApplet'] = new PopupApplet(currentApplet.Name, currentApplet.Id);
                this.#CurrentElementsInfo['TargetApplet'] = new TileApplet(null, view.GetTargetId(event));
                break;
        }

        // ��������� �������� ��������
        this.#CurrentElementsInfo['ControlName'] = controlName;
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

    //!W ��� ������ �������� ��������, ����� ������ ��� ������������ ���� 
    static DisplayedRecords(componentName) {
        return new Promise((resolve, reject) => {
            let request = new RequestsToDB;
            request.GetDisplayedRecords(componentName)
                .fail(error => console.log(error))
                .done(info => console.log(info));
        })
    }

    static GetSelectedRecords() {
        return new Promise((resolve, reject) => {
            let request = new RequestsToDB;
            request.GetSelectedRecords()
                .fail(error => console.log(error))
                .done(info => console.log(info));
        })
    }

    static BackScreenInfo() {
        return new Promise((resolve, reject) => {
            let request = new RequestsToDB;
            request.GetBackScreenInfo()
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
            let request = new RequestsToDB;
            request.GetBackViewInfo()
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
            let request = new RequestsToDB;
            request.GetBackAppletInfo()
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