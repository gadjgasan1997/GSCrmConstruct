import DefaultFormAppletPR from '../PhysicalRenders/DefaultFormAppletPR.js'

export default function (application) {
    let formApplet = function(name, id) {
        application.prototype.Applet.apply(this, [name, id]);
    }
    
    formApplet.prototype = Object.create(application.prototype.Applet.prototype);
    
    //#region //@R Инициализация
    //@M Инициализация форм апплета
    formApplet.prototype.Initialize = function() {
        return new Promise((resolve, reject) => {
            this.InitializeApplet(this.Name)
                .catch(error => reject(error))
                .then(() => {
                    // Получение сведений о записи, отоброжаемой в апплете
                    GSCrmInfo.Application.CommonRequests.GetRecord(this.Info)
                        .catch(error => reject(error))
                        .then(recordInfo => {
                            // Установка recordSet-а
                            this.SetRecordSet(recordInfo);
        
                            // Инициализация апплета
                            let PR = new DefaultFormAppletPR();
                            let applet = PR.RenderApplet(this.Name);
        
                            // Инициализация контролов
                            this.InitializeControls(this.Name);

                            // Установка выбранной записи
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
    
    //#region //@R Методы для работы с формой
   /**
    *@M Обновление строки в апплете из recordSet-а
    * @param {String} tileItem Строка в апплете
    */
    formApplet.prototype.RefreshControls = function() {
        $('#' + this.Id + ' [data-type="control"]').empty();
        this.InitializeControls();
    }

    /**
     *@M Обновление контрола в апплете из recordSet-а
     * @param {String} tileItem Строка в апплете
     */
     formApplet.prototype.RefreshControl = function(controlName) {
         let control = $('#' + this.Id + ' [data-name="' + controlName + '"]');
         control.empty();
         this.InitializeControl(control);
     }
    //#endregion
    return formApplet;
}