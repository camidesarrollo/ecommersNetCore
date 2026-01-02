/**
 * ============================================
 * SISTEMA GLOBAL DE LOADING + SPINNERS (TS)
 * ============================================
 */
// =====================
// Referencias DOM
// =====================
const loadingModal = document.getElementById("loadingModal");
const spinnerContent = document.getElementById("spinnerContent");
const resultModal = document.getElementById("resultModal");
const resultIcon = document.getElementById("resultIcon");
const resultTitle = document.getElementById("resultTitle");
const resultMessage = document.getElementById("resultMessage");
const resultButton = document.getElementById("resultButton");
// =====================
// Estados globales
// =====================
let activeSpinner = false;
let timeoutRef = null;
// =====================
// Configuración spinners
// =====================
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
// =====================
// Helpers
// =====================
function buildSpinnerHTML(cfg) {
    const visuals = {
        "": "",
        circle: `<div class="spinner mx-auto mb-6"><div class="spinner-ring"></div></div>`,
        bars: `<div class="spinner-bars">${"<div class='spinner-bar'></div>".repeat(5)}</div>`,
        dots: `<div class="spinner-dots">${"<div class='spinner-dot'></div>".repeat(3)}</div>`,
        upload: `
            <div class="spinner-upload">
                <div class="upload-box">
                    <div class="upload-arrow">⬆️</div>
                </div>
            </div>
            <div class="progress-bar-container">
                <div class="progress-bar"></div>
            </div>
        `
    };
    return `
        <div class="action-icon ${cfg.iconClass}">${cfg.icon}</div>
        ${visuals[cfg.type]}
        <div class="spinner-title" style="color:${cfg.colorTitle}">
            ${cfg.title}
        </div>
        <div class="spinner-subtitle loading-dots" style="color:${cfg.colorSubtitle}">
            ${cfg.subtitle}<span>.</span><span>.</span><span>.</span>
        </div>
    `;
}
// =====================
// Spinner principal
// =====================
export function showSpinner(type = "loading", duration = 0, callback) {
    if (!loadingModal || !spinnerContent)
        return;
    activeSpinner = true;
    if (timeoutRef !== null) {
        clearTimeout(timeoutRef);
        timeoutRef = null;
    }
    const cfg = spinnerTypes[type] ?? spinnerTypes.loading;
    spinnerContent.innerHTML = buildSpinnerHTML(cfg);
    loadingModal.classList.add("show");
    document.body.style.overflow = "hidden";
    if (duration > 0) {
        timeoutRef = window.setTimeout(() => {
            hideSpinner();
            callback?.();
        }, duration);
    }
}
export function hideSpinner() {
    if (!loadingModal)
        return;
    activeSpinner = false;
    loadingModal.classList.remove("show");
    document.body.style.overflow = "";
    if (timeoutRef !== null) {
        clearTimeout(timeoutRef);
        timeoutRef = null;
    }
}
// =====================
// Spinner para Promesas
// =====================
export async function showSpinnerAsync(type, promise) {
    showSpinner(type);
    try {
        const result = await promise;
        hideSpinner();
        return result;
    }
    catch (error) {
        hideSpinner();
        throw error;
    }
}
// =====================
// Modal de resultado
// =====================
export function showResultModal(type, title, message) {
    if (!resultModal || !resultIcon || !resultTitle || !resultMessage || !resultButton)
        return;
    if (type === "success") {
        resultIcon.textContent = "✅";
        resultTitle.style.color = "var(--color-mint-green)";
        resultButton.className = "result-button btn-success";
    }
    else {
        resultIcon.textContent = "❌";
        resultTitle.style.color = "var(--color-burgundy-red)";
        resultButton.className = "result-button btn-error";
    }
    resultTitle.textContent = title;
    resultMessage.textContent = message;
    resultModal.classList.add("show");
    document.body.style.overflow = "hidden";
}
export function hideResultModal() {
    if (!resultModal)
        return;
    resultModal.classList.remove("show");
    document.body.style.overflow = "";
}
// =====================
// ESC handler
// =====================
document.addEventListener("keydown", (e) => {
    if (e.key === "Escape" && resultModal?.classList.contains("show")) {
        hideResultModal();
    }
});
