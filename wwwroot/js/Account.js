class Account {
    /** �������� ������� */
    Create() {
        return new Promise((resolve, reject) => {
            Utils.ClearErrors();
            let request = new AjaxRequests();
            let createEmpUrl = $("#accountModal form").attr("action");
            let createEmpData = this.CreateGetData();
            let accountType = createEmpData["AccountType"];

            request.JsonPostRequest(createEmpUrl, createEmpData)
                .fail(response => {
                    switch(accountType) {
                        case "Individual":
                            Utils.CommonErrosHandling(response["responseJSON"], ["CreateAccount", "CreateAccountIndividual"]);
                            break;
                        case "IndividualEntrepreneur":
                            Utils.CommonErrosHandling(response["responseJSON"], ["CreateAccount", "CreateAccountIE"]);
                            break;
                        case "LegalEntity":
                            Utils.CommonErrosHandling(response["responseJSON"], ["CreateAccount", "CreateAccountLE"]);
                            break;
                    }
                })
                .done(newAccountUrl => {
                    $("#accountModal").modal("hide");
                    location.replace(newAccountUrl);
                });
        })
    }

    /** ��������� ������ ��� �������� ��� �������� ������� */
    CreateGetData() {
        let accountType = this.GetAccountType();
        return {
            FirstName: $("#accFirstName").val(),
            LastName: $("#accLastName").val(),
            MiddleName: $("#accMiddleName").val(),
            Name: accountType == "IndividualEntrepreneur" ? $("#accNameIE").val() : $("#accNameLE").val(),
            INN: this.GetAccountINN(accountType),
            KPP: $("#accKPP").val(),
            OKPO: $("#accOKPO").val(),
            OGRN: $("#accOGRN").val(),
            Country: $("#accCountry .autocomplete-input").val(),
            PrimaryManagerInitialName: $("#accManager .autocomplete-input").val(),
            AccountType: accountType
        }
    }

    /** ����������, ������ �� ������������ ���. ����, �� ��� ��. ���� ��� ��� ������� */
    GetAccountType() {
        let check = $("#accountModal .radio-tabs .form-check");
        if ($(check[0]).hasClass("active")) return "Individual";
        else if ($(check[1]).hasClass("active")) return "IndividualEntrepreneur";
        return "LegalEntity";
    }

    /**
     * � ����������� �� ���� ������� ���������� ����, ��� ������� ��� ���
     * @param {*} accountType ��� �������
     */
    GetAccountINN(accountType) {
        let INN = "";
        switch(accountType) {
            case "Individual":
                INN = $("#accINNIndividual").val();
                break;
            case "IndividualEntrepreneur":
                INN = $("#accINNIE").val();
                break;
            case "LegalEntity":
                INN = $("#accINNLE").val();
                break;
        }
        return INN;
    }

    /** ������� ���� � ��������� ���� �������� ������� */
    CreateClearFields() {
        ["#accCountry .autocomplete-input", "#accManager .autocomplete-input"]
            .map(item => $(item).val(""));
        ["#accFirstName", "#accLastName", "#accMiddleName", "#accINNIndividual", "#accNameIE", "#accINNIE", "#accNameLE", "#accINNLE", "#accKPP", "#accOKPO", "#accOGRN"]
            .map(item => $(item).val(""));  
    }

    /** �������� ��� ������ �������� ������� */
    CancelCreate() {
        $("#accountModal").modal("hide");
        this.CreateClearFields();
        Utils.ClearErrors();
        Account.SetDefaultTab();
    }

    /** ��������� ����� ������� */
    ChangeSite(event) {
        let siteInput = $(event.currentTarget).closest(".input-group").find(".form-control-url a");
        let changeSiteUrl = $(event.currentTarget).find(".icon-pencil").attr("href");
        Swal.fire(MessageManager.Invoke("NewSiteMessage", {
            "siteInput": siteInput,
            "changeSiteUrl": changeSiteUrl
        }));
    }

    /**
     * ����� ��������� ��������� ���� � ���������� ������������ ������ �������
     * @param {*} event 
     */
    ChangeLegalAddressShowModal(event) {
        return new Promise((resolve, reject) => {
            this.HasAccNotLegalAddress()
                .catch(error => reject(error))
                .then(hasAccounts => {
                    // ���� ��� ������� ��������� ��������� �� ������
                    if (!hasAccounts) {
                        Swal.fire(MessageManager.Invoke("AddressListIsEmpty"))
                    }

                    // ����� ����������� ��������� ���� � ������� ������
                    else {
                        $("#changeLEAddrModal").find("#targetFormId").val("accountForm");
                        $("#changeLEAddrModal").modal("show");
                        let allAddresses = $("#changeLEAddrModal #accAddressNotLegalList").find(".list-group-item");
                        Array.from($(allAddresses)).map((address, index) => {
                            if (index == 0) {
                                $(address).addClass("active");
                            }
                            else {
                                $(address).removeClass("active");
                            }
                        })

                        let dropdown = new Dropdowns();
                        dropdown.SetDropdownValue("changeAccAddressType", "None");
                        $("#changeAccAddressType").val("None");
                    }
                });
        });
    }

    /**
     * ����� �������� �������, ������� �� �� ������� ������ �� ����������� ������������
     */
    HasAccNotLegalAddress() {
        return new Promise((resolve, reject) => {
            let accountId = $("#accountId").val();
            let getAddressesUrl = Localization.GetUri("hasAccNotLegalAddress") + accountId;
            let request = new AjaxRequests();
            request.JsonGetRequest(getAddressesUrl)
                .catch(error => reject(error))
                .then(response => resolve(response));
        })
    }

    /**
     * ��������� ������������ ������ ������� � ����������� �� ����, ������ ���� ������� ��������� ����
     */
    ChangeLegalAddressDispatcher(event) {
        return new Promise((resolve, reject) => {
            let modal = $(event.currentTarget).closest("#changeLEAddrModal");
            let targetFormId = $(modal).find("#targetFormId").val();

            // � ����������� �� ����, ����� ���� ������� ��������� ����
            switch(targetFormId) {
                // ���� ��� ���� ������� �� ���� ��������� ������ ������
                // ���������� ��������� ��������� ��������, ��� ��� ������ � ����� ������� ������ �� ������ ������ ��� ������������� ��������� � ������
                case "accAddressUpdateModal":
                    this.ChangeLegalAddressFromUpdateModal();
                    break;
                    
                // ���� ��� ���� ������� � ����� �������, ���������� ����� �������� ����������� ����� �������
                case "accountForm":
                    this.ChangeLegalAddressFromAccountForm();
                    break;
            }
            
        })
    }

    /**
     * ��������� ���� ������������ ������, ����������� �� ���������� ���� ��������� ������ ������
     */
    ChangeLegalAddressFromUpdateModal() {
        let selectedAddressId = this.GetSelectedAddressId();
        localStorage.setItem("selectedAddressId", selectedAddressId);
        $("#changeLEAddrModal").modal("hide");
    }

    /**
     * ��������� ���� ������������ ������,����������� � ����� �������
     */
    ChangeLegalAddressFromAccountForm() {
        return new Promise((resolve, reject) => {
            let changeLegalAddressUrl = $("#changeLEAddrModal").find("form").attr("action");
            let changeLegalAddressData = this.ChangeLegalAddressGetData();
            let request = new AjaxRequests();
            let needStop = false;

            request.JsonPostRequest(changeLegalAddressUrl, changeLegalAddressData)
                .catch(response => {
                    needStop = true;
                    Utils.CommonErrosHandling(response["responseJSON"], ["ChangeLegalAddress"]);
                    reject(response);
                })
                .then(() => {
                    if (!needStop) {
                        Swal.fire(MessageManager.Invoke("LegalAddressHasBeenChanged")).then(() => location.reload());
                    }
                    else reject();
                })
        })
    }

    /**
     * ����� ���������� ������, ����������� ��� ��������� ������������ ������ �������
     */
    ChangeLegalAddressGetData() {
        return {
            AccountId: $("#accountId").val(),
            NewLegalAddressId: this.GetSelectedAddressId(),
            CurrentAddressNewType: $("#changeAccAddressType").val()
        }
    }

    /**
     * ����� ���������� id ���������� ������ �� ���������� ���� ������ ������ ������������ ������
     */
    GetSelectedAddressId() {
        // ��������� ���������� ������
        let allAddresses = $("#changeLEAddrModal #accAddressNotLegalList").find(".list-group-item");
        let selectedAddress = Array.from(allAddresses).filter(address => {
            return $(address).hasClass("active");
        })[0];

        // ��������� id ���������� ������
        let selectedAddressId = "";
        if (selectedAddress != undefined) {
            selectedAddressId = $(selectedAddress).find("p").text();
        }

        return selectedAddressId;
    }

    /**
     * ������ ��������� ������������ ������ �������
     */
    CancelChangeLegalAddress() {
        let addressList = $("#changeLEAddrModal #accAddressNotLegalList");
        Array.from($(addressList).find(".list-group-item")).map((address, index) => {
            if (index == 0) {
                $(address).addClass("active");
            }
            else {
                $(address).removeClass("active");
            }
        });
    }

    /** ������� �������, ���� ������������ ��� �������� */
    Remove(event) {
        return new Promise((resolve, reject) => {
            Swal.fire(MessageManager.Invoke("RemoveAccountConfirmation")).then(result => {
                if (result["value"]) {
                    let removeAccUrl = $(event.currentTarget).attr("href");
                    let request = new AjaxRequests();
                    request.CommonDeleteRequest(removeAccUrl)
                        .fail(() => {
                            Swal.fire(MessageManager.Invoke("RemoveAccountError"));
                            reject();
                        })
                        .done(() => {
                            let accountsBackUrl = $("#accountsBackUrl a").attr("href");
                            location.replace(location.origin + accountsBackUrl);
                        });
                }
            })
        })
    }

    /**
     * ���������� �������
     * @param {*} event 
     */
    Update(event) {
        return new Promise((resolve, reject) => {
            let updateAccUrl = $(event.currentTarget).closest("#accountForm").find("form").attr("action");
            let updateAccData = this.UpdateGetData();
            let request = new AjaxRequests();
            request.JsonPostRequest(updateAccUrl, updateAccData)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["UpdateAccount"]);
                })
                .done(updatedAccUrl => location.replace(location.origin + updatedAccUrl))
        });
    }

    UpdateGetData() {
        let accountType = $("#AccountType").val();
        let kpp, okpo, ogrn = "";
        if (accountType == "LegalEntity") {
            kpp = $("#accountKPP").val();
            okpo = $("#accountOKPO").val();
            ogrn = $("#accountOGRN").val();
        }
        return {
            Id: $("#accountId").val(),
            OrganizationId: $("#OrganizationId").val(),
            AccountType: accountType,
            Name: $("#accountName").val(),
            INN: $("#accountINN").val(),
            KPP: kpp,
            OKPO: okpo,
            OGRN: ogrn
        }
    }

    AddAccManager(event) {
        return new Promise((resolve, reject) => {
            let modal = $(event.currentTarget).closest("#addAccountManagerModal");
            let addAccManagerUrl = $(modal).find("form").attr("action");
            let addAccManagerData = this.AddAccManagerGetData();
            let request = new AjaxRequests();
            request.CommonPostRequest(addAccManagerUrl, addAccManagerData)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["AddAccManager"]);
                })
                .done(accUrl => {
                    $("#addAccountManagerModal").hide();
                    location.replace(accUrl);
                });
        })
    }

    AddAccManagerGetData() {
        return {
            Id: $("#accountId").val(),
            NewPrimaryManagerId: $("#accManagerId").val()
        }
    }

    /**
     * ������������� ������� �� ��������� � ��������� ���� �������� �������
     */
    static SetDefaultTab() {
        let modal = $("#accountModal");
        let checkGroup = modal.find(".radio-tabs .form-check");
        modal.modal("hide");
        $(checkGroup).removeClass("active");
        $(checkGroup).find("input").removeAttr("checked");
        $(checkGroup[0]).addClass("active").find("input").prop("checked", "checked");
        modal.find(".tabs-content .tabs-content-item").hide();
        modal.find("#IndividualOption").fadeIn();
    }
}

