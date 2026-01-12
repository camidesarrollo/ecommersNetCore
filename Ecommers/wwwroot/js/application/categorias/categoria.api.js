import { ajaxPost } from "../../domain/utils/api.helper.js";


// ===============================
// CONFIGURACIÓN DE ENDPOINTS
// ===============================
const CATEGORIA_API = {
    GetByName: {
        url: "/Gestion/Categorias/GetByNameAsync",
        errorMessage: "Error al obtener la categoría"
    },
    CambiarEstado: {
        url: "/Gestion/Categorias/CambiarEstado",
        errorMessage: "Error al cambiar el estado del categoría"
    }
};

// ===============================
// FUNCIONES ESPECÍFICAS
// ===============================
function GetByName(data) {
    return ajaxPost(CATEGORIA_API.GetByName, data);
}

function CambiarEstado(data) {
    return ajaxPost(CATEGORIA_API.CambiarEstado, data);
}

// ===============================
// EXPORTS
// ===============================
export {
    GetByName,
    CambiarEstado
};
