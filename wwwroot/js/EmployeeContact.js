class EmployeeContact {
    /**
     * ��������� ������ �� ������ ��� �������� ��������
     * ��� ������� ������ ������������ ��
     */
    Create() {
        return new Promise((resolve, reject) => {
            Utils.ClearErrors();
            let request = new AjaxRequests();
            let createEmpContactUrl = $("#empContactCreateModal form").attr("action");
            let createEmpContactData = this.CreateGetData();

            request.JsonPostRequest(createEmpContactUrl, createEmpContactData)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["CreateEmployeeContact"]);
                })
                .done(() => {
                    $("#empContactCreateModal").modal("hide");
                    location.reload();
                });
        })
    }

    /**
     * ���������� ������ ��� ������� �� ������ ��� �������� ��������
     */
    CreateGetData() {
        let contactType = $("#createEmpContactType").val();
        if (contactType.length == 0) {
            contactType = "None";
        }
        return {
            EmployeeId: $("#employeeId").val(),
            ContactType: contactType,
            PhoneNumber: $("#createEmpContactPhone").val(),
            Email: $("#createEmpContactEmail").val()
        }
    }

    /**
     * ������� ���� � ��������� ���� �������� ��������
     */
    CreateClearFields() {
        $("#createEmpContactType").val("");
        $("#createEmpContactPhone").val("");
        $("#createEmpContactEmail").val("");
        $(".dropdown-el input").map(item => $(item).removeAttr("checked"))
        let contactNoneType = $(".dropdown-el input")[0];
        $(contactNoneType).prop('checked', true);
    }

    /**
     * �������� ��� ������ �������� ��������
     */
    CancelCreate() {
        $("#empContactCreateModal").modal("hide");
        this.CreateClearFields();
        Utils.ClearErrors();
    }

    /**
     * ��������� ������ �� ������ ��� ��������� �������� � �������� ��� ��� ��������������
     * ���� ��� �������, ����������� ������� ��������� ����
     * ��� ������� ������ ������������ ��
     * @param {*} event 
     */
    Initialize(event) {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let editItemBtn = $(event.currentTarget).closest(".edit-item-btn");
            let getContactUrl = $(editItemBtn).find(".edit-item-url a").attr("href");
            
            request.JsonGetRequest(getContactUrl)
                .catch(() => reject())
                .then(response => {
                    this.InitializeFields(response);
                    resolve();
                })
        })
    }

    /**
     * ��������� ���� � ��������� ���� �������������� �������� ����������� � ������� ����������
     * @param {*} contactData ������ � ��������
     */
    InitializeFields(contactData) {
        localStorage.setItem("contactData", contactData);
        let dropdownElement = $("#updateEmpContactType").closest(".dropdown-area").find(".dropdown-el");
        $(dropdownElement).find("input").map((index, item) => {
            if ($(item).attr("value") == contactData["contactType"]) {
                $(item).prop("checked", true);
            }
            else $(item).removeAttr("checked");
        })
        $("#empContactId").val(contactData["id"]);
        $("#updateEmpContactType").val(contactData["contactType"]);
        $("#updateEmpContactPhone").val(contactData["phoneNumber"]);
        $("#updateEmpContactEmail").val(contactData["email"]);
    }

    /**
     * ��������� ������ �� ������, �������� ��������
     * ��� ������� ������ ������������ ��
     */
    Update() {
        return new Promise((resolve, reject) => {
            Utils.ClearErrors();
            let request = new AjaxRequests();
            let updateEmpContactUrl = $("#empContactUpdateModal form").attr("action");
            let updateEmpContactData = this.UpdateGetData();

            request.JsonPostRequest(updateEmpContactUrl, updateEmpContactData)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["UpdateEmployeeContact"]);
                })
                .done(() => {
                    $("#empContactUpdateModal").modal("hide");
                    location.reload();
                });
        })
    }

    /**
     * ��������� ������ ��� �������� ������� �� ������ ��� ���������� ��������
     */
    UpdateGetData() {
        let contactType = $("#updateEmpContactType").val();
        if (contactType.length == 0) {
            contactType = "None";
        }
        return {
            Id: $("#empContactId").val(),
            EmployeeId: $("#employeeId").val(),
            ContactType: contactType,
            PhoneNumber: $("#updateEmpContactPhone").val(),
            Email: $("#updateEmpContactEmail").val()
        }
    }

    /**
     * ��������, ����������� ��� ������ �������������� ��������
     */
    CancelUpdate() {
        $("#empContactUpdateModal").modal("hide");
        let contactData = localStorage.getItem("contactData");
        this.InitializeFields(contactData);
        Utils.ClearErrors();
    }

    /**
     * ��������� ������ �� ������, ������ ������� �� ������
     * ��� ������� ������ ������������ ��
     * @param {*} event 
     */
    Remove(event) {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let removeEmpContactUrl = $(event.currentTarget).find(".remove-item-url a").attr("href");

            request.CommonDeleteRequest(removeEmpContactUrl)
                .fail(() => {
                    Swal.fire(MessageManager.Invoke("RemoveEmployeeContactError"));
                    reject();
                })
                .done(() => location.reload());
        })
    }
}

// ������ ��������� ����������
$("#employeeContactsList")
    .off("click", ".remove-item-btn").on("click", ".remove-item-btn", event => {
        event.preventDefault();
        let employeeContact = new EmployeeContact();
        employeeContact.Remove(event);
    })
    .off("click", ".edit-item-btn").on("click", ".edit-item-btn", event => {
        event.preventDefault();
        let employeeContact = new EmployeeContact();
        employeeContact.Initialize(event);
    });

// ��������� ���� �������� �������� ����������
$("#empContactCreateModal")
    .off("click", "#createEmpContactBtn").on("click", "#createEmpContactBtn", event => {
        event.preventDefault();
        let employeeContact = new EmployeeContact();
        employeeContact.Create();
    })
    .off("click", "#cancelCreationEmpContactBtn").on("click", "#cancelCreationEmpContactBtn", event => {
        event.preventDefault();
        let employeeContact = new EmployeeContact();
        employeeContact.CancelCreate();
    });

// ��������� ���� ��������� �������� ����������
$("#empContactUpdateModal")
    .off("click", "#updateEmpContactBtn").on("click", "#updateEmpContactBtn", event => {
        event.preventDefault();
        let employeeContact = new EmployeeContact();
        employeeContact.Update();
    })
    .off("click", "#cancelUpdateEmpContactBtn").on("click", "#cancelUpdateEmpContactBtn", event => {
        event.preventDefault();
        let employeeContact = new EmployeeContact();
        employeeContact.CancelUpdate();
    });