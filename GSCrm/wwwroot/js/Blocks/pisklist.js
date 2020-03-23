class PickList {
    Initialize(field, event) {
        $('.select-area')
        .empty()
        .width(field.width() + 2)
        .attr('data-target-id', $(field).attr('id'))
        .removeClass('d-none')
        .append('<div class="picklist"></div>');
    }

    /*GetPickListRecords(event)
        .catch(error => console.log(error))
        .then(data => {
            console.log(data);
            var pickListTop = field.offset().top + field.height() + 2;
            if (pickListTop + 300 > $(document).height()) {
                pickListTop = pickListTop - 300 - field.height();
            }

            var pickListLeft = field.offset().left;

            $('.select-area')
                .css('left', pickListLeft)
                .css('top', pickListTop);
        });
    }*/

    Dispose() {
        
    }
}

$(document).on('click', '.picklist', event => {
    event.stopPropagation();
})