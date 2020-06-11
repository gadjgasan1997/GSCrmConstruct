export default class DefaultNavbarPR {
    RenderBrand() {
        let navbar = $('[data-type="Navbar"]');
        navbar
            .addClass('gs-navbar row')
            .append('<div class="gs-nav-brand"><p style="line-height: 50px" class="label-md">GSCrm</p></div>' +
            '<div class="gs-nav-toggler" data-type="NavbarToggler" collapsed="false"><a class="icon-menu"></a></div>' +
            '<div class="gs-nav-toggler-brand" data-type="NavbarToggler" style="display: none" collapsed="false"><h4>Screens</h4></div>');
    }
    
    RenderNavbar(event, screens, currentScreen, isInit) {
        let navbar = $('[data-type="Navbar"]');
        navbar
            .empty()
            .addClass('gs-navbar row')
            .append('<div class="gs-nav-brand"><p style="line-height: 50px" class="label-md">GSCrm</p></div>' +
                '<div class="gs-nav-toggler" data-type="NavbarToggler"><a class="icon-menu"></a></div>' +
                '<div class="gs-nav-toggler-brand" data-type="NavbarToggler"><h4>Screens</h4></div></div>' +
                '<div class="gs-nav-area" style="display: none">' +
                '<div style="margin-top: 10px" class="row gs-nav-area-header"><p class="label-lg">Screens</p></div>' +
                '<div class="divider-mr-md"></div><div class="row gs-nav-area-body"></div></div>');

        screens.forEach(screen => {
            let screenItem = '<div class="col-md-4 col-lg-3 gs-nav-item" data-type="ScreenMenuItem" data-name="' +
                screen["Name"] + '"><div class="gs-nav-link row"><div class="col"><p>' + screen["Header"] + '</p><p></p></div><div class="expand-items col-auto">' +
                    '<span class="icon-stack expand-icon"></span></div></div></div>';
            if (screen["Name"] == currentScreen["Name"]) {
                screenItem = '<div disabled class="col-md-4 col-lg-3 gs-nav-item-static" data-type="ScreenMenuItem" data-name="' +
                    screen["Name"] + '"><div class="gs-nav-link row"><div class="col"><p>' + screen["Header"] + '</p><p></p></div>' +
                    '<div class="expand-items col-auto"><span class="icon-stack expand-icon"></span></div></div></div>';
            }
            $(navbar).find('.gs-nav-area > .row').last().append(screenItem);
            $('.gs-nav-link .col')
                .off('click')
                .on('click', event => {
                    $(navbar).trigger('SelectScreenMenuItem', { 
                        'Event': event, 
                        'Screen': $(event.currentTarget).closest('[data-type="ScreenMenuItem"]').attr('data-name'),
                        'View': null
                    });
                });
        });
        
        $('.gs-nav-toggler-brand').css("display", 'block');
        
        navbar
            .find('.expand-items')
            .off('click')
            .on('click', event => {
                let screen = new GSCrmInfo.Application.Screen($(event.currentTarget).closest('[data-type="ScreenMenuItem"]').attr('data-name'));
                screen.RequestInfo()
                    .catch(error => console.log(error))
                    .then(screenInfo => {
                        $('.gs-nav-area')
                            .empty()
                            .append('<div class="row gs-nav-area-header"><div class="hide-items col-auto">' +
                                '<span class="hide-icon icon-long-arrow-left"></span></div>' +
                                '<div style="margin-top: 5px" class="col"><p class="label-lg">Screen views</p></div>' +
                                '<div style="visibility: hidden" class="hide-items col-auto"><span class="hide-icon icon-long-arrow-left"></span></div></div>' +
                                '<div style="background-color: black" class="divider-mr-none"></div><div class="row"></div>')
                            .off('click', '.hide-items')
                            .on('click', '.hide-items', event => this.RenderNavbar(event, screens, currentScreen, false));

                        let categories = screenInfo['AllCategoriesViews'];
                        let allCategories = screenInfo['AllCategories'];
                        let currentView = GSCrmInfo.CurrentElementsInfo['View'];
                        for (let category in categories) {
                            if (allCategories[category]['DisplayInSiteMap']) {
                                let categoryColumn = $('<div class="col"><div style="margin-top: 10px; margin-bottom: 10px" class="row gs-nav-area-header"><p class="label-md">' + 
                                category + '</p></div><div class="divider-mr-md"></div>' + '<div class="row gs-nav-area-body"></div></div>');
                                categories[category].forEach(view => {
                                    if (view['DisplayInSiteMap']) {
                                        let viewElement = $('<div class="col-md-4 col-lg-3" data-type="ScreenItem" data-name="' + view["Name"] + 
                                        '"><div class="gs-nav-link-short row"><div class="col col-padding"><p>' + view["Header"] + '</p><p></p></div></div>');
        
                                        if (currentView != null && view['Name'] == currentView['Name'] && view['Name'] == screenInfo['CurrentView']['Name']) 
                                            $(viewElement).addClass('gs-nav-item-static');
                                        else $(viewElement).addClass('gs-nav-item');
        
                                        $(viewElement)
                                            .off('click')
                                            .on('click', event => {
                                                $(navbar).trigger('SelectScreenItem', { 
                                                    'Event': event,
                                                    'Screen': screenInfo['Name'],
                                                    'View': $(event.currentTarget).attr('data-name')
                                                });
                                            });
                                        
                                        $(categoryColumn).find('.gs-nav-area-body').last().append(viewElement);                                        
                                    }
                                })
                                $('.gs-nav-area > .row').last().append(categoryColumn);
                            }
                        }
                    });
            });
            
        navbar
            .find('.gs-nav-area')
            .off('click')
            .on('click', event => event.stopPropagation());
        
        if (isInit)
            navbar.find('.gs-nav-area').slideDown('fast');
        else {
            navbar.find('.gs-nav-toggler').attr('collapsed', true);
            navbar.find('.gs-nav-toggler-brand').attr('collapsed', true);
            navbar.find('.gs-nav-area').css('display', 'block');
        }

        return navbar;
    }

    DisposeNavbar() {
        let navbar = $('.gs-nav-area');
        $('.gs-nav-toggler-brand').css("display", 'none');
        navbar.slideUp('fast').empty();
        return navbar;
    }

    HideNavbar() {
        let navbar = $('.gs-nav-area');
        $('.gs-nav-toggler-brand').css("display", 'none');
        navbar.css("display", 'none');
        return navbar;
    }
}