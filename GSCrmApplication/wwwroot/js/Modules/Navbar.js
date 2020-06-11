import DefaultNavbarPR from '../PhysicalRenders/DefaultNavbarPR.js'

export default class Navbar {
    Initialize(event) {
        event.stopPropagation();
        let PR = new DefaultNavbarPR();
        let applicationInfo = GSCrmInfo["ApplicationInfo"];
        let navbar = PR.RenderNavbar(event, applicationInfo["Screens"], applicationInfo["CurrentScreen"], true);
        $(navbar)
            .closest('[data-type="GSCrmHeader"]')
            .find('[data-type="NavbarToggler"]')
            .attr('collapsed', 'true');
        $(navbar)
            .off('SelectScreenMenuItem')
            .off('SelectScreenItem')
            .on('SelectScreenMenuItem', (event, args) => {
                PR.HideNavbar();
                let currentElementsInfo = GSCrmInfo.CurrentElementsInfo;
                if (currentElementsInfo['AppletToUpdate'] != null) {
                    // Вызов автообновления записи
                    GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, currentElementsInfo['RecordToUpdate']).then(errorMessage => {
                        if (errorMessage == undefined) {
                            GSCrmInfo.Application.SelectScreenMenuItem(args['Event'], args['Screen'], args['View']).catch(error => console.log(error));
                        }
                    });
                }
                else GSCrmInfo.Application.SelectScreenMenuItem(args['Event'], args['Screen'], args['View']).catch(error => console.log(error));
            })
            .on('SelectScreenItem', (event, args) => {
                PR.HideNavbar();
                let currentElementsInfo = GSCrmInfo.CurrentElementsInfo;
                if (currentElementsInfo['AppletToUpdate'] != null) {
                    // Вызов автообновления записи
                    GSCrmInfo.Application.CommonAction.Invoke('AutoUpdateRecord', event, currentElementsInfo['RecordToUpdate']).then(errorMessage => {
                        if (errorMessage == undefined) {
                            GSCrmInfo.Application.SelectScreenItem(args['Event'], args['Screen'], args['View']).catch(error => console.log(error));
                        }
                    });
                }
                else GSCrmInfo.Application.SelectScreenItem(args['Event'], args['Screen'], args['View']).catch(error => console.log(error));
            });
    }

    Dispose(event) {
        event.stopPropagation();
        let PR = new DefaultNavbarPR();
        let applicationInfo = GSCrmInfo["ApplicationInfo"];
        let navbar = PR.DisposeNavbar(event, applicationInfo["Screens"], applicationInfo["CurrentScreen"]);
        $(navbar)
            .closest('[data-type="GSCrmHeader"]')
            .find('[data-type="NavbarToggler"]')
            .attr('collapsed', 'false');
    }
}