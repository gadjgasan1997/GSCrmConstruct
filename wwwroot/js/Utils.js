class Utils {
    /**
     * ����� ������������ ������ ������
     * @param {*} errors ������ � ��������
     * @param {*} errorsTypeCodes ������ � ������ ����� ������
     */
    static CommonErrosHandling(errorsArray, errorsTypeCodes) {
        !Utils.IsNullOrEmpty(errorsTypeCodes) && errorsTypeCodes.map(errorsTypeCode => {
            let typeErrors = ErrorsManager.GetError(errorsTypeCode);
            let errorAlertsArray = [];
            if (typeErrors != undefined) {
                for(let error in errorsArray) {
                    let errorText = Array.isArray(errorsArray[error]) ? errorsArray[error][0] : errorsArray[error];

                    // ��������� ������ ���� �������� � ��������, ����� ������ ������� ���������� �������� ������� ������
                    let errorHandlers = typeErrors.filter(typeError => {
                        let errorCodes = typeError[0];
                        if (errorCodes.indexOf(error) != -1) {
                            return typeError;
                        }
                    });

                    // ���� ����������� �� �������, ������� ���������� ���������� ������
                    if (errorHandlers.length == 0) {
                        switch(error) {
                            case "UnhandledException":
                                errorAlertsArray.push(MessageManager.Invoke("CommonError", { "error": Localization.GetString("unhandledException") }));
                                break;
                            case "RecordNotFound":
                                errorAlertsArray.push(MessageManager.Invoke("CommonError", { "error": Localization.GetString("recordNotFound") }));
                                break;
                            default:
                                break;
                        }
                    }
        
                    // ��������� ������
                    errorHandlers.map(errorHandler => {
                        let errorSettings = errorHandler[1];
                        let errorType = errorSettings["type"];
                        switch(errorType) {
                            // ����� ������ ������ ����������� � �������
                            case "attach":
                                let elementSelectors = errorSettings["elements"];
                                if (elementSelectors != undefined) {
                                    elementSelectors.map(elementSelector => {
                                        $(elementSelector).each((index, item) => {
                                            $(item).removeClass("d-none").append("<li>" + errorText + "</li>");
                                        });
                                    });
                                }
                                break;

                            // ����� ������ ����������� �����
                            case "swal":
                                let messageName = errorSettings["messageName"];
                                if (messageName == undefined) {
                                    messageName = "CommonError";
                                }
                                errorAlertsArray.push(MessageManager.Invoke(messageName, { "error": errorText }));
                                break;
                        }
                    });
                }
            }

            if (errorAlertsArray.length > 0) {
                Swal.queue(errorAlertsArray);
            }
        });
    }

    /** ������� ������ */
    static ClearErrors() {
        $('.under-field-error').empty();
    }

    /**
     * ����� ���������, �������� �� ������ ������
     * @param {*} obj 
     */
    static IsNullOrEmpty(obj) {
        if (obj == undefined) return true;
        if (obj == null) return true;
        if (obj == "") return true;
        return false;
    }
}