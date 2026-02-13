import { CambiarEstadoVariante, EliminarImagenProducto } from './products.api.js';
import { handleConfirmAction } from "../../bundle/utils/form-helpers.js";
import { showSpinner, hideSpinner } from "../../bundle/components/spinner.js";
import { showSuccess, showError, showWarning, showInfo } from '../../bundle/notifications/notyf.config.js';
// ============================
//   ACTIVAR / DESACTIVAR
// ============================
const confirmToggleVariante = (element, id, textActivo) =>
    handleConfirmAction({
        title: `¿Deseas ${textActivo} la variante?`,
        icon: "warning",
        confirmText: `Sí, ${textActivo}`,
        cancelText: "Cancelar",
        spinnerAction: "updating",

        action: async () => {
            const accion = await CambiarEstadoVariante({ VarianteId: id });

            if (!accion.success) {
                hideSpinner();
                showError(accion.message || `Error al ${textActivo} la variante`);
                throw new Error(accion.message || 'Error al cambiar estado de la variante');
            }

            // 🔄 Estado actual desde el DOM
            const currentState = element.dataset.isactive === "true";
            const newState = !currentState;

            // 🧠 Persistir nuevo estado
            element.dataset.isactive = String(newState);

            // ✅ Si es checkbox
            if (element instanceof HTMLInputElement && element.type === "checkbox") {
                element.checked = newState;
            }

            // 🎨 Opcional: clases visuales
            element.classList.toggle("is-active", newState);
            element.classList.toggle("is-inactive", !newState);
        },

        onConfirm: () => {
            hideSpinner();
            if(textActivo.toLowerCase().includes("activar")) {
                showSuccess(`Variante activada correctamente`);
            } else {    
                showSuccess(`Variante desactivada correctamente`);
            }
        },

        onCancel: () => {
            hideSpinner();
        }
    });

document.addEventListener("click", (e) => {
    const btn = e.target.closest(".btn-toggle-variante");
    if (!btn) return;

    const id = btn.dataset.id;
    const isActive = btn.dataset.isactive === "true";
    const textActivo = isActive ? "desactivar" : "activar";

    confirmToggleVariante(btn, id, textActivo)(e);
});




const DeleteImageProducto = (element, id, index) =>
    handleConfirmAction({
          title: "¿Deseas eliminar la imagen?",
        icon: "warning",
        confirmText: "Sí, eliminar",
        cancelText: "Cancelar",
        spinnerAction: "deleting",

        action: async () => {
            const accion = await EliminarImagenProducto({ ImagenId: id });

            if (!accion.success) {
                hideSpinner();
                showError(accion.message || `Error al eliminar la imagen`);
                throw new Error(accion.message || 'Error al eliminar la imagen');
            }
        },

        onConfirm: () => {
            hideSpinner();
            removeImageInput(event,index);
            showSuccess(`Imagen eliminada correctamente`);
        },

        onCancel: () => {
            hideSpinner();
        }
    });
    
window.confirmDeleteImageProducto = function confirmDeleteImageProducto(element, id, index) {
    return DeleteImageProducto(element, id, index)(element);
}

/* =====================================================
   FORM VALIDATION
===================================================== */
document.addEventListener("DOMContentLoaded", () => {

    const form = document.getElementById("formProducts");
    if (!form) {
        console.error("No se encontró el formulario con id: formProducts");
        return;
    }

    form.addEventListener("submit", function (e) {
        e.preventDefault(); // 🚫 Detiene el submit

        let isValid = true;
        let firstInvalidField = null;

        // 🔎 Todos los campos required
        const requiredFields = form.querySelectorAll(
            "input[required]:not([disabled]), select[required], textarea[required]"
        );

        requiredFields.forEach(field => {

            field.classList.remove("border-red-500");

            // Validación base
            if (!field.value || field.value.trim() === "" || field.value === "0") {
                isValid = false;
                field.classList.add("border-red-500");

                if (!firstInvalidField) {
                    firstInvalidField = field;
                }
            }
        });

        // ❌ Si hay errores
        if (!isValid) {
            showWarning("Por favor completa todos los campos obligatorios.");
            firstInvalidField?.focus();
            return;
        }

        // ✅ REMOVER TODOS LOS DISABLED (CRÍTICO)
        const disabledFields = form.querySelectorAll("[disabled]");
        disabledFields.forEach(field => {
            field.removeAttribute("disabled");
        });

        // ✅ Enviar formulario
        form.submit();
    });

});
