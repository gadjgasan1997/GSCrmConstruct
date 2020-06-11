import Action from './Modules/Action.js'
import Applet from './Modules/Applet.js'
import Calendar from './Modules/Calendar.js'
import Control from './Modules/Control.js'
import FormApplet from './Modules/FormApplet.js'
import Navbar from './Modules/Navbar.js'
import PickList from './Modules/PickList.js'
import PopupApplet from './Modules/PopupApplet.js'
import Requests from './Modules/Requests.js'
import Screen from './Modules/Screen.js'
import TileApplet from './Modules/TileApplet.js'
import View from './Modules/View.js'
import DefaultApplicationPR from './PhysicalRenders/DefaultApplicationPR.js'
import DefaultErrorPR from './PhysicalRenders/DefaultErrorPR.js'
/*import '../css/Applets.css'
import '../css/Application.css'
import '../css/CommonBlocks.css'
import '../css/Controls.css'
import '../css/Fonts.css'
import '../css/Navbar.css'
import '../css/Tabs.css'
import '../css/TreadBar.css'
import '../css/View.css'*/

//@C GSCrmUI
export default (function () {
    var application = function(appName) {
        this.Name = appName;
    }
    
    //@M ������������� ����������
    application.prototype.Initialize = function () {
        return new Promise((resolve, reject) => {
            GSCrmInfo.Application.CommonRequests.InitializeApplication()
                .fail(error => {
                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating application info.', errorMessage);
                    reject(error);
                })
                .done(info => {
                    sessionStorage.setItem('GSCrmApplicationInfo', JSON.stringify(info));
                    GSCrmInfo.ApplicationInfo = info;
                    let PR = new DefaultApplicationPR();
                    PR.RenderApplication();

                    // ��������, ��� � ���������� ���������� ������� �����
                    if (!Object.is(info['CurrentScreen'], null)) {
                        let newScreenName = info['CurrentScreen']['Name'];
                        
                        // ��������, ��� � ���������� ���������� �������� �������������
                        let newViewName = info['CurrentView'] == null ? null : info['CurrentView']['Name'];
                        let screen = new GSCrmInfo.Application.Screen(newScreenName);
                        let inputsObj = {
                            NewScreenName: newScreenName,
                            NewViewName: newViewName
                        }
                        screen.Initialize(inputsObj)
                            .catch(error => {
                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while initialize screen.', errorMessage);
                                reject(error);
                            })
                            .then(() => resolve(info));
                    }
                    resolve(info);
                });
        });
    }

    //@M ���������� ����������
    application.prototype.UpdateApplication = function (data) {
        return new Promise((resolve, reject) => {
            let inputsObj = {
                ScreenName: data['ScreenName'],
                ViewName: data['ViewName'],
            }
            GSCrmInfo.Application.CommonRequests.UpdateApplicationInfo(inputsObj)
                .fail(error => {
                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating application info.', errorMessage);
                    reject(error);
                })
                .done(info => {
                    GSCrmInfo.ApplicationInfo = info;
                    let PR = new DefaultApplicationPR();
                    PR.RenderApplication();

                    // ��������, ��� � ���������� ���������� ������� �����
                    if (!Object.is(info['CurrentScreen'], null)) {
                        let newScreenName = info['CurrentScreen']['Name'];

                        // ��������, ��� � ���������� ���������� �������� �������������
                        let newViewName = data['CurrentView'] == null ? null : data['CurrentView']['Name'];
                        let screen = new GSCrmInfo.Application.Screen(newScreenName);
                        let inputsObj = {
                            NewScreenName: newScreenName,
                            ViewName: newViewName
                        }
                        screen.Initialize(inputsObj)
                            .catch(error => {
                                let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while initialize screen.', errorMessage);
                                reject(error);
                            })
                            .then(() => resolve(data));
                    }
                });
        });
    }

    //@M ������� ���� � �������
    application.prototype.CloseSelectArea = function() {
        let PR = new DefaultApplicationPR();
        PR.CloseSelectArea();
        GSCrmInfo.SetElement("Picklist", null);
    }

    //@M ��������� �������������, ������ �������� ��������� �������
    application.prototype.GetActiveView = function (event) {
        return $(event.currentTarget).closest('[data-type="view"]').attr('data-name');
    };

    //@M ������������ ������� �� �����
    application.prototype.GoToScreen = function(screenName) {
        let routing;
        if (screenName != "Home Screen") {
            routing = document.location.protocol + "//" + document.location.host + GSCrmInfo['ApplicationInfo']['ScreensRouting'][screenName]
        }
        else {
            routing = document.location.protocol + "//" + document.location.host + "/";
        }
        document.location.href = routing;
    }

    //@M ����� ������
    application.prototype.SelectScreenMenuItem = function(event, newScreenName, newViewName) {
        return new Promise((resolve, reject) => {
            let data = {
                ScreenName: newScreenName,
                ViewName: newViewName
            }
            
            // ���������� ���������� � ������������� � ���������� ������ ���������� ������
            GSCrmInfo.Application.CommonRequests.UpdateApplicationInfo(data)
                .fail(error => {
                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating application info.', errorMessage);
                    reject(error);
                })
                // ���� ��� �������, ���������� ���������� � ������ � ���������� ������� ������
                .done(info => {
                        let oldScreen = GSCrmInfo.CurrentElementsInfo['Screen'];
                        let currentElementsInfo = GSCrmInfo['CurrentElementsInfo'];
                        let oldView = currentElementsInfo['View'];
                        GSCrmInfo['ApplicationInfo'] = info;

                        // � ������, ���� � ������� ������ ���� �������������
                        if (!Object.is(oldView, null)) {
                            let inputsObj = {
                                NewScreenName: newScreenName,
                                OldScreenName: oldScreen['Name'],
                                OldViewName: oldView['Name'],
                                Action: "SelectScreen"
                            }
                            
                            oldScreen.UpdateInfo(inputsObj)
                                .catch(error => {
                                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating screen info', errorMessage);
                                    reject(error);
                                })
                                // ���� ��� �������, ������� �� ������ ��������
                                .then(() => GSCrmInfo.Application.GoToScreen(newScreenName));
                        }
        
                        // ����� ������ ������� �� ������
                        else GSCrmInfo.Application.GoToScreen(newScreenName);
                    });
        });
    }

    //@M ������� ����� ���� treadbar
    application.prototype.SelectTreadItem = function(event, screen, oldView) {
        let crumb = GSCrmInfo.ScreenInfo['Crumbs'].filter(item => item['CrumbId'] == $(event.currentTarget).closest('[data-type="TreadItem"]').attr('id'))[0];
        let newScreenName = crumb['ScreenName'];
        let newViewName = crumb['ViewName'];
        // ���� ������ ����� �� ������ �����
        if (screen.Name != newScreenName) {
            let data = {
                ScreenName: newScreenName
            }

            // ���������� ���������� � ����������, ���� ���������� ����� ��������� �����
            GSCrmInfo.Application.CommonRequests.UpdateApplicationInfo(data)
                .fail(error => {
                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating application info.', errorMessage);
                    reject(error);
                })
                // ���� ��� �������, ���������� ���������� �� ������ � ������������� ���������, � ������� �� ��������� � ���������� �������������
                .done(info => {
                    GSCrmInfo.ApplicationInfo = info;
                    data = {
                        CrumbId: crumb['Id'],
                        NewScreenName: newScreenName,
                        NewViewName: newViewName,
                        Action: "SelectCrumb"
                    }
                    screen.UpdateInfo(data)
                        .catch(error => {
                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating screen info', errorMessage);
                            reject(error);
                        })
                        // ���� ��� �������, ������� �� ������ ��������
                        .then(() => GSCrmInfo.Application.GoToScreen(newScreenName));
                });
        }

        // ����� ���������� ���������� ���������� � ��������� �������������
        else {
            let info = {
                CrumbId: crumb['Id'],
                NewScreenName: screen.Name,
                OldViewName: oldView['Name'],
                NewViewName: newViewName,
                Action: "SelectCrumb"
            }

            screen.UpdateInfo(info)
                .catch(error => {
                    let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                    GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating screen info', errorMessage);
                    reject(error);
                })
                .then(() => screen.RenderScreen(newViewName));
        }
    }

    //@M ����� �������� ������(�������������)
    application.prototype.SelectScreenItem = function(event, newScreenName, newViewName) {
        return new Promise((resolve, reject) => {
            let oldScreen = GSCrmInfo.CurrentElementsInfo['Screen'];
            let oldScreenName = oldScreen['Name'];

            // ���� �������� ������������� ������ �������� ������, ���������� ���������� ���������� � ��� � ������������� ������
            if (newScreenName == oldScreenName) {
                let oldViewName = GSCrmInfo.CurrentElementsInfo['View']['Name'];
                if (newViewName != oldViewName) {
                    let info = {
                        OldScreenName: oldScreenName,
                        NewScreenName: newScreenName,
                        OldViewName: oldViewName,
                        NewViewName: newViewName,
                        Action: "SelectScreenItem"
                    }
    
                    oldScreen.UpdateInfo(info)
                        .catch(error => {
                            let errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                            GSCrmInfo.Application.CommonAction.RaiseErrorText('An error occured while updating screen info', errorMessage);
                            reject(error);
                        })
                        .then(() => oldScreen.RenderScreen(newViewName));
                }
            }

            // �����, ����������� ���������� � ���������� � ���������� �������� �� ����� �����
            else application.prototype.SelectScreenMenuItem(event, newScreenName, newViewName);
        })
    }

    //@C ������� �� ���
    application.prototype.CommonRequests = Requests;

    //@C ��������
    application.prototype.CommonAction = Action;

    //@C �����
    application.prototype.Screen = Screen;

    //@C �������������
    application.prototype.View = View;

    //@C ������
    application.prototype.Applet = Applet;

    //@C ���� ������
    application.prototype.TileApplet = (TileApplet)(application);
    
    //@C ���� ������
    application.prototype.FormApplet = (FormApplet)(application);

    //@C ����� ������
    application.prototype.PopupApplet = (PopupApplet)(application);

    //@C �������
    application.prototype.Control = Control;

    //@C ���������
    application.prototype.Calendar = Calendar;
    
    //@C �������
    application.prototype.PickList = PickList;

    //@C ������
    application.prototype.Navbar = Navbar;
    
    return application;
})()