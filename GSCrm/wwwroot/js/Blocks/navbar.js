$('body').on('click', '.global-search', function (e) {
    e.stopPropagation();
    var search = $(this);
    search
        .css('border', '1px solid #008c95')
        .find('.search-panel')
        .animate({
            left: '20'
        }, 'fast')
        .find('span:first-child')
        .addClass('d-none');
    setTimeout(function () {
        search
            .find('.search-input')
            .removeClass('d-none')
            .find('input')
            .focus()
    }, 400)
    $('.search-panel').css('color', '#008c95')
})

$(document).on('click', function () {
    $('.kf-search').removeClass('d-none');
    $('.global-search').css('border', '1px solid lightgrey');
    $('.search-input')
        .addClass('d-none')
        .find('input')
        .val('')
    $('.search-panel')
        .css('left', 'calc(100% - 100px)')
        .css('color', 'black')
        .find('span:first-child')
        .removeClass('d-none');
})