export default class DefaultErrorPR {
    RaiserError(title, errorText) {
        Swal.fire({
            icon: 'error',
            title: title,
            html: errorText
          });
    }

    RenderError(errorMessages) {
        let errorHtml = "<div class='popup-errors-container'>";
        for (let fieldName in errorMessages) {
            errorHtml += "<p class='error-label-md'>" + errorMessages[fieldName] + "</p>";
        }
        return errorHtml + "</div>";
    }
}