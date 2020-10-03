class AccountAddress {
    /**
     * �������� ������
     * @param {*} event 
     */
    Create(event) {
        return new Promise((resolve, reject) => {
            this.Clear();
            let modal = $(event.currentTarget).closest("#accAddressCreateModal");
            let createAddressUrl = $(modal).find("form").attr("action");
            let createAddressData = this.CreateGetData();
            let request = new AjaxRequests();

            request.JsonPostRequest(createAddressUrl, createAddressData)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["CreateAddress"]);
                    reject(response);
                })
                .done(() => {
                    $("#accAddressCreateModal").modal("hide");
                    location.reload();
                });
        });
    }

    /**
     * ��������� ������, ����������� ��� �������� ������
     */
    CreateGetData() {
        let addressType = $("#createAccAddressType").val();
        if (addressType.length == 0) {
            addressType = "None";
        }
        return {
            AccountId: $("#accountId").val(),
            Country: $("#createAccAddressCountry").find(".autocomplete-input").val(),
            Region: $("#createAccAddressRegion").val(),
            City: $("#createAccAddressCity").val(),
            Street: $("#createAccAddressStreet").val(),
            House: $("#createAccAddressHouse").val(),
            AddressType: addressType,
        }
    }

    /**
     * ������ �������� ������
     */
    CancelCreate() {
        $("#accAddressCreateModal").modal("hide");
        this.CreateClearFields();
        this.Clear();
    }

    /**
     * ������� ���� � ��������� ���� �������� ������
     */
    CreateClearFields() {
        ["createAccAddressCountry", "createAccAddressRegion", "createAccAddressCity", "createAccAddressStreet", "createAccAddressHouse"].map(item => $(item).val(""));
        let modal = $("#accAddressCreateModal");
        $(modal).find(".dropdown-el input").map(item => $(item).removeAttr("checked"));
        let addressNoneType = $(modal).find(".dropdown-el input")[0];
        $(addressNoneType).prop('checked', true);
    }

    /**
     * ��������� ������ �� ������ ��� ��������� �������� �� ������ ��� ��� ��������������
     * ���� ��� �������, ����������� ������� ��������� ����
     * ��� ������� ������ ������������ ��
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
     * ��������� ���� � ��������� ���� �������������� ������ ����������� � ������� ����������
     * @param {*} addressData ������ �� ������
     */
    InitializeFields(addressData) {
        localStorage.setItem("accountAddressData", addressData);
        let dropdown = new Dropdowns();
        dropdown.SetDropdownValue("updateAccAddressType", addressData["addressType"]);
        $("#accAddressId").val(addressData["id"]);
        $("#updateAccAddressCountry").find(".autocomplete-input").val(addressData["country"]);
        $("#updateAccAddressRegion").val(addressData["region"]);
        $("#updateAccAddressCity").val(addressData["city"]);
        $("#updateAccAddressStreet").val(addressData["street"]);
        $("#updateAccAddressHouse").val(addressData["house"]);
        $("#updateAccAddressType").val(addressData["addressType"]);
    }

    /**
     * ���������� ������
     * @param {*} event 
     */
    Update(event) {
        return new Promise((resolve, reject) => {
            this.Clear();
            let modal = $(event.currentTarget).closest("#accAddressUpdateModal");
            let updateAddressUrl = $(modal).find("form").attr("action");
            let updateAddressData = this.UpdateGetData();
            let needStop = false;

            // ����� ������� ������������ ������ ��� �������������
            this.UpdateCheckLegalAddressNotChanged(updateAddressData)
                .catch(() => {
                    this.SetAddressTypeLegal();
                    needStop = true;
                    reject();
                })
                .then(response => {
                    if (!needStop && !response["WaitForChanges"]) {
                        let request = new AjaxRequests();
                        request.JsonPostRequest(updateAddressUrl, updateAddressData)
                            .fail(response => {
                                Utils.CommonErrosHandling(response["responseJSON"], ["UpdateAddress"]);
                                reject(response);
                            })
                            .done(() => {
                                localStorage.removeItem("selectedAddressId");
                                $("#accAddressUpdateModal").modal("hide");
                                location.reload();
                            });
                    }
                });
        });
    }

    /**
     * ��������� ������, ����������� ��� ���������� ������
     */
    UpdateGetData() {
        let addressType = $("#updateAccAddressType").val();
        if (addressType.length == 0) {
            addressType = "None";
        }
        return {
            Id: $("#accAddressId").val(),
            AccountId: $("#accountId").val(),
            Country: $("#updateAccAddressCountry").find(".autocomplete-input").val(),
            Region: $("#updateAccAddressRegion").val(),
            City: $("#updateAccAddressCity").val(),
            Street: $("#updateAccAddressStreet").val(),
            House: $("#updateAccAddressHouse").val(),
            AddressType: addressType,
            NewLegalAddressId: localStorage.getItem("selectedAddressId")
        }
    }

    /**
     * ����� ���������, ��� ��� ���������� ������������ ������ ��� ��� �� ���������
     * ���� �� ��� ���������, ���������� ������������ ������� ����� ����� � �������� ������������, � � ����� �������� ���
     * @param {*} updateAddressData ������, ������������ �� ������ ��� ��������� ������
     */
    UpdateCheckLegalAddressNotChanged(updateAddressData) {
        return new Promise((resolve, reject) => {
            let addressList = $("#accAddressesList");
            let addressesIdList = $(addressList).find(".address-id");

            // ����� ���� �������� ����������� ������
            let changedAddress = Array.from(addressesIdList).filter(addressIdElement => {
                return $(addressIdElement).text() == updateAddressData["Id"];
            });
            let changedAddressType = $(changedAddress).closest("tr").find(".address-type").text();
            let addressHasBeenSelected = localStorage.getItem("selectedAddressId") != "" && localStorage.getItem("selectedAddressId") != undefined && localStorage.getItem("selectedAddressId") != null;

            // ���� ����� ��� ������ �������� �����������, �� � ����� ������ ����� ������������ ������ �� ������
            if (updateAddressData["AddressType"] == "Legal") resolve({ "WaitForChanges": false });

            // �����, ���� ���������� ��� ������ ��� �����������, ���������� ������� ������ ����� ��� �����������
            else if (changedAddressType == Localization.GetString("Legal") && !addressHasBeenSelected) {

                // ������������� ������ ������� ������������ ������ � ������������
                Swal.fire(MessageManager.Invoke("ChangeLegalAddressInfo")).then(dialogResult => {

                    // ���� ������������ ����� �������� ��� ������� ������
                    if (dialogResult["value"]) {
                        
                        // ��������� �������� ������� �� ������� �������, ����� ������������
                        let account = new Account();
                        account.HasAccNotLegalAddress()
                            .catch(() => reject())
                            .then(hasAccounts => {
                                if (!hasAccounts) {
                                    Swal.fire(MessageManager.Invoke("AddressListIsEmpty"));
                                    reject();
                                }
                                else {
                                    // ���� ���� ������, ����������� ���� ��������� ������������ ������ �������
                                    $("#accAddressUpdateModal").modal("hide");
                                    $("#changeLEAddrModal").find("#changeCurrentAddrType").addClass("d-none");
                                    $("#changeLEAddrModal").find("#targetFormId").val("accAddressUpdateModal");
                                    $("#changeLEAddrModal").modal("show");
                                    resolve({ "WaitForChanges": true });
                                }
                            })
                    }
    
                    else reject();
                })
            }

            else resolve({ "WaitForChanges": false });
        })
    }

    /**
     * ���������� ��� ������� ���������� ���� � ���������� ������������ ������
     */
    OnChangeLegalAddressModalHide() {
        // � ����������� �� ����, ����� ���� ������� ��������� ����
        let targetFormId = $("#changeLEAddrModal").find("#targetFormId").val();
        switch(targetFormId) {

            // ���� ��� ���� ������� �� ���� ��������� ������ ������
            case "accAddressUpdateModal":
                $("#accAddressUpdateModal").modal("show");
                break;

            // ���� ��� ���� ������� � ����� �������, ���������� �������� ���������� ������ � ������� ���� ������
            case "accountForm":
                setTimeout(() => {
                    let dropdown = new Dropdowns();
                    dropdown.SetDropdownValue("changeAccAddressType", "None");
                    $("#changeAccAddressType").val("None");
                }, 300)
                break;
        }

        // ��������� �� ���� ������� ��������� ������� � ����� ������ ���� �������� ������ � ��������� ����
        // ������� ���������� ��������� ��-�� ����, ��� ���� ���� �� ��������� ���������� ��� �������� ����� ���� �� ���� ��������� ������ ������
        setTimeout(() => {
            $("#changeLEAddrModal").find("#changeCurrentAddrType").removeClass("d-none");
        }, 300);
    }

    /**
     * ����� ������������ ����� �����������
     */
    SetAddressTypeLegal() {
        $("#updateAccAddressType").val("Legal");
        let dropdown = new Dropdowns();
        dropdown.SetDropdownValue("updateAccAddressType", "Legal");
    }

    /**
     * ������ ���������� ������
     */
    CancelUpdate() {
        localStorage.removeItem("selectedAddressId");
        $("#accAddressUpdateModal").modal("hide");
        this.Clear();
    }

    /**
     * �������� ������ �������
     * @param {*} event 
     */
    Remove(event) {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let removeAccAddressUrl = $(event.currentTarget).find(".remove-item-url a").attr("href");

            request.CommonDeleteRequest(removeAccAddressUrl)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["RemoveAddress"]);
                    reject();
                })
                .done(() => location.reload());
        });
    }

    /**
     * ������� ���� � ��������
     */
    Clear() {
        $('.under-field-error').addClass("d-none").empty();
    }
}

