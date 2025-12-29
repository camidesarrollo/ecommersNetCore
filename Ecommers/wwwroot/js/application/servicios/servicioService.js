const URL_SERVICIOS = {
    GetByName: "/Gestion/Servicios/GetByNameAsync",
};

function GetByName(data) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: URL_SERVICIOS.GetByName,
            type: 'POST',
            data: data,
            success: response => resolve(response),
            error: xhr =>
                reject(xhr.responseJSON?.message || "Error al obtener el servicio")
        });
    });
}

export {
    GetByName
};
