/**
         * ============================================
         * SISTEMA GLOBAL DE LOADING + SPINNERS
         * ============================================
         */

// Referencias DOM
const loadingModal = document.getElementById('loadingModal');
const spinnerContent = document.getElementById('spinnerContent');
const resultModal = document.getElementById('resultModal');
const resultIcon = document.getElementById('resultIcon');
const resultTitle = document.getElementById('resultTitle');
const resultMessage = document.getElementById('resultMessage');
const resultButton = document.getElementById('resultButton');

// Estados globales
let activeSpinner = false;
let timeoutRef = null;

// Configuración de spinners
const spinnerTypes = {
    creating: {
        icon: "✨",
        iconClass: "icon-pulse",
        title: "Creando Registro",
        subtitle: "Por favor espere",
        colorTitle: "var(--color-mint-green)",
        colorSubtitle: "var(--color-dark-chocolate)",
        type: ""
    },
    editing: {
        icon: "✏️",
        iconClass: "icon-rotate",
        title: "Editando Datos",
        subtitle: "Actualizando información",
        colorTitle: "var(--color-golden-yellow)",
        colorSubtitle: "var(--color-dark-chocolate)",
        type: ""
    },
    deleting: {
        icon: "🗑️",
        iconClass: "icon-pulse",
        title: "Eliminando",
        subtitle: "Eliminando registro",
        colorTitle: "var(--color-burgundy-red)",
        colorSubtitle: "var(--color-dark-chocolate)",
        type: "circle"
    },
    uploading: {
        icon: "📤",
        iconClass: "icon-bounce",
        title: "Subiendo Archivo",
        subtitle: "Cargando",
        colorTitle: "var(--color-olive-green)",
        colorSubtitle: "var(--color-dark-chocolate)",
        type: "upload"
    },
    processing: {
        icon: "⚙️",
        iconClass: "icon-rotate",
        title: "Procesando Datos",
        subtitle: "Analizando información",
        colorTitle: "var(--color-nut-brown)",
        colorSubtitle: "var(--color-dark-chocolate)",
        type: "bars"
    },
    saving: {
        icon: "💾",
        iconClass: "icon-pulse",
        title: "Guardando Cambios",
        subtitle: "Almacenando datos",
        colorTitle: "var(--color-orange-warm)",
        colorSubtitle: "var(--color-dark-chocolate)",
        type: "dots"
    },
    loading: {
        icon: "",
        iconClass: "icon-bounce",
        title: "Cargando",
        subtitle: "Obteniendo información",
        colorTitle: "var(--color-golden-yellow)",
        colorSubtitle: "var(--color-dark-chocolate)",
        type: "circle"
    }
};

/**
 * Genera el HTML del spinner
 */
function buildSpinnerHTML(cfg) {
    const types = {
        circle: `<div class="spinner mx-auto mb-6"><div class="spinner-ring"></div></div>`,
        bars: `<div class="spinner-bars">${"<div class='spinner-bar'></div>".repeat(5)}</div>`,
        dots: `<div class="spinner-dots">${"<div class='spinner-dot'></div>".repeat(3)}</div>`,
        upload: `
                    <div class="spinner-upload">
                        <div class="upload-box"><div class="upload-arrow">⬆️</div></div>
                    </div>
                    <div class="progress-bar-container"><div class="progress-bar"></div></div>
                `
    };

    const spinnerHTML = types[cfg.type] || "";

    return `
                <div class="action-icon ${cfg.iconClass}">${cfg.icon}</div>
                ${spinnerHTML}
                <div class="spinner-title" style="color:${cfg.colorTitle}">${cfg.title}</div>
                <div class="spinner-subtitle loading-dots" style="color:${cfg.colorSubtitle}">
                    ${cfg.subtitle}<span>.</span><span>.</span><span>.</span>
                </div>
            `;
}

/**
 * Mostrar spinner
 */
function showSpinner(type = "loading", duration = 0, callback = null) {
    activeSpinner = true;

    if (timeoutRef) clearTimeout(timeoutRef);

    const cfg = spinnerTypes[type] ?? spinnerTypes.loading;
    spinnerContent.innerHTML = buildSpinnerHTML(cfg);

    loadingModal.classList.add("show");
    document.body.style.overflow = "hidden";

    if (duration > 0) {
        timeoutRef = setTimeout(() => {
            hideSpinner();
            if (typeof callback === "function") callback();
        }, duration);
    }
}

