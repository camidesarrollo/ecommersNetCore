import Shepherd from "../../bundle/vendors_shepherd.js";
import {
    handleZodFormSubmit
} from "../../bundle/utils/form-helpers.js";
import {
    initBlurValidation,
} from "../../domain/utils/ui/input.validation.js";
import { showSuccess, showError, showWarning, showInfo } from '../../bundle/notifications/notyf.config.js';

/* =====================================================
   CONFIGURACIÓN DEL TOUR - CREAR PRODUCTO
===================================================== */
function initProductCreateTour() {
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

    /* =========================================
       PASO 1: Bienvenida
    ========================================= */
    tour.addStep({
        id: 'welcome',
        title: '🛍️ ¡Bienvenido al Creador de Productos!',
        text: `
            <p>Te guiaremos paso a paso para crear un producto correctamente.</p>
            <ul>
                <li>📝 Información básica</li>
                <li>🖼️ Imágenes</li>
                <li>⚙️ Atributos</li>
                <li>📦 Stock y estado</li>
            </ul>
            <p><strong>Duración:</strong> 3–5 minutos</p>
        `,
        buttons: [
            { text: 'Saltar', classes: 'shepherd-button-secondary', action: tour.cancel },
            { text: 'Comenzar', classes: 'shepherd-button-primary', action: tour.next }
        ]
    });

    /* =========================================
       PASO 2: Breadcrumb
    ========================================= */
    tour.addStep({
        id: 'breadcrumb',
        title: 'Navegación',
        text: `
            <p>Este breadcrumb muestra dónde estás.</p>
            <p>Puedes volver a <strong>Productos</strong> o al <strong>Inicio</strong>.</p>
        `,
        attachTo: {
            element: 'nav[aria-label="breadcrumb"]',
            on: 'bottom'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 3: Nombre
    ========================================= */
    tour.addStep({
        id: 'product-name',
        title: 'Nombre del producto',
        text: `
            <p>Nombre visible para los clientes.</p>
            <ul>
                <li>✔ Claro y descriptivo</li>
                <li>✔ Obligatorio</li>
            </ul>
        `,
        attachTo: {
            element: 'input[name="Products.Name"]',
            on: 'right'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 4: Slug
    ========================================= */
    tour.addStep({
        id: 'product-slug',
        title: 'Slug URL',
        text: `
            <p>Se genera automáticamente desde el nombre.</p>
            <p>No es editable manualmente.</p>
            <p><strong>Ejemplo:</strong> mix-frutos-secos</p>
        `,
        attachTo: {
            element: 'input[name="Products.Slug"]',
            on: 'right'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 5: Categoría
    ========================================= */
    tour.addStep({
        id: 'category',
        title: 'Categoría',
        text: `
            <p>Define dónde se agrupa el producto.</p>
            <p class="text-red-600"><strong>Campo obligatorio</strong></p>
        `,
        attachTo: {
            element: 'select[name="Products.CategoryId"]',
            on: 'right'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 6: Descripción corta
    ========================================= */
    tour.addStep({
        id: 'short-description',
        title: 'Descripción corta',
        text: `
            <p>Resumen breve del producto.</p>
            <p>Máx. 150 caracteres.</p>
        `,
        attachTo: {
            element: 'textarea[name="Products.ShortDescription"]',
            on: 'right'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 7: Descripción completa
    ========================================= */
    tour.addStep({
        id: 'description',
        title: 'Descripción completa',
        text: `
            <p>Detalle completo del producto.</p>
            <ul>
                <li>Beneficios</li>
                <li>Uso</li>
                <li>Ingredientes</li>
            </ul>
        `,
        attachTo: {
            element: 'textarea[name="Products.Description"]',
            on: 'top'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 8: Imágenes
    ========================================= */
    tour.addStep({
        id: 'images',
        title: 'Imágenes del producto',
        text: `
            <p>Agrega una o más imágenes.</p>
            <p>Se recomienda buena resolución.</p>
        `,
        attachTo: {
            element: '#btnAddImage',
            on: 'left'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 9: Atributos
    ========================================= */
    tour.addStep({
        id: 'attributes',
        title: 'Atributos del producto',
        text: `
            <p>Estos campos cambian según la categoría.</p>
            <p>Algunos son obligatorios.</p>
        `,
        attachTo: {
            element: '.fa-sliders-h',
            on: 'top'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 10: SKU
    ========================================= */
    tour.addStep({
        id: 'sku',
        title: 'SKU',
        text: `<p>Código único del producto.</p>`,
        attachTo: {
            element: 'input[name="ProductVariants.SKU"]',
            on: 'left'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 11: Stock
    ========================================= */
    tour.addStep({
        id: 'stock',
        title: 'Stock disponible',
        text: `<p>Cantidad disponible para la venta.</p>`,
        attachTo: {
            element: 'input[name="ProductVariants.StockQuantity"]',
            on: 'left'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 12: Estado
    ========================================= */
    tour.addStep({
        id: 'active',
        title: 'Producto activo',
        text: `
            <p>Define si el producto será visible.</p>
        `,
        attachTo: {
            element: 'input[name="Products.IsActive"]',
            on: 'left'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 13: Guardar
    ========================================= */
    tour.addStep({
        id: 'submit',
        title: '💾 Guardar producto',
        text: `
            <p>Cuando todo esté listo, guarda el producto.</p>
        `,
        attachTo: {
            element: 'button[type="submit"]',
            on: 'top'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       FINAL
    ========================================= */
    tour.addStep({
        id: 'complete',
        title: '🎉 ¡Listo!',
        text: `
            <p>Ya sabes cómo crear productos.</p>
            <ul>
                <li>🖼️ Buenas imágenes</li>
                <li>📄 Descripciones claras</li>
                <li>📦 Stock correcto</li>
            </ul>
        `,
        buttons: [
            { text: 'Repetir', classes: 'shepherd-button-secondary', action: tour.start },
            { text: 'Finalizar', classes: 'shepherd-button-primary', action: tour.complete }
        ]
    });

    return tour;
}
/* =====================================================
   BOTONES REUTILIZABLES
===================================================== */
function navigationButtons(tour) {
    return [
        {
            text: 'Atrás',
            classes: 'shepherd-button-secondary',
            action: tour.back
        },
        {
            text: 'Siguiente',
            classes: 'shepherd-button-primary',
            action: tour.next
        }
    ];
}

/* =====================================================
   INIT
===================================================== */
document.addEventListener("DOMContentLoaded", () => {
    const tour = initProductCreateTour();

    if (!localStorage.getItem('productCreateTourCompleted')) {
        setTimeout(() => tour.start(), 800);
    }

    document
        .getElementById('help-tour-button')
        ?.addEventListener('click', () => tour.start());

    tour.on('complete', () =>
        localStorage.setItem('productCreateTourCompleted', 'true')
    );

    window.productCreateTour = tour;

});

/* =====================================================
   FORM VALIDATION
===================================================== */
document.addEventListener("DOMContentLoaded", () => {

    const form = document.getElementById("formProducts");
    if (!form) {
        console.error("No se encontró el formulario con id: formProducts");
        return;
    }

    form.addEventListener("submit", function (e) {
        e.preventDefault(); // 🚫 Detiene el submit

        let isValid = true;
        let firstInvalidField = null;

        if (!validacionFormulario()) {
            return;
        }


        // 🔎 Todos los campos required
        const requiredFields = form.querySelectorAll(
            "input[required]:not([disabled]), select[required], textarea[required]"
        );

        requiredFields.forEach(field => {

            field.classList.remove("border-red-500");

            // Validación base
            if (!field.value || field.value.trim() === "" || field.value === "0") {
                isValid = false;
                field.classList.add("border-red-500");

                if (!firstInvalidField) {
                    firstInvalidField = field;
                }
            }
        });

        // ❌ Si hay errores
        if (!isValid) {
            showWarning("Por favor completa todos los campos obligatorios.");
            firstInvalidField?.focus();
            return;
        }

        // ✅ REMOVER TODOS LOS DISABLED (CRÍTICO)
        const disabledFields = form.querySelectorAll("[disabled]");
        disabledFields.forEach(field => {
            field.removeAttribute("disabled");
        });

        // ✅ Enviar formulario
        form.submit();
    });

});
