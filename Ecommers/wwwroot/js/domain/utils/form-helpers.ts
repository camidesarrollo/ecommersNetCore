import { castFormDataBySchema } from "../schema/zod-generic";
import Swal from "sweetalert2";
import { showSpinner, hideSpinner } from "../components/spinner"; // 👈 IMPORTANTE

/**
 * @typedef {"creating"|"editing"|"deleting"|"uploading"|"processing"|"saving"|"loading"} SpinnerType
 */

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

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        // Limpiar errores previos
        form.querySelectorAll(".is-invalid").forEach(el =>
            el.classList.remove("is-invalid")
        );

        showSpinner(spinnerAction);

        const formData = new FormData(form);
        const data = castFormDataBySchema(formData, castSchema);

        try {
            const validated = await schema.parseAsync(data);

            console.log("✅ Datos validados:", validated);

            if (typeof onSuccess === "function") {
                await onSuccess(validated);
            } else {
                form.submit();
            }

        } catch (err) {
            if (err?.errors) {
                err.errors.forEach(error => {
                    const fieldName = error.path[0];
                    const input = form.querySelector(`[name="${fieldName}"]`);
                    if (input) {
                        showError(input, error.message);
                    }
                });
            }

            console.warn("❌ Errores de validación:", err);

            onError?.(err);

        } finally {
            hideSpinner();
        }
    });
}
