import { isImageFile } from "../../domain/utils/validators.js";
import {  showError } from '../../bundle/notifications/notyf.config.js';
function addImageBase(config) {
    const {
        containerElement,
        indexRef,
        indexName,
        namePrefix,
        previewFn,
        updatePrimaryFn,
        removeFn,
        radioName,
        isPrimaryParam = false
    } = config;

    console.log(config);
    const container = containerElement;
    if (!container) {
        console.error(`Contenedor ${container} no encontrado`);
        return;
    }

    let currentIndex;

    if (Array.isArray(window[indexRef])) {
        const match = namePrefix.match(/\[(\d+)\]/);

        if (!match) {
            console.error('No se pudo extraer el índice desde namePrefix:', namePrefix);
            return;
        }

        var indexElementoPadre = parseInt(match[1], 10);
        // Asegurar que exista la posición para la variante
        if (typeof window[indexRef][indexElementoPadre] !== 'number') {
            window[indexRef][indexElementoPadre] = 0;
        }

        currentIndex = window[indexRef][indexElementoPadre];
        window[indexRef][indexElementoPadre]++;

    } else {
        // Fallback: contador simple
        if (typeof window[indexRef] !== 'number') {
            window[indexRef] = 0;
        }

        currentIndex = window[indexRef];
        window[indexRef]++;
    }
    
    const isFirstImage = container.querySelectorAll(`[data-${indexName}-index]`).length === 0;
    const shouldBePrimary = isPrimaryParam || isFirstImage;

    const imageDiv = document.createElement('div');
    imageDiv.className = 'border-2 border-olive-green-300 rounded-lg p-3 md:p-5 bg-white hover:border-olive-green-500 transition-all duration-300 shadow-sm hover:shadow-md';
    imageDiv.setAttribute(`data-${indexName}-index`, currentIndex);

    imageDiv.innerHTML = `
        <div class="image-block-layout">
            <!-- Preview -->
            <div class="image-preview-container">
                <div class="image-preview-box">
                    <img id="${namePrefix}_preview_${currentIndex}" class="w-full h-full object-cover hidden">
                    <i id="${namePrefix}_icon_${currentIndex}" class="fas fa-image text-3xl md:text-4xl text-gray-400"></i>
                </div>
            </div>

            <!-- Controles -->
            <div class="image-controls-container">
                <div>
                    <label for="${namePrefix}_file_${currentIndex}" class="block font-semibold mb-2 text-sm md:text-base">
                        Archivo de imagen <span class="text-red-700">*</span>
                    </label>
                    <input type="file"
                        id="${namePrefix}_file_${currentIndex}"
                        name="${namePrefix}[${currentIndex}].ImageFile"
                        accept="image/*"
                        required
                        onchange="${previewFn}(this, ${currentIndex})"
                        class="image-file-input">
                    <p class="text-xs text-gray-500 mt-2 flex items-center gap-1">
                        <i class="fas fa-info-circle"></i>
                        Formatos: JPG, JPEG, PNG, GIF, WebP, SVG, BMP
                    </p>
                </div>

                <div class="image-order-grid">
                    <div>
                        <label for="${namePrefix}_sortorder_${currentIndex}" class="block font-semibold mb-2 text-xs md:text-sm">
                            Orden <span class="text-red-700">*</span>
                        </label>
                        <input type="number"
                            id="${namePrefix}_sortorder_${currentIndex}"
                            name="${namePrefix}[${currentIndex}].SortOrder"
                            value="${currentIndex + 1}"
                            min="0"
                            required
                            class="w-full px-3 md:px-4 py-2 md:py-3 text-sm md:text-base border-2 border-gray-300 rounded-lg focus:border-olive-green-500 outline-none transition">
                    </div>

                    <div class="flex items-end">
                        <label for="${namePrefix}_primary_${currentIndex}" class="image-primary-label">
                            <input type="radio"
                                id="${namePrefix}_primary_${currentIndex}"
                                name="${radioName}"
                                value="${currentIndex}"
                                ${shouldBePrimary ? 'checked' : ''}
                                onchange="${updatePrimaryFn}(${currentIndex})"
                                class="w-4 h-4 md:w-5 md:h-5 text-olive-green-600 focus:ring-olive-green-500 cursor-pointer flex-shrink-0">
                            <span class="text-xs md:text-sm font-semibold text-dark-chocolate whitespace-nowrap">Imagen principal</span>
                        </label>
                    </div>
                </div>

                <input type="hidden"
                    name="${namePrefix}[${currentIndex}].IsPrimary"
                    id="isPrimary_${currentIndex}"
                    value="${shouldBePrimary}">
            </div>

            <button type="button"
                onclick="${removeFn}(event, ${currentIndex})"
                class="image-delete-button"
                title="Eliminar imagen"
                aria-label="Eliminar imagen">
                <i class="fas fa-trash-alt text-base md:text-lg"></i>
            </button>
        </div>
    `;

    container.appendChild(imageDiv);

    // Ocultar mensaje de "no hay imágenes" si existe
    const noImageMessage = document.querySelector(`#no${indexName}Message`);
    if (noImageMessage) {
        noImageMessage.classList.add('hidden');
    }

    setTimeout(() => {
        imageDiv.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }, 100);
}

