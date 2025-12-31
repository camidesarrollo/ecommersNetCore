export function ajaxPost(endpoint, data = {}) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: endpoint.url,
            type: "POST",
            data,
            success: response => resolve(response),
            error: xhr => {
                reject(
                    xhr.responseJSON?.message ||
                    endpoint.errorMessage ||
                    "Ocurrió un error inesperado"
                );
            }
        });
    });
}
