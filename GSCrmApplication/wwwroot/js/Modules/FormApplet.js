import DefaultFormAppletPR from '../PhysicalRenders/DefaultFormAppletPR.js'

export default function (application) {
    let formApplet = function(name, id) {
        application.prototype.Applet.apply(this, [name, id]);
    }
    
    formApplet.prototype = Object.create(application.prototype.Applet.prototype);
    
    //#region //@R �������������
    //@M ������������� ���� �������
    formApplet.prototype.Initialize = function() {
        return new Promise((resolve, reject) => {
            this.InitializeApplet(this.Name)
                .catch(error => reject(error))
                .then(() => {
                    // ��������� �������� � ������, ������������ � �������
                    GSCrmInfo.Application.CommonRequests.GetRecord(this.Info)
                        .catch(error => reject(error))
                        .then(recordInfo => {
                            // ��������� recordSet-�
                            this.SetRecordSet(recordInfo);
        
                            // ������������� �������
                            let PR = new DefaultFormAppletPR();
                            let applet = PR.RenderApplet(this.Name);
        
                            // ������������� ���������
                            this.InitializeControls(this.Name);

                            // ��������� ��������� ������
                            if (recordInfo != null) {
                                GSCrmInfo.SetSelectedRecord(this.Name, {
                                    "record": applet,
                                    "properties": recordInfo
                                });
                            }
                            else GSCrmInfo.SetSelectedRecord(this.Name, null);
                            resolve();
                        });
                });
        });
    };
    //#endregion
    
    //#region //@R ������ ��� ������ � ������
   /**
    *@M ���������� ������ � ������� �� recordSet-�
    * @param {String} tileItem ������ � �������
    */
    formApplet.prototype.RefreshControls = function() {
        $('#' + this.Id + ' [data-type="control"]').empty();
        this.InitializeControls();
    }

    /**
     *@M ���������� �������� � ������� �� recordSet-�
     * @param {String} tileItem ������ � �������
     */
     formApplet.prototype.RefreshControl = function(controlName) {
         let control = $('#' + this.Id + ' [data-name="' + controlName + '"]');
         control.empty();
         this.InitializeControl(control);
     }
    //#endregion
    return formApplet;
}