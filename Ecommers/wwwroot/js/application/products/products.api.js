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

// ===============================
// EXPORTS
// ===============================
export {
    CambiarEstado,
    ObtenerProductVariantView
};