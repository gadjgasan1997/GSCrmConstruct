﻿class Employee {
    /**
     * Создание сотрудника
     */
    Create() {
        return new Promise((resolve, reject) => {
            Utils.ClearErrors();
            let request = new AjaxRequests();
            let createEmpUrl = $("#employeeModal form").attr("action");
            let createEmpData = this.CreateGetData();

            request.JsonPostRequest(createEmpUrl, createEmpData)
                .fail(response => {
                    if (this.IsUserAccountExists()) {
                        Utils.CommonErrosHandling(response["responseJSON"], ["CreateDivision", "CreateEmployeeExistsAccount"]);
                    }
                    else {
                        Utils.CommonErrosHandling(response["responseJSON"], ["CreateDivision", "CreateEmployeeNewAccount"]);
                    }
                })
                .done(newEmployeeUrl => {
                    $("#employeeModal").modal("hide");
                    location.replace(newEmployeeUrl);
                });
        })
    }

    /**
     * Получение данных для отправки при создании сотрудника
     */
    CreateGetData() {
        let isUserAccountExists = this.IsUserAccountExists();
        let userName;
        if (isUserAccountExists)
            userName = $("#existsUserName").val();
        else userName = $("#newEmpUserName").val();

        return {
            UserAccountExists: isUserAccountExists,
            UserName: userName,
            Email: $("#newEmpEmail").val(),
            Password: $("#newEmpPassword").val(),
            ConfirmPassword: $("#newEmpConfirmPassword").val(),
            OrganizationId: $("#orgId").val(),
            FirstName: $("#empFirstName").val(),
            LastName: $("#empLastName").val(),
            MiddleName: $("#empMidName").val(),
            DivisionName: BaseAutocomplete.GetValue($("#employeeDiv")),
            PrimaryPositionName: BaseAutocomplete.GetValue($("#employeePosition")),
        }
    }

    /**
     * Показывает, выбрал ли пользователь создать новый аккаунт или найти существующий 
     */
    IsUserAccountExists() {
        return $($("#employeeModal .radio-tabs .form-check")[0]).hasClass("active");
    }

    /**
     * Очищает поля в модальном окне создания сотрудника
     */
    CreateClearFields() {
        $("#employeeDiv .autocomplete-input").val("");
        $("#employeePosition .autocomplete-input").val("");
        ["newEmpUserName", "newEmpEmail", "newEmpPassword", "newEmpConfirmPassword", "existsUserName", "empFirstName", "empLastName", "empMidName"].map(item => $(item).val(""));
    }

    /**
     * Действия при отмене создания сотрудника
     */
    CancelCreate() {
        $("#employeeModal").modal("hide");
        this.CreateClearFields();
        Utils.ClearErrors();
        Employee.SetDefaultTab();
    }
    
    /**
     * Удаление сотрудника из списка
     * @param {*} event 
     */
    Remove(event) {
        return new Promise((resolve, reject) => {
            Swal.fire(MessageManager.Invoke("RemoveEmployeeConfirmation")).then(result => {
                if (result["value"]) {
                    let request = new AjaxRequests();
                    let removeEmpUrl = $(event.currentTarget).find(".remove-item-url a").attr("href");

                    request.CommonDeleteRequest(removeEmpUrl)
                        .fail(() => {
                            Swal.fire(MessageManager.Invoke("RemoveEmployeeError"));
                            reject();
                        })
                        .done(() => location.reload());
                }
            })
        })
    }

    /**
     * Запрашивает у пользователя подтверждение на обновление подразделения
     * Выполняет запрос на сервер, меняя подразделение
     * При наличии ошибок обрабатывает их
     */
    ChangeDivision() {
        return new Promise((resolve, reject) => {
            Swal.fire(MessageManager.Invoke("ChangeEmpDivisionConfirmation")).then(result => {
                Utils.ClearErrors();
                if (result.value) {
                    let request = new AjaxRequests();
                    let changeDivisionUrl = $("#changeEmployeeDivisionModal form").attr("action");
                    let changeDivisionData = this.ChangeDivisionGetData();
                    
                    request.JsonPostRequest(changeDivisionUrl, changeDivisionData)
                        .fail(response => {
                            Utils.CommonErrosHandling(response["responseJSON"], ["ChangeDivision"]);
                        })
                        .done(() => location.reload())
                }
            });
        })
    }

    /**
     * Формирует данные для отправки на сервер при смене подразделения
     */
    ChangeDivisionGetData() {
        return {
            Id: $("#employeeId").val(),
            OrganizationId: $("#OrganizationId").val(),
            DivisionName: BaseAutocomplete.GetValue($("#empNewDiv")),
            PrimaryPositionName: BaseAutocomplete.GetValue($("#empNewPrimaryPos"))
        }
    }

    /**
     * Обновление сотрудника
     * @param {*} event 
     */
    Update(event) {
        return new Promise((resolve, reject) => {
            let updateEmpUrl = $(event.currentTarget).closest("#employeeForm").find("form").attr("action");
            let updateEmpData = this.UpdateGetData();
            let request = new AjaxRequests();
            request.JsonPostRequest(updateEmpUrl, updateEmpData)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["UpdateEmployee"]);
                })
                .done(updatedEmpUrl => location.replace(location.origin + updatedEmpUrl))
        });
    }

    UpdateGetData() {
        return {
            Id: $("#employeeId").val(),
            FirstName: $("#FirstName").val(),
            LastName: $("#LastName").val(),
            MiddleName: $("#MiddleName").val()
        }
    }

    /**
     * Устанавливает вкладку по умолчанию в модальном окне создания сотрудника
     */
    static SetDefaultTab() {
        let modal = $("#employeeModal");
        let checkGroup = modal.find(".radio-tabs .form-check");
        modal.modal("hide");
        $(checkGroup).removeClass("active");
        $(checkGroup).find("input").removeAttr("checked");
        $(checkGroup[0]).addClass("active").find("input").attr("checked", "checked");
        modal.find("#selectUserOption").fadeIn();
        modal.find(".tabs-content .tabs-content-item").hide();
    }
}

