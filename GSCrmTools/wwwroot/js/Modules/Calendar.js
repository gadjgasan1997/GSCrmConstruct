export default class Calendar {
    //#region ��������
    get SelectedMonth() {
        return this.selectedMonth;
    }

    set SelectedMonth(month) {
        this.selectedMonth = month;
    }

    get SelectedYear() {
        return this.selectedYear;
    }

    set SelectedYear(year) {
        this.selectedYear = year;
    }

    get SelectedDay() {
        return this.selectedDay;
    }

    set SelectedDay(day) {
        this.selectedDay = day;
    }

    get Field() {
        return this.field;
    }

    set Field(field) {
        this.field = field;
    }

    get DateIsSelected() {
        return this.dateIsSelected;
    }

    set DateIsSelected(isSelected) {
        this.dateIsSelected = isSelected;
    }
    //#endregion

    //#region �������������
    // ������������� ���������
    Initialize(field, e) {
        var date = new Date();
        var val = $(field).find('.gs-field-input').val();     // ������� �������� � ����
        this.Field = field;

        // ���� ���� �� ���������, ����������� ������� ����
        if (val === '') {
            this.SelectedDay = date.getDate();
            this.SelectedMonth = date.getMonth();
            this.SelectedYear = date.getFullYear();
        }

        // ����� �������� �������� �� ����
        if (val !== '') {
            this.SelectedDay = val.split(' ')[0];
            this.SelectedMonth = GetMonthIndex(val.split(' ')[1], 'long');
            this.SelectedYear = val.split(' ')[2];
            this.DateIsSelected = true;
        }

        this.FillCalendar();
        this.SetStyle();
        this.SetLocation(e);

        // �������
        $('[data-type="SelectArea"]')
            // ������ ������ �����������
            .off('click', '.month-day')
            .off('click', '#nextMonth')
            .off('click', '#previousMonth')
            .off('click', '#nextYear')
            .off('click', '#previousYear')
            .off('click', '#selectedYear')
            .off('click', '#choiseDay')
            .off('click', '.square-tile-item')
            .off('click', '#currentDate')
            // �������� �����
            .on('click', '.month-day', event => {
                let target = event.currentTarget;
                this.DateIsSelected = true;

                // ��� ���� ���� ������ ���������
                $('.month-day').removeClass('month-day-focused');

                // ������������ ��� ��� ���������� ���
                $(target).addClass('month-day-focused');
                this.SelectedDay = $(target)[0].innerText;

                // ��������� ���������� �������� � ���� � �����
                $('#' + $('[data-type="SelectArea"]').attr('data-target-id'))
                    .find('.gs-field-input')
                    .val(this.SelectedDay + " " + this.GetMonthName(this.SelectedMonth, 'long') + " " + this.SelectedYear);
            })
            // ��� �������� ������ �� �������
            .on('click', '#nextMonth', event => {
                event.stopPropagation();
                this.SelectedMonth = this.SelectedYear + 1;
                if (this.SelectedMonth == 12) {
                    this.SelectedMonth = 0;
                    this.SelectedYear = this.SelectedYear + 1;
                }
                this.SelectMonthYear();
            })
            // ��� �������� ����� �� �������
            .on('click', '#previousMonth', event => {
                event.stopPropagation();
                this.SelectedMonth = this.SelectedYear - 1;
                if (this.SelectedMonth == -1) {
                    this.SelectedMonth = 11;
                    this.SelectedYear = this.SelectedYear - 1;
                }
                this.SelectMonthYear();
            })
            // ��� �������� ������ �� �����
            .on('click', '#nextYear', event => {
                event.stopPropagation();
                this.SelectedYear = this.SelectedYear + 1;
                this.SelectMonthYear();
            })
            // ��� �������� ����� �� �����
            .on('click', '#previousYear', event => {
                event.stopPropagation();
                this.SelectedYear = this.SelectedYear - 1;
                this.SelectMonthYear();
            })
            // ��� ������ �� ������ �������
            .on('click', '#selectedMonth', event => {
                event.stopPropagation();
                let header = $('<div class="row no-gutters justify-content-center" style="height: 40px"><div class="light-green-btn" id="choiseDay">' +
                    '<div class="block-center"><p>Choise day</p></div></div></div>');
                let monthRow = $('<div class="popup-tile-container"><div class="row no-gutters justify-content-between square-tile"></div></div>');
                this.GetMonthList('short').forEach((item, index, arr) => {
                    monthRow
                        .find('.square-tile')
                        .append('<div class="col"><div class="square-tile-item" id="all_month_list_item_' + index + '"><p>' + item + '</p></div></div>')
                })
                $('.calendar-mini')
                    .empty()
                    .append(header)
                    .append('<div class="divider-mr-md"></div>')
                    .append(monthRow);
                $('#all_month_list_item_' + this.SelectedMonth)
                    .css('border', '2px solid #2ecc71')
                    .css('border-radius', '50%');
                $('#all_month_list_item_' + date.getMonth())
                    .css('border', '2px solid #2980b9')
                    .css('border-radius', '50%');
                this.SetLocation(event);
            })
            // ��� ������ ������ �� ������
            .on('click', '.square-tile-item', event => {
                let target = event.currentTarget;
                event.stopPropagation();
                let month = $(target).attr('id').split('all_month_list_item_')[1];
                let year = $(target).attr('id').split('all_year_list_item_')[1];
                if (month !== undefined) {
                    this.SelectedMonth = month;
                }
                if (year !== undefined) {
                    this.SelectedMonth = year;
                }
                this.SelectMonthYear();
            })
            // ��� ������� �� ������ ��������� �����
            .on('click', '#choiseDay', event => {
                event.stopPropagation();
                this.SelectMonthYear()
            })
            // ��� ������� �� ������ �������� � ������� ����
            .on('click', '#currentDate', event => {
                event.stopPropagation();
                this.DateIsSelected = true;
                this.SelectedDay = date.getDate();
                this.SelectedMonth = date.getMonth();
                this.SelectedYear = date.getFullYear();
                this.SelectMonthYear();
            })
    }
    //#endregion

    //#region ������ ��� ��������� ������� �� ���������
    // ������� �� ������� ��� �����
    SelectMonthYear() {
        // ��������, ��� ��� �������� �� �������, �� ��������� ����, ������� ��� � ���������� ������, � �������� ��� � �������
        // ��������, ��� �������� � ������ �� �������, ���� � ������ ����� ������ �� 31 �����
        if (this.GetDaysInMonth(this.SelectedMonth, this.SelectedYear) < this.SelectedDay) {
            this.SelectedDay = this.GetDaysInMonth(this.SelectedMonth, this.SelectedYear);
        }
        this.FillCalendar();
        this.SetStyle();
        this.SetLocation();

        // ��������� ���������� �������� � ���� � �����
        $('#' + $('[data-type="SelectArea"]')
            .attr('data-target-id'))
            .find('.gs-field-input')
            .val(this.SelectedDay + " " + this.GetMonthName(this.SelectedMonth, 'long') + " " + this.SelectedYear);
    }
    //#endregion

    //#region ������ ������������ ���������
    // ����������
    FillCalendar() {
        // ������ ���� ������
        let daysList = $('<div class="row no-gutters justify-content-around days-list"></div>');

        ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"].forEach((item, index) => {
            daysList
                .last('div')
                .append('<div class="col" id="day_' + (index + 1) + '"><div class="block-center"><p>' + item + "</div>");
        })

        $('[data-type="SelectArea"]')
            .empty()
            .append(this.GetMonthYearList())
            .removeClass('d-none')
            .find('.calendar-mini')
            .append('<div class="divider-mr-md" style="background-color: grey"></div>')
            .append(daysList)
            .append('<div class="divider-mr-md" style="background-color: grey"></div>')
            .append(this.GetMonthDays());
    }

    // �����
    SetStyle() {
        var date = new Date();
        $('[data-type="SelectArea"]').attr('data-target-id', $(this.Field).attr('id'));
        $('.field-date').removeClass('gs-field-is-focused');

        // ��������� ������� ����
        if (this.SelectedMonth == date.getMonth() && this.SelectedYear == date.getFullYear()) {
            $('.month-day').filter((index, item) => {
                if (item.textContent == date.getDate())
                    $(item).addClass('current-day')
            })
        }

        // ��������� ��������� ���� ����
        if (this.DateIsSelected) {
            $('.month-day').filter((index, item) => {
                if (item.textContent == this.SelectedDay)
                    $(item).addClass('month-day-focused')
            })
        }

        // ��������� ��������
        $('.month-day').each((index, item) => {
            if (this.isHoliday(index)) {
                $(item).addClass('holiday-day');
            }
        })

        $(this.Field)
            .addClass('gs-field-is-focused')
            .find('.gs-field-input')
            .focus();
    }

    // ��������������
    SetLocation(event) {
        // ����������� ������� ���������� �����
        $('[data-type="SelectArea"]').attr('style', '');

        // ���� ��������� ������ � ��������� ����
        if ($(event.currentTarget).closest('[data-type="PopupArea"]').length !== 0) {

            var calendarHeight = $('[data-type="SelectArea"]').find('.calendar-mini').height();     // ������ ���������
            var fieldWidth = this.Field.offsetWidth;                                // ������ ����
            var fieldHeight = this.Field.offsetHeight;                              // ������ ����
            var windowWidth = window.innerWidth;                                        // ������ ����
            var windowHeight = window.innerHeight;                                      // ������ ����
            var modalWidth = $(this.Field).closest('.popup-wrap').width();          // ������ ���������� ����
            var modalHeight = $(this.Field).closest('.popup-wrap').height();        // ������ ���������� ����
            var fieldLeft = $(this.Field).offset().left - 1;                        // ������ ���� ����� ������������ ���������
            var fieldTop = $(this.Field).offset().top;                              // ������ ���� ������ ������������ ���������
            var modalTop = $('.gs-popup-header').offset().top;                             // ������ ��������� ���������� ���� ������ ������������ ���������
            var modalLeft = $('.gs-popup-body').offset().left;                           // ������ ��������� ���������� ���� ����� ������������ ���������
            var fieldModalTop = fieldTop - modalTop;                                    // ������ ���� �� �������� ���� ���������� ����
            var fieldModalLeft = fieldLeft - modalLeft;                                 // ������ ���� �� ������ ���� ���������� ����

            // ������ ���� �� ������� ���� ���������� ����
            var fieldModalBottom = modalHeight - (fieldModalTop + fieldHeight);

            // ������ ���� �� ������� ���� ���������� ����
            var fieldModalRight = modalWidth - (fieldModalLeft + fieldWidth);

            // ���� ��� ����� � ��������� ���� ��� ����� ��� ���������
            if (fieldModalBottom < calendarHeight) {
                //fieldTop = fieldModalTop - calendarHeight - fieldHeight + (fieldHeight / 2);
                fieldTop = fieldModalTop + modalTop - calendarHeight - fieldHeight;
            }

            // ���� ��� ����� � ��������� ���� ��� ����� ��� ���������
            if (fieldModalTop < (calendarHeight + fieldHeight)) {
                // �������������� ��� ����� ������� ���������� ���� �� �������� ���� �������� + ������ ���� �� �������� ���� ���������� ���� + ������ ����
                fieldTop = (windowHeight - modalHeight) / 2 + fieldModalTop + fieldHeight;
            }

            // ���� ������ �� ���� � ��������� ���� ��� ����� ��� ���������
            if (fieldModalRight < 400) {
                // �������������� ��� ����� ������� ���������� ���� �� ������ ���� �������� + ������ ���� �� ������ ���� ���������� ���� + ������ ����
                // - ������ ���������� ����
                $('[data-type="SelectArea"]')[0].style.left = (modalLeft + fieldModalLeft + fieldWidth - 400) + 'px';
            }

            // ���� ����� �� ���� � ��������� ���� ��� ����� ��� ���������
            if (fieldModalLeft < 400) {
                $('[data-type="SelectArea"]')[0].style.left = fieldLeft + 'px';
            }

            // ���� �� ��� �� ��� ����� � ��������� ���� ��� ����� ��� ���������
            if ((fieldModalBottom < calendarHeight) && (fieldModalTop < (calendarHeight + fieldHeight))) {
                fieldTop = (windowHeight - calendarHeight) / 2;

                // ���� ����� �� ���� � ��������� ���� ��� ����� ��� ���������
                if (fieldModalLeft < 400) {
                    $('[data-type="SelectArea"]')[0].style.left = (fieldLeft + fieldWidth) + 'px';
                }

                // ���� ������ �� ���� � ��������� ���� ��� ����� ��� ���������
                else {
                    $('[data-type="SelectArea"]')[0].style.left = (fieldLeft - 400) + 'px';
                }
            }

            // ���� �� ����� �� ������ �� ���� � ��������� ���� ��� ����� ��� ���������
            if ((fieldModalRight < 400) && (fieldModalLeft < 400)) {
                $('[data-type="SelectArea"]')[0].style.left = ((windowWidth - 400) / 2) + 'px';
            }

            $('[data-type="SelectArea"]')[0].style.top = fieldTop + 'px';
            $('[data-type="SelectArea"]')[0].style.position = 'fixed';
        }

        else {
            var fieldLeft = $(this.Field).offset().left - 1;                        // ������ �����
            var fieldTop = $(this.Field).offset().top;                              // ������ ������
            var documentWidth = $(document).width();                                    // ������ ���������
            var documentHeight = $(document).height();                                  // ������ ���������
            var calendarHeight = $('[data-type="SelectArea"]').find('.calendar-mini').height();     // ������ ���������

            // ���� ��� ����� ����� � ��������� ��� ����� ��� ���������
            if ((documentHeight - calendarHeight) < fieldTop) {
                fieldTop = fieldTop - calendarHeight - 21;
            }

            else {
                fieldTop = fieldTop + 46;
            }

            $('[data-type="SelectArea"]')[0].style.top = fieldTop + 'px';

            // ���� ����� �� ���� � ��������� ��� ����� ��� ���������
            if ((documentWidth - 400) < fieldLeft) {
                fieldLeft = fieldLeft - 400;
                $('[data-type="SelectArea"]')[0].style.right = documentWidth - (fieldLeft + this.Field.offsetWidth) + 'px';
            }

            else {
                $('[data-type="SelectArea"]')[0].style.left = fieldLeft + 'px';
            }
        }
    }
    //#endregion

    //#region ��������������� ������ ��� ������ ��������
    // ���������� ��������� ��������� � ������� ������ � ����
    GetMonthYearList() {
        return '<div class="calendar-mini"><div class="row no-gutters justify-content-between" style="height: 40px">' +
            '<div class="row no-gutters justify-content-around" style="height: 100%">' +
            '<div class="arr-previous-md" style="color: lightgrey" id="previousYear"><span class="icon-chevron-thin-left"></span></div>' +
            '<div style="height: 100%"><h5 class="label-think-md" id="selectedYear">' + this.SelectedYear + '</h5></div>' +
            '<div class="arr-next-md" style="color: lightgrey" id="nextYear"><span class="icon-chevron-thin-right"></span></div></div>' +
            '<div class="blue-btn" id="currentDate"><div class="block-center"><p>Now</p></div></div>' +
            '<div class="row no-gutters justify-content-around" style="height: 100%">' +
            '<div class="arr-previous-md" style="color: lightgrey" id="previousMonth"><span class="icon-chevron-thin-left"></span></div>' +
            '<div style="height: 100%"><h5 class="label-think-md" id="selectedMonth">' + this.GetMonthName(this.SelectedMonth) + '</h5></div>' +
            '<div class="arr-next-md" style="color: lightgrey" id="nextMonth"><span class="icon-chevron-thin-right"></span></div>' +
            '</div></div>';
    }

    // ���������� ��� ������
    GetMonthDays() {
        // ��� ������
        var monthDaysList = $('<section></section');
        var daysInMonth = this.GetDaysInMonth(this.SelectedMonth, this.SelectedYear);
        var counter = 1;

        // ������ ���� ������
        var firstMonthDay = new Date(this.SelectedYear, this.SelectedMonth, 1).getDay();

        // ��� ��� ������ ���� � js ���� � �����������, � ������� � ������������, ������������ �������� ����������
        if (firstMonthDay == 0) firstMonthDay = 7;

        // ������ ������ ������
        var daysRow = $('<div class="row no-gutters justify-content-around month-days-list" style="height: 50px"></div>');
        for (var j = 1; j < 8; j++) {
            if (j < firstMonthDay) {
                daysRow.append('<div class="col non-selected"></div>');
            }
            else {
                daysRow.append('<div class="col" style="height: 50px; width: 50px">' +
                    '<div class="month-day">' + counter + '</div></div>');
                counter++;
            }
        }
        monthDaysList.append(daysRow);

        // ��������� ������
        while (counter <= daysInMonth) {
            daysRow = $('<div class="row no-gutters justify-content-around month-days-list" style="height: 50px"></div>');
            for (var j = 0; j < 7; j++) {
                daysRow.append('<div class="col" style="height: 50px; width: 50px">' +
                    '<div class="month-day">' + counter + '</div></div>');
                counter++;
                if (counter == (daysInMonth + 1)) break;
            }
            while (daysRow.find('.col').length < 7) {
                daysRow.append('<div class="col non-selected"></div>');
            }
            monthDaysList.append(daysRow);
        }

        return monthDaysList;
    }

    // ���������� ���������� ���� � ������
    GetDaysInMonth(month, year) {
        return new Date(year, parseInt(month) + 1, 0).getDate();
    }

    // ���������� �������� ������
    GetMonthName(monthIndex, monthLength) {
        return this.GetMonthList(monthLength)[monthIndex];
    }

    // ���������� ������ ������
    GetMonthIndex(monthName, monthLength) {
        return this.GetMonthList(monthLength).indexOf(monthName, monthLength);
    }

    // ���������� ������ �������
    GetMonthList(monthLength) {
        let monthList;
        switch (monthLength) {
            case "short":
                monthList = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "July", "Aug", "Sep", "Oct", "Nov", "Dec"];
                break;
            case "long":
                monthList = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                break;
            default:
                monthList = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "July", "Aug", "Sep", "Oct", "Nov", "Dec"];
                break;
        }
        return monthList;
    }

    // ���������� ���� ������ �� ��� ������
    GetDayName(dayIndex, dayLength) {
        return this.GetDaysList(dayLength)[dayIndex];
    }

    // ���������� ������ ����
    GetDaysList(dayLength) {
        var dayList;
        switch (dayLength) {
            case "short":
                dayList = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];
                break;
            case "long":
                dayList = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];
                break;
            default:
                dayList = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];
                break;
        }
        return dayList;
    }

    // ����������, �������� �� ���� ���������� ��� ��������
    isHoliday(dayIndex, inputsPS) {
        let date = new Date(inputsPS.selectedYear, inputsPS.selectedMonth, dayIndex);
        if (GetDayName(date.getDay()) == 'Sat' || GetDayName(date.getDay()) == 'Sun') return true;
        else return false;
    }
    //#endregion
}