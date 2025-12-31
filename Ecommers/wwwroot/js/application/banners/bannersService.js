import { ajaxPost } from "../../domain/utils/api.helper.js";


// ===============================
// CONFIGURACIÓN DE ENDPOINTS
// ===============================
const BANNERS_API = {
    GetByName: {
        url: "/Gestion/Banners/GetByNameAsync",
        errorMessage: "Error al obtener el banner"
    },
    CambiarEstado: {
        url: "/Gestion/Banners/CambiarEstado",
        errorMessage: "Error al cambiar el estado del banner"
    }
};

// ===============================
// FUNCIONES ESPECÍFICAS
// ===============================
function GetByName(data) {
    return ajaxPost(BANNERS_API.GetByName, data);
}

function CambiarEstado(data) {
    return ajaxPost(BANNERS_API.CambiarEstado, data);
}

// ===============================
// EXPORTS
// ===============================
export {
    GetByName,
    CambiarEstado
};