// Модальное окно создания сотрудника
$("#employeeModal")
    .off("click", "#createEmpBtn").on("click", "#createEmpBtn", event => {
        event.preventDefault();
        let employee = new Employee();
        employee.Create();
    })
    .off("click", "#cancelCreationEmpBtn").on("click", "#cancelCreationEmpBtn", event => {
        event.preventDefault();
        let employee = new Employee();
        employee.CancelCreate();
    });

// Карточка сотрудника
$("#employeeForm")
    .off("click", "#empTabs .nav-item").on("click", "#empTabs .nav-item", event => {
        let navTab = new NavTab();
        navTab.Remember(event, "currentEmpTab");
        let navConnectedTab = new NavConnectedTab();
        navConnectedTab.Remember(event, "currentEmpConnectedTab");
    })
    .off("click", "#updateEmpBtn").on("click", "#updateEmpBtn", event => {
        event.preventDefault();
        let employee = new Employee();
        employee.Update(event);
    })
    .off("click", "#initializePositionsBtn").on("click", "#initializePositionsBtn", event => {
        event.preventDefault();
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.InitializePositions();
    });

// Карточка заблокированного сотрудника
$("#lockEmployeeForm")
    .off("click", "#cancelUnlockEmpBtn").on("click", "#cancelUnlockEmpBtn", event => location.reload());

// Список сотрудников
$("#employeesList").off("click", ".remove-item-btn").on("click", ".remove-item-btn", event => {
    event.preventDefault();
    let employee = new Employee();
    employee.Remove(event);
})

// Модальное окно изменения подразделения
$("#changeEmployeeDivisionModal").off("click", "#changeDivisionBtn").on("click", "#changeDivisionBtn", event => {
    event.preventDefault();
    let employee = new Employee();
    employee.ChangeDivision();
})