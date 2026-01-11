import { ajaxPost } from "../../domain/utils/api.helper.js";


// ===============================
// CONFIGURACIÓN DE ENDPOINTS
// ===============================
const PRODUCTOS_API = {
    CambiarEstado: {
        url: "/Gestion/Products/CambiarEstado",
        errorMessage: "Error al cambiar el estado del producto"
    },
    ObtenerProductVariantView: {
        url: "/Gestion/Products/ObtenerProductVariantView",
        errorMessage: "Error al obtener la variante de productos"
    },
};

// ===============================
// FUNCIONES ESPECÍFICAS
// ===============================
function CambiarEstado(data) {
    return ajaxPost(PRODUCTOS_API.CambiarEstado, data);
}

function ObtenerProductVariantView(data) {
    return ajaxPost(PRODUCTOS_API.ObtenerProductVariantView, data);
}


// ===============================
// EXPORTS
// ===============================
export {
    CambiarEstado,
    ObtenerProductVariantView
};