/**
 * Ocultar spinner
 */
function hideSpinner() {
    activeSpinner = false;
    loadingModal.classList.remove("show");
    document.body.style.overflow = "";
    if (timeoutRef) clearTimeout(timeoutRef);
}

/**
 * Spinner para promesas
 */
async function showSpinnerAsync(type, promise) {
    showSpinner(type, 0);
    try {
        const result = await promise;
        hideSpinner();
        return result;
    } catch (err) {
        hideSpinner();
        throw err;
    }
}

/**
 * ============================================
 * MODAL DE RESULTADO (ÉXITO/ERROR)
 * ============================================
 */

/**
 * Mostrar modal de resultado
 * @param {string} type - 'success' o 'error'
 * @param {string} title - Título del modal
 * @param {string} message - Mensaje del modal
 */
function showResultModal(type, title, message) {
    if (type === 'success') {
        resultIcon.textContent = '✅';
        resultTitle.textContent = title;
        resultTitle.style.color = 'var(--color-mint-green)';
        resultButton.className = 'result-button btn-success';
    } else {
        resultIcon.textContent = '❌';
        resultTitle.textContent = title;
        resultTitle.style.color = 'var(--color-burgundy-red)';
        resultButton.className = 'result-button btn-error';
    }

    resultMessage.textContent = message;
    resultModal.classList.add('show');
    document.body.style.overflow = 'hidden';
}

/**
 * Ocultar modal de resultado
 */
function hideResultModal() {
    resultModal.classList.remove('show');
    document.body.style.overflow = '';
}

/**
 * ============================================
 * DETECCIÓN AUTOMÁTICA DE MENSAJES DEL BACKEND
 * ============================================
 */

document.addEventListener("DOMContentLoaded", () => {
    const estado = $("#Estado").val();
    const mensaje = $("#Mensaje").val();

    if (estado === "Error") {
        // Mostrar loading primero
        showSpinner("loading", 0);

        // Después de 800ms mostrar el modal de error
        setTimeout(() => {
            hideSpinner();
            showResultModal('error', 'Error', mensaje || 'Ocurrió un error en la operación');
        }, 800);

    } else if (estado === "Exito") {
        // Mostrar loading primero
        showSpinner("loading", 0);

        // Después de 800ms mostrar el modal de éxito
        setTimeout(() => {
            hideSpinner();
            showResultModal('success', '¡Éxito!', mensaje || 'Operación completada correctamente');
        }, 800);

    } else if (!activeSpinner) {
        // Carga normal - mostrar loading
        showSpinner("loading", 0);
    }
});

window.addEventListener("load", () => {
    const estado = $("#Estado").val();

    if (estado !== "Error" && estado !== "Exito") {
        // Solo ocultar si es carga normal
        if (activeSpinner) {
            setTimeout(() => hideSpinner(), 500);
        }
    }
    // Si es Error o Éxito, el modal ya se mostró en DOMContentLoaded
});

/**
 * ============================================
 * FUNCIONES DE SIMULACIÓN (PARA TESTING)
 * ============================================
 */

function simularExito() {
    $("#Estado").val("Exito");
    $("#Mensaje").val("¡Producto creado exitosamente! 🎉");
    location.reload();
}

function simularError() {
    $("#Estado").val("Error");
    $("#Mensaje").val("No se pudo crear el producto. Verifique los datos.");
    location.reload();
}

function simularCargaNormal() {
    $("#Estado").val("");
    $("#Mensaje").val("");
    location.reload();
}

/**
 * ============================================
 * EVENTOS OPCIONALES
 * ============================================
 */

// Cerrar modal de resultado con ESC
document.addEventListener("keydown", e => {
    if (e.key === "Escape") {
        if (resultModal.classList.contains("show")) {
            hideResultModal();
        }
        // Descomentar para permitir cerrar spinner con ESC
        // if (loadingModal.classList.contains("show")) {
        //     hideSpinner();
        // }
    }
});

console.log('🚀 Sistema de Spinners y Modales cargado correctamente');