function handleImagePreview({
    input,
    wrapperSelector,
    fallbackWrapperSelector = null,
    previewSelector,
    iconSelector
}) {
    const wrapper =
        input.closest(wrapperSelector) ||
        (fallbackWrapperSelector ? input.closest(fallbackWrapperSelector) : null);

    if (!wrapper) {
        console.error('No se encontró el contenedor de la imagen');
        return;
    }

    const preview = wrapper.querySelector(previewSelector);
    const icon = wrapper.querySelector(iconSelector);

    if (!preview || !icon) {
        console.error('Preview o icono no encontrados');
        return;
    }

    if (!input.files || !input.files[0]) return;

    const file = input.files[0];

    // Validaciones
    const maxSize = 5 * 1024 * 1024;
    const allowedTypes = ['image/jpeg', 'image/png', 'image/webp', 'image/jpg'];

    if (file.size > maxSize) {
        showError('La imagen no puede superar los 5MB');
        input.value = '';
        return;
    }

    if (!allowedTypes.includes(file.type)) {
        showError('Formato no permitido. Use JPG, PNG o WebP');
        input.value = '';
        return;
    }

    const reader = new FileReader();
    reader.onload = e => {
        preview.src = e.target.result;
        preview.classList.remove('hidden');
        icon.classList.add('hidden');
    };

    const imageDiv = wrapper.closest('[data-image-index]');
    if (imageDiv) {
        imageDiv.classList.remove('shake');
    }

    reader.readAsDataURL(file);
}

function markImageHelperError(input, message) {
    const wrapper = input.closest('[data-image-index]');
    if (!wrapper) return;

    const helperText = wrapper.querySelector('.text-xs');
    if (!helperText) return;

    helperText.classList.remove('text-gray-500');
    helperText.classList.add('text-red-500');
    if (message){
        helperText.textContent = message;
    }
}

function clearImageHelperError(input) {
    const wrapper = input.closest('[data-image-index]');
    if (!wrapper) return;

    const helperText = wrapper.querySelector('.text-xs');
    if (!helperText) return;

    helperText.classList.remove('text-red-500');
    helperText.classList.add('text-gray-500');
}

function validateImageUpload(input) {
    const file = input.files && input.files[0];
    if (!file) return false;

    // Validar extensión usando tu util
    if (!isImageFile(file.name)) {
        showError('El archivo seleccionado no es una imagen válida');

        markImageHelperError(input, null);

        input.value = '';
        return false;
    }else{
        clearImageHelperError(input);
    }

    return true;
}
window.addImageBase = addImageBase;
window.handleImagePreview = handleImagePreview;
window.markImageHelperError = markImageHelperError;
window.clearImageHelperError = clearImageHelperError;
window.validateImageUpload = validateImageUpload;