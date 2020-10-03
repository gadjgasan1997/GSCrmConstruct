class AccountManagement {
    static managersToAdd = [];
    static managersToRemove = [];
    static primaryManagerId = "";
    static primaryManagerIdCash = "";

    /**
     * ����� �������������� ����� ���������� �������� �� �������
     */
    InitializeAccTeam() {
        return new Promise((resolve, reject) => {
            let accountTeamUrl = $("#accountForm").find("#accTeamManagementUrl").attr("href");
            let request = new AjaxRequests();
            request.CommonGetRequest(accountTeamUrl)
                .fail(error => reject(error))
                .done(response => {
                    this.ResetLists();
                    this.Render(response);
                    this.InitializePrimaryManager();
                    resolve(response);
                });
        });
    }

    /**
     * ����� �������� ������ ��� ���������� ���� ���������� �������� �� �������
     */
    GetRecords(event) {
        return new Promise((resolve, reject) => {
            let accountTeamUrl = $(event.currentTarget).attr("data-href");
            let request = new AjaxRequests();
            request.JsonGetRequest(accountTeamUrl)
                .fail(error => reject(error))
                .done(response => resolve(response));
        })
    }
    
    /**
     * ����� ���������� ����������� ������
     */
    ResetLists() {
        $("#teamAllEmployees #allEmployeesList tbody").empty();
        $("#selectedEmployeesList .list-card").empty();
    }

    /**
     * ����� ������������ ��������� ���� ��� �������������
     * @param {*} response 
     */
    Render(response) {
        this.RenderTeamAllEmployees(response["teamAllEmployees"]);
        this.RenderTeamSelectedEmployees(response["teamSelectedEmployees"]);
    }

    /**
     * ����� ������������ ������ ���� ��������� ����������
     * @param {*} teamAllEmployees ������ ���� ����������� �����������, ������� ������� �������
     */
    RenderTeamAllEmployees(teamAllEmployees) {
        let allEmployeesList = $("#teamAllEmployees #allEmployeesList tbody");
        $(allEmployeesList).empty();
        teamAllEmployees.map(employee => {
            let employeeRow = this.GetAllEmployeesRow(employee);
            $(allEmployeesList).append($(employeeRow));
        });
    }

    /**
     * ����� ������������ ������� �� �������
     * @param {*} teamSelectedEmployees ������� �� �������
     */
    RenderTeamSelectedEmployees(teamSelectedEmployees) {
        let selectedEmployeesList = $("#teamSelectedEmployees #selectedEmployeesList .list-card");
        $(selectedEmployeesList).empty();
        teamSelectedEmployees.map(employee => {
            let employeeCard = this.GetSelectedEmployeeCard(employee);
            $("#teamSelectedEmployees #selectedEmployeesList .list-card").append($(employeeCard));
        });
    }

    /**
     * ����� ��������������� �����������, �������� ��� ���������� � ������� �� �������
     */
    RestoreEmployeesToAdd() {
        AccountManagement.managersToAdd.map(managerId => {
            $("#accTeamManagementModal #allEmployeesList tbody .employee-id").each((index, managerIdElement) => {
                let row = $(managerIdElement).closest("tr");
                if ($(managerIdElement).text() == managerId) {
                    $(row).find(".checkmark").addClass("checkmark-checked");
                }
            })
        });
    }
    
    /**
     * ����� ��������������� ���������� ��� �������� ����������
     */
    RestoreEmployeesToRemove() {
        AccountManagement.managersToRemove.map(employee => {
            $("#accTeamManagementModal #selectedEmployeesList .list-card-content").each((index, card) => {
                let managerId = $(card).find(".manager-id").val();
                if (managerId == employee) {
                    $(card).find(".list-group-item").addClass("account-manager-selected");
                }
            })
        });
    }

    /**
     * ����� ��������������� ��������� ���������� ��������� ��� ������� �� �������
     */
    RestorePrimaryEmployee() {
        let modal = $("#accTeamManagementModal");
        let selectedEmployees = $(modal).find("#teamSelectedEmployees #selectedEmployeesList .list-card");
        $(selectedEmployees).find(".manager-id").each((index, managerId) => {
            let card = $(managerId).closest(".list-card-content");
            let listGroupItem = $(card).find(".list-group-item");

            // ���� �������� ��� ������ ��� ��������, �������������� ��������
            if (AccountManagement.primaryManagerIdCash == $(managerId).val()) {
                this.SetUpPrimaryManager(listGroupItem);

                // �������� ������ �� ������ ���������� ��� ��������
                let managerIndex = AccountManagement.managersToRemove.indexOf(managerId);
                if (managerIndex != -1) {
                    AccountManagement.managersToRemove.splice(managerIndex, 1);
                }
            }

            // ����� ������� ��������� ��������� � ��������
            else {
                this.ClearPrimaryManagerSign(listGroupItem);
            }
        })
    }

    /**
     * ����� ���������� �������� ������ ���������� ��� ���������� ���� ���������� ��������
     * @param {*} employee 
     */
    GetSelectedEmployeeCard(employee) {
        let initialName = employee["initialName"];
        let positionName = employee["positionName"] == null ? "" : employee["positionName"];
        let phoneNumber = employee["phoneNumber"] == null ? "" : employee["phoneNumber"];

        // �������� ���������
        let employeeCard = $("<div class='list-card-content'><input class='manager-id' type='hidden' value='" + employee["id"] + "' />" +
            "<a class='list-group-item list-group-item-action account-manager'>" +
            "<div class='d-flex w-100 justify-content-between'>" +
            "<h5 class='mr-1'>" + initialName + "</h5>" +
            "<div><div class='oval-mark'></div></div></div>" +
            "<p class='mb-1'>" + positionName + "</p>" +
            "<small class='mb-1'>" + phoneNumber + "</small>" +
            "</a></div>");

        // �������� ��������� ��������� �� �������
        let primaryEmployeeCard = $("<div class='list-card-content'><input class='manager-id' type='hidden' value='" + employee["id"] + "' />" +
            "<a class='list-group-item account-manager primary-manager'>" +
            "<div class='d-flex w-100 justify-content-between'>" +
            "<h5 class='mr-1'>" + initialName + "</h5>" +
            "<div><div class='oval-mark-readonly'><div class='icon-checkmark'></div></div></div></div>" +
            "<p class='mb-1'>" + positionName + "</p>" +
            "<small class='mb-1'>" + phoneNumber + "</small>" +
            "</a></div>");

        return employee["isPrimary"] ? primaryEmployeeCard : employeeCard;
    }

    /**
     * ����� ���������� ������ ������ ���������� ��� ���������� ���� ���������� ��������
     * @param {*} employee 
     */
    GetAllEmployeesRow(employee) {
        return "<tr>" +
        "<td class='employee-id d-none'>" + employee["id"] + "</td>" +
        "<td>" + employee["fullInitialName"] + "</td>" +
        "<td>" + employee["divisionName"] + "</td>" +
        "<td>" + employee["primaryPositionName"] + "</td>" +
        '<td class="checkmark"><span class="icon-checkmark"></span></td>' +
        "</tr>"
    }

    /**
     * �������� ��� �������� ������ �� ����� ������������ ������
     * @param {*} event 
     */
    NextAllEmployees(event) {
        return new Promise((resolve, reject) => {
            this.GetRecords(event).then(response => {
                this.RenderTeamAllEmployees(response);
                this.RestoreEmployeesToAdd();
                resolve();
            });
        })
    }
    
    /**
     * �������� ��� �������� ������ �� ����� ������������ �����
     * @param {*} event 
     */
    PreviousAllEmployees(event) {
        return new Promise((resolve, reject) => {
            this.GetRecords(event).then(response => {
                this.RenderTeamAllEmployees(response);
                this.RestoreEmployeesToAdd();
                resolve();
            });
        })
    }

    /**
     * ����� �� ���� ���������� �����������, ��������� �������
     * @param {*} event 
     */
    AllEmployeesSearch(event) {
        return new Promise((resolve, reject) => {
            let allEmployeesSearchUrl = $(event.currentTarget).attr("data-href");
            let allEmployeesSearchData = this.GetAllEmployeesSearchData();
            let request = new AjaxRequests();

            request.CommonPostRequest(allEmployeesSearchUrl, allEmployeesSearchData)
                .fail(error => reject(error))
                .done(response => {
                    this.RenderTeamAllEmployees(response["teamAllEmployees"]);
                    this.RenderEmployeesFilterFields(response);
                    resolve();
                })
        });
    }

    /**
     * ����� ���������� ������ ��� �������� �� ������ ��� ������ �� ������ ���� �����������
     */
    GetAllEmployeesSearchData() {
        return {
            OrganizationId: $("#organizationId").val(),
            SearchAllManagersName: $("#SearchAllManagersName").val(),
            SearchAllManagersDivision: $("#SearchAllManagersDivision").val(),
            SearchAllManagersPosition: $("#SearchAllManagersPosition").val()
        }
    }

    /**
     * ����� �� ������� �������
     * @param {*} event 
     */
    SelectedEmployeesSearch(event) {
        return new Promise((resolve, reject) => {
            let selectedEmployeesSearchUrl = $(event.currentTarget).attr("data-href");
            let selectedEmployeesSearchData = this.GetSelectedEmployeesSearchData();
            let request = new AjaxRequests();

            request.CommonPostRequest(selectedEmployeesSearchUrl, selectedEmployeesSearchData)
                .fail(error => reject(error))
                .done(response => {
                    this.RenderTeamSelectedEmployees(response["teamSelectedEmployees"]);
                    this.RenderEmployeesFilterFields(response);
                    resolve();
                })
        });
    }
    
    /**
     * ����� ���������� ������ ��� �������� �� ������ ��� ������ �� ������ ���� �����������
     */
    GetSelectedEmployeesSearchData() {
        return {
            OrganizationId: $("#organizationId").val(),
            SearchSelectedManagersName: $("#SearchSelectedManagersName").val(),
            SearchSelectedManagersPosition: $("#SearchSelectedManagersPosition").val(),
            SearchSelectedManagersPhone: $("#SearchSelectedManagersPhone").val()
        }
    }

    /**
     * ������� ������ �� ���� ���������� �����������, ��������� �������
     * @param {*} event 
     */
    ClearAllEmployeesSearch(event) {
        return new Promise((resolve, reject) => {
            let clearAllEmployeesSearchUrl = $(event.currentTarget).attr("data-href");
            let request = new AjaxRequests();

            request.CommonGetRequest(clearAllEmployeesSearchUrl)
                .fail(error => reject(error))
                .done(response => {
                    this.RenderTeamAllEmployees(response["teamAllEmployees"]);
                    this.RenderEmployeesFilterFields(response);
                    resolve();
                })
        });
    }

    /**
     * ������� ������ �� ������� �������
     * @param {*} event 
     */
    ClearSelectedEmployeesSearch(event) {
        return new Promise((resolve, reject) => {
            let clearSelectedEmployeesSearchUrl = $(event.currentTarget).attr("data-href");
            let request = new AjaxRequests();

            request.CommonGetRequest(clearSelectedEmployeesSearchUrl)
                .fail(error => reject(error))
                .done(response => {
                    this.RenderTeamSelectedEmployees(response["teamSelectedEmployees"]);
                    this.RenderEmployeesFilterFields(response);
                    resolve();
                })
        });
    }

    /**
     * ����� ��������� ���� �������� ����������, ���������� � �������
     * @param {*} response �����, ��������� � ������� � ������� ��� ��������
     */
    RenderEmployeesFilterFields(response) {
        let teamAllEmployeesVM = response["teamAllEmployeesVM"];
        let teamSelectedEmployeesVM = response["teamSelectedEmployeesVM"];
        $("#SearchAllManagersName").val(teamAllEmployeesVM["searchAllManagersName"]);
        $("#SearchAllManagersDivision").val(teamAllEmployeesVM["searchAllManagersDivision"]);
        $("#SearchAllManagersPosition").val(teamAllEmployeesVM["searchAllManagersPosition"]);
        $("#SearchSelectedManagersName").val(teamSelectedEmployeesVM["searchSelectedManagersName"]);
        $("#SearchSelectedManagersPosition").val(teamSelectedEmployeesVM["searchSelectedManagersPosition"]);
        $("#SearchSelectedManagersPhone").val(teamSelectedEmployeesVM["searchSelectedManagersPhone"]);
    }

    /**
     * ��� ������� �� ������ ���������� ���������� � ������� �� �������
     */
    OnEmployeeAddBtnClick(event) {
        let row = $(event.currentTarget).closest("tr");
        let employeeId = $(row).find(".employee-id").text();
        if (!AccountManagement.managersToAdd.includes(employeeId)) {
            AccountManagement.managersToAdd.push(employeeId);
        }
        else {
            let employeeIndex = AccountManagement.managersToAdd.indexOf(employeeId);
            if (employeeIndex != -1) {
                AccountManagement.managersToAdd.splice(employeeIndex, 1);
            }
        }
    }

    /**
     * ��� ������� �� ������ �������� ��������� �� ������� �� �������
     */
    OnManagerRemoveBtnClick(event) {
        // ���������� �������� id ���������� ��������� ��������� � �������� ��� � ��������� �� ������� ������
        // ���� ��� �����������, ������ ������� ��������� ����� ��������� � ������ �� ��������
        let card = $(event.currentTarget).closest(".list-card-content");
        let managerId = $(card).find(".manager-id").val();
        if (managerId != AccountManagement.primaryManagerIdCash) {
            if (!AccountManagement.managersToRemove.includes(managerId)) {
                AccountManagement.managersToRemove.push(managerId);
            }
            else {
                let managerIndex = AccountManagement.managersToRemove.indexOf(managerId);
                if (managerIndex != -1) {
                    AccountManagement.managersToRemove.splice(managerIndex, 1);
                }
            }
        }
    }
    
    /**
     * ����� ������������� ���������� ��������� ��������
     * @param {*} event 
     */
    CheckEmployeePrimary(event) {
        event.stopPropagation();
        let selectedEmployeesList = $("#selectedEmployeesList .list-card");

        // ������ ��������� � ��������� ���������
        $(selectedEmployeesList).find(".primary-manager").each((index, employee) => {
            this.ClearPrimaryManagerSign(employee);
        })

        // ����������� ��������� ��������� ��� ���������� ����������
        let accountManager = $(event.currentTarget).closest(".account-manager");
        this.SetUpPrimaryManager(accountManager);

        // ����������� ���������� ���������� � ���
        let card = $(accountManager).closest(".list-card-content");
        let managerId = $(card).find(".manager-id").val();
        AccountManagement.primaryManagerIdCash = managerId;

        // �������� ������ �� ������ ���������� ��� ��������
        let managerIndex = AccountManagement.managersToRemove.indexOf(managerId);
        if (managerIndex != -1) {
            AccountManagement.managersToRemove.splice(managerIndex, 1);
        }
    }

    /**
     * ����� ���������� ��������� ���������� ��� �������� ���������� ����
     */
    InitializePrimaryManager() {
        let modal = $("#accTeamManagementModal");
        let selectedEmployees = $(modal).find("#teamSelectedEmployees #selectedEmployeesList .list-card");
        let primaryEmployee = $(selectedEmployees).find(".primary-manager");
        let primaryManagerId = $(primaryEmployee).closest(".list-card-content").find(".manager-id").val();
        AccountManagement.primaryManagerId = primaryManagerId;
        AccountManagement.primaryManagerIdCash = primaryManagerId;
    }

    /**
     * ����� ������� ������� ��������� ��������� � �������� ����������
     * @param {*} accountManager �������� ����������
     */
    ClearPrimaryManagerSign(accountManager) {
        $(accountManager).addClass("list-group-item-action");
        $(accountManager).removeClass("primary-manager");
        let checkMark = $(accountManager).find(".oval-mark-readonly");
        if (checkMark.length == 0) {
            checkMark = $(accountManager).find(".oval-mark-check");
        }
        $(checkMark).removeClass("oval-mark-readonly").addClass("oval-mark");
        $(checkMark).empty();
    }

    /**
     * ����� �������� �������� ���������� ��� � ��������� ����������
     * @param {*} accountManager �������� ����������
     */
    SetUpPrimaryManager(accountManager) {
        $(accountManager).removeClass("account-manager-selected");
        $(accountManager).removeClass("list-group-item-action");
        $(accountManager).addClass("primary-manager");
        let checkMark = $(accountManager).find(".oval-mark");
        let button = new Button();
        button.OvalCheckmarkReadonly(checkMark);
    }

    /**
     * �������, ������������ ��� �������� ���������� ����
     */
    OnAccTeamModalClosed(event) {
        // ���� ���� ������ �� �������� ���������� ���� � ���������� ����������������� ���������
        if (this.ExistsAccTeamChanges()) {
            event.preventDefault();

            // ������ ������������� �� ��������, � ������ ������ ��������� ���������� � �������� ����
            Swal.fire(MessageManager.Invoke("AccTeamModalClosedConfirmation")).then(dialogResult => {
                if (dialogResult.value) {
                    this.ClearAccTeamManagementSearch().then(() => {
                        this.ClearAccTeamChangesHistory();
                        $(event.currentTarget).modal("hide");
                    });
                }
            });
        }

        // ����� �������������� ������������ ��������
        else {
            this.ClearAccTeamManagementSearch().then(() => location.reload());
        }
    }

    /**
     * ����� ���������, ���������� �� ����������������� ���������
     */
    ExistsAccTeamChanges() {
        return AccountManagement.managersToAdd.length > 0 || AccountManagement.managersToRemove.length > 0 ||
            (AccountManagement.primaryManagerId != "" && AccountManagement.primaryManagerIdCash != AccountManagement.primaryManagerId);
    }

    /**
     * ������� ���������� � ��������� ���� ���������� �������� �� �������
     */
    ClearAccTeamManagementSearch() {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let clearAccTeamSearchUrl = Localization.GetUri("clearAccTeamSearch");

            request.CommonGetRequest(clearAccTeamSearchUrl)
                .fail(() => reject(error))
                .done(() => resolve())
        })
    }

    /**
     * ����� ������� ������� ��������� � ��������� ���� ���������� �������� �� �������
     */
    ClearAccTeamChangesHistory() {
        AccountManagement.managersToAdd = [];
        AccountManagement.managersToRemove = [];
        AccountManagement.primaryManagerId = "";
    }

    /**
     * ����� ��������� ������������� ������� �� �������
     * @param {*} event 
     */
    SynchronizeAccTeam(event) {
        return new Promise((resolve, reject) => {
            let syncAccTeamUrl = $(event.currentTarget).attr("data-href");
            let syncAccTeamData = this.SynchronizeAccTeamGetData();
            let request = new AjaxRequests();

            request.JsonPostRequest(syncAccTeamUrl, syncAccTeamData)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["Synchronize"]);
                    reject(errors);
                })
                .done(response => {
                    this.ClearAccTeamChangesHistory();
                    this.ResetLists();
                    this.Render(response);
                    this.InitializePrimaryManager();
                    ReInitScrools();
                    resolve();
                });
        })
    }

    /**
     * ����� ���������� ������, ����������� ��� ������������� ������� �� �������
     */
    SynchronizeAccTeamGetData() {
        return {
            AccountId: $("#accountId").val(),
            PrimaryManagerId: AccountManagement.primaryManagerIdCash,
            ManagersToAdd: AccountManagement.managersToAdd,
            ManagersToRemove: AccountManagement.managersToRemove
        }
    }
}

