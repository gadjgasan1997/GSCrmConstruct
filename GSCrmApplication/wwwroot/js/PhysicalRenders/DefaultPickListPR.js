export default class DefaultPickListPR {
    RenderPickList(pickListElement, pickListRecords) {
        if (!$('[data-type="PopupArea"]').hasClass('d-none')) {
            // ѕереписать местоположение пиклиста в попапе
            let pickListTop = pickListElement.offset().top + pickListElement.height() + 2;
            if (pickListTop + 300 > $(document).height()) {
                pickListTop = pickListTop - 300 - pickListElement.height();
            }
            let pickListLeft = pickListElement.offset().left;
            $('[data-type="SelectArea"]')
                .css('left', pickListLeft).css('top', pickListTop)
                .empty()
                .width($(pickListElement).width() + 2)
                .attr('data-target-id', $(pickListElement).attr('id'));
        }
        else {
            let pickListTop = pickListElement.offset().top + pickListElement.height() + 2;
            if (pickListTop + 300 > $(document).height()) {
                pickListTop = pickListTop - 300 - pickListElement.height() - 2;
            }
            let pickListLeft = pickListElement.offset().left;
            $('[data-type="SelectArea"]')
                .css('left', pickListLeft).css('top', pickListTop)
                .empty()
                .width($(pickListElement).width() + 2)
                .attr('data-target-id', $(pickListElement).attr('id'));
        }
        $(pickListElement)
            .find('.icon-chevron-thin-right')
            .removeClass('icon-chevron-thin-right')
            .addClass('icon-chevron-thin-left');
        $('[data-type="SelectArea"]')
            .removeClass('d-none')
            .append('<div data-type="PickList" class="gs-picklist"></div>');
        pickListRecords.forEach(record => {
            $('.gs-picklist').append('<div data-type="PickListItem" class="picklist-item block-center"><p class="label-sm">' + record + '</p></div>')
        });
        $(pickListElement)
            .off('PickListItemSelect')
            .on('PickListItemSelect', (event, args) => {
                $(pickListElement).find('.gs-field-input').val(args['NewValue']);
            });
        return $(pickListElement);
    }
    
    SetCurrentElement(currentElementId) {
        $('#' + currentElementId).addClass('picklist-item-focused');
        $('[data-type="PickList"]').animate({
            scrollTop: $('#' + currentElementId).offset().top - $('[data-type="PickList"]').offset().top - 20
        }, 300);
    }
}