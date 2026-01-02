/* js/application/categorias/create.js */

import { serviciosCreateSchema } from "../../bundle/schema/servicios.create.shema.js";
import {
    initBlurValidation,

} from "../../domain/utils/ui/input.validation.js";

import { ImagePreviewHandler } from "../../domain/utils/image-handler.js";
import {
    setupLivePreview
} from "./generic.js";
import {
    handleZodFormSubmit
} from "../../bundle/utils/form-helpers.js";

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
handleZodFormSubmit({
    form,
    schema: serviciosCreateSchema,
    castSchema: serviciosCreateSchema,
    spinnerAction: "creating"
});
