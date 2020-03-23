// При клике по зоне с выбором остановка всплытия
$(document).on('click', '.select-area', event => event.stopPropagation());

// При клике по документу скрывать зону с выбором, если она отображается
$(document).on('click', () => {
    if (!$('.select-area').hasClass('d-none')) {
        CloseSelectArea();
    }
})