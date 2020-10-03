class EmployeePosition {
    /**
     * ��������� ������ �� ������ ��� �������� ��������� �� ������ ����������
     * ��� ������� ������ ������������ ��
     * @param {*} event 
     */
    Remove(event) {
        return new Promise((resolve, reject) => {
            let request = new AjaxRequests();
            let removeEmpPositionUrl = $(event.currentTarget).find(".remove-item-url a").attr("href");

            request.CommonDeleteRequest(removeEmpPositionUrl)
                .fail(response => {
                    Utils.CommonErrosHandling(response["responseJSON"], ["RemoveEmployeePosition"]);
                    reject();
                })
                .done(() => location.reload());
        })
    }
}

// ������ ���������� ����������
$("#employeePositionsList")
    .off("click", ".remove-item-btn").on("click", ".remove-item-btn", event => {
        event.preventDefault();
        let employeePosition = new EmployeePosition();
        employeePosition.Remove(event);
    });