$("#accTeamManagementModal")
    .off("click", "#selectedEmployeesList .account-manager").on("click", "#selectedEmployeesList .account-manager", event => {
        if ($(event.currentTarget).hasClass("account-manager-selected")) {
            $(event.currentTarget).removeClass("account-manager-selected");
        }
        else if (!$(event.currentTarget).hasClass("primary-manager")) {
            $(event.currentTarget).addClass("account-manager-selected");
        }
        let accountManagement = new AccountManagement();
        accountManagement.OnManagerRemoveBtnClick(event);
    })
    .off("click", "#selectedEmployeesList .oval-mark").on("click", "#selectedEmployeesList .oval-mark", event => {
        let accountManagement = new AccountManagement();
        accountManagement.CheckEmployeePrimary(event);
    })
    .off("checkmark-check", ".checkmark").on("checkmark-check", ".checkmark", event => {
        let button = new Button();
        button.CheckmarkCheck(event);
        let accountManagement = new AccountManagement();
        accountManagement.OnEmployeeAddBtnClick(event);
    })
    .off("click", "#allEmployeesNav .nav-next .nav-url").on("click", "#allEmployeesNav .nav-next .nav-url", event => {
        let accountManagement = new AccountManagement();
        accountManagement.NextAllEmployees(event);
    })
    .off("click", "#allEmployeesNav .nav-previous .nav-url").on("click", "#allEmployeesNav .nav-previous .nav-url", event => {
        let accountManagement = new AccountManagement();
        accountManagement.PreviousAllEmployees(event);
    })
    .off("click", "#allEmployeesSearch").on("click", "#allEmployeesSearch", event => {
        let accountManagement = new AccountManagement();
        accountManagement.AllEmployeesSearch(event).then(() => {
            accountManagement.RestoreEmployeesToAdd();
        });
    })
    .off("click", "#selectedEmployeesSearch").on("click", "#selectedEmployeesSearch", event => {
        let accountManagement = new AccountManagement();
        accountManagement.SelectedEmployeesSearch(event).then(() => {
            accountManagement.RestoreEmployeesToRemove();
            accountManagement.RestorePrimaryEmployee();
        });
    })
    .off("click", "#clearAllEmployeesSearch").on("click", "#clearAllEmployeesSearch", event => {
        let accountManagement = new AccountManagement();
        accountManagement.ClearAllEmployeesSearch(event).then(() => {
            accountManagement.RestoreEmployeesToAdd();
        });
    })
    .off("click", "#clearSelectedEmployeesSearch").on("click", "#clearSelectedEmployeesSearch", event => {
        let accountManagement = new AccountManagement();
        accountManagement.ClearSelectedEmployeesSearch(event).then(() => {
            accountManagement.RestoreEmployeesToRemove();
            accountManagement.RestorePrimaryEmployee();
        });
    })
    .off("click", '#syncAccTeamBtn').on("click", "#syncAccTeamBtn", event => {
        let accountManagement = new AccountManagement();
        accountManagement.SynchronizeAccTeam(event);
    })
    .off('hide.bs.modal').on("hide.bs.modal", event => {
        let accountManagement = new AccountManagement();
        accountManagement.OnAccTeamModalClosed(event);
    });