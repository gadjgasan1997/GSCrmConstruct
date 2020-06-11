import DefaultApplicationPR from './PhysicalRenders/DefaultApplicationPR.js'
import GSCrm from './CommonGSCrm.js'

$(document)
    .off('click')
    .on('click', () => {
        if (!$('[data-type="SelectArea"]').hasClass('d-none')) {
            GSCrmInfo.Application.CloseSelectArea();
        }
        if ($('[data-type="Navbar"]').css('display') == 'block') {
            $('[data-type="NavbarToggler"]').css("display", 'none');
            $('[data-type="Navbar"]').slideUp("fast");
        }
        if ($('[data-type="NavbarToggler"]').attr('collapsed'))
        {
            let siteMap = new GSCrmInfo.Application.Navbar();
            siteMap.Dispose(event);
        }
    })
    .ready(() => {
        let errorMessage;
        GSCrmInfo.Application = new GSCrm("GSCrm");
        
        if (sessionStorage.getItem('GSCrmApplicationInfo') == undefined) {
            GSCrmInfo.Application.Initialize().catch(error => {
                errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', errorMessage);
            });
        }

        else {
            GSCrmInfo.ApplicationInfo = JSON.parse(sessionStorage.getItem('GSCrmApplicationInfo'));
            let PR = new DefaultApplicationPR();
            PR.RenderApplication();
            let pathName = document.location.pathname.split('/')[1];
            if (pathName != undefined) {
                let newScreenName = pathName.split("%20").join(" ").split("/").join("");
                GSCrmInfo.Application.CommonRequests.GetApplicationInfo()
                    .catch(error => {
                        errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                        GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', errorMessage);
                    })
                    .then(info => {
                        GSCrmInfo.ApplicationInfo = info;
                        let PR = new DefaultApplicationPR();
                        PR.RenderApplication();

                        // Проверка, что в приложении существует текущий экран
                        if (!Object.is(info['CurrentScreen'], null)) {
                            newScreenName = info['CurrentScreen']['Name'];
                            
                            // Проверка, что в приложении существует текущиее представление
                            let newViewName = info['CurrentView'] == null ? null : info['CurrentView']['Name'];
                            let screen = new GSCrmInfo.Application.Screen(newScreenName);
                            let inputsObj = {
                                NewScreenName: newScreenName,
                                NewViewName: newViewName
                            }
                            
                            screen.Initialize(inputsObj).catch(error => {
                                errorMessage = GSCrmInfo.Application.CommonAction.RenderError(error);
                                GSCrmInfo.Application.CommonAction.RaiseErrorText('Error', errorMessage);
                            });
                        }
                    });
            }
        }
    });

// При клике по зоне с выбором остановка всплытия
$(document).on('click', '[data-type="SelectArea"]', event => event.stopPropagation());

// При закрытии модального окна
$(document).on('click', '.gs-popup-close', event => {
    let currentElements = GSCrmInfo.CurrentElementsInfo;
    GSCrmInfo.Application.CommonAction.DisposePopupApplet('ClosePopup', currentElements['CurrentRecord']['Id'], 'ClosePopup');
});

// При открытии навбара
$(document).on('click', '[data-type="NavbarToggler"]', event => {
    let siteMap = new GSCrmInfo.Application.Navbar();
    let isCollapsed = $('[data-type="NavbarToggler"]').attr('collapsed');
    if (isCollapsed == 'false') {
        siteMap.Initialize(event);
    }
});

// При изменнии размеров документа скрывать или отоброжать скроллы (если модальное окно раскрыто)
$(window).on('resize', () => {
    if (!$('[data-type="PopupArea"]').hasClass('d-none')) {
        if (window.innerWidth < 992) {
            $('html').css('overflow-y', 'hidden')
        }
        else {
            $('html').css('overflow-y', 'auto')
        }
    }
})