/* js/application/categorias/create.js */
import Shepherd from "../../bundle/vendors_shepherd.js";
import { categoriesCreateSchema } from "../../bundle/schema/categories.create.shema.js";
import {
    initBlurValidation,
    
} from "../../domain/utils/ui/input.validation.js";

import {
    handleZodFormSubmit
} from "../../bundle/utils/form-helpers.js";

//import { ImagePreviewHandler } from "../../domain/utils/image-handler.js";
//import {
//    setupSlugAutoGeneration,
//    setupGradientSelector,
//    setupLivePreview
//} from "./generic.js";

///* =====================================================
//   CONFIGURACIÓN DEL TOUR
//===================================================== */
//function initCategoryCreateTour() {
//    const tour = new Shepherd.Tour({
//        defaultStepOptions: {
//            classes: 'shepherd-theme-custom',
//            scrollTo: { behavior: 'smooth', block: 'center' },
//            cancelIcon: { enabled: true },
//            modalOverlayOpeningPadding: 8,
//            modalOverlayOpeningRadius: 8
//        },
//        useModalOverlay: true
//    });

//    // =================================================
//    // PASO 1: Bienvenida
//    // =================================================
//    tour.addStep({
//        id: 'welcome',
//        title: '¡Bienvenido al Creador de Categorías!',
//        text: `
//            <p>👋 Te guiaremos paso a paso para crear una categoría correctamente.</p>
//            <ul>
//                <li>🏷️ Definir nombre y URL</li>
//                <li>📝 Agregar descripciones</li>
//                <li>🖼️ Subir imagen</li>
//                <li>🎨 Configurar estilo y estado</li>
//            </ul>
//            <p><strong>Duración:</strong> 2–3 minutos</p>
//        `,
//        buttons: [
//            { text: 'Saltar', action: tour.cancel },
//            { text: 'Comenzar', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 2: Breadcrumb
//    // =================================================
//    tour.addStep({
//        id: 'breadcrumb',
//        title: 'Navegación',
//        text: `
//            <p>Este breadcrumb te indica dónde estás.</p>
//            <ul>
//                <li><strong>Inicio</strong> → Dashboard</li>
//                <li><strong>Categorías</strong> → Listado</li>
//                <li><strong>Crear Categoría</strong> → Página actual</li>
//            </ul>
//        `,
//        attachTo: {
//            element: 'nav[aria-label="breadcrumb"]',
//            on: 'bottom'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 3: Nombre
//    // =================================================
//    tour.addStep({
//        id: 'name',
//        title: 'Nombre de la Categoría',
//        text: `
//            <p>Este es el nombre visible para los usuarios.</p>
//            <ul>
//                <li>✔ Claro y descriptivo</li>
//                <li>✔ Sin símbolos innecesarios</li>
//            </ul>
//            <p><strong>Ejemplo:</strong> Frutos Secos Premium</p>
//            <p class="text-red-600"><strong>Campo obligatorio</strong></p>
//        `,
//        attachTo: {
//            element: 'input[name="Name"]',
//            on: 'right'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 4: Slug
//    // =================================================
//    tour.addStep({
//        id: 'slug',
//        title: 'Slug (URL)',
//        text: `
//            <p>El slug define la URL de la categoría.</p>
//            <ul>
//                <li>🔗 Sin espacios</li>
//                <li>🔗 Usar guiones (-)</li>
//                <li>🔗 Todo en minúsculas</li>
//            </ul>
//            <p><strong>Ejemplo:</strong> frutos-secos-premium</p>
//        `,
//        attachTo: {
//            element: 'input[name="Slug"]',
//            on: 'right'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 5: Descripción corta
//    // =================================================
//    tour.addStep({
//        id: 'short-description',
//        title: 'Descripción Corta',
//        text: `
//            <p>Resumen breve de la categoría.</p>
//            <ul>
//                <li>📝 Máx. 150 caracteres</li>
//                <li>📝 Ideal para listados</li>
//            </ul>
//        `,
//        attachTo: {
//            element: 'textarea[name="ShortDescription"]',
//            on: 'right'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 6: Descripción completa
//    // =================================================
//    tour.addStep({
//        id: 'description',
//        title: 'Descripción Completa',
//        text: `
//            <p>Describe en detalle la categoría.</p>
//            <p>Se muestra en la página principal de la categoría.</p>
//        `,
//        attachTo: {
//            element: 'textarea[name="Description"]',
//            on: 'right'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 7: Vista previa
//    // =================================================
//    tour.addStep({
//        id: 'preview',
//        title: '👁️ Vista Previa',
//        text: `
//            <p>Así se verá tu categoría en el sitio.</p>
//            <ul>
//                <li>✨ Cambios en tiempo real</li>
//                <li>🎨 Fondo y estado visibles</li>
//            </ul>
//        `,
//        attachTo: {
//            element: '.fa-eye',
//            on: 'bottom'
//        },
//        classes: 'shepherd-theme-success',
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 8: Imagen
//    // =================================================
//    tour.addStep({
//        id: 'image',
//        title: '🖼️ Imagen de Categoría',
//        text: `
//            <p>Imagen representativa de la categoría.</p>
//            <ul>
//                <li>📏 Tamaño recomendado: cuadrado</li>
//                <li>📦 Máx. 2MB</li>
//                <li>🎨 JPG, PNG o WEBP</li>
//            </ul>
//        `,
//        attachTo: {
//            element: '#imageInput',
//            on: 'top'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 9: Clase de fondo
//    // =================================================
//    tour.addStep({
//        id: 'bgclass',
//        title: '🎨 Estilo Visual',
//        text: `
//            <p>Selecciona el fondo decorativo de la categoría.</p>
//            <p>Esto solo afecta a la presentación visual.</p>
//        `,
//        attachTo: {
//            element: '#gradientButton',
//            on: 'top'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 10: Categoría padre
//    // =================================================
//    tour.addStep({
//        id: 'parent',
//        title: 'Categoría Padre',
//        text: `
//            <p>Permite crear jerarquías.</p>
//            <ul>
//                <li>📂 Categoría principal → dejar vacío</li>
//                <li>📁 Subcategoría → seleccionar padre</li>
//            </ul>
//        `,
//        attachTo: {
//            element: 'select[name="ParentId"]',
//            on: 'top'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 11: Orden
//    // =================================================
//    tour.addStep({
//        id: 'sort',
//        title: '🔢 Orden de Visualización',
//        text: `
//            <p>Controla el orden de aparición.</p>
//            <p><strong>Menor número = mayor prioridad</strong></p>
//        `,
//        attachTo: {
//            element: 'input[name="SortOrder"]',
//            on: 'left'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 12: Estado
//    // =================================================
//    tour.addStep({
//        id: 'active',
//        title: '⚡ Estado de la Categoría',
//        text: `
//            <p>Define si la categoría es visible.</p>
//            <ul>
//                <li>✅ Activa → visible</li>
//                <li>❌ Inactiva → oculta</li>
//            </ul>
//        `,
//        attachTo: {
//            element: '#isActiveSwitch',
//            on: 'left'
//        },
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Siguiente', classes: 'shepherd-button-primary', action: tour.next }
//        ]
//    });

