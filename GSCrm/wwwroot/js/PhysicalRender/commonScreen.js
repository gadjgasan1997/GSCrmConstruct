class Screen {
    constructor(name, id) {
        this.Name = name;
        this.Id = id;
    }

    //#region //@R Свойства
    get Info() {
        return Info.ScreenInfo;
    }

    // Возвращает путь для представления
    get Path() {
        return {
            "Applet Screen": "/api/Applet/",
            "Business Component Screen": "/api/BusinessComponent/",
            "Business Object Screen": "/api/BusinessObject/",
            "Link Screen": "/api/Link/",
            "Screens Screen": "/api/Screen/",
            "View Screen": "/api/Views/",
            "PickList Screen": "/api/PickList/",
            "Physical Render Screen": "/api/PhysicalRender/",
            "Icon Screen": "/api/Icon/",
            "Action Screen": "/api/Action/",
            "Table Screen": "/api/Table/"
        }[this.Name];
    }
    //#endregion

    //#region //@R Инициализация
    //@M Инициализация скрина
    Initialize() {
        return new Promise((resolve, reject) => {
            let request = new RequestsToDB;
            request.InitializeScreen(this)
                .fail(error => reject(error))
                .done(info => {
                    // Установка текущих элементов
                    Info.ScreenInfo = info;
                    
                    // Рендеринг скрина
                    let PR = new DefaultScreenPR;
                    PR.RenderScreen(this)
                        .catch(error => reject(error))
                        .then(() => resolve());
                });
        });
    }
    //#endregion

    //#region //@R Методы для работы со скринами
    //@M Осущесятвляет запрос на бек, получая информацию о скрине
    RequestInfo() {
        return new Promise((resolve, reject) => {
            var request = new RequestsToDB;
            request.GetScreenInfo(this)
                .fail(error => reject(error))
                .done(info => resolve(info));
        })
    }

    //@M Осуществляет запрос на бек, обновляя информацию о представлении
    UpdateInfo(info) {
        return new Promise((resolve, reject) => {
            let request = new RequestsToDB;
            request.UpdateScreenInfo(this, info)
                .fail(error => reject(error))
                .done(screenInfo => {
                    Info.ScreenInfo = screenInfo;
                    resolve();
                });
        });
    }
    //#endregion
}