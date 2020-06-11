export default class DefaultPopupPR {
    RenderApplet(size) {
        let popupApplet = GSCrmInfo.CurrentElementsInfo['PopupApplet'];
        let targetApplet = GSCrmInfo.CurrentElementsInfo['TargetApplet'];
        let appletInfo = popupApplet['Info'];
        let appletId = appletInfo["AppletId"];
        var controls = appletInfo['Controls'];

        // ????
        $('[data-type="PopupArea"]')
            .empty()
            .append('<div class="gs-popup-' + size + '">' +
                '<div class="popup-wrap">' +
                '<div class="row gs-popup-header">' +
                '<div class="gs-popup-dummy"><div></div></div>' +
                '<div><h4>' + appletInfo['Header'] + '</h4></div>' +
                '<div class="gs-popup-close"><div></div></div>' +
                '</div>' +
                '<div class="divider-mr-none"></div>' +
                '<div class="gs-popup-body"></div>' +
                '<div class="divider-mr-none"></div>' +
                '<div class="gs-popup-footer"></div>' +
                '</div>' +
                '</div>')
            .removeClass('d-none');

        // ???????????
        $('[data-type="MaskArea"]').removeClass('d-none');

        // ??? ???????? ?????????? ???????? ??????? (???? ????????? ???? ???????? ?? ???? ?????)
        $('html').css('overflow-x', 'hidden');
        if (window.innerWidth < 992) {
            $('html').css('overflow-y', 'hidden');
        }

        // ???????????? ?????????
        $('.popup-wrap')
            .attr('id', appletInfo['AppletId'])
            .attr('data-name', appletInfo['Name'])
            .attr('data-type', 'applet')
            .attr('data-target-id', targetApplet.GetId());
    
        // ????
        var body = $('<div class="row justify-content-around">' +
            '<div class="col-md">' +
            '<div class="row justify-content-center"><div class="block-center"><p class="label-md">Fill in the data</p></div></div>' +
            '<div class="divider-mr-md"></div>' +
            '<div class="row justify-content-center" id="Fields_' + appletId + '"></div></div>' +
            '<div class="col-md">' +
            '<div class="row justify-content-center"><div class="block-center"><p class="label-md">Actions</p></div></div>' +
            '<div class="divider-mr-md"></div>' +
            '<div class="d-flex flex-row flex-md-column flex-lg-row flex-wrap" id="Actions_' + appletId + '"></div>' +
            '</div></div>');
    
        // ????????
        controls.forEach((control, index) => {
            let name = control['Name'];
            switch(control['Type']) {
                case "button":
                    body
                        .find('#Actions' + '_' + appletId)
                        .append('<div class="col-auto ml-auto mr-auto mb-4"><div data-type="control" data-name="'
                            + name + '" tabindex="' + index + '"></div></div>');
                    break;

                case "checkbox":
                    // ????????
                    let checkbox = $('<div class="col-auto" style="width: 80%; margin: auto; height: 45px">' +
                        '<div class="gs-field" data-type="control" data-name="' + name + '" style="margin: auto" tabindex="' + index + '"></div></div>');
    
                    // ??? ?????????? ? ????? checkbox ????? ??????? ???????
                    if (controls[index + 1] && controls[index + 1]['Type'] == 'checkbox')
                        checkbox.addClass('mb-1');
                    else checkbox.addClass('mb-3');
    
                    // ?????????? ????????
                    body.find('#Fields' + '_' + appletId).append(checkbox);  
                    break;

                case "picklist":
                    // ????????
                    let picklsit = $('<div class="col-auto" style="width: 80%; margin: auto; height: 45px">' +
                        '<div class="gs-field" data-type="control" data-name="' + name + '" style="margin: auto" tabindex="' + index + '"></div></div>');
    
                    // ??? ?????????? ? ????? checkbox ????? ??????? ???????
                    if (controls[index + 1] && controls[index + 1]['Type'] == 'checkbox')
                        picklsit.addClass('mb-1');
                    else picklsit.addClass('mb-3');
    
                    // ?????????? ????????
                    body.find('#Fields' + '_' + appletId).append(picklsit);  
                    break;  

                case "input":
                    // ????????
                    let el = $('<div class="col-auto" style="width: 80%; margin: auto">' +
                        '<div class="gs-field" data-type="control" data-name="' + name + '" style="margin: auto" tabindex="' + index + '"></div></div>');
    
                    // ??? ?????????? ? ????? checkbox ????? ??????? ???????
                    if (controls[index + 1] && controls[index + 1]['Type'] != 'checkbox')
                        el.addClass('mb-4');
                    else el.addClass('mb-3');
    
                    // ?????????? ????????
                    body.find('#Fields' + '_' + appletId).append(el);                        
                    break;
            }
        });
    
        $('#' + appletId).find('.gs-popup-body').append(body);
    }
}