class DefaultScreenPR {
    //@M Рендеринг скрина
    RenderScreen(screen) {
        return new Promise((resolve, reject) => {
            let info = Info.ScreenInfo;
            let currentView = new View(info.CurrentView['Name']);
            let currentViews = info.CurrentViews;
            Info.SetElement('View', currentView);

            // Если имеются дочерние представления, то к ним необходимо добавить текущее, заккоментил на этапе разработки
            // if (currentViews.length > 0)
               // currentViews.unshift(info.CurrentView);
    
            // Инициализация текущего представления
            currentView.Initialize()    
                .catch(error => reject(error))
                .then(() => {
                    currentView = new View(info.CurrentView['Name']);
                    let currentApplet = currentView['Info']['ViewItems'][0];
                    let selectedRecords = Info.SelectedRecords.get(currentApplet['AppletName']);

                    Info.SetElement('TargetApplet', currentApplet);
                    if (selectedRecords != undefined)
                        Info.SetElement('Record', selectedRecords['properties']);

                    // Добавление табов
                    $('<div class="gs-tabs mt-4 mb-2"></div>')
                    .insertBefore($('.large-container [data-type="view"]')
                    .first());

                    currentViews.forEach(view => {
                        let tab = $('<div class="gs-tab" data-name="' + view['Name'] + '"><div>' + 
                        view['Header'] + '</div><div></div></div>');

                        // Если название представления этой вкладки совпадает с названием текущего представления, вкладка выделяется
                        if (view['Name'] == info.CurrentView['Name'])
                            tab.addClass('gs-tab-focused');

                        // При нажатии на вкладку необходимо обновить информацию о скрине на беке, а затем заново отрендерить скрин
                        tab.on('click', () => {
                            if (view['Name'] != Info.CurrentElementsInfo['View']['Name']) {
                                let info = {
                                    Name: screen.Name,
                                    ViewName: view['Name'],
                                    Action: "SelectScreenItem"
                                }
    
                                screen.UpdateInfo(info)
                                    .catch(error => reject(error))
                                    .then(() => {
                                        this.RenderScreen(screen);
                                    });
                            }
                        });

                         $('.gs-tabs').append(tab);
                    });
                    resolve();
                });
        });
    }
}