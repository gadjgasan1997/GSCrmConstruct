export default class DefaultFormAppletPR {
    RenderApplet(appletName) {
        let info = GSCrmInfo.GetAppletInfo(appletName);
        let $applet = $("#" + info['AppletId']);   
        $applet
            .empty()
            .addClass('gs-applet')
            .append('<div class="gs-form-wrapper"><div class="applet-header"></div><div class="row justify-content-between"></div></div>');

        if (info['Header'] != null)
            $applet.find('.applet-header').append('<p>' + info['Header'] + '</p>');

        info['Controls'].forEach((control, index) => {
            switch(control['Type']) {
                case "button": {
                    $applet
                        .find('.row')
                        .append('<div class="col-md-4 col-lg-4 mb-4"><div style="width: 80%; margin-left: 10%" data-type="control" data-name="'
                         + control['Name'] + '" tabindex="' + (!control['Readonly'] ? Number(index + 1) : -1) + '"></div></div>');
                    break;
                }
                case "checkbox": {
                    $applet.find('.row').append('<div class="col-md-4 col-lg-4 mb-4"><div class="gs-field" style="width: 80%; margin-left: 10%"' +
                    ' data-type="control" data-name="' + control['Name'] + '"></div></div>');
                    break;
                }
                case "input":
                case "picklist":
                {
                    $applet.find('.row').append('<div class="col-md-4 col-lg-4 mb-4">' +
                        '<div class="gs-field" style="width: 80%; margin-left: 10%" data-type="control" data-name="' + control['Name'] + 
                        '" tabindex="' + (!control['Readonly'] ? Number(index + 1) : -1) + '"></div></div>');
                        break;
                }

                case "drilldown":
                    $applet.find('.row').append('<div class="col-md-4 col-lg-4 mb-4"><div class="gs-field" style="width: 80%; margin-left: 10%"' +
                    ' data-type="control" data-name="' + control['Name'] + '"></div></div>');
                    break;
            }
        });

        return $applet;
    }
}