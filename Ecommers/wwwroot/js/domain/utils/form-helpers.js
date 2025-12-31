import { castFormDataBySchema } from "../schema/zod-generic.js";

export function handleZodFormSubmit({
    form,
    schema,
    castSchema,
    spinnerAction = "creating",
    onSuccess,
    onError
}) {
    if (!form || !schema || !castSchema) {
        console.error("Faltan parámetros obligatorios");
        return;
    }

    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        // Limpia errores previos
        form.querySelectorAll(".is-invalid").forEach(el => {
            el.classList.remove("is-invalid");
        });

        if (typeof showSpinner === "function") {
            showSpinner(spinnerAction);
        }

        const formData = new FormData(form);
        const data = castFormDataBySchema(formData, castSchema);

        try {
            const validated = await schema.parseAsync(data);

            console.log("Datos validados correctamente:", validated);

            if (typeof onSuccess === "function") {
                onSuccess(validated);
            } else {
                form.submit();
            }

        } catch (err) {
            if (err.errors) {
                err.errors.forEach(error => {
                    const fieldName = error.path[0];
                    const input = form.querySelector(`[name="${fieldName}"]`);
                    if (input) {
                        showError(input, error.message);
                    }
                });
            }

            console.warn("Errores de validación:", err);

            if (typeof onError === "function") {
                onError(err);
            }

            if (typeof hideSpinner === "function") {
                hideSpinner();
            }
        }
    });
}



export function handleConfirmFormSubmit({
    form,
    title = "¿Estás seguro?",
    html,
    icon = "warning",
    confirmText = "Confirmar",
    cancelText = "Cancelar",
    spinnerAction = "processing",
    onConfirm
}) {
    if (!form) {
        console.error("Formulario no encontrado");
        return;
    }

    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        const result = await Swal.fire({
            title,
            html,
            icon,
            showCancelButton: true,
            confirmButtonText: confirmText,
            cancelButtonText: cancelText
        });

        if (!result.isConfirmed) return;

        // Mostrar spinner
        if (typeof showSpinner === "function") {
            showSpinner(spinnerAction);
        }

        if (typeof onConfirm === "function") {
            onConfirm();
        } else {
            form.submit();
        }
    });
}
