import DefaultScreenPR from '../PhysicalRenders/DefaultScreenPR.js'

export default (function () {
    let screen = function(name, id) {
        this.Id = id;
        this.Name = name;
        this.Info = GSCrmInfo.ScreenInfo['Name'] == this.Name ? GSCrmInfo.ScreenInfo['Name'] : undefined;
        if (this.Info != undefined)
            this.Id = this.Info['ScreenId'];
    }

    //#region //@R ��������
    screen.prototype.GetId = function() {
        return this.Id;
    };

    screen.prototype.GetName = function() {
        return this.Name;
    };

    screen.prototype.GetInfo = function() {
        return this.Info;
    };
    //#endregion

    //#region //@R ������������
    //@M ������������ ������
    screen.prototype.Initialize = function(data) {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.InitializeScreen(data)
                .fail(error => reject(error))
                .done(info => {
                    // ���������� ������ � ������
                    GSCrmInfo.ScreenInfo = info;
                    GSCrmInfo.SetElement('Screen', this);
                    this.Info = info;

                    // ��������� ������
                    if (!Object.is(this.Info['CurrentView'], null)) {
                        this.RenderScreen(this.Info['CurrentView']['Name'])
                            .catch(error => reject(error))
                            .then(() => resolve());
                    }
                });
        });
    };

    //@M �������� �����
    screen.prototype.RenderScreen = function(currentViewName) {
        return new Promise((resolve, reject) => {
            let screen = $('<div data-type="Screen" data-name="' + this.Info['Name'] + '"></div>');
            
            // ������������ ������� ������
            GSCrmInfo.ScreenInfo['Crumbs'].map((item, index) => {
                item['CrumbId'] = "Crumb_" + index;
            });
            
            // ��������� ������
            let PR = new DefaultScreenPR();
            screen = PR.RenderScreen(screen);
            $('.large-container').empty().append(screen);

            // ������������� �������� �������������
            let currentView = new GSCrmInfo.Application.View(currentViewName);
            currentView.Initialize(screen)
                .catch(error => reject(error))
                .then(newView => {
                    let appletInfo = newView.GetInfo()['ViewItems'][0];
                    let selectedRecords = GSCrmInfo.GetSelectedRecord(appletInfo['AppletName']);
                    GSCrmInfo.SetElement('View', newView);
                    switch(appletInfo['Type']) {
                        case "Tile":
                            let tileApplet = new GSCrmInfo.Application.TileApplet(appletInfo['AppletName'], null);
                            GSCrmInfo.SetElement('TargetApplet', tileApplet);
                            GSCrmInfo.SetElement('CurrentApplet', tileApplet);
                            GSCrmInfo.SetElement('CurrentControl', null);
                            GSCrmInfo.SetElement('CurrentPopupControl', null);
                            break;
                        case "Form":
                            let formApplet = new GSCrmInfo.Application.FormApplet(appletInfo['AppletName'], null);
                            GSCrmInfo.SetElement('TargetApplet', formApplet);
                            GSCrmInfo.SetElement('CurrentApplet', formApplet);
                            GSCrmInfo.SetElement('CurrentControl', null);
                            GSCrmInfo.SetElement('CurrentPopupControl', null);
                            break;
                    }
                    if (selectedRecords != undefined) {
                        GSCrmInfo.SetElement('CurrentRecord', selectedRecords['properties']);
                    };
                    let currentElementsInfo = GSCrmInfo.CurrentElementsInfo;

                    // ��� ������� �� ������� ���������� �������� ���������� � ������ �� ����, � ����� ������ ����������� �����
                    $(screen)
                        .find('[data-type="ScreenItem"]')
                        .off('click')
                        .on('click', event => {
                            $(screen).trigger("SelectScreenItem", [{
                                Event: event
                            }]);
                            if (currentElementsInfo['AppletToUpdate'] != null) {
                                // ����� �������������� ������
                                let errorMessage;
                                GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, currentElementsInfo['RecordToUpdate'])
                                    .catch(error => {
                                        errorMessage = error;
                                        reject(error);
                                    })
                                    .then(() => {
                                        if (errorMessage == undefined) {
                                            let newViewName = $(event.currentTarget).closest('[data-type="ScreenItem"]').attr("data-name");
                                            GSCrmInfo.Application.SelectScreenItem(event, this.Name, newViewName).then(() => resolve());
                                        }
                                    });
                            }
                            else {
                                let newViewName = $(event.currentTarget).closest('[data-type="ScreenItem"]').attr("data-name");
                                let errorMessage;
                                GSCrmInfo.Application.SelectScreenItem(event, this.Name, newViewName)
                                    .catch(error => {
                                        errorMessage = error;
                                        reject(error);
                                    })
                                    .then(() => resolve());
                            }
                        });

                    // ��� ������� �� ������� ���������� �������� ���������� � ������, � ����� ������ ����������� ���
                    $(screen)
                        .find('[data-type="TreadItem"]')
                        .off('click')
                        .on('click', event => {
                            $(screen).trigger('SelectTreadItem', [{
                                Event: event
                            }]);
                            if (currentElementsInfo['AppletToUpdate'] != null) {
                                // ����� �������������� ������
                                let errorMessage;
                                GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, currentElementsInfo['RecordToUpdate'])
                                    .catch(error => {
                                        errorMessage = error;
                                        reject(error);
                                    })
                                    .then(() => {
                                        if (errorMessage == undefined)
                                            GSCrmInfo.Application.SelectTreadItem(event, this, currentView)
                                    });
                            }
                            else GSCrmInfo.Application.SelectTreadItem(event, this, currentView);
                        });
                    
                    resolve();
                });
        });
    };
    //#endregion

    //#region //@R ������ �� ������ �� ��������
    //@M �������������� ������ �� ���, ������� ���������� � ������� ������
    screen.prototype.RequestInfo = function() {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.GetScreenInfo(this.Name)
                .fail(error => reject(error))
                .done(info => resolve(info));
        })
    };
    
    //@M �������������� ������ �� ���, �������� ���������� �� ������
    screen.prototype.UpdateInfo = function(info) {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.UpdateScreenInfo(info)
                .fail(error => reject(error))
                .done(screenInfo => {
                    GSCrmInfo.ScreenInfo = screenInfo;
                    resolve();
                });
        });
    };
    //#endregion
    
    return screen;
})()