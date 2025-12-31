/* =====================================================
   TOUR GUIADO - CREAR BANNER
   js/application/banners/create.js (ACTUALIZADO)
===================================================== */

import Shepherd from "../../bundle/vendors_shepherd.js";
import { bannersCreateSchema } from "../../bundle/schema/banners.create.shema.js";
import {
    initBlurValidation,
} from "../../domain/utils/ui/input.validation.js";
import {
    handleZodFormSubmit
} from "../../domain/utils/form-helpers.js";
import { ImagePreviewHandler } from "../../domain/utils/image-handler.js";
import {
    setupLivePreview
} from "./generic.js";

/* =====================================================
   CONFIGURACIÓN DEL TOUR
===================================================== */
function initBannerCreateTour() {
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
        title: '¡Bienvenido al Creador de Banners!',
        text: `
            <p>👋 Te guiaremos paso a paso para crear un banner promocional atractivo.</p>
            <p>Aprenderás a:</p>
            <ul>
                <li>📝 Completar los campos requeridos</li>
                <li>🖼️ Subir y previsualizar imágenes</li>
                <li>👁️ Ver una vista previa en tiempo real</li>
                <li>⚙️ Configurar opciones avanzadas</li>
            </ul>
            <p><strong>Duración:</strong> 3-4 minutos</p>
        `,
        buttons: [
            {
                text: 'Saltar Tour',
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

    // =========================================
    // PASO 2: Breadcrumb y navegación
    // =========================================
    tour.addStep({
        id: 'breadcrumb',
        title: 'Navegación',
        text: `
            <p>El breadcrumb te muestra dónde estás en el sistema.</p>
            <p>Puedes hacer clic en cualquier parte para navegar rápidamente:</p>
            <ul>
                <li><strong>Inicio</strong> → Dashboard principal</li>
                <li><strong>Banners</strong> → Lista de todos los banners</li>
                <li><strong>Crear Banner</strong> → Donde estás ahora</li>
            </ul>
        `,
        attachTo: {
            element: 'nav[aria-label="breadcrumb"]',
            on: 'bottom'
        },
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 3: Sección del banner
    // =========================================
    tour.addStep({
        id: 'seccion-field',
        title: 'Sección del Banner',
        text: `
            <p>Define dónde se mostrará este banner en tu sitio web.</p>
            <p><strong>Ejemplos comunes:</strong></p>
            <ul>
                <li>🏠 <code>Home</code> - Página principal</li>
                <li>🏷️ <code>Ofertas</code> - Sección de promociones</li>
                <li>🛍️ <code>Productos</code> - Categoría de productos</li>
                <li>🎉 <code>Black Friday</code> - Eventos especiales</li>
            </ul>
            <p class="text-red-600"><strong>⚠️ Campo obligatorio</strong></p>
        `,
        attachTo: {
            element: 'input[name="Seccion"]',
            on: 'right'
        },
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 4: Título del banner
    // =========================================
    tour.addStep({
        id: 'titulo-field',
        title: 'Título Principal',
        text: `
            <p>El título es el mensaje principal de tu banner.</p>
            <p><strong>Tips para un buen título:</strong></p>
            <ul>
                <li>✅ Claro y directo</li>
                <li>✅ Máximo 60 caracteres</li>
                <li>✅ Usa palabras clave llamativas</li>
                <li>✅ Genera urgencia o emoción</li>
            </ul>
            <p><strong>Ejemplo:</strong> "¡50% OFF en Frutos Secos!"</p>
        `,
        attachTo: {
            element: 'input[name="Titulo"]',
            on: 'right'
        },
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 5: Subtítulo
    // =========================================
    tour.addStep({
        id: 'subtitulo-field',
        title: 'Subtítulo Descriptivo',
        text: `
            <p>El subtítulo complementa al título con información adicional.</p>
            <p><strong>Úsalo para:</strong></p>
            <ul>
                <li>📅 Fechas de validez: "Del 1 al 15 de enero"</li>
                <li>🎯 Condiciones: "En productos seleccionados"</li>
                <li>💡 Detalles: "Envío gratis en compras +$50"</li>
            </ul>
        `,
        attachTo: {
            element: 'input[name="Subtitulo"]',
            on: 'right'
        },
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 6: Texto alternativo (SEO)
    // =========================================
    tour.addStep({
        id: 'alt-text-field',
        title: 'Texto Alternativo (SEO)',
        text: `
            <p>El texto alternativo es crucial para:</p>
            <ul>
                <li>♿ <strong>Accesibilidad:</strong> Lectores de pantalla</li>
                <li>🔍 <strong>SEO:</strong> Posicionamiento en Google</li>
                <li>🖼️ <strong>Carga fallida:</strong> Se muestra si la imagen no carga</li>
            </ul>
            <p><strong>Describe la imagen brevemente:</strong></p>
            <p>"Promoción de frutos secos con 50% de descuento"</p>
        `,
        attachTo: {
            element: 'input[name="AltText"]',
            on: 'right'
        },
        classes: 'shepherd-theme-warning',
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 7: Texto del botón
    // =========================================
    tour.addStep({
        id: 'boton-texto-field',
        title: 'Llamada a la Acción (CTA)',
        text: `
            <p>El texto del botón debe motivar al usuario a hacer clic.</p>
            <p><strong>Ejemplos efectivos:</strong></p>
            <ul>
                <li>🛒 "Comprar Ahora"</li>
                <li>👀 "Ver Ofertas"</li>
                <li>📦 "Descubrir Productos"</li>
                <li>🎁 "Aprovechar Descuento"</li>
            </ul>
            <p><strong>Evita:</strong> Textos genéricos como "Click aquí"</p>
        `,
        attachTo: {
            element: 'input[name="BotonTexto"]',
            on: 'right'
        },
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 8: Enlace del botón
    // =========================================
    tour.addStep({
        id: 'boton-enlace-field',
        title: 'Destino del Botón',
        text: `
            <p>URL a la que se redirigirá al usuario al hacer clic.</p>
            <p><strong>Formatos válidos:</strong></p>
            <ul>
                <li>🌐 URL completa: <code>https://tutienda.com/ofertas</code></li>
                <li>📄 Ruta relativa: <code>/productos/frutos-secos</code></li>
                <li>🔗 Ancla: <code>#promociones</code></li>
            </ul>
            <p><strong>⚠️ Asegúrate de que el enlace funcione correctamente</strong></p>
        `,
        attachTo: {
            element: 'input[name="BotonEnlace"]',
            on: 'right'
        },
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 9: Vista previa en tiempo real
    // =========================================
    tour.addStep({
        id: 'preview-section',
        title: '👁️ Vista Previa en Tiempo Real',
        text: `
            <p><strong>¡Esta es tu mejor herramienta!</strong></p>
            <p>Mientras completas los campos, la vista previa se actualiza automáticamente.</p>
            <ul>
                <li>✨ Ve cómo quedará tu banner antes de guardarlo</li>
                <li>🎨 Ajusta textos hasta lograr el efecto deseado</li>
                <li>📱 Verifica que se vea bien en diferentes tamaños</li>
            </ul>
            <p><strong>Tip:</strong> Prueba diferentes combinaciones de título y subtítulo</p>
        `,
        attachTo: {
            element: '#ImagePreview',
            on: 'left'
        },
        classes: 'shepherd-theme-success',
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 10: Subir imagen
    // =========================================
    tour.addStep({
        id: 'image-upload',
        title: '🖼️ Subir Imagen del Banner',
        text: `
            <p>Selecciona la imagen de fondo para tu banner.</p>
            <p><strong>Requisitos:</strong></p>
            <ul>
                <li>📏 <strong>Tamaño recomendado:</strong> 1920x600px</li>
                <li>📦 <strong>Peso máximo:</strong> 2MB</li>
                <li>🎨 <strong>Formatos:</strong> JPG, PNG, WebP</li>
            </ul>
            <p><strong>Tip:</strong> Usa imágenes de alta calidad con buena iluminación</p>
            <p>La imagen se previsualizará automáticamente 👆</p>
        `,
        attachTo: {
            element: '#imageInput',
            on: 'bottom'
        },
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 11: Orden de visualización
    // =========================================
    tour.addStep({
        id: 'sort-order',
        title: '🔢 Orden de Visualización',
        text: `
            <p>Controla el orden en que aparecen tus banners.</p>
            <p><strong>¿Cómo funciona?</strong></p>
            <ul>
                <li>📈 <strong>Menor número = mayor prioridad</strong></li>
                <li>🥇 Orden 100: Se muestra primero</li>
                <li>🥈 Orden 50: Se muestra después</li>
                <li>🥉 Orden 10: Se muestra al final</li>
            </ul>
            <p><strong>Ejemplo:</strong> Banner de Black Friday = 100</p>
        `,
        attachTo: {
            element: 'input[name="SortOrder"]',
            on: 'left'
        },
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 12: Estado activo/inactivo
    // =========================================
    tour.addStep({
        id: 'is-active',
        title: '⚡ Estado del Banner',
        text: `
            <p>Controla si el banner se muestra en tu sitio web.</p>
            <p><strong>Estados:</strong></p>
            <ul>
                <li>✅ <strong>Activo:</strong> Se muestra al público</li>
                <li>❌ <strong>Inactivo:</strong> Oculto (útil para preparar campañas)</li>
            </ul>
            <p><strong>Caso de uso:</strong></p>
            <p>Crea el banner ahora, déjalo inactivo y actívalo cuando inicie la promoción 📅</p>
        `,
        attachTo: {
            element: '#isActiveSwitch',
            on: 'left'
        },
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 13: Guardar banner
    // =========================================
    tour.addStep({
        id: 'save-button',
        title: '💾 Guardar tu Banner',
        text: `
            <p>Cuando hayas completado todos los campos:</p>
            <ul>
                <li>✅ Haz clic en <strong>"Guardar Cambios"</strong></li>
                <li>⚡ El sistema validará automáticamente los datos</li>
                <li>🔒 Si hay errores, te indicará qué corregir</li>
                <li>✨ Si todo está bien, el banner se creará</li>
            </ul>
            <p><strong>Validación automática:</strong></p>
            <p>Campos obligatorios, formato de URL, tamaño de imagen, etc.</p>
        `,
        attachTo: {
            element: 'button[type="submit"]',
            on: 'top'
        },
        classes: 'shepherd-theme-success',
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 14: Botón cancelar
    // =========================================
    tour.addStep({
        id: 'volver-button',
        title: '🚪 Cancelar Creación',
        text: `
            <p>Si decides no crear el banner:</p>
            <ul>
                <li>❌ Haz clic en "Cancelar"</li>
                <li>🔙 Volverás a la lista de banners</li>
                <li>⚠️ Los cambios NO se guardarán</li>
            </ul>
            <p><strong>Advertencia:</strong> No hay borrador automático, asegúrate de guardar si quieres conservar tu trabajo.</p>
        `,
        attachTo: {
            element: 'a[href*="Index"]',
            on: 'top'
        },
        classes: 'shepherd-theme-warning',
        buttons: [
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
        ]
    });

    // =========================================
    // PASO 15: Finalización
    // =========================================
    tour.addStep({
        id: 'complete',
        title: '🎉 ¡Tour Completado!',
        text: `
            <p><strong>¡Felicitaciones!</strong> Ya conoces cómo crear banners efectivos.</p>
            <p><strong>Resumen de lo aprendido:</strong></p>
            <ul>
                <li>📝 Completar información del banner</li>
                <li>🖼️ Subir y previsualizar imágenes</li>
                <li>⚙️ Configurar orden y estado</li>
                <li>💾 Guardar y publicar</li>
            </ul>
            <p><strong>Consejos finales:</strong></p>
            <ul>
                <li>✨ Usa imágenes de alta calidad</li>
                <li>🎯 Textos claros y concisos</li>
                <li>📱 Verifica la vista previa</li>
                <li>🔄 Actualiza regularmente tus banners</li>
            </ul>
            <p>¿Listo para crear tu primer banner? 🚀</p>
        `,
        classes: 'shepherd-theme-success',
        buttons: [
            {
                text: 'Repetir Tour',
                classes: 'shepherd-button-secondary',
                action: () => {
                    tour.start();
                }
            },
            {
                text: '¡Comenzar a Crear!',
                classes: 'shepherd-button-primary',
                action: () => {
                    tour.complete();
                    // Hacer foco en el primer campo
                    document.querySelector('input[name="Seccion"]')?.focus();
                }
            }
        ]
    });

    // =========================================
    // EVENTOS DEL TOUR
    // =========================================

    // Agregar indicador de progreso
    tour.on('show', () => {
        const currentStep = tour.getCurrentStep();
        if (!currentStep) return;

        const stepElement = currentStep.getElement();
        if (!stepElement) return; // 👈 CLAVE

        const currentStepIndex = tour.steps.indexOf(currentStep);
        const totalSteps = tour.steps.length;

        // ==========================
        // Contador de pasos
        // ==========================
        const footer = stepElement.querySelector('.shepherd-footer');
        if (footer && !footer.querySelector('.shepherd-step-count')) {
            const stepCount = document.createElement('div');
            stepCount.className = 'shepherd-step-count';
            stepCount.textContent = `Paso ${currentStepIndex + 1} de ${totalSteps}`;
            footer.insertBefore(stepCount, footer.firstChild);
        }

        // ==========================
        // Barra de progreso
        // ==========================
        const content = stepElement.querySelector('.shepherd-content');
        if (content && !content.querySelector('.shepherd-progress')) {
            const progressBar = document.createElement('div');
            progressBar.className = 'shepherd-progress';
            progressBar.innerHTML = `
            <div class="shepherd-progress-bar" style="width: ${((currentStepIndex + 1) / totalSteps) * 100}%"></div>
        `;
            content.insertBefore(progressBar, content.firstChild);
        }
    });


    // Cuando completa el tour
    tour.on('complete', () => {
        localStorage.setItem('bannerCreateTourCompleted', 'true');
        console.log('✅ Tour de creación de banner completado');
    });

    // Cuando cancela el tour
    tour.on('cancel', () => {
        localStorage.setItem('bannerCreateTourCancelled', 'true');
        console.log('❌ Tour de creación de banner cancelado');
    });

    return tour;
}

/* =====================================================
   VERIFICAR SI MOSTRAR EL TOUR
===================================================== */
function shouldShowTour() {
    const completed = localStorage.getItem('bannerCreateTourCompleted');
    const cancelled = localStorage.getItem('bannerCreateTourCancelled');
    return !completed && !cancelled;
}

/* =====================================================
   INIT
===================================================== */
document.addEventListener("DOMContentLoaded", () => {
    // UI genérica de categorías
    setupLivePreview();

    // Preview de imagen
    ImagePreviewHandler.init();

    // Inicializar tour automáticamente si es primera visita
    const tour = initBannerCreateTour();

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
    window.bannerCreateTour = tour;
});

/* =====================================================
   FORM VALIDATION
===================================================== */
const form = document.getElementById("formBanners");
if (!form) {
    console.error("No se encontró el formulario con id: formBanners");
    throw new Error("Formulario no encontrado");
}

console.log("Schema cargado:", bannersCreateSchema);
initBlurValidation({
    form,
    schema: bannersCreateSchema
});

/* =====================================================
   SUBMIT
===================================================== */
handleZodFormSubmit({
    form,
    schema: bannersCreateSchema,
    castSchema: bannersCreateSchema,
    spinnerAction: "creating"
});