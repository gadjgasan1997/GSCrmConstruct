class EmployeePositionsManagement {
    static positionsToAdd = [];
    static positionsToRemove = [];
    static primaryPositionName = "";
    static primaryPositionChanged = false;

    /**
     * ������������� ������ ���������� � ��������� ���� ���������� �����������
     * ��������� ������ �� ������, ����� �������� ����� ��������� ����� �� ���������� ������
     */
    InitializePositions() {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let getPositionsUrl = $("#initializePositionsUrl").attr("href") + "/" + $("#employeeId").val();

            request.JsonGetRequest(getPositionsUrl)
                .catch(error => reject(error))
                .then(response => {
                    this.InitializePositionFields(response);
                    resolve(response);
                })
        })
    }

    /**
     * ������������� ����� � ��������� ���� � ����������� �� ���� ����������
     * @param {*} positionData ����� � ������� �� ������� ���������� ������������� � ��������� ���������� ����������
     */
    InitializePositionFields(positionData) {
        this.InitializeAllPositions(positionData["allPositions"]);
        this.InitializeSelectedPositions(positionData["selectedPositions"]);
    }

    /**
     * ������������� ������ �� ����� ����������� ������������� � ��������� ����
     * @param {*} allPositions ������ ���� ���������� �������������
     */
    InitializeAllPositions(allPositions) {
        $("#allPositions tbody").empty();
        allPositions.map(position => {
            let newPositionRow = this.GetNewAllPositionRow(position);
            $("#allPositions tbody").append(newPositionRow);
        });
    }

    /**
     * ������������� ������ � ����������� ���������� � ��������� ����
     * @param {*} selectedPositions ������ ���������� ����������
     */
    InitializeSelectedPositions(selectedPositions) {
        $("#selectedPositions tbody").empty();
        selectedPositions.map(position => {
            let newPositionRow = this.GetNewSelectedPositionRow(position);
            $("#selectedPositions tbody").append(newPositionRow);
        });
    }

    /**
     * ��������� ������ ��� ������ � ����������� ������������� � ��������� ����
     * @param {*} position ������ � ����������� � ���������
     */
    GetNewAllPositionRow(position) {
        let parentPositionName = position["parentPositionName"];
        let newPositionRow = "";
        if (parentPositionName != null && parentPositionName.length > 0) {
            newPositionRow = "<tr>" + this.GetAllPositionsNameCell(position) +
                '<td class="label-non-select">' + position["parentPositionName"] + '</td>' +
                this.GetCheckmarkCell() + "</tr>";
        }
        else {
            newPositionRow = "<tr>" + this.GetAllPositionsNameCell(position) +
                '<td class="label-non-select"></td>' +
                this.GetCheckmarkCell() + "</tr>";
        }
        return newPositionRow;
    }

    /**
     * ��������� ������ ��� ������ � ����������� ���������� � ��������� ����
     * @param {*} position ������ � ����������� � ���������
     */
    GetNewSelectedPositionRow(position) {
        let parentPositionName = position["parentPositionName"];
        let newPositionRow = "";
        if (parentPositionName != null && parentPositionName.length > 0) {
            newPositionRow = "<tr>" + this.GetSelectedPositionsNameCell(position) +
                '<td class="label-non-select">' + position["parentPositionName"] + '</td>' +
                this.GetPrimarySignCell(position) +
                this.GetCrossCell(position) + "</tr>";
        }
        else {
            newPositionRow = "<tr>" + this.GetSelectedPositionsNameCell(position) +
                '<td class="label-non-select"></td>' +
                this.GetPrimarySignCell(position) +
                this.GetCrossCell(position) + "</tr>";
        }
        return newPositionRow;
    }

    /**
     * ���������� ������ � ��������� ��������� ��� ������ ���� ���������� ������������� � ��������� ����
     * @param {*} position ������ � ����������� � ���������
     */
    GetAllPositionsNameCell(position) {
        return '<td class="label-non-select position-name">' + position["name"] + '</td>';
    }
    
    /**
     * ���������� ������ � ��������� ��������� ��� ������ ���������� ���������� � ��������� ����
     * @param {*} position ������ � ����������� � ���������
     */
    GetSelectedPositionsNameCell(position) {
        return '<td class="label-non-select position-name">' + position["positionName"] + '</td>';
    }

    /**
     * ���������� ������ � ���������, �������� �� ��������� �������� ��� ������ ���������� ����������
     * @param {*} position ������ � ����������� � ���������
     */
    GetPrimarySignCell(position) {
        if (position["isPrimary"]) {
            // ���� �� ���� �������� �������� ���������, ���������� �� ��������
            if (!EmployeePositionsManagement.primaryPositionChanged) {
                EmployeePositionsManagement.primaryPositionName = position["positionName"];
            }
            return '<td class="readonly-checkmark"><span class="icon-checkmark"></span></td>';
        }
        return '<td class="hide-checkmark"><span class="icon-checkmark"></span></td>';
    }

    /**
     * ���������� ������ � ������� �������� ��������� �� ������ ���������� ���������� � ��������� ����
     * @param {*} position ������ � ����������� � ���������
     */
    GetCrossCell(position) {
        if (!position["isPrimary"]) {
            return '<td class="cross"><span class="icon-close"></span></td>';
        }
        return '<td class="cross readonly-cross"><span class="icon-close"></span></td>';
    }

    /**
     * ���������� ������ � ������� ���������� ��������� � ������ ���������� ���������� � ��������� ����
     */
    GetCheckmarkCell() {
        return '<td class="checkmark"><span class="icon-checkmark"></span></td>';
    }

    /**
     * ��������� ������ �� ������ ��� ������������� ����������(���������� � �������� �� ������ ���������� ����������)
     * ����� ��������� ���������� ���������� ���� ��� ����� ������
     */
    SynchronizePositions() {
        return new Promise((resolve, reject) => {
            Utils.ClearErrors();
            let request = new AjaxRequests();
            let syncPositionsUrl = $("#employeePosisionModal form").attr("action");
            let syncPositionsData = this.GetSyncPositionsData();

            request.JsonPostRequest(syncPositionsUrl, syncPositionsData)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["SyncPositions"]);
                    reject();
                })
                .done(() => {
                    this.ClearPositionChangesHistory();
                    this.InitializePositions()
                        .catch(error => reject(error))
                        .then(response => resolve());
                    resolve();
                })
        })
    }

    /**
     * ��������� ������ ��� ������� �� ������ ��� ������������� ����������
     */
    GetSyncPositionsData() {
        let modal = $("#employeePosisionModal");
        
        let checkmark = $("#selectedPositions .readonly-checkmark")[0];
        let primaryPositionName = EmployeePositionsManagement.primaryPositionName == "" ? $(checkmark).closest("tr").find(".position-name").text() : EmployeePositionsManagement.primaryPositionName;

        let allPositions = EmployeePositionsManagement.positionsToAdd;
        modal.find("#allPositions .checkmark-checked").each((index, item) => {
            allPositions[index + EmployeePositionsManagement.positionsToAdd.length] = $(item).closest("tr").find(".position-name").text();
        });

        let selectedPositions = EmployeePositionsManagement.positionsToRemove;
        modal.find("#selectedPositions .cross-crossed").each((index, item) => {
            selectedPositions[index + EmployeePositionsManagement.positionsToRemove.length] = $(item).closest("tr").find(".position-name").text();
        });

        return {
            EmployeeId: $("#employeeId").val(),
            PrimaryPositionName: primaryPositionName,
            PositionsToAdd: allPositions,
            PositionsToRemove: selectedPositions
        }
    }

    /**
     * ����� �� ������ ���� ���������� �������������
     * @param {*} event 
     */
    AllPositionsSearch(event) {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let allPositionsUrl = $(event.currentTarget).attr("data-href");
            let allPositionsSearchData = this.AllPositionsSearchGetData();

            request.CommonPostRequest(allPositionsUrl, allPositionsSearchData).done(response => {
                this.RenderPositionsFilterFields(response);
                this.RenderAllPositionsList(response["allPositions"]);
                resolve();
            });
        })
    }

    /**
     * �������� ������ ��� �������� �� ������ ��� ������ �� ������ ���� ���������� �������������
     */
    AllPositionsSearchGetData() {
        return {
            Id: $("#employeeId").val(),
            DivisionId: $("#DivisionId").val(),
            OrganizationId: $("#OrganizationId").val(),
            SearchAllPosName: $("#SearchAllPosName").val(),
            SearchAllParentPosName: $("#SearchAllParentPosName").val(),
        }
    }
    
    /**
     * ����� ������ �� ������ ���� ���������� �������������
     * @param {*} event 
     */
    ClearAllPositionsSearch(event) {
        return new Promise((resolve, reject) => {
            this.GetRecords(event).then(response => {
                this.RenderPositionsFilterFields(response);
                this.RenderAllPositionsList(response["allPositions"]);
                resolve();
            });
        })
    }

    /**
     * ����� �� ������ ���������� �����������
     * @param {*} event
     */
    SelectedPositionsSearch(event) {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let selectedPositionsUrl = $(event.currentTarget).attr("data-href");
            let selectedPositionsSearchData = this.SelectedPositionsSearchGetData();

            request.CommonPostRequest(selectedPositionsUrl, selectedPositionsSearchData).done(response => {
                this.RenderPositionsFilterFields(response);
                this.RenderSelectedPositionsList(response["selectedPositions"]);
                resolve();
            });
        })
    }

    /**
     * �������� ������ ��� �������� �� ������ ��� ������ �� ������ ���� ���������� ����������
     */
    SelectedPositionsSearchGetData() {
        return {
            Id: $("#employeeId").val(),
            DivisionId: $("#DivisionId").val(),
            OrganizationId: $("#OrganizationId").val(),
            SearchSelectedPosName: $("#SearchSelectedPosName").val(),
            SearchSelectedParentPosName: $("#SearchSelectedParentPosName").val(),
        }
    }

    /**
     * ����� ������ �� ������ ���� ���������� �����������
     * @param {*} event 
     */
    ClearSelectedPositionsSearch(event) {
        return new Promise((resolve, reject) => {
            this.GetRecords(event).then(response => {
                this.RenderPositionsFilterFields(response);
                this.RenderSelectedPositionsList(response["selectedPositions"]);
                resolve();
            });
        })
    }

    /**
     * ��������� ���� �������� �� ������� ���������� ����������(� ��������� ���� ���������� �����������)
     * @param {*} response 
     */
    RenderPositionsFilterFields(response) {
        let allPositionsVM = response["allPositionsVM"];
        let selectedPositionsVM = response["selectedPositionsVM"];
        $("#SearchAllPosName").val(allPositionsVM["searchAllPosName"]);
        $("#SearchAllParentPosName").val(allPositionsVM["searchAllParentPosName"]);
        $("#SearchSelectedPosName").val(selectedPositionsVM["searchSelectedPosName"]);
        $("#SearchSelectedParentPosName").val(selectedPositionsVM["searchSelectedParentPosName"]);
    }

    /**
     * �������� ��� �������� ������ �� ����� ����������� ������
     * @param {*} event 
     */
    NextAllPositions(event) {
        return new Promise((resolve, reject) => {
            this.GetRecords(event).then(response => {
                this.RenderAllPositionsList(response);
                resolve();
            });
        })
    }
    
    /**
     * �������� ��� �������� ������ �� ����� ����������� �����
     * @param {*} event 
     */
    PreviousAllPositions(event) {
        return new Promise((resolve, reject) => {
            this.GetRecords(event).then(response => {
                this.RenderAllPositionsList(response);
                resolve();
            });
        })
    }

    /**
     * ������������ ������ ���� ����������
     * @param {*} allPositions ������ ���� ���������� �������������
     */
    RenderAllPositionsList(allPositions) {
        this.InitializeAllPositions(allPositions);
        this.RestorePositionsToAdd();
    }

    /**
     * ���������� ���������, ���������� �� ����������
     */
    RememberPositionsToAdd() {
        let positionsToAdd = EmployeePositionsManagement.positionsToAdd;
        $("#employeePosisionModal").find("#allPositions .checkmark").each((i, item) => {
            let positionName = $(item).closest("tr").find(".position-name").text();
            
            // ��� ��������, ���������� �������� �� ���������� ������������ � ������ EmployeePositionsManagement.positionsToAdd
            if ($(item).hasClass("checkmark-checked") && EmployeePositionsManagement.positionsToAdd.indexOf(positionName) == -1) {
                let elementToAddIndex = positionsToAdd.length;
                positionsToAdd[elementToAddIndex] = positionName;
            }

            // ��� ��������, �� ���������� �������� �� ����������, ��� �� ������� � ������� EmployeePositionsManagement.positionsToAdd �������� ������
            else if (!$(item).hasClass("checkmark-checked")) {
                let elementToRemoveIndex = positionsToAdd.indexOf(positionName);
                if (elementToRemoveIndex != -1) {
                    positionsToAdd.splice(elementToRemoveIndex, 1);
                }
            }
        });
        EmployeePositionsManagement.positionsToAdd = positionsToAdd;
    }

    /**
     * ��������������� ��������� ��� ���������� ��������� ����� ��������
     */
    RestorePositionsToAdd() {
        EmployeePositionsManagement.positionsToAdd.map(positionName => {
            $("#employeePosisionModal #allPositions tbody .position-name").each((index, positionNameElement) => {
                let row = $(positionNameElement).closest("tr");
                if ($(positionNameElement).text() == positionName) {
                    $(row).find(".checkmark").addClass("checkmark-checked");
                }
            })
        });
    }
    
    /**
     * �������� ��� �������� ������ � ����������� ���������� ������
     * @param {*} event 
     */
    NextSelectedPositions(event) {
        return new Promise((resolve, reject) => {
            this.GetRecords(event).then(response => {
                this.RenderSelectedPositionsList(response);
                resolve();
            });
        })
    }
    
    /**
     * �������� ��� �������� ������ � ����������� ���������� �����
     * @param {*} event 
     */
    PreviousSelectedPositions(event) {
        return new Promise((resolve, reject) => {
            this.GetRecords(event).then(response => {
                this.RenderSelectedPositionsList(response);
                resolve();
            });
        })
    }

    /**
     * ����������� ������ ���������� ����������
     * @param {*} selectedPositions ������ ���������� ����������
     */
    RenderSelectedPositionsList(selectedPositions) {
        this.InitializeSelectedPositions(selectedPositions);
        this.RestorePositionsToRemove();
        this.RestorePrimaryPosition();
    }

    /**
     * ���������� ���������, ���������� �� ��������
     */
    RememberPositionsToRemove() {
        let positionsToRemove = EmployeePositionsManagement.positionsToRemove;
        $("#employeePosisionModal").find("#selectedPositions .cross").each((i, item) => {
            let positionName = $(item).closest("tr").find(".position-name").text();
            
            // ��� ��������, ���������� ��������� �� �������� ������������ � ������ EmployeePositionsManagement.positionsToRemove
            if ($(item).hasClass("cross-crossed") && EmployeePositionsManagement.positionsToRemove.indexOf(positionName) == -1) {
                let elementToAddIndex = positionsToRemove.length;
                positionsToRemove[elementToAddIndex] = positionName;
            }

            // ��� ��������, �� ���������� ��������� �� ��������, ��� �� ������� � ������� EmployeePositionsManagement.positionsToRemove �������� ������
            else if (!$(item).hasClass("cross-crossed")) {
                let elementToRemoveIndex = positionsToRemove.indexOf(positionName);
                if (elementToRemoveIndex != -1) {
                    positionsToRemove.splice(elementToRemoveIndex, 1);
                }
            }
        });
        EmployeePositionsManagement.positionsToRemove = positionsToRemove;
    }

    /**
     * ���������� �������� ��������� ���������
     */
    RememberPrimaryPosition() {
        let checkmark = $("#employeePosisionModal #selectedPositions tbody .readonly-checkmark");
        if (checkmark.length != 0) {
            let row = $(checkmark).closest("tr");
            let primaryPositionName = $(row).find(".position-name").text();
            EmployeePositionsManagement.primaryPositionName = primaryPositionName;
        }
    }

    /**
     * ��������������� ��������� ��� �������� ��������� ����� ��������
     */
    RestorePositionsToRemove() {
        EmployeePositionsManagement.positionsToRemove.map(positionName => {
            $("#employeePosisionModal #selectedPositions tbody .position-name").each((index, positionNameElement) => {
                let row = $(positionNameElement).closest("tr");
                if ($(positionNameElement).text() == positionName) {
                    $(row).find(".cross").addClass("cross-crossed");
                }
            })
        });
    }

    /**
     * ��������������� �������� ��������� ���������
     */
    RestorePrimaryPosition() {
        $("#employeePosisionModal #selectedPositions tbody .position-name").each((index, positionNameElement) => {
            let positionName = $(positionNameElement).text();
            let row = $(positionNameElement).closest("tr");

            // ������������ ������� �� �������� ���������
            if (positionName == EmployeePositionsManagement.primaryPositionName) {
                $(row).find(".hide-checkmark").removeClass("hide-checkmark").addClass("readonly-checkmark");
                $(row).find(".cross").addClass("readonly-cross");
            }

            // ������ ������� � ��������� ����������
            else {
                $(row).find(".readonly-checkmark").removeClass("readonly-checkmark").addClass("hide-checkmark");
                $(row).find(".readonly-cross").removeClass("readonly-cross");
            }
        });
    }

    /**
     * �������� ������ ��� ������ ����������
     * @param {*} event 
     */
    GetRecords(event) {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let url = $(event.currentTarget).attr("data-href");

            request.JsonGetRequest(url)
                .fail(error => reject(error))
                .done(response => {
                    console.log(response);
                    resolve(response);
                });
        })
    }

    /**
     * ��������� ������� ��� �������� ���������� ���� ���������� ����������� ����������
     * @param {*} event 
     */
    OnPositionModalClosed(event) {
        // ���� ���� ������ �� �������� ���������� ���� � ���������� ����������������� ���������
        if (this.ExistsPositionChanges()) {
            event.preventDefault();

            // ������ ������������� �� ��������, � ������ ������ ��������� ���������� � �������� ����
            Swal.fire(MessageManager.Invoke("PositionModalClosedConfirmation")).then(dialogResult => {
                if (dialogResult.value) {
                    this.ClearPositionManagementSearch().then(() => {
                        this.ClearPositionChangesHistory();
                        $(event.currentTarget).modal("hide");
                    });
                }
            });
        }

        // ����� �������������� ������������ ��������
        else {
            this.ClearPositionManagementSearch().then(() => location.reload());
        }
    }

    /**
     * ����� ���������� ��������, �����������, ��������� �� ��������� ��� ��������� ����������
     */
    ExistsPositionChanges() {
        let modal = $("#employeePosisionModal");
        let allPositions = $(modal).find("#allPositions");
        let selectedPositions = $(modal).find("#selectedPositions");
        let positionsToAdd = $(allPositions).find(".checkmark-checked");
        let positionsToRemove = $(selectedPositions).find(".cross-crossed");
        return EmployeePositionsManagement.positionsToAdd.length > 0 || positionsToAdd.length > 0 || 
            EmployeePositionsManagement.positionsToRemove.length > 0 || positionsToRemove.length > 0 ||
            EmployeePositionsManagement.primaryPositionChanged == true;
    }
    
    /**
     * ������� ���������� �� ����������, ������������ ��� ������ � ����������� ����������
     */
    ClearPositionChangesHistory() {
        EmployeePositionsManagement.positionsToAdd = [];
        EmployeePositionsManagement.positionsToRemove = [];
        EmployeePositionsManagement.primaryPositionName = "";
        EmployeePositionsManagement.primaryPositionChanged = false;
        let modal = $("#employeePosisionModal");
        let allPositions = $(modal).find("#allPositions");
        $(allPositions).find(".checkmark-checked").removeClass("checkmark-checked");
        let selectedPositions = $(modal).find("#selectedPositions");
        $(selectedPositions).find(".cross-crossed").removeClass("cross-crossed");
    }

    /**
     * ����� ������������ ������ �� ���, ������ ������ �� ����������
     */
    ClearPositionManagementSearch() {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let clearPositionSearchUrl = Localization.GetUri("clearPositionSearch");

            request.CommonGetRequest(clearPositionSearchUrl)
                .fail(() => reject(error))
                .done(() => resolve())
        })
    }
}

