/* =====================================================
   TOUR GUIADO - CREAR MESTRO DE ATRIBUTOS
   js/application/masterAttributes/create.js (ACTUALIZADO)
===================================================== */

import Shepherd from "../../bundle/vendors_shepherd.js";
import { masterAttributesCreateSchema } from "../../bundle/schema/master.attributes.create.shema.js";
import {
    initBlurValidation,
} from "../../domain/utils/ui/input.validation.js";
import { castFormDataBySchema } from "../../bundle/schema/zod-generic.js"

/* =====================================================
   CONFIGURACIÓN DEL TOUR
===================================================== */
function initMasterAttributesCreateTour() {
    const tour = new Shepherd.Tour({
        defaultStepOptions: {
            classes: 'shepherd-theme-custom',
            scrollTo: { behavior: 'smooth', block: 'center' },
            cancelIcon: { enabled: true },
            modalOverlayOpeningPadding: 8,
            modalOverlayOpeningRadius: 8
        },
        useModalOverlay: true
    });

    // =========================================
    // PASO 1: Bienvenida
    // =========================================
    tour.addStep({
        id: 'welcome',
        title: '¡Bienvenido al Maestro de Atributos!',
        text: `
            <p>👋 Este tour te ayudará a crear atributos dinámicos para tus productos.</p>
            <p>Aprenderás a:</p>
            <ul>
                <li>📝 Definir nombre y slug correctamente</li>
                <li>⚙️ Configurar tipos de datos e inputs</li>
                <li>🧩 Crear atributos filtrables y variantes</li>
                <li>💾 Guardar atributos reutilizables</li>
            </ul>
            <p><strong>Duración:</strong> 2-3 minutos</p>
        `,
        buttons: [
            {
                text: 'Saltar tour',
                classes: 'shepherd-button-secondary',
                action: tour.cancel
            },
            {
                text: 'Comenzar',
                classes: 'shepherd-button-primary',
                action: tour.next
            }
        ]
    });

    // =========================================
    // PASO 2: Breadcrumb
    // =========================================
    tour.addStep({
        id: 'breadcrumb',
        title: 'Navegación',
        text: `
            <p>Este breadcrumb indica dónde te encuentras:</p>
            <ul>
                <li><strong>Inicio</strong> → Dashboard</li>
                <li><strong>Maestro de atributos</strong> → Listado</li>
                <li><strong>Crear</strong> → Formulario actual</li>
            </ul>
        `,
        attachTo: {
            element: 'nav[aria-label="breadcrumb"]',
            on: 'bottom'
        },
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Siguiente', action: tour.next }
        ]
    });

    // =========================================
    // PASO 3: Nombre
    // =========================================
    tour.addStep({
        id: 'name',
        title: 'Nombre del atributo',
        text: `
            <p>Nombre visible del atributo para los usuarios.</p>
            <p><strong>Ejemplos:</strong></p>
            <ul>
                <li>Marca</li>
                <li>Calorías</li>
                <li>Contenido neto</li>
            </ul>
            <p class="text-red-600"><strong>Campo obligatorio</strong></p>
        `,
        attachTo: {
            element: 'input[name="Name"]',
            on: 'right'
        },
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Siguiente', action: tour.next }
        ]
    });

    // =========================================
    // PASO 4: Slug
    // =========================================
    tour.addStep({
        id: 'slug',
        title: 'Slug técnico',
        text: `
            <p>Identificador interno del atributo.</p>
            <p><strong>Buenas prácticas:</strong></p>
            <ul>
                <li>🔡 Minúsculas</li>
                <li>🔗 Sin espacios (usa _)</li>
                <li>🚫 No usar tildes ni ñ</li>
            </ul>
            <p><strong>Ejemplo:</strong> <code>contenido_neto</code></p>
        `,
        attachTo: {
            element: 'input[name="Slug"]',
            on: 'right'
        },
        classes: 'shepherd-theme-warning',
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Siguiente', action: tour.next }
        ]
    });

    // =========================================
    // PASO 5: Descripción
    // =========================================
    tour.addStep({
        id: 'description',
        title: 'Descripción del atributo',
        text: `
            <p>Describe el propósito del atributo.</p>
            <p>Esta información es útil para:</p>
            <ul>
                <li>👥 Otros administradores</li>
                <li>🧠 Mantenimiento futuro</li>
            </ul>
        `,
        attachTo: {
            element: 'textarea[name="Description"]',
            on: 'right'
        },
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Siguiente', action: tour.next }
        ]
    });

    // =========================================
    // PASO 6: Tipo de dato
    // =========================================
    tour.addStep({
        id: 'datatype',
        title: 'Tipo de dato',
        text: `
            <p>Define cómo se almacenará el valor.</p>
            <ul>
                <li><strong>string:</strong> Texto</li>
                <li><strong>decimal:</strong> Números con decimales</li>
                <li><strong>integer:</strong> Números enteros</li>
                <li><strong>boolean:</strong> Sí / No</li>
                <li><strong>date:</strong> Fecha</li>
            </ul>
        `,
        attachTo: {
            element: 'select[name="DataType"]',
            on: 'right'
        },
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Siguiente', action: tour.next }
        ]
    });

    // =========================================
    // PASO 7: Tipo de input
    // =========================================
    tour.addStep({
        id: 'inputtype',
        title: 'Tipo de input',
        text: `
            <p>Define cómo el usuario ingresará el valor.</p>
            <ul>
                <li>📝 text / textarea</li>
                <li>🔢 number</li>
                <li>☑️ checkbox</li>
                <li>📅 date</li>
                <li>🔽 select / multiselect</li>
            </ul>
        `,
        attachTo: {
            element: 'select[name="InputType"]',
            on: 'right'
        },
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Siguiente', action: tour.next }
        ]
    });

    // =========================================
    // PASO 8: Unidad
    // =========================================
    tour.addStep({
        id: 'unit',
        title: 'Unidad de medida',
        text: `
            <p>Opcional para valores numéricos.</p>
            <p><strong>Ejemplos:</strong></p>
            <ul>
                <li>g</li>
                <li>ml</li>
                <li>kcal</li>
            </ul>
        `,
        attachTo: {
            element: 'input[name="Unit"]',
            on: 'right'
        },
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Siguiente', action: tour.next }
        ]
    });

    // =========================================
    // PASO 9: Categoría
    // =========================================
    tour.addStep({
        id: 'category',
        title: 'Categoría del atributo',
        text: `
            <p>Agrupa atributos por tipo de producto.</p>
            <p><strong>Ejemplo:</strong> Alimento, Bebida, General</p>
        `,
        attachTo: {
            element: 'select[name="Category"]',
            on: 'right'
        },
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Siguiente', action: tour.next }
        ]
    });

    // =========================================
    // PASO 10: Comportamiento
    // =========================================
    tour.addStep({
        id: 'flags',
        title: 'Comportamiento del atributo',
        text: `
            <ul>
                <li>⭐ <strong>Obligatorio:</strong> Fuerza su ingreso</li>
                <li>🔍 <strong>Filtrable:</strong> Aparece en filtros</li>
                <li>🎨 <strong>Variante:</strong> Genera variantes (color, tamaño)</li>
                <li>⚡ <strong>Activo:</strong> Disponible en el sistema</li>
            </ul>
        `,
        attachTo: {
            element: '.form-checkbox',
            on: 'left'
        },
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Siguiente', action: tour.next }
        ]
    });

    // =========================================
    // PASO 11: Guardar
    // =========================================
    tour.addStep({
        id: 'save',
        title: 'Guardar atributo',
        text: `
            <p>Cuando todo esté listo:</p>
            <ul>
                <li>💾 Haz clic en <strong>Guardar atributo</strong></li>
                <li>🔍 El sistema validará los datos</li>
                <li>✅ El atributo quedará disponible para productos</li>
            </ul>
        `,
        attachTo: {
            element: 'button[type="submit"]',
            on: 'top'
        },
        classes: 'shepherd-theme-success',
        buttons: [
            { text: 'Atrás', action: tour.back },
            { text: 'Finalizar', action: tour.complete }
        ]
    });

    // =========================================
    // EVENTOS
    // =========================================
    tour.on('complete', () => {
        localStorage.setItem('masterAttributesCreateTourCompleted', 'true');
        console.log('✅ Tour Maestro de Atributos completado');
    });

    tour.on('cancel', () => {
        localStorage.setItem('masterAttributesCreateTourCancelled', 'true');
        console.log('❌ Tour Maestro de Atributos cancelado');
    });

    return tour;
}
/* =====================================================
   VERIFICAR SI MOSTRAR EL TOUR
===================================================== */
function shouldShowTour() {
    const completed = localStorage.getItem('masterAttributesCreateTourCompleted');
    const cancelled = localStorage.getItem('masterAttributesCreateTourCancelled');
    return !completed && !cancelled;
}

/* =====================================================
INIT
===================================================== */
document.addEventListener("DOMContentLoaded", () => {

    // Inicializar tour automáticamente si es primera visita
    const tour = initMasterAttributesCreateTour();

    if (shouldShowTour()) {
        setTimeout(() => {
            tour.start();
        }, 1000);
    }

    // Botón de ayuda para iniciar el tour manualmente
    const helpButton = document.getElementById('help-tour-button');
    if (helpButton) {
        helpButton.addEventListener('click', () => {
            tour.start();
        });
    }

    // Exponer tour globalmente para acceso manual
    window.masterAttributesCreateTour = tour;
});

/* =====================================================
FORM VALIDATION
===================================================== */
const form = document.getElementById("formMasterAttributes");
if (!form) {
    console.error("No se encontró el formulario con id: formMasterAttributes");
    throw new Error("Formulario no encontrado");
}

console.log("Schema cargado:", masterAttributesCreateSchema);
initBlurValidation({
    form,
    schema: masterAttributesCreateSchema
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
    const data = castFormDataBySchema(formData, masterAttributesCreateSchema);

    try {
        const validated = await masterAttributesCreateSchema.parseAsync(data);
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