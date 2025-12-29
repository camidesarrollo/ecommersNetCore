const URL_BANNERS = {
    GetByName: "/Gestion/Banners/GetByNameAsync",
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

export {
    GetByName
};
