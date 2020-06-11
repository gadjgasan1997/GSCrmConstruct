export default class DefaultScreenPR {
    //@M Рендеринг экрана
    RenderScreen(screen) {
        let info = GSCrmInfo.ScreenInfo;
        let childViews = info['ChildViews'];
        
        if (info['Crumbs'].length > 0) {
            let treadBar = $('<div data-type="TreadBar" class="gs-treadbar-wrapper"><div><p class="label-lg">Treads</p></div><div class="gs-treadbar mt-4 mb-4"></div></div>');

            info['Crumbs'].forEach((item, index) => {
                if (index != 0)
                    treadBar.find('.gs-treadbar').append('<span style="user-select: none">&emsp;>&emsp;</span>');
                treadBar.find('.gs-treadbar').append('<div data-type="TreadItem" class="gs-treadbar-item" id="' + item['CrumbId'] + '">' + item['Header'] +'</div>');
            });

            treadBar.find('.gs-treadbar').append('<span style="user-select: none">&emsp;&emsp;&emsp;&emsp;</span>');
    
            treadBar
                .find('.gs-treadbar')
                .append('<div data-type="TreadScroll" class="gs-treadbar-scroll">' +
                    '<span data-type="TreadPrevious" class="icon-backward" aria-hidden="true"></span>' +'<span data-type="TreadNext" class="icon-forward" aria-hidden="true"></span>')
                .on('click', '[data-type="TreadPrevious"]', event => {
                    $('.gs-treadbar')[0].scrollLeft -= 100
                })
                .on('click', '[data-type="TreadNext"]', event => {
                    $('.gs-treadbar')[0].scrollLeft += 100;
                });
            
            $(screen).append(treadBar);
        }

            
        $(screen).on('OnViewLoad', () => {
            if (childViews.length > 0) {
                $(screen).append('<div class="gs-tabs"><div class="gs-tabs-header"><p class="label-md">Child views</p></div><div class="gs-tabs-body mt-4 mb-4"></div></div>');
                
                childViews.forEach(view => {
                    let tab = $('<div class="gs-tab" data-type="ScreenItem" data-name="' + view['Name'] + '"><div><p class="label-sm">' + view['Header'] + '</p></div><div></div></div>');
                    
                    // Если название представления этой вкладки совпадает с названием текущего представления, вкладка выделяется
                    if (view['Name'] == info['CurrentView']['Name'])
                        tab.addClass('gs-tab-focused');
        
                    $('.gs-tabs-body').append(tab);
                });
            }
        });
            
        return screen;
    }
}