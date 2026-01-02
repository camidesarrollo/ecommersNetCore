import { castFormDataBySchema } from "../schema/zod-generic";
import Swal from "sweetalert2";
import { showSpinner, hideSpinner, type SpinnerType } from "../components/spinner";

interface HandleZodFormSubmitParams {
    form: HTMLFormElement;
    schema: any;
    castSchema: any;
    spinnerAction?: SpinnerType;
    onSuccess?: (validated: any) => void | Promise<void>;
    onError?: (error: any) => void;
}

export function handleZodFormSubmit({
    form,
    schema,
    castSchema,
    spinnerAction = "creating",
    onSuccess,
    onError
}: HandleZodFormSubmitParams): void {
    if (!form || !schema || !castSchema) {
        console.error("Faltan parámetros obligatorios");
        return;
    }

    form.addEventListener("submit", async (e: Event) => {
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

        } catch (err: any) {
            if (err?.errors) {
                err.errors.forEach((error: any) => {
                    const fieldName = error.path[0];
                    const input = form.querySelector(`[name="${fieldName}"]`);
                    if (input) {
                        showError(input as HTMLElement, error.message);
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

function showError(element: HTMLElement, message: string): void {
    element.classList.add("is-invalid");
    const feedback = element.nextElementSibling;
    if (feedback && feedback.classList.contains("invalid-feedback")) {
        feedback.textContent = message;
    }
}