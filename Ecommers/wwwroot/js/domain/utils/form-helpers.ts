import Swal, { SweetAlertIcon } from "sweetalert2";
import { showSpinner, hideSpinner, type SpinnerType } from "../components/spinner";
import { castFormDataBySchema } from "../schema/zod-generic";
import { ZodError } from "zod";

interface HandleConfirmFormSubmitParams {
    form: HTMLFormElement;
    title?: string;
    html?: string;
    text?: string;
    icon?: 'warning' | 'error' | 'success' | 'info' | 'question';
    confirmText?: string;
    cancelText?: string;
    spinnerAction?: SpinnerType;
    onConfirm?: () => void | Promise<void>;
    onCancel?: () => void;
}

/**
 * Maneja el envío de un formulario con confirmación SweetAlert2
 * Ideal para acciones destructivas como eliminación
 */
export function handleConfirmFormSubmit({
    form,
    title = "¿Estás seguro?",
    html,
    text,
    icon = "warning",
    confirmText = "Sí, continuar",
    cancelText = "Cancelar",
    spinnerAction = "deleting",
    onConfirm,
    onCancel
}: HandleConfirmFormSubmitParams): void {
    if (!form) {
        console.error("El formulario es obligatorio");
        return;
    }

    form.addEventListener("submit", async (e: Event) => {
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
                } else {
                    // Enviar formulario normalmente
                    form.submit();
                }
            } else if (result.isDismissed) {
                // Usuario canceló
                if (typeof onCancel === "function") {
                    onCancel();
                }
            }
        } catch (error) {
            console.error("Error en confirmación:", error);
            hideSpinner();
        }
    });
}

// =====================================================
// VERSIÓN SIMPLIFICADA PARA ELIMINACIÓN
// =====================================================

interface HandleDeleteFormParams {
    form: HTMLFormElement;
    itemName: string;
    itemType?: string;
    onConfirm?: () => void | Promise<void>;
}

/**
 * Versión especializada para eliminación de productos/entidades
 */
export function handleDeleteForm({
    form,
    itemName,
    itemType = "elemento",
    onConfirm
}: HandleDeleteFormParams): void {
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

interface HandleZodFormSubmitParams<T = any> {
    form: HTMLFormElement;
    schema: any;
    castSchema: any;
    spinnerAction?: SpinnerType;
    onSuccess?: (data: T) => void | Promise<void>;
    onError?: (error: unknown) => void;
}

export function handleZodFormSubmit<T = any>({
    form,
    schema,
    castSchema,
    spinnerAction = "creating",
    onSuccess,
    onError
}: HandleZodFormSubmitParams<T>): void {

    if (!form || !schema || !castSchema) {
        console.error("❌ Faltan parámetros obligatorios en handleZodFormSubmit");
        return;
    }

    form.addEventListener("submit", async (e: Event) => {
        e.preventDefault();
        e.stopPropagation();

        // 🔹 Limpiar errores previos
        form.querySelectorAll(".is-invalid").forEach(el => {
            el.classList.remove("is-invalid");
        });

        showSpinner(spinnerAction);

        try {
            const formData = new FormData(form);
            const data = castFormDataBySchema(formData, castSchema);

            const validated = await schema.parseAsync(data);

            console.log("✅ Datos validados:", validated);

            if (typeof onSuccess === "function") {
                await onSuccess(validated);
            } else {
                form.submit();
            }

        } catch (err) {

            // 🔴 Errores de Zod
            if (err instanceof ZodError) {
                console.log(err);
                // err.errors.forEach(error => {
                //     const fieldName = error.path[0];
                //     if (!fieldName) return;

                //     const input = form.querySelector<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>(
                //         `[name="${fieldName}"]`
                //     );

                //     if (input) {
                //         showError(input, error.message);
                //     }
                // });

                // console.warn("❌ Errores de validación Zod:", err.errors);
            } else {
                console.error("❌ Error inesperado:", err);
            }

            if (typeof onError === "function") {
                onError(err);
            }

        } finally {
            hideSpinner();
        }
    });
}

export function handleConfirmAction({
  action,
  title = "¿Estás seguro?",
  html,
  text,
  icon = "warning" as SweetAlertIcon,
  confirmText = "Sí, continuar",
  cancelText = "Cancelar",
  spinnerAction = null,
  onConfirm,
  onCancel
}: {
  action: (event?: Event) => void | Promise<void>;
  title?: string;
  html?: string;
  text?: string;
  icon?: SweetAlertIcon;
  confirmText?: string;
  cancelText?: string;
  spinnerAction?: SpinnerType | null; // <-- Cambiado aquí
  onConfirm?: () => void;
  onCancel?: () => void;
}) {
  return async function (event?: Event) {
    if (event) {
      event.preventDefault();
      event.stopPropagation();
    }

    try {
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
        if (spinnerAction) {
          showSpinner(spinnerAction);
        }

        await action(event);

        onConfirm?.();
      } else {
        onCancel?.();
      }
    } catch (error) {
      console.error("Error en confirmación:", error);
      hideSpinner();
    }
  };
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