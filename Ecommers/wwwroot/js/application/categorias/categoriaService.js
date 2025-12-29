const URL_CATEGORIA = {
    GetByName: "/Gestion/Categorias/GetByNameAsync",
};

function GetByName(data) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: URL_CATEGORIA.GetByName,
            type: 'POST',
            data: data,
            success: response => resolve(response),
            error: xhr =>
                reject(xhr.responseJSON?.message || "Error al obtener la categoría")
        });
    });
}

export {
    GetByName
};
