import { ajaxPost } from "../../domain/utils/api.helper.js";


// ===============================
// CONFIGURACI�N DE ENDPOINTS
// ===============================
const BANNERS_API = {
    GetByName: {
        url: "/Configuracion/Banners/GetByNameAsync",
        errorMessage: "Error al obtener el banner"
    },
    CambiarEstado: {
        url: "/Configuracion/Banners/CambiarEstado",
        errorMessage: "Error al cambiar el estado del banner"
    }
};

// ===============================
// FUNCIONES ESPEC�FICAS
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
