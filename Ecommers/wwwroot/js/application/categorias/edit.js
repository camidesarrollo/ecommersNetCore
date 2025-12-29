/* js\application\categorias\create.js */
import { categoriesUpdateSchema } from "../../bundle/schema/categories.create.shema.js";
import {
    initBlurValidation,
} from "../../domain/utils/ui/input.validation.js";
import { castFormDataBySchema } from "../../domain/utils/zod-generic.js"
import ImagePreviewHandler from '../../domain/utils/image-handler.js';

// Inicializar el manejador de imágenes
ImagePreviewHandler.init();

// ==============================
// 1. Capturar el formulario
// ==============================
const form = document.getElementById("formCategoria");
if (!form) {
    console.error("No se encontró el formulario con id: formCategoria");
    throw new Error("Formulario no encontrado");
}

// ==============================
// 2. Validación individual en blur
// ==============================
console.log("Schema cargado:", categoriesUpdateSchema);
initBlurValidation({
    form,
    schema: categoriesUpdateSchema
});

// ==============================
// 3. Validación total al hacer submit
// ==============================
form.addEventListener("submit", async function (e) {
    e.preventDefault();

    // Mostrar spinner si existe la función
    if (typeof showSpinner === 'function') {
        showSpinner('editing');
    }

    const formData = new FormData(form);

    // 🎯 CAST AUTOMÁTICO SEGÚN SCHEMA
    const data = castFormDataBySchema(formData, categoriesUpdateSchema);

    try {
        // Validar datos con el schema correcto
        const validated = await categoriesUpdateSchema.parseAsync(data);

        console.log("Datos validados correctamente:", validated);

        // Enviar el formulario
        form.submit();

    } catch (err) {
        // Manejar errores de validación de Zod
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

        // Ocultar spinner si existe la función
        if (typeof hideSpinner === 'function') {
            hideSpinner();
        }
    }
});