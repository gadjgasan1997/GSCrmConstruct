import DefaultNavbarPR from './DefaultNavbarPR.js'

export default class DefaultApplicationPR {
    RenderApplication() {
        if (!$('[data-type="GSCrmBody"]').hasClass('large-container')) {
            $('[data-type="GSCrmBody"]').addClass('large-container');
        }
        if ($('.gs-header').length == 0) {
            $('[data-type="GSCrmHeader"]').append('<div class="gs-header"></div>');
        }
        if ($('[data-type="MaskArea"]').length == 0) {
            $('[data-type="GSCrmAreas"]').append('<div data-type="MaskArea" class="mask-all-screen d-none"></div>');
        }
        if ($('[data-type="SelectArea"]').length == 0) {
            $('[data-type="GSCrmAreas"]').append('<div data-type="SelectArea" class="select-area d-none"></div>');
        }
        if ($('[data-type="PopupArea"]').length == 0) {
            $('[data-type="GSCrmAreas"]').append('<div data-type="PopupArea" class="gs-popup-applet d-none"></div>');
        }
        if ($('[data-type="ExpectationArea"]').length == 0) {
            $('[data-type="GSCrmAreas"]').append('<div data-type="ExpectationArea" class="cssload-wrapper d-none"><div class="cssload-loader"><div class="cssload-inner cssload-one"></div>' +
            '<div class="cssload-inner cssload-two"></div><div class="cssload-inner cssload-three"></div></div></div>');
        }
        if ($('[data-type="ModalArea"]').length == 0) {
            $('[data-type="GSCrmAreas"]').append('<div data-type="ModalArea" class="d-none"></div>');
        }
        if ($('[data-type="Navbar"]').length == 0) {
            $('.gs-header').append('<div class="border-bottom box-shadow mb-3"><div data-type="Navbar"></div></div>');
            let navbar = new DefaultNavbarPR();
            navbar.RenderBrand();
        }
    }

    CloseSelectArea() {
        // Проставление класса
        let selectArea = $("#" + $('[data-type="SelectArea"]').attr('data-target-id'));
        selectArea.removeClass('gs-field-is-focused');
        if (selectArea.find('.gs-field-input').val() != '') {
            selectArea.addClass('gs-field-is-filled')
        }
        else {
            selectArea.removeClass('gs-field-is-filled')
        }
        
        // Скрытие выпадушки
        $('[data-type="SelectArea"]')
            .addClass('d-none')
            .removeAttr('style')
            .removeAttr('data-target-id')
            .empty();

        // Возврат стрелки пиклиста в прежнее положение
        $('.pick-area')
            .find('.icon-chevron-thin-left')
            .removeClass('icon-chevron-thin-left')
            .addClass('icon-chevron-thin-right');

        // Стили кнопки открывающей выпадшуку
        $('.pick-area').css('color', 'lightgrey');        
    }
}