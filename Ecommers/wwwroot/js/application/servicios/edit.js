import { serviciosUpdateSchema } from "../../bundle/schema/servicios.create.shema.js";
import {
    initBlurValidation,

} from "../../domain/utils/ui/input.validation.js";


import {
    handleZodFormSubmit
} from "../../domain/utils/form-helpers.js";

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
console.log("Schema cargado:", serviciosUpdateSchema);

initBlurValidation({
    form,
    schema: serviciosUpdateSchema
});

/* =====================================================
   SUBMIT
===================================================== */
handleZodFormSubmit({
    form,
    schema: serviciosUpdateSchema,
    castSchema: serviciosUpdateSchema,
    spinnerAction: "editig"
});