// ������ ������� �������
$("#accAddressesList")
    .off("click", ".remove-item-btn").on("click", ".remove-item-btn", event => {
        event.preventDefault();
        let accountAddress = new AccountAddress();
        accountAddress.Remove(event);
    })
    .off("click", ".edit-item-btn").on("click", ".edit-item-btn", event => {
        event.preventDefault();
        let accountAddress = new AccountAddress();
        accountAddress.Initialize(event);
    });

// ��������� ���� �������� ������
$("#accAddressCreateModal")
    .off("click", "#createAccAddressBtn").on("click", "#createAccAddressBtn", event => {
        let accountAddress = new AccountAddress();
        accountAddress.Create(event);
    })
    .off("click", "#cancelCreationAccAddressBtn").on("click", "#cancelCreationAccAddressBtn", event => {
        let accountAddress = new AccountAddress();
        accountAddress.CancelCreate();
    });
    
// ��������� ���� ��������� ������
$("#accAddressUpdateModal")
    .off("click", "#updateAccAddressBtn").on("click", "#updateAccAddressBtn", event => {
        let accountAddress = new AccountAddress();
        accountAddress.Update(event);
    })
    .off("click", ".close").on("click", ".close", event => {
        let accountAddress = new AccountAddress();
        accountAddress.CancelUpdate();
    })
    .off("click", "#cancelUpdateAccAddressBtn").on("click", "#cancelUpdateAccAddressBtn", event => {
        let accountAddress = new AccountAddress();
        accountAddress.CancelUpdate();
    });

$("#changeLEAddrModal")
    .off("click", ".close").on("click", ".close", event => {
        event.preventDefault();
        let accountAddress = new AccountAddress();
        accountAddress.SetAddressTypeLegal();
        accountAddress.OnChangeLegalAddressModalHide();
    })
    .off("click", "#cancelChangeLegalAddress").on("click", "#cancelChangeLegalAddress", event => {
        event.preventDefault();
        let accountAddress = new AccountAddress();
        accountAddress.SetAddressTypeLegal();
        accountAddress.OnChangeLegalAddressModalHide();
    })
    .off('hide.bs.modal').on("hide.bs.modal", event => {
        let accountAddress = new AccountAddress();
        accountAddress.OnChangeLegalAddressModalHide();
    });