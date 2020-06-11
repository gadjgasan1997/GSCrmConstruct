export default class Calendar {
    //#region Свойства
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

    //#region Инициализация
    // Инициализация календаря
    Initialize(field, e) {
        var date = new Date();
        var val = $(field).find('.gs-field-input').val();     // Текущее значение в поле
        this.Field = field;

        // Если поле не заполнено, проставляем текущую дату
        if (val === '') {
            this.SelectedDay = date.getDate();
            this.SelectedMonth = date.getMonth();
            this.SelectedYear = date.getFullYear();
        }

        // Иначе получаем значение из поля
        if (val !== '') {
            this.SelectedDay = val.split(' ')[0];
            this.SelectedMonth = GetMonthIndex(val.split(' ')[1], 'long');
            this.SelectedYear = val.split(' ')[2];
            this.DateIsSelected = true;
        }

        this.FillCalendar();
        this.SetStyle();
        this.SetLocation(e);

        // События
        $('[data-type="SelectArea"]')
            // Удаляю старые обработчики
            .off('click', '.month-day')
            .off('click', '#nextMonth')
            .off('click', '#previousMonth')
            .off('click', '#nextYear')
            .off('click', '#previousYear')
            .off('click', '#selectedYear')
            .off('click', '#choiseDay')
            .off('click', '.square-tile-item')
            .off('click', '#currentDate')
            // Добавляю новые
            .on('click', '.month-day', event => {
                let target = event.currentTarget;
                this.DateIsSelected = true;

                // Для всех дней снимаю выделение
                $('.month-day').removeClass('month-day-focused');

                // Устанавливаю его для выбранного дня
                $(target).addClass('month-day-focused');
                this.SelectedDay = $(target)[0].innerText;

                // Установка выбранного значения в поле с датой
                $('#' + $('[data-type="SelectArea"]').attr('data-target-id'))
                    .find('.gs-field-input')
                    .val(this.SelectedDay + " " + this.GetMonthName(this.SelectedMonth, 'long') + " " + this.SelectedYear);
            })
            // При переходе вперед по месяцам
            .on('click', '#nextMonth', event => {
                event.stopPropagation();
                this.SelectedMonth = this.SelectedYear + 1;
                if (this.SelectedMonth == 12) {
                    this.SelectedMonth = 0;
                    this.SelectedYear = this.SelectedYear + 1;
                }
                this.SelectMonthYear();
            })
            // При переходе назад по месяцам
            .on('click', '#previousMonth', event => {
                event.stopPropagation();
                this.SelectedMonth = this.SelectedYear - 1;
                if (this.SelectedMonth == -1) {
                    this.SelectedMonth = 11;
                    this.SelectedYear = this.SelectedYear - 1;
                }
                this.SelectMonthYear();
            })
            // При переходе вперед по годам
            .on('click', '#nextYear', event => {
                event.stopPropagation();
                this.SelectedYear = this.SelectedYear + 1;
                this.SelectMonthYear();
            })
            // При переходе назад по годам
            .on('click', '#previousYear', event => {
                event.stopPropagation();
                this.SelectedYear = this.SelectedYear - 1;
                this.SelectMonthYear();
            })
            // При выборе из списка месяцев
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
            // При выборе месяца из списка
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
            // При нажатии на кнопку вернуться назад
            .on('click', '#choiseDay', event => {
                event.stopPropagation();
                this.SelectMonthYear()
            })
            // При нажатии на кнопку перехода к текущей дате
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

    //#region Методы для обработки событий на календаре
    // Переход по месяцам или годам
    SelectMonthYear() {
        // Проверка, что при переходе по месяцам, не выделится день, который был в предыдущем месяце, и которого нет в текущем
        // Например, при переходе с января на февраль, если в январе стоял курсор на 31 числе
        if (this.GetDaysInMonth(this.SelectedMonth, this.SelectedYear) < this.SelectedDay) {
            this.SelectedDay = this.GetDaysInMonth(this.SelectedMonth, this.SelectedYear);
        }
        this.FillCalendar();
        this.SetStyle();
        this.SetLocation();

        // Установка выбранного значения в поле с датой
        $('#' + $('[data-type="SelectArea"]')
            .attr('data-target-id'))
            .find('.gs-field-input')
            .val(this.SelectedDay + " " + this.GetMonthName(this.SelectedMonth, 'long') + " " + this.SelectedYear);
    }
    //#endregion

    //#region Методы формирования календаря
    // Заполнение
    FillCalendar() {
        // Список дней недели
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

    // Стили
    SetStyle() {
        var date = new Date();
        $('[data-type="SelectArea"]').attr('data-target-id', $(this.Field).attr('id'));
        $('.field-date').removeClass('gs-field-is-focused');

        // Подсветка текущей даты
        if (this.SelectedMonth == date.getMonth() && this.SelectedYear == date.getFullYear()) {
            $('.month-day').filter((index, item) => {
                if (item.textContent == date.getDate())
                    $(item).addClass('current-day')
            })
        }

        // Подсветка выбранной даты даты
        if (this.DateIsSelected) {
            $('.month-day').filter((index, item) => {
                if (item.textContent == this.SelectedDay)
                    $(item).addClass('month-day-focused')
            })
        }

        // Подсветка выходных
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

    // Местоположение
    SetLocation(event) {
        // Обязательно очищаем предыдущие стили
        $('[data-type="SelectArea"]').attr('style', '');

        // Если календарь открыт в модальном окне
        if ($(event.currentTarget).closest('[data-type="PopupArea"]').length !== 0) {

            var calendarHeight = $('[data-type="SelectArea"]').find('.calendar-mini').height();     // Высота выпадушки
            var fieldWidth = this.Field.offsetWidth;                                // Ширина поля
            var fieldHeight = this.Field.offsetHeight;                              // Высота поля
            var windowWidth = window.innerWidth;                                        // Ширина окна
            var windowHeight = window.innerHeight;                                      // Высота окна
            var modalWidth = $(this.Field).closest('.popup-wrap').width();          // Ширина модального окна
            var modalHeight = $(this.Field).closest('.popup-wrap').height();        // Высота модального окна
            var fieldLeft = $(this.Field).offset().left - 1;                        // Отступ поля слева относительно документа
            var fieldTop = $(this.Field).offset().top;                              // Отступ поля сверху относительно документа
            var modalTop = $('.gs-popup-header').offset().top;                             // Отступ заголовка модального окна сверху относительно документа
            var modalLeft = $('.gs-popup-body').offset().left;                           // Отступ заголовка модального окна слева относительно документа
            var fieldModalTop = fieldTop - modalTop;                                    // Отступ поля от верхнего края модального окна
            var fieldModalLeft = fieldLeft - modalLeft;                                 // Отступ поля от левого края модального окна

            // Отступ поля от нижнего края модального окна
            var fieldModalBottom = modalHeight - (fieldModalTop + fieldHeight);

            // Отступ поля от правого края модального окна
            var fieldModalRight = modalWidth - (fieldModalLeft + fieldWidth);

            // Если под полем в модальном окне нет места для календаря
            if (fieldModalBottom < calendarHeight) {
                //fieldTop = fieldModalTop - calendarHeight - fieldHeight + (fieldHeight / 2);
                fieldTop = fieldModalTop + modalTop - calendarHeight - fieldHeight;
            }

            // Если над полем в модальном окне нет места для календаря
            if (fieldModalTop < (calendarHeight + fieldHeight)) {
                // Рассчитывается как сумма отступа модального окна от верхнего края браузера + отступ поля от верхнего края модального окна + высота поля
                fieldTop = (windowHeight - modalHeight) / 2 + fieldModalTop + fieldHeight;
            }

            // Если справа от поля в модальном окне нет места для календаря
            if (fieldModalRight < 400) {
                // Рассчитывается как сумма отступа модального окна от левого края браузера + отступ поля от левого края модального окна + ширина поля
                // - ширина модального окна
                $('[data-type="SelectArea"]')[0].style.left = (modalLeft + fieldModalLeft + fieldWidth - 400) + 'px';
            }

            // Если слева от поля в модальном окне нет места для календаря
            if (fieldModalLeft < 400) {
                $('[data-type="SelectArea"]')[0].style.left = fieldLeft + 'px';
            }

            // Если ни под ни над полем в модальном окне нет места для календаря
            if ((fieldModalBottom < calendarHeight) && (fieldModalTop < (calendarHeight + fieldHeight))) {
                fieldTop = (windowHeight - calendarHeight) / 2;

                // Если слева от поля в модальном окне нет места для календаря
                if (fieldModalLeft < 400) {
                    $('[data-type="SelectArea"]')[0].style.left = (fieldLeft + fieldWidth) + 'px';
                }

                // Если справа от поля в модальном окне нет места для календаря
                else {
                    $('[data-type="SelectArea"]')[0].style.left = (fieldLeft - 400) + 'px';
                }
            }

            // Если ни слева ни справа от поля в модальном окне нет места для календаря
            if ((fieldModalRight < 400) && (fieldModalLeft < 400)) {
                $('[data-type="SelectArea"]')[0].style.left = ((windowWidth - 400) / 2) + 'px';
            }

            $('[data-type="SelectArea"]')[0].style.top = fieldTop + 'px';
            $('[data-type="SelectArea"]')[0].style.position = 'fixed';
        }

        else {
            var fieldLeft = $(this.Field).offset().left - 1;                        // Отступ слева
            var fieldTop = $(this.Field).offset().top;                              // Отсутп сверху
            var documentWidth = $(document).width();                                    // Ширина документа
            var documentHeight = $(document).height();                                  // Высота документа
            var calendarHeight = $('[data-type="SelectArea"]').find('.calendar-mini').height();     // Высота выпадушки

            // Если под полем снизу в документе нет места для календаря
            if ((documentHeight - calendarHeight) < fieldTop) {
                fieldTop = fieldTop - calendarHeight - 21;
            }

            else {
                fieldTop = fieldTop + 46;
            }

            $('[data-type="SelectArea"]')[0].style.top = fieldTop + 'px';

            // Если слева от поля в документе нет места для календаря
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

    //#region Вспомогательные методы для работы каледаря
    // Возвращает заголовок календаря с выбором месяца и года
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

    // Возвращает дни месяца
    GetMonthDays() {
        // Дни месяца
        var monthDaysList = $('<section></section');
        var daysInMonth = this.GetDaysInMonth(this.SelectedMonth, this.SelectedYear);
        var counter = 1;

        // Первый день месяца
        var firstMonthDay = new Date(this.SelectedYear, this.SelectedMonth, 1).getDay();

        // Так как отсчет дней в js идет с воскресенья, а удобнее с понедельника, пересчитываю значение переменной
        if (firstMonthDay == 0) firstMonthDay = 7;

        // Первая неделя месяца
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

        // Остальные недели
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

    // Возвращает количество дней в месяце
    GetDaysInMonth(month, year) {
        return new Date(year, parseInt(month) + 1, 0).getDate();
    }

    // Возвращает название месяца
    GetMonthName(monthIndex, monthLength) {
        return this.GetMonthList(monthLength)[monthIndex];
    }

    // Возвращает индекс месяца
    GetMonthIndex(monthName, monthLength) {
        return this.GetMonthList(monthLength).indexOf(monthName, monthLength);
    }

    // Возвращает список месяцев
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

    // Возвращает день недели по его номеру
    GetDayName(dayIndex, dayLength) {
        return this.GetDaysList(dayLength)[dayIndex];
    }

    // Возвращает список дней
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

    // Определяет, является ли день праздником или выходным
    isHoliday(dayIndex, inputsPS) {
        let date = new Date(inputsPS.selectedYear, inputsPS.selectedMonth, dayIndex);
        if (GetDayName(date.getDay()) == 'Sat' || GetDayName(date.getDay()) == 'Sun') return true;
        else return false;
    }
    //#endregion
}