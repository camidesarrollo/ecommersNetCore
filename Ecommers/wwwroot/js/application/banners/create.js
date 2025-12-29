/* js/application/banners/create.js */

import Shepherd from 'shepherd.js';
import { bannersCreateSchema } from "../../bundle/schema/banners.create.shema.js";
import {
    initBlurValidation,

} from "../../domain/utils/ui/input.validation.js";

import { castFormDataBySchema } from "../../bundle/schema/zod-generic.js"

import { ImagePreviewHandler } from "../../domain/utils/image-handler.js";
import {
    setupSlugAutoGeneration,
    setupGradientSelector,
    setupLivePreview
} from "./generic.js";

/* =====================================================
   INIT
===================================================== */
document.addEventListener("DOMContentLoaded", () => {
    // UI genérica de categorías
    setupSlugAutoGeneration();
    setupGradientSelector();
    setupLivePreview();

    // Preview de imagen
    ImagePreviewHandler.init();
});

/* =====================================================
   FORM
===================================================== */
const form = document.getElementById("formBanners");
if (!form) {
    console.error("No se encontró el formulario con id: formBanners");
    throw new Error("Formulario no encontrado");
}

/* =====================================================
   VALIDACIÓN BLUR
===================================================== */
console.log("Schema cargado:", bannersCreateSchema);

initBlurValidation({
    form,
    schema: bannersCreateSchema
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
    const data = castFormDataBySchema(formData, bannersCreateSchema);

    try {
        const validated = await bannersCreateSchema.parseAsync(data);

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
