import Shepherd from "../../bundle/vendors_shepherd.js";
import { productsCreateSchema } from "../../bundle/schema/products.create.shema.js";
import {
    handleZodFormSubmit
} from "../../bundle/utils/form-helpers.js";
import {
    initBlurValidation,
} from "../../domain/utils/ui/input.validation.js";

import {
    setupSlugAutoGeneration,
} from "./generic.js";

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
                <li>📝 Información básica del producto</li>
                <li>🏷️ Categoría y descripciones</li>
                <li>⚙️ Atributos dinámicos</li>
                <li>📦 Stock y estado</li>
            </ul>
            <p><strong>Duración:</strong> 3–5 minutos</p>
        `,
        buttons: [
            {
                text: 'Saltar',
                classes: 'shepherd-button-skip',
                action: tour.cancel
            },
            {
                text: 'Comenzar',
                classes: 'shepherd-button-primary',
                action: tour.next
            }
        ]
    });

    /* =========================================
       PASO 2: Breadcrumb
    ========================================= */
    tour.addStep({
        id: 'breadcrumb',
        title: 'Navegación',
        text: `
            <p>Este breadcrumb indica dónde estás en el sistema.</p>
            <p>Puedes volver a <strong>Productos</strong> o al <strong>Inicio</strong> en cualquier momento.</p>
        `,
        attachTo: {
            element: 'nav[aria-label="breadcrumb"]',
            on: 'bottom'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 3: Nombre del producto
    ========================================= */
    tour.addStep({
        id: 'product-name',
        title: 'Nombre del producto',
        text: `
            <p>Nombre visible para los clientes.</p>
            <ul>
                <li>✔ Claro y descriptivo</li>
                <li>✔ Máx. 255 caracteres</li>
            </ul>
            <p><strong>Ejemplo:</strong> Mix de Frutos Secos Premium</p>
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
            <p>Se usa en la URL del producto.</p>
            <ul>
                <li>🔗 Solo minúsculas</li>
                <li>🔗 Sin espacios</li>
                <li>🔗 Usa guiones</li>
            </ul>
            <p><strong>Ejemplo:</strong> mix-frutos-secos-premium</p>
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
            <p>Esto ayuda a la navegación y al SEO.</p>
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
            <ul>
                <li>✔ Máx. 150 caracteres</li>
                <li>✔ Ideal para listados</li>
            </ul>
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
            <p>Describe el producto en detalle.</p>
            <ul>
                <li>📝 Beneficios</li>
                <li>📝 Ingredientes</li>
                <li>📝 Uso o conservación</li>
            </ul>
        `,
        attachTo: {
            element: 'textarea[name="Products.Description"]',
            on: 'top'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 8: Atributos dinámicos
    ========================================= */
    tour.addStep({
        id: 'attributes',
        title: 'Atributos del producto',
        text: `
            <p>Estos atributos cambian según la categoría.</p>
            <ul>
                <li>📏 Peso</li>
                <li>🌱 Origen</li>
                <li>⚖️ Formato</li>
            </ul>
            <p>Algunos pueden ser obligatorios.</p>
        `,
        attachTo: {
            element: '.glass-effect h5:contains("Atributos")',
            on: 'top'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 9: SKU
    ========================================= */
    tour.addStep({
        id: 'sku',
        title: 'SKU',
        text: `
            <p>Identificador único del producto.</p>
            <p><strong>Recomendado:</strong> Código interno o proveedor</p>
        `,
        attachTo: {
            element: 'input[name="ProductVariants.SKU"]',
            on: 'left'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 10: Stock
    ========================================= */
    tour.addStep({
        id: 'stock',
        title: 'Stock disponible',
        text: `
            <p>Cantidad disponible para la venta.</p>
            <p>Debe ser mayor o igual a 0.</p>
        `,
        attachTo: {
            element: 'input[name="ProductVariants.StockQuantity"]',
            on: 'left'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 11: Estado activo
    ========================================= */
    tour.addStep({
        id: 'active',
        title: 'Producto activo',
        text: `
            <p>Define si el producto será visible.</p>
            <ul>
                <li>✅ Activo → Visible</li>
                <li>❌ Inactivo → Oculto</li>
            </ul>
        `,
        attachTo: {
            element: 'input[name="ProductVariants.IsActive"]',
            on: 'left'
        },
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO 12: Guardar
    ========================================= */
    tour.addStep({
        id: 'submit',
        title: '💾 Guardar producto',
        text: `
            <p>Cuando todo esté listo:</p>
            <ul>
                <li>✔ Valida los datos</li>
                <li>✔ Crea el producto</li>
            </ul>
        `,
        attachTo: {
            element: 'button[type="submit"]',
            on: 'top'
        },
        classes: 'shepherd-theme-success',
        buttons: navigationButtons(tour)
    });

    /* =========================================
       PASO FINAL
    ========================================= */
    tour.addStep({
        id: 'complete',
        title: '🎉 ¡Producto listo!',
        text: `
            <p>Ya sabes cómo crear productos correctamente.</p>
            <p>Tip final:</p>
            <ul>
                <li>🖼️ Usa buenas imágenes</li>
                <li>📄 Descripciones claras</li>
                <li>📦 Stock actualizado</li>
            </ul>
        `,
        classes: 'shepherd-theme-success',
        buttons: [
            {
                text: 'Repetir',
                classes: 'shepherd-button-secondary',
                action: tour.start
            },
            {
                text: 'Finalizar',
                classes: 'shepherd-button-primary',
                action: tour.complete
            }
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

    setupSlugAutoGeneration();
});

/* =====================================================
   FORM VALIDATION
===================================================== */
const form = document.getElementById("formProducts");
if (!form) {
    console.error("No se encontró el formulario con id: formProducts");
    throw new Error("Formulario no encontrado");
}

console.log("Schema cargado:", productsCreateSchema);
initBlurValidation({
    form,
    schema: productsCreateSchema
});

/* =====================================================
   SUBMIT
===================================================== */
handleZodFormSubmit({
    form,
    schema: productsCreateSchema,
    castSchema: productsCreateSchema,
    spinnerAction: "creating"
});