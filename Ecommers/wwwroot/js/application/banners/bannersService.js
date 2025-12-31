const URL_BANNERS = {
    GetByName: "/Gestion/Banners/GetByNameAsync",
    CambiarEstado: "/Gestion/Banners/CambiarEstado",
};

function GetByName(data) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: URL_BANNERS.GetByName,
            type: 'POST',
            data: data,
            success: response => resolve(response),
            error: xhr =>
                reject(xhr.responseJSON?.message || "Error al obtener el banners")
        });
    });
}

function CambiarEstado(data) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: URL_BANNERS.CambiarEstado,
            type: 'POST',
            data: data,
            success: response => resolve(response),
            error: xhr =>
                reject(xhr.responseJSON?.message || "Error al cambiar el estado")
        });
    });
}

export {
    GetByName,
    CambiarEstado
};
