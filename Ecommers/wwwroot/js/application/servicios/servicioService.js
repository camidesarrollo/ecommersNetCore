import { ajaxPost } from "../../domain/utils/api.helper.js";


// ===============================
// CONFIGURACIÓN DE ENDPOINTS
// ===============================
const URL_SERVICIOS = {
    GetByName: {
        url: "/Gestion/Servicios/GetByNameAsync",
        errorMessage: "Error al obtener el servicio"
    },
    CambiarEstado: {
        url: "/Gestion/Servicios/CambiarEstado",
        errorMessage: "Error al cambiar el estado del servicio"
    }
};

// ===============================
// FUNCIONES ESPECÍFICAS
// ===============================
function GetByName(data) {
    return ajaxPost(URL_SERVICIOS.GetByName, data);
}

function CambiarEstado(data) {
    return ajaxPost(URL_SERVICIOS.CambiarEstado, data);
}

// ===============================
// EXPORTS
// ===============================
export {
    GetByName,
    CambiarEstado
};
