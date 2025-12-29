
import { configuracionUpdateSchema } from "../../bundle/schema/configuracion.update.shema.js";
import {
    initBlurValidation,
    initRealtimeValidation,
    validateForm,
    setButtonState
} from "../../domain/utils/ui/input.validation.js";
import { castFormDataBySchema } from "../../domain/schema/zod-generic.js";
import ImagePreviewHandler from '../../domain/utils/image-handler.js';

ImagePreviewHandler.init();
// ==============================
// 1. Capturar el formulario
// ==============================
const form = document.getElementById("formConfiguracion");

if (!form) {
    console.error("No se encontró el formulario con id: formConfiguracion");
}

// ==============================
// 2. Validación individual en blur
// ==============================
initBlurValidation({
    form: document.getElementById('formConfiguracion'),
    schema: configuracionUpdateSchema
});


// ==============================
// 3. Validación total al hacer submit
// ==============================
fform.addEventListener("submit", async function (e) {
    showSpinner('editing');
    e.preventDefault();

    const formData = new FormData(form);

    // 🎯 CAST AUTOMÁTICO SEGÚN SCHEMA
    const data = castFormDataBySchema(formData, configuracionUpdateSchema);

    try {
        const validated = await configuracionUpdateSchema.parseAsync(data);
        form.submit();
    } catch (err) {
        if (err.errors) {
            err.errors.forEach(e => {
                const input = form.querySelector(`[name="${e.path[0]}"]`);
                if (input) showError(input, e.message);
            });
        }

        console.warn("Errores de validación:", err);
        hideSpinner();
    }
});


// Sincronizar color picker con input de texto
const colorInput = document.querySelector('input[type="color"]');
const colorText = document.querySelectorAll('input[name="ColorTemaNavegador"]')[1];

if (colorInput && colorText) {
    colorInput.addEventListener('input', function () {
        colorText.value = this.value;
    });

    colorText.addEventListener('input', function () {
        if (/^#[0-9A-F]{6}$/i.test(this.value)) {
            colorInput.value = this.value;
        }
    });
}