// ��������� ���� ���������� ����������� ����������
$("#employeePosisionModal")
    .off("click", "#syncEmpPositionsBtn").on("click", "#syncEmpPositionsBtn", event => {
        event.preventDefault();
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.SynchronizePositions();
    })
    .off("checkmark-check", ".checkmark").on("checkmark-check", ".checkmark", event => {
        let button = new Button();
        button.CheckmarkCheck(event);
    })
    .off("hide-checkmark-click", ".hide-checkmark").on("hide-checkmark-click", ".hide-checkmark", event => {
        let button = new Button();
        button.HideCheckmarkCheck(event);
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.RememberPrimaryPosition();
        EmployeePositionsManagement.primaryPositionChanged = true;
    })
    .off("cross-click", ".cross").on("cross-click", ".cross", event => {
        let button = new Button();
        button.CrossClick(event);
    })
    .off("click", "#allPositionsNav .nav-next .nav-url").on("click", "#allPositionsNav .nav-next .nav-url", event => {
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.RememberPositionsToAdd();
        employeePositionsManagement.NextAllPositions(event);
    })
    .off("click", "#allPositionsNav .nav-previous .nav-url").on("click", "#allPositionsNav .nav-previous .nav-url", event => {
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.RememberPositionsToAdd();
        employeePositionsManagement.PreviousAllPositions(event);
    })
    .off("click", "#selectedPositionsNav .nav-next .nav-url").on("click", "#selectedPositionsNav .nav-next .nav-url", event => {
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.RememberPositionsToRemove();
        employeePositionsManagement.RememberPrimaryPosition();
        employeePositionsManagement.NextSelectedPositions(event);
    })
    .off("click", "#selectedPositionsNav .nav-previous .nav-url").on("click", "#selectedPositionsNav .nav-previous .nav-url", event => {
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.RememberPositionsToRemove();
        employeePositionsManagement.RememberPrimaryPosition();
        employeePositionsManagement.PreviousSelectedPositions(event);
    })
    .off("click", "#allPositionsSearch").on("click", "#allPositionsSearch", event => {
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.RememberPositionsToAdd();
        employeePositionsManagement.AllPositionsSearch(event);
    })
    .off("click", "#selectedPositionsSearch").on("click", "#selectedPositionsSearch", event => {
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.RememberPositionsToRemove();
        employeePositionsManagement.RememberPrimaryPosition();
        employeePositionsManagement.SelectedPositionsSearch(event);
    })
    .off("click", "#clearAllPositionsSearch").on("click", "#clearAllPositionsSearch", event => {
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.RememberPositionsToAdd();
        employeePositionsManagement.ClearAllPositionsSearch(event);
    })
    .off("click", "#clearSelectedPositionsSearch").on("click", "#clearSelectedPositionsSearch", event => {
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.RememberPositionsToRemove();
        employeePositionsManagement.RememberPrimaryPosition();
        employeePositionsManagement.ClearSelectedPositionsSearch(event);
    })
    .off('hide.bs.modal').on("hide.bs.modal", event => {
        let employeePositionsManagement = new EmployeePositionsManagement();
        employeePositionsManagement.OnPositionModalClosed(event);
    });