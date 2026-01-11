import { Notyf, INotyfOptions, NotyfNotification } from 'notyf';

// ===============================
// CONFIGURACIÓN PERSONALIZADA
// ===============================
const notyfConfig: Partial<INotyfOptions> = {
    duration: 3500,
    position: {
        x: 'right',
        y: 'top',
    },
    ripple: true,
    dismissible: true,
    types: [
        {
            type: 'success',
            background: '#10b981', // green-600
            icon: {
                className: 'fas fa-check-circle',
                tagName: 'i',
                color: 'white'
            }
        },
        {
            type: 'error',
            background: '#ef4444', // red-500
            icon: {
                className: 'fas fa-times-circle',
                tagName: 'i',
                color: 'white'
            },
            duration: 4000 // Los errores duran un poco más
        },
        {
            type: 'warning',
            background: '#f59e0b', // amber-500
            icon: {
                className: 'fas fa-exclamation-triangle',
                tagName: 'i',
                color: 'white'
            }
        },
        {
            type: 'info',
            background: '#3b82f6', // blue-500
            icon: {
                className: 'fas fa-info-circle',
                tagName: 'i',
                color: 'white'
            }
        }
    ]
};

// Crear instancia de Notyf
const notyf = new Notyf(notyfConfig);

// ===============================
// FUNCIONES PÚBLICAS
// ===============================

/**
 * Muestra una notificación de éxito
 * @param message - Mensaje a mostrar
 * @param duration - Duración personalizada (opcional)
 */
export const showSuccess = (message: string, duration?: number): NotyfNotification => {
    return notyf.success({
        message,
        duration: duration || notyfConfig.duration
    });
};

/**
 * Muestra una notificación de error
 * @param message - Mensaje a mostrar
 * @param duration - Duración personalizada (opcional)
 */
export const showError = (message: string, duration?: number): NotyfNotification => {
    return notyf.error({
        message,
        duration: duration || 4000
    });
};

/**
 * Muestra una notificación de advertencia
 * @param message - Mensaje a mostrar
 * @param duration - Duración personalizada (opcional)
 */
export const showWarning = (message: string, duration?: number): NotyfNotification => {
    return notyf.open({
        type: 'warning',
        message,
        duration: duration || notyfConfig.duration
    });
};

/**
 * Muestra una notificación informativa
 * @param message - Mensaje a mostrar
 * @param duration - Duración personalizada (opcional)
 */
export const showInfo = (message: string, duration?: number): NotyfNotification => {
    return notyf.open({
        type: 'info',
        message,
        duration: duration || notyfConfig.duration
    });
};

/**
 * Muestra una notificación personalizada
 * @param type - Tipo de notificación
 * @param message - Mensaje a mostrar
 * @param options - Opciones adicionales
 */
export const showCustom = (
    type: 'success' | 'error' | 'warning' | 'info',
    message: string,
    options?: {
        duration?: number;
        dismissible?: boolean;
        ripple?: boolean;
    }
): NotyfNotification => {
    return notyf.open({
        type,
        message,
        duration: options?.duration || notyfConfig.duration,
        dismissible: options?.dismissible ?? true,
        ripple: options?.ripple ?? true
    });
};

/**
 * Cierra todas las notificaciones activas
 */
export const dismissAll = (): void => {
    notyf.dismissAll();
};

/**
 * Cierra una notificación específica
 * @param notification - Notificación a cerrar
 */
export const dismiss = (notification: NotyfNotification): void => {
    notyf.dismiss(notification);
};

// ===============================
// UTILIDADES ADICIONALES
// ===============================

/**
 * Muestra una notificación de guardado exitoso
 */
export const showSaveSuccess = (entityName: string = 'Registro'): NotyfNotification => {
    return showSuccess(`${entityName} guardado correctamente`);
};

/**
 * Muestra una notificación de eliminación exitosa
 */
export const showDeleteSuccess = (entityName: string = 'Registro'): NotyfNotification => {
    return showSuccess(`${entityName} eliminado correctamente`);
};

/**
 * Muestra una notificación de actualización exitosa
 */
export const showUpdateSuccess = (entityName: string = 'Registro'): NotyfNotification => {
    return showSuccess(`${entityName} actualizado correctamente`);
};

/**
 * Muestra una notificación de error de guardado
 */
export const showSaveError = (entityName: string = 'el registro'): NotyfNotification => {
    return showError(`Error al guardar ${entityName}`);
};

/**
 * Muestra una notificación de error de eliminación
 */
export const showDeleteError = (entityName: string = 'el registro'): NotyfNotification => {
    return showError(`Error al eliminar ${entityName}`);
};

/**
 * Muestra una notificación de error de carga
 */
export const showLoadError = (entityName: string = 'los datos'): NotyfNotification => {
    return showError(`Error al cargar ${entityName}`);
};

/**
 * Muestra una notificación de validación
 */
export const showValidationError = (message: string = 'Por favor, complete todos los campos requeridos'): NotyfNotification => {
    return showWarning(message);
};

/**
 * Muestra una notificación de confirmación requerida
 */
export const showConfirmationRequired = (message: string = 'Esta acción requiere confirmación'): NotyfNotification => {
    return showInfo(message);
};

/**
 * Muestra una notificación de proceso en progreso
 */
export const showProcessing = (message: string = 'Procesando...'): NotyfNotification => {
    return showInfo(message, 0); // Duración infinita hasta que se cierre manualmente
};

// ===============================
// EXPORTS
// ===============================
export { notyf };

// Export default para facilitar la importación
export default {
    success: showSuccess,
    error: showError,
    warning: showWarning,
    info: showInfo,
    custom: showCustom,
    dismissAll,
    dismiss,
    // Utilidades
    saveSuccess: showSaveSuccess,
    deleteSuccess: showDeleteSuccess,
    updateSuccess: showUpdateSuccess,
    saveError: showSaveError,
    deleteError: showDeleteError,
    loadError: showLoadError,
    validationError: showValidationError,
    confirmationRequired: showConfirmationRequired,
    processing: showProcessing,
    // Instancia
    instance: notyf
};