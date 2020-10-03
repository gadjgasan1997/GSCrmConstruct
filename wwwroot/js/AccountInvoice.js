class AccountInvoice {
    /**
     * �������� ���������
     * @param {*} event 
     */
    Create(event) {
        this.Clear();
        let filter = $(event.currentTarget).closest("#accInvoiceCreateModal");
        let accInvoiceCreateUrl = $(filter).find("form").attr("action");
        let accInvoiceCreateData = this.CreateGetData();
        let request = new AjaxRequests();

        request.JsonPostRequest(accInvoiceCreateUrl, accInvoiceCreateData)
            .fail(response => {
                Utils.CommonErrosHandling(response["responseJSON"], ["CreateAccountInvoice"]);
            })
            .done(() => {
                $("#accInvoiceCreateModal").modal("hide");
                location.reload();
            });
    }

    /** ����� ���������� ������, ���������� ��� �������� ��������� */
    CreateGetData() {
        return {
            AccountId: $("#accountId").val(),
            BankName: $("#createAccInvoiceBankName").val(),
            City: $("#createAccInvoiceCity").val(),
            CheckingAccount: $("#createAccInvoiceChecking").val(),
            CorrespondentAccount: $("#createAccInvoiceCorrespondent").val(),
            BIC: $("#createAccInvoiceBIC").val(),
            SWIFT: $("#createAccInvoiceSWIFT").val()
        }
    }

    /** ������ �������� ��������� */
    CancelCreate() {
        $("#accInvoiceCreateModal").modal("hide");
        this.CreateClearFields();
        this.Clear();
    }

    /** ������� ���� � ��������� ���� �������� ��������� */
    CreateClearFields() {
        ["createAccInvoiceBankName", "createAccInvoiceCity", "createAccInvoiceChecking", "createAccInvoiceCorrespondent", "createAccInvoiceBIC", "createAccInvoiceSWIFT"].map(item => $("#" + item).val())
    }

    /**
     * ��������� ������ �� ������ ��� ��������� �������� � ���������� ��� �� ��������������
     * ���� ��� �������, ����������� ������� ��������� ����
     * @param {*} event 
     */
    Initialize(event) {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let editItemBtn = $(event.currentTarget).closest(".edit-item-btn");
            let getAddressUrl = $(editItemBtn).find(".edit-item-url a").attr("href");
            
            request.JsonGetRequest(getAddressUrl)
                .fail(() => reject())
                .done(response => {
                    this.InitializeFields(response);
                    resolve();
                })
        })
    }

    /**
     * ��������� ���� � ��������� ���� �������������� ��������� ����������� � ������� ����������
     * @param {*} invoiceData ������ �� ������
     */
    InitializeFields(invoiceData) {
        localStorage.setItem("accountinvoiceData", invoiceData);
        $("#accAddressId").val(invoiceData["id"]);
        $("#updateAccInvoiceBankName").val(invoiceData["bankName"]);
        $("#updateAccInvoiceCity").val(invoiceData["city"]);
        $("#updateAccInvoiceChecking").val(invoiceData["checkingAccount"]);
        $("#updateAccInvoiceCorrespondent").val(invoiceData["correspondentAccount"]);
        $("#updateAccInvoiceBIC").val(invoiceData["bic"]);
        $("#updateAccInvoiceSWIFT").val(invoiceData["swift"]);
    }


    /**
     * ���������� ����������
     * @param {*} event 
     */
    Update(event) {
        return new Promise((resolve, reject) => {
            let accInvoiceUpdateUrl = $(event.currentTarget).find(".remove-item-url").text();
            let accInvoiceUpdateData = this.UpdateGetData();
            let request = new AjaxRequests();

            request.JsonPostRequest(accInvoiceUpdateUrl, accInvoiceUpdateData)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["UpdateAccountInvoice"]);
                })
                .done(() => {
                    $("#accInvoiceUpdateModal").modal("hide");
                    location.reload();
                });
        });
    }

    /** ����� ���������� ������, ���������� ��� ��������� ��������� */
    UpdateGetData() {
        return {
            AccountId: $("#accountId").val(),
            BankName: $("#updateAccInvoiceBankName").val(),
            City: $("#updateAccInvoiceCity").val(),
            CheckingAccount: $("#updateAccInvoiceChecking").val(),
            CorrespondentAccount: $("#updateAccInvoiceCorrespondent").val(),
            BIC: $("#updateAccInvoiceBIC").val(),
            SWIFT: $("#updateAccInvoiceSWIFT").val()
        }
    }

    /** ������ ��������� ��������� */
    CancelUpdate() {
        $("#accInvoiceUpdateModal").modal("hide");
        this.UpdateClearFields();
        this.Clear();
    }

    /** ������� ���� � ��������� ���� ��������� ��������� */
    UpdateClearFields() {
        ["updateAccInvoiceBankName", "updateAccInvoiceCity", "updateAccInvoiceChecking", "updateAccInvoiceCorrespondent", "updateAccInvoiceBIC", "updateAccInvoiceSWIFT"].map(item => $("#" + item).val())
    }

    /**
     * �������� ������ �������
     * @param {*} event 
     */
    Remove(event) {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let removeAccInvoiceUrl = $(event.currentTarget).find(".remove-item-url a").attr("href");

            request.CommonDeleteRequest(removeAccInvoiceUrl)
                .fail(() => reject())
                .done(() => location.reload());
        });
    }

    /** ������� ���� � �������� */
    Clear() {
        $('.under-field-error').empty();
    }
}

$("#accInvoicesList")
    .off("click", ".remove-item-btn").on("click", ".remove-item-btn", event => {
        event.preventDefault();
        let accountInvoice = new AccountInvoice();
        accountInvoice.Remove(event);
    })
    .off("click", ".edit-item-btn").on("click", ".edit-item-btn", event => {
        event.preventDefault();
        let accountInvoice = new AccountInvoice();
        accountInvoice.Initialize(event);
    });

$("#accInvoiceCreateModal")
    .off("click", "#createAccInvoiceBtn").on("click", "#createAccInvoiceBtn", event => {
        event.preventDefault();
        let accountInvoice = new AccountInvoice();
        accountInvoice.Create(event);
    })
    .off("click", "#cancelCreationAccInvoiceBtn").on("click", "#cancelCreationAccInvoiceBtn", event => {
        event.preventDefault();
        let accountInvoice = new AccountInvoice();
        accountInvoice.CancelCreate(event);
    });
    
$("#accInvoiceUpdateModal")
    .off("click", "#updateAccInvoiceBtn").on("click", "#updateAccInvoiceBtn", event => {
        event.preventDefault();
        let accountInvoice = new AccountInvoice();
        accountInvoice.Update(event);
    })
    .off("click", "#cancelUpdateAccInvoiceBtn").on("click", "#cancelUpdateAccInvoiceBtn", event => {
        event.preventDefault();
        let accountInvoice = new AccountInvoice();
        accountInvoice.CancelUpdate(event);
    });