//    // =================================================
//    // PASO 13: Guardar
//    // =================================================
//    tour.addStep({
//        id: 'save',
//        title: '💾 Guardar Categoría',
//        text: `
//            <p>Cuando todo esté listo:</p>
//            <ul>
//                <li>✔ Haz clic en Guardar Cambios</li>
//                <li>✔ Se validarán los campos</li>
//                <li>✔ La categoría quedará disponible</li>
//            </ul>
//        `,
//        attachTo: {
//            element: 'button[type="submit"]',
//            on: 'top'
//        },
//        classes: 'shepherd-theme-success',
//        buttons: [
//            { text: 'Atrás', action: tour.back },
//            { text: 'Finalizar', classes: 'shepherd-button-primary', action: tour.complete }
//        ]
//    });

//    return tour;
//}

///* =====================================================
//   VERIFICAR SI MOSTRAR EL TOUR
//===================================================== */
//function shouldShowTour() {
//    const completed = localStorage.getItem('categoriasCreateTourCompleted');
//    const cancelled = localStorage.getItem('categoriasCreateTourCancelled');
//    return !completed && !cancelled;
//}

///* =====================================================
//   INIT
//===================================================== */
//document.addEventListener("DOMContentLoaded", () => {
//    // UI genérica de categorías
//    setupSlugAutoGeneration();
//    setupGradientSelector();
//    setupLivePreview();

//    // Preview de imagen
//    ImagePreviewHandler.init();

//    // Inicializar tour automáticamente si es primera visita
//    const tour = initCategoryCreateTour();

//    if (shouldShowTour()) {
//        setTimeout(() => {
//            tour.start();
//        }, 1000);
//    }

//    // Botón de ayuda para iniciar el tour manualmente
//    const helpButton = document.getElementById('help-tour-button');
//    if (helpButton) {
//        helpButton.addEventListener('click', () => {
//            tour.start();
//        });
//    }

//    // Exponer tour globalmente para acceso manual
//    window.categoriasCreateTour = tour;
//});

///* =====================================================
//   FORM
//===================================================== */
//const form = document.getElementById("formCategoria");
//if (!form) {
//    console.error("No se encontró el formulario con id: formCategoria");
//    throw new Error("Formulario no encontrado");
//}

///* =====================================================
//   VALIDACIÓN BLUR
//===================================================== */
//console.log("Schema cargado:", categoriesCreateSchema);

//initBlurValidation({
//    form,
//    schema: categoriesCreateSchema
//});

///* =====================================================
//   SUBMIT
//===================================================== */
//handleZodFormSubmit({
//    form,
//    schema: categoriesCreateSchema,
//    castSchema: categoriesCreateSchema,
//    spinnerAction: "creating"
//});