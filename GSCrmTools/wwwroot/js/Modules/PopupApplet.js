import DefaultPopupPR from '../PhysicalRenders/DefaultPopupPR.js'

export default function (application) {
    let popupApplet = function(name, id) {
        application.prototype.Applet.apply(this, [name, id]);
    }
    
    popupApplet.prototype = Object.create(application.prototype.Applet.prototype);

    //#region //@R ������������� � ����������� ������
    /**
     *@M ������������� ����� �������, �� ���� ����������� ������� ������� �� �������, �� �������� ����������� �����
    * @param {Event} event ������� ������� �� �������
    */
    popupApplet.prototype.Initialize = function(event) {
        return new Promise((resolve, reject) => {
            let curretnElementsInfo = GSCrmInfo["CurrentElementsInfo"];
            let targetApplet = curretnElementsInfo['TargetApplet'];
            let currentControl = curretnElementsInfo['CurrentControl'];
            let currentRecord = curretnElementsInfo['CurrentRecord'];
            let recordId = currentRecord == null ? null : currentRecord['Id'];
            let appletItem = $(event.currentTarget).closest('[data-type="applet_item"]');

            // ��������� ���������� � ������
            this.InitializeApplet(this.Name)
                .catch(error => reject(error))
                .then(() => { 
                    // ��������� �������� � ������, ������������ � ������
                    this.GetAppletRecord()
                        .catch(error => reject(error))
                        .then(data => {
                            // ��������� recordSet-�
                            this.SetRecordSet(data);
                        
                            // ��������� ������� ������ �� user property �������, � �������� �� ��� ������
                            let size = targetApplet.GetInfo()['ControlUPs'][currentControl].filter(item => item['Name'] == 'Size')[0]['Value'];

                            // �������� ���������� ����
                            // ������� ������� � �������, ����������� �������, ��� ��� ������������� record-set
                            /*if (!$('[data-type="SelectArea"]').hasClass('d-none')) {
                                view.CloseSelectArea();
                            }*/

                            // ������������� ����� �������
                            GSCrmInfo.SetElement("PopupApplet", this);
                            GSCrmInfo.SetElement('CurrentApplet', this);
                            let PR = new DefaultPopupPR();
                            PR.RenderApplet(size);

                            // ������������� ���������
                            this.InitializeControls(this.Name);

                            /* ���� �������� ������ ��������� �� ������ ���� �������, ���������� �������� �������� �������� ���������
                            � ��������������� �� ���������� */
                            if (targetApplet.GetInfo()['Type'] == 'Tile' && appletItem.length > 0 && recordId != null) {
                                targetApplet = new GSCrmInfo.Application.TileApplet(targetApplet.GetName(), targetApplet.GetId());
                                targetApplet.Focus(appletItem);
                                curretnElementsInfo['View'].PartialUpdateContext(targetApplet.GetInfo(), false)
                                    .catch(error => reject(error))
                                    .then(() => resolve());
                            }
                            else resolve();
                        });
                });
        });
    }

    /**
     *@M �������� ������, � ����������� �� ���� �������� �������� ���� ���������� ���������� � �������������, ���� �� ������ � ����
    * @param {String} operationType ��� ��������
    */
    popupApplet.prototype.Dispose = function() {
        let curretnElementsInfo = GSCrmInfo.CurrentElementsInfo;
        let targetApplet = curretnElementsInfo['TargetApplet'];
        this.DeleteRecordSet();
        GSCrmInfo.RemoveAppletInfo(this.Name);
        GSCrmInfo.SetElement('PopupApplet', null);
        GSCrmInfo.SetElement('CurrentApplet', targetApplet);
        GSCrmInfo.SetElement('CurrentPopupControl', null);
        GSCrmInfo.Application.CloseSelectArea();
        $('[data-type="PopupArea"]').addClass('d-none').empty();
        $('[data-type="MaskArea"]').addClass('d-none');
        $('html').css('overflow', 'auto');
    }
    //#endregion
    
    //#region //@R ������ ��� ������ � ������
   /**
    *@M ���������� ������ � ������� �� recordSet-�
    * @param {String} tileItem ������ � �������
    */
   popupApplet.prototype.RefreshControls = function() {
    $('#' + this.Id + ' [data-type="control"]').empty();
    this.InitializeControls();
    }

    /**
     *@M ���������� �������� � ������� �� recordSet-�
    * @param {String} tileItem ������ � �������
    */
   popupApplet.prototype.RefreshControl = function(controlName) {
        let control = $('#' + this.Id + ' [data-name="' + controlName + '"]');
        control.empty();
        this.InitializeControl(control);
    }
    //#endregion
    return popupApplet;
}