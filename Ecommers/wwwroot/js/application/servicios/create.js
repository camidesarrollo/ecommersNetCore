/* js/application/categorias/create.js */

import { serviciosCreateSchema } from "../../bundle/schema/servicios.create.shema.js";
import {
    initBlurValidation,

} from "../../domain/utils/ui/input.validation.js";

import { castFormDataBySchema } from "../../bundle/schema/zod-generic.js"

import { ImagePreviewHandler } from "../../domain/utils/image-handler.js";
import {
    setupLivePreview
} from "./generic.js";

/* =====================================================
   INIT
===================================================== */
document.addEventListener("DOMContentLoaded", () => {
    setupLivePreview();

    // Preview de imagen
    ImagePreviewHandler.init();
});

/* =====================================================
   FORM
===================================================== */
const form = document.getElementById("formServicios");
if (!form) {
    console.error("No se encontró el formulario con id: formServicios");
    throw new Error("Formulario no encontrado");
}

/* =====================================================
   VALIDACIÓN BLUR
===================================================== */
console.log("Schema cargado:", serviciosCreateSchema);

initBlurValidation({
    form,
    schema: serviciosCreateSchema
});

/* =====================================================
   SUBMIT
===================================================== */
form.addEventListener("submit", async function (e) {
    e.preventDefault();

    if (typeof showSpinner === "function") {
        showSpinner("creating");
    }

    const formData = new FormData(form);

    // 🎯 Cast automático según schema
    const data = castFormDataBySchema(formData, serviciosCreateSchema);

    try {
        const validated = await serviciosCreateSchema.parseAsync(data);

        console.log("Datos validados correctamente:", validated);

        // Envío real al backend
        form.submit();

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

        if (typeof hideSpinner === "function") {
            hideSpinner();
        }
    }
});
