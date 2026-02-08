/**
 * ============================================
 * SISTEMA GLOBAL DE LOADING + SPINNERS (TS)
 * ============================================
 */

// =====================
// Tipos
// =====================
type SpinnerType =
    | "creating"
    | "editing"
    | "deleting"
    | "uploading"
    | "processing"
    | "saving"
    | "loading";

type SpinnerVisualType =
    | ""
    | "circle"
    | "bars"
    | "dots"
    | "upload";

interface SpinnerConfig {
    icon: string;
    iconClass: string;
    title: string;
    subtitle: string;
    colorTitle: string;
    colorSubtitle: string;
    type: SpinnerVisualType;
}

// =====================
// Referencias DOM
// =====================
const loadingModal = document.getElementById("loadingModal") as HTMLElement | null;
const spinnerContent = document.getElementById("spinnerContent") as HTMLElement | null;

const resultModal = document.getElementById("resultModal") as HTMLElement | null;
const resultIcon = document.getElementById("resultIcon") as HTMLElement | null;
const resultTitle = document.getElementById("resultTitle") as HTMLElement | null;
const resultMessage = document.getElementById("resultMessage") as HTMLElement | null;
const resultButton = document.getElementById("resultButton") as HTMLButtonElement | null;

// =====================
// Estados globales
// =====================
let activeSpinner: boolean = false;
let timeoutRef: number | null = null;

// =====================
// Configuración spinners
// =====================
const spinnerTypes: Record<SpinnerType, SpinnerConfig> = {
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
function buildSpinnerHTML(cfg: SpinnerConfig): string {
    const visuals: Record<SpinnerVisualType, string> = {
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
export function showSpinner(
    type: SpinnerType = "loading",
    duration: number = 0,
    callback?: () => void
): void {
    if (!loadingModal || !spinnerContent) return;

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

export function hideSpinner(): void {
    if (!loadingModal) return;

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
export async function showSpinnerAsync<T>(
    type: SpinnerType,
    promise: Promise<T>
): Promise<T> {
    showSpinner(type);

    try {
        const result = await promise;
        hideSpinner();
        return result;
    } catch (error) {
        hideSpinner();
        throw error;
    }
}

// =====================
// Modal de resultado
// =====================
export function showResultModal(
    type: "success" | "error",
    title: string,
    message: string
): void {
    if (!resultModal || !resultIcon || !resultTitle || !resultMessage || !resultButton) return;

    if (type === "success") {
        resultIcon.textContent = "✅";
        resultTitle.style.color = "var(--color-mint-green)";
        resultButton.className = "result-button btn-success";
    } else {
        resultIcon.textContent = "❌";
        resultTitle.style.color = "var(--color-burgundy-red)";
        resultButton.className = "result-button btn-error";
    }

    resultTitle.textContent = title;
    resultMessage.textContent = message;

    resultModal.classList.add("show");
    document.body.style.overflow = "hidden";
}

export function hideResultModal(): void {
    if (!resultModal) return;
    resultModal.classList.remove("show");
    document.body.style.overflow = "";
}

// =====================
// Bind de eventos UI
// =====================
function bindResultModalEvents(): void {
    if (!resultButton) return;

    resultButton.addEventListener("click", () => {
        hideResultModal();
    });
}

// =====================
// Eventos globales
// =====================
function bindGlobalEvents(): void {
    document.addEventListener("keydown", (e: KeyboardEvent) => {
        if (e.key !== "Escape") return;

        if (resultModal?.classList.contains("show")) {
            hideResultModal();
            return;
        }

        if (loadingModal?.classList.contains("show")) {
            hideSpinner();
        }
    });
}

// =====================
// Inicializador UI global
// =====================
export function initGlobalUI(): void {
    bindResultModalEvents();
    bindGlobalEvents();
}

// En spinner.ts, añade al final:
export type { SpinnerType };