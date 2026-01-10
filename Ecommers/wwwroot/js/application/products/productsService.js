import { ajaxPost } from "../../domain/utils/api.helper.js";


// ===============================
// CONFIGURACIÓN DE ENDPOINTS
// ===============================
const PRODUCTOS_API = {
    CambiarEstado: {
        url: "/Gestion/Categorias/CambiarEstado",
        errorMessage: "Error al cambiar el estado del categoría"
    }
};

// ===============================
// FUNCIONES ESPECÍFICAS
// ===============================
function CambiarEstado(data) {
    return ajaxPost(PRODUCTOS_API.CambiarEstado, data);
}

// ===============================
// EXPORTS
// ===============================
export {
    CambiarEstado
};
