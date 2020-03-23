class View {
    constructor(name) {
        this.Name = name;
    }

    //#region //@R ��������
    // �������� ���������� � ������������� �� ��� ��������
    get Info() {
        return Info.GetViewInfo(this.Name);
    }

    // ���������� ���� ��� �������������
    get Path() {
    return Info.ScreenInfo['Routing'][this.Name];
    }
    //#endregion
    
    //#region //@R �������������
    // �������������� �������������
    Initialize() {
        return new Promise((resolve, reject) => {
            // ������� ����������
            $('.large-container main').empty();

            // �������������
            var $view = $('<div data-type="view" data-name="' + this.Name + '"></div>');

            // Id �������������
            var viewId = "GSV_0";
            var counter = 0;

            // �� ��� ���, ���� ������� � ����� id ������������ �� ��������, ��������� 1, ����� Id ��� ����������
            while ($('#' + viewId).length > 0) {
                viewId = viewId.split(counter).join((counter + 1));
                counter++;
            }

            // ������������ id �������������
            this.Id = viewId;
            $view.attr('id', this.Id);

            // ���������� ������������� � ��������
            $('.large-container main').append($view);

            // ����������� �������� ��������
            $('.cssload-wrapper').removeClass('d-none');

            // ������������� �������� � �������������, ����������� ���������� � ���
            var request = new RequestsToDB;
            
            request.InitializeView(this)
                .fail(error => reject(error))
                .done(info => {
                    info['ViewId'] = viewId;
                    Info.RemoveOldViewInfo();
                    Info.SetViewInfo(this.Name, info);

                    var count = 0;
                    this.InitializeItems($view, info['ViewItems'], count)
                        .catch(error => reject(error))
                        .then(() => resolve());
                });
        });
    }

    // �������������� �������� �������������
    InitializeItems($view, applets, count) {
        return new Promise((resolve, reject) => {
            if (applets[count] != undefined) {
                var appletName = applets[count]['AppletName'];
                var appletId = "GSA_0";
                var counter = 0;
    
                // �� ��� ���, ���� ������� � ����� id ������������ �� ��������, ��������� 1, ����� Id ��� ����������
                while ($('#' + appletId).length > 0) {
                    appletId = appletId.split(counter).join((counter + 1));
                    counter++;
                }
    
                switch (applets[count]['Type']) {
                    case "Tile":
                        // �������� ������ � ��������
                        $view.append('<div data-type="applet" data-name="' + appletName + '" id="' + appletId + '"></div>');
    
                        var tileApplet = new TileApplet(appletName, appletId);
                        // ������������� ���
                        tileApplet.Initialize()
                            .catch(error => reject(error))
                            .then(() => {
                                count++;
    
                                // ������������� ���������� �������
                                this.InitializeItems($view, applets, count)
                                    .catch(error => reject(error))
                                    .then(() => resolve());
                            });
                        break;
    
                    default:
                        count++;
                        this.InitializeItems($view, applets, count)
                            .catch(error => reject(error))
                            .then(() => resolve());
                        break;
                }
            }
            else this.UpdateContext()
                .catch(error => reject(error))
                .then(() => resolve())
                .finally(() => {
                    // ������� �������� ��������
                    $('.cssload-wrapper').addClass('d-none');
                });
        })
    }
    //#endregion

    //#region //@R ������ ��� ���������� ���������� � ��������� �������������
    // ����������� ���������� � ������������� � ����
    RequestInfo() {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.GetViewInfo(this)
                .fail(error => reject(error))
                .done(info => {
                    Info.RemoveOldViewInfo();
                    Info.SetViewInfo(this.Name, info);
                    resolve();
                });
        });
    }

    // ��������� ��������� � �������������
    UpdateViewInfo(data) {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.UpdateViewInfo(this, data)
                .fail(error => reject(error))
                .done(info => {
                    Info.RemoveOldViewInfo();
                    Info.SetViewInfo(this.Name, info);
                    resolve();
                });
        });
    }

    // ������ ���������� ���������
    UpdateContext() {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.UpdateContext(this)
                .fail(error => reject(error))
                .done(applets => {
                    if (applets.length > 0) {
                        var count = 0;
                        this.RefreshApplets(applets, count)
                            .catch(error => reject(error))
                            .then(() => resolve());
                    }
                    else resolve();
                });
        });
    }

    // ��������� ���������� ���������
    PartialUpdateContext(applet, recordId, refreshCurrentApplet) {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.PartialUpdateContext(applet, recordId, refreshCurrentApplet)
                .fail(error => reject(error))
                .done(applets => {
                    if (applets.length > 0) {
                        var count = 0;
                        this.RefreshApplets(applets, count)
                            .catch(error => reject(error))
                            .then(() => resolve());
                    }
                    else resolve();
                });
        });
    }

    // ���������� ��������� ��� ������ ��������
    RefreshApplets(applets, count) {
        return new Promise((resolve, reject) => {
            if (applets[count] != undefined) {
                let applet = new Applet(applets[count], null);
                switch(applet.Info['Type']) {
                    case "Tile":
                        let tileApplet = new TileApplet(applet.Name);
                        tileApplet.GetRecords()
                            .catch(error => reject(error))
                            .then(() => {
                                count++;
                                this.RefreshApplets(applets, count);
                                resolve();
                            });
                        break;
                }
            }
        });
    }

    // ���������� ��������� ��� ������ �������
    RefreshApplet(appletName) {
        return new Promise((resolve, reject) => {
            var tileApplet = new TileApplet(appletName);
            tileApplet.GetRecords()
                .catch(error => reject(error))
                .then(() => resolve());
        });
    }
    //#endregion

    //#region //@R ������ ��� ������ � ��������
    // ���������� id �������, ������ �������� ��������� �������
    GetAppletId(event) {
        return $(event.currentTarget).closest('[data-type="applet"]').attr('id');
    }

    // �� ������� �� ������� ���������� ��� ��������
    GetAppletName(event) {
        return this.GetAppletNameById(this.GetAppletId(event));
    }

    // �� �������� ������� ���������� ��� id
    GetAppletIdByName(appletName) {
        var appletId;
        Info.AppletsInfo.forEach((applet, index) => {
            if (index == appletName) {
                appletId = applet['AppletId'];
            }
        });
        return appletId;
    }

    // �� id ������� ���������� ��� ��������
    GetAppletNameById(appletId) {
        var appletName;
        Info.AppletsInfo.forEach((applet, index) => {
            if (applet['AppletId'] == appletId) {
                appletName = index;
            }
        });
        return appletName;
    }

    // ���������� id ����, � ������� ��� ������ ������� ������/������� � �������
    GetTargetId(event) {
        return $(event.currentTarget).closest('[data-target-id]').attr('data-target-id');
    }

    // �������� ���� � �������(��������� �� �������� � ���������)
    CloseSelectArea() {
        // �������, � �������� ��������� �������� ����
        let el = $("#" + $('.select-area').attr('data-target-id'));
        el.removeClass('gs-field-is-focused');
        if (el.find('.gs-field-input').val() != '') {
            el.addClass('gs-field-is-filled')
        }
        else {
            el.removeClass('gs-field-is-filled')
        }

        // ��������� �������� ��� �������� � recordSet-�
        let appletName = GetAppletNameById(el.closest('[data-type="applet"]').attr('id'));
        let applet = new Applet(appletName);
        applet.RecordSet[el.attr('data-name')] = el.find('.gs-field-input').val();

        // �������� ���� � �������
        $('.select-area')
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
}

// �������� �������������, ������ �������� ��������� �������
var GetActiveView = event => {
    return $(event.currentTarget).closest('[data-type="view"]').attr('data-name');
}