$(document).on('click', '.popup-wrap', event => {
    event.stopPropagation();
    if (!$('.select-area').hasClass('d-none')) {
        CloseSelectArea();
    }
})

$(document).on('click', '.p-modal-close', event => ClosePopup(event, true));

// При изменнии размеров документа скрывать или отоброжать скроллы (если модальное окно раскрыто)
$(window).on('resize', () => {
    if (!$('.all-popup').hasClass('d-none')) {
        if (window.innerWidth < 992) {
            $('html').css('overflow', 'hidden')
        }
        else {
            $('html').css('overflow', 'auto')
        }
    }
})