// ��������� ���� �������� �������
$("#accountModal")
    .off("click", "#createAccBtn").on("click", "#createAccBtn", event => {
        event.preventDefault();
        let account = new Account();
        account.Create();
    })
    .off("click", "#cancelCreationAccBtn").on("click", "#cancelCreationAccBtn", event => {
        event.preventDefault();
        let account = new Account();
        account.CancelCreate();
    });

// ������ ��������
$("#accountsForm")
    .off("click", ".account-item").on("click", ".account-item", event => {
        let navTab = new Tab();
        navTab.ClearAccTab();
    })
    .off("click", "#accountTabs .nav-item").on("click", "#accountTabs .nav-item", event => {
        let navTab = new NavTab();
        navTab.Remember(event, "currentAccsTab");
        let navConnectedTab = new NavConnectedTab();
        navConnectedTab.Remember(event, "currentAccsConnectedTab");
    });

// �������� �������
$("#accountForm")
    .off("click", ".change-url-btn").on("click", ".change-url-btn", event => {
        let account = new Account();
        account.ChangeSite(event);
    })
    .off("click", ".change-address-btn").on("click", ".change-address-btn", event => {
        let account = new Account();
        account.ChangeLegalAddressShowModal(event);
    })
    .off("click", "#showAllManagers").on("click", "#showAllManagers", event => {
        $("#accountTeam").find(".account-manager").each((index, managerItem) => {
            // ������� �������� ������� ��������� � �������� ���������
            if (index != 0) {
                $(managerItem).slideToggle();
            }
        })
    })
    .off("click", "#accTeamManagementBtn").on("click", "#accTeamManagementBtn", event => {
        let accountManagement = new AccountManagement();
        accountManagement.InitializeAccTeam();
    })
    .off("click", "#updateAccBtn").on("click", "#updateAccBtn", event => {
        event.preventDefault();
        let account = new Account();
        account.Update(event);
    })
    .off("click", "#removeAccBtn").on("click", "#removeAccBtn", event => {
        event.preventDefault();
        let account = new Account();
        account.Remove(event);
    });

$("#accountAddInfoForm")
    .off("click", ".vert-nav-tab").on("click", ".vert-nav-tab", event => {
        let vertNavTab = new VertNavTab();
        vertNavTab.Remember(event, "currentAccTab");
    });

$("#changeLEAddrModal")
    .off("click", "#accAddressNotLegalList .list-group-item").on("click", "#accAddressNotLegalList .list-group-item", event => {
        if (!$(event.currentTarget).hasClass("active")) {
            let allAddresses = $("#changeLEAddrModal #accAddressNotLegalList").find(".list-group-item");
            Array.from(allAddresses).map(address => $(address).removeClass("active"));
            $(event.currentTarget).addClass("active");
        }
    })
    .off("click", "#changeLegalAddress").on("click", "#changeLegalAddress", event => {
        let account = new Account();
        account.ChangeLegalAddressDispatcher(event);
    })
    .off("click", "#cancelChangeLegalAddress").on("click", "#cancelChangeLegalAddress", event => {
        setTimeout(() => {
            let account = new Account();
            account.CancelChangeLegalAddress();
        }, 300);
    });

$("#addAccountManagerModal").off("click", "#addAccManager").on("click", "#addAccManager", event => {
    let account = new Account();
    account.AddAccManager(event);
})