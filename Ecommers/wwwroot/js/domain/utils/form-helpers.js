import Swal from "sweetalert2";
import { showSpinner, hideSpinner } from "../components/spinner";
/**
 * Maneja el envío de un formulario con confirmación SweetAlert2
 * Ideal para acciones destructivas como eliminación
 */
export function handleConfirmFormSubmit({ form, title = "¿Estás seguro?", html, text, icon = "warning", confirmText = "Sí, continuar", cancelText = "Cancelar", spinnerAction = "deleting", onConfirm, onCancel }) {
    if (!form) {
        console.error("El formulario es obligatorio");
        return;
    }
    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        e.stopPropagation();
        try {
            // Mostrar confirmación
            const result = await Swal.fire({
                title,
                html,
                text,
                icon,
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: confirmText,
                cancelButtonText: cancelText,
                reverseButtons: true,
                focusCancel: true
            });
            if (result.isConfirmed) {
                // Usuario confirmó
                showSpinner(spinnerAction);
                if (typeof onConfirm === "function") {
                    // Si hay callback personalizado
                    await onConfirm();
                }
                else {
                    // Enviar formulario normalmente
                    form.submit();
                }
            }
            else if (result.isDismissed) {
                // Usuario canceló
                if (typeof onCancel === "function") {
                    onCancel();
                }
            }
        }
        catch (error) {
            console.error("Error en confirmación:", error);
            hideSpinner();
        }
    });
}
/**
 * Versión especializada para eliminación de productos/entidades
 */
export function handleDeleteForm({ form, itemName, itemType = "elemento", onConfirm }) {
    handleConfirmFormSubmit({
        form,
        title: `¿Eliminar ${itemType}?`,
        html: `Estás a punto de eliminar <b>${itemName}</b>.<br>Esta acción no se puede deshacer.`,
        icon: "warning",
        confirmText: "Sí, eliminar",
        cancelText: "Cancelar",
        spinnerAction: "deleting",
        onConfirm
    });
}
// =====================================================
// USO EN TU CÓDIGO
// =====================================================
/*
// OPCIÓN 1: Uso básico (envía el form automáticamente)
import { handleConfirmFormSubmit } from "../../bundle/utils/form-helpers.js";

const form = document.getElementById("formProducto") as HTMLFormElement;

handleConfirmFormSubmit({
    form,
    title: "¿Eliminar el producto?",
    html: `Estás a punto de eliminar <b>${$("#nombre").val()}</b>.`,
    icon: "warning",
    confirmText: "Sí, eliminar",
    cancelText: "Cancelar",
    spinnerAction: "deleting"
});

// =====================================================
// OPCIÓN 2: Con callback personalizado
// =====================================================
import { handleConfirmFormSubmit } from "../../bundle/utils/form-helpers.js";

const form = document.getElementById("formProducto") as HTMLFormElement;

handleConfirmFormSubmit({
    form,
    title: "¿Eliminar el producto?",
    html: `Estás a punto de eliminar <b>${$("#nombre").val()}</b>.`,
    icon: "warning",
    confirmText: "Sí, eliminar",
    cancelText: "Cancelar",
    spinnerAction: "deleting",
    onConfirm: async () => {
        // Lógica personalizada antes de enviar
        console.log("Eliminando producto...");
        
        // Puedes hacer una petición AJAX aquí
        // const response = await fetch('/api/delete', { ... });
        
        // O enviar el formulario
        form.submit();
    },
    onCancel: () => {
        console.log("Eliminación cancelada");
    }
});

// =====================================================
// OPCIÓN 3: Versión simplificada para eliminación
// =====================================================
import { handleDeleteForm } from "../../bundle/utils/form-helpers.js";

const form = document.getElementById("formProducto") as HTMLFormElement;
const nombreProducto = $("#nombre").val() as string;

handleDeleteForm({
    form,
    itemName: nombreProducto,
    itemType: "producto"
});

// =====================================================
// OPCIÓN 4: Con validación Zod + Confirmación
// =====================================================
import { handleZodFormSubmit } from "../../bundle/utils/form-helpers.js";
import { handleConfirmFormSubmit } from "../../bundle/utils/form-helpers.js";
import { productSchema, productCastSchema } from "../schema/product-schema";

const form = document.getElementById("formProducto") as HTMLFormElement;

// Primero configurar confirmación
let isConfirmed = false;

handleConfirmFormSubmit({
    form,
    title: "¿Eliminar el producto?",
    html: `Estás a punto de eliminar <b>${$("#nombre").val()}</b>.`,
    icon: "warning",
    confirmText: "Sí, eliminar",
    cancelText: "Cancelar",
    spinnerAction: "deleting",
    onConfirm: () => {
        isConfirmed = true;
        form.dispatchEvent(new Event('submit', { cancelable: true }));
    }
});

// Luego validar con Zod solo si fue confirmado
handleZodFormSubmit({
    form,
    schema: productSchema,
    castSchema: productCastSchema,
    spinnerAction: "deleting",
    onSuccess: async (validated) => {
        if (isConfirmed) {
            form.submit();
        }
    }
});
*/ 
