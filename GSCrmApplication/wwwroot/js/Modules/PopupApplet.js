import DefaultPopupPR from '../PhysicalRenders/DefaultPopupPR.js'

export default function (application) {
    let popupApplet = function(name, id) {
        application.prototype.Applet.apply(this, [name, id]);
    }
    
    popupApplet.prototype = Object.create(application.prototype.Applet.prototype);

    //#region //@R Инициализация и уничтожение попапа
    /**
     *@M Инициализация попап апплета, на вход принимается события нажатия на контрол, по которому открывается попап
    * @param {Event} event Событие нажатия на контрол
    */
    popupApplet.prototype.Initialize = function(event) {
        return new Promise((resolve, reject) => {
            let curretnElementsInfo = GSCrmInfo["CurrentElementsInfo"];
            let targetApplet = curretnElementsInfo['TargetApplet'];
            let currentControl = curretnElementsInfo['CurrentControl'];
            let currentRecord = curretnElementsInfo['CurrentRecord'];
            let recordId = currentRecord == null ? null : currentRecord['Id'];
            let appletItem = $(event.currentTarget).closest('[data-type="applet_item"]');

            // Получение информации о попапе
            this.InitializeApplet(this.Name)
                .catch(error => reject(error))
                .then(() => { 
                    // Получение сведений о записи, отоброжаемой в попапе
                    this.GetAppletRecord()
                        .catch(error => reject(error))
                        .then(data => {
                            // Установка recordSet-а
                            this.SetRecordSet(data);
                        
                            // Получение размера попапа из user property апплета, с которого он был открыт
                            let size = targetApplet.GetInfo()['ControlUPs'][currentControl].filter(item => item['Name'] == 'Size')[0]['Value'];

                            // Создание модального окна
                            // Скрытие области с выбором, обязательно вначале, так как проставляется record-set
                            /*if (!$('[data-type="SelectArea"]').hasClass('d-none')) {
                                view.CloseSelectArea();
                            }*/

                            // Инициализация попап апплета
                            GSCrmInfo.SetElement("PopupApplet", this);
                            GSCrmInfo.SetElement('CurrentApplet', this);
                            let PR = new DefaultPopupPR();
                            PR.RenderApplet(size);

                            // Инициализация контролов
                            this.InitializeControls(this.Name);

                            /* Если поднятие попапа произошло на записи тайл апплета, необходимо обновить контекст дочерних сущностей
                            и сфокусироваться на выделенной */
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
     *@M Закрытие попапа, в зависимости от типа операции выполняя либо обновление информации о представлении, либо ее запрос с бека
    * @param {String} operationType Тип операции
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
    
    //#region //@R Методы для работы с формой
   /**
    *@M Обновление строки в апплете из recordSet-а
    * @param {String} tileItem Строка в апплете
    */
   popupApplet.prototype.RefreshControls = function() {
    $('#' + this.Id + ' [data-type="control"]').empty();
    this.InitializeControls();
    }

    /**
     *@M Обновление контрола в апплете из recordSet-а
    * @param {String} tileItem Строка в апплете
    */
   popupApplet.prototype.RefreshControl = function(controlName) {
        let control = $('#' + this.Id + ' [data-name="' + controlName + '"]');
        control.empty();
        this.InitializeControl(control);
    }
    //#endregion
    return popupApplet;
}