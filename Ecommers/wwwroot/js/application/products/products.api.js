import { ajaxPost } from "../../domain/utils/api.helper.js";

// ===============================
// CONFIGURACIÓN DE ENDPOINTS
// ===============================
const PRODUCTOS_API = {
    CambiarEstadoProducto: {
        url: "/Gestion/Products/EstadoProducto",
        errorMessage: "Error al cambiar el estado del producto"
    },

    CambiarEstadoVariante: {
        url: "/Gestion/Products/EstadoVariante",
        errorMessage: "Error al cambiar el estado de la variante"
    },

    SubirImagenProducto: {
        url: "/Gestion/Products/SubirImagenProducto",
        errorMessage: "Error al subir imagen del producto"
    },

    SubirImagenVariante: {
        url: "/Gestion/Products/SubirImagenVariante",
        errorMessage: "Error al subir imagen de la variante"
    },

    EliminarImagenProducto: {
        url: "/Gestion/Products/EliminarImagenProducto",
        errorMessage: "Error al eliminar imagen del producto"
    },

    EliminarImagenVariante: {
        url: "/Gestion/Products/EliminarImagenVariante",
        errorMessage: "Error al eliminar imagen de la variante"
    },

    EliminarVariante: {
        url: "/Gestion/Products/EliminarVariante",
        errorMessage: "Error al eliminar la variante"
    },

    ObtenerProductVariantView: {
        url: "/Gestion/Products/ObtenerProductVariantView",
        errorMessage: "Error al obtener la variante de productos"
    }
};
// ===============================
// FUNCIONES ESPECÍFICAS
// ===============================

/**
 * Cambia el estado de un producto
 * @param {Object} data - Datos del producto
 * @returns {Promise}
 */
function CambiarEstado(data) {
    return ajaxPost(PRODUCTOS_API.CambiarEstado, data);
}

/**
 * Obtiene la vista parcial de una variante de producto
 * @param {Object} data - { index: number }
 * @returns {Promise<{success: boolean, data: string, index: number}>}
 */
async function ObtenerProductVariantView(data) {
    try {
        // Validar que el índice exista
        if (typeof data.index !== 'number' || data.index < 0) {
            throw new Error('Índice de variante inválido');
        }

        const response = await ajaxPost(PRODUCTOS_API.ObtenerProductVariantView, data);

        // Validar la respuesta
        if (!response) {
            throw new Error('Respuesta vacía del servidor');
        }

        // Si la respuesta no tiene la estructura esperada
        if (!response.hasOwnProperty('success')) {
            console.warn('Respuesta sin estructura esperada:', response);
            return {
                success: false,
                message: 'Respuesta del servidor en formato inválido'
            };
        }

        // Si hay datos HTML, asegurarse que no esté vacío
        if (response.success && response.data) {
            const trimmedData = response.data.trim();
            if (!trimmedData) {
                return {
                    success: false,
                    message: 'Vista parcial vacía'
                };
            }
            response.data = trimmedData;
        }

        return response;
    } catch (error) {
        console.error('Error en ObtenerProductVariantView:', error);
        return {
            success: false,
            message: error.message || PRODUCTOS_API.ObtenerProductVariantView.errorMessage,
            error: error
        };
    }
}

function CambiarEstadoProducto(data) {
    return ajaxPost(PRODUCTOS_API.CambiarEstadoProducto, data);
}
    
function CambiarEstadoVariante(data) {
    return ajaxPost(PRODUCTOS_API.CambiarEstadoVariante, data);
}

function SubirImagenProducto(data) {
    return ajaxPost(PRODUCTOS_API.SubirImagenProducto, data);
}

function SubirImagenVariante(data) {
    return ajaxPost(PRODUCTOS_API.SubirImagenVariante, data);
}

function EliminarImagenProducto(data) {
    return ajaxPost(PRODUCTOS_API.EliminarImagenProducto, data);
}

function EliminarImagenVariante(data) {
    return ajaxPost(PRODUCTOS_API.EliminarImagenVariante, data);
}
function EliminarVariante(data) {
    return ajaxPost(PRODUCTOS_API.EliminarVariante, data);
}
// ===============================
// EXPORTS
// ===============================
export {
    CambiarEstadoProducto,
    CambiarEstadoVariante,
    SubirImagenProducto,
    SubirImagenVariante,
    EliminarImagenProducto,
    EliminarImagenVariante,
    EliminarVariante,
    ObtenerProductVariantView
};