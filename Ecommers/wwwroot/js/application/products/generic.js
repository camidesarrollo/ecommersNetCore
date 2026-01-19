// Importar todo lo necesario desde select2-init.js
import $, { onNewOptionCreated, addOption, getOptions } from '../utils/select2-init.js';
import { ObtenerProductVariantView } from './products.api.js';
import { showSuccess, showError, showWarning, showInfo } from '../../bundle/notifications/notyf.config.js';

// =============================== 
// CONTADORES GLOBALES 
// =============================== 
window.imageIndex = 0;
window.variantIndex = 0;
window.variantImageIndex = [0];

// =============================== 
// INICIALIZACIÓN 
// =============================== 
document.addEventListener('DOMContentLoaded', function () {
    initializeImageManager();
    initVariants();
    setupSlugAutoGeneration();

    // Inicializar el contador de imágenes existentes
    const existingImages = document.querySelectorAll('#ProductImagesDContainer [data-image-index]');
    if (existingImages.length > 0) {
        imageIndex = Math.max(...Array.from(existingImages).map(img =>
            parseInt(img.getAttribute('data-image-index')) || 0
        )) + 1;
    }

    // Inicializar el contador de imágenes de variantes existentes
    const existingVariantImages = document.querySelectorAll('#ProductVariantImagesDContainer [data-variant-image-index]');
    if (existingVariantImages.length > 0) {
        variantImageIndex = Math.max(...Array.from(existingVariantImages).map(img =>
            parseInt(img.getAttribute('data-variant-image-index')) || 0
        )) + 1;
    }
});

/* ===================================================== 
   GESTIÓN DE IMÁGENES DEL PRODUCTO
   ===================================================== */
function initializeImageManager() {
    const btnAddImage = document.getElementById('btnAddImage');
    if (!btnAddImage) {
        console.error('Botón de agregar imagen no encontrado');
        return;
    }

    btnAddImage.addEventListener('click', function (e) {
        addImageInput(e);
        updateNoImagesMessage();
    });

    const existingImages = document.querySelectorAll(
        '#ProductImagesDContainer .border-2.border-olive-green-300'
    );

    if (existingImages.length === 0) {
        addImageInput(null, true); // 👈 sin event
    }

    updateNoImagesMessage();
}

function addImageInput(event = null, isPrimary = false) {
    let wrapper;

    if (event?.currentTarget) {
        wrapper = event.currentTarget.closest('.product-images-wrapper');
    } else {
        // fallback: primera sección disponible
        wrapper = document.querySelector('.product-images-wrapper');
    }

    if (!wrapper) return;

    const container = wrapper.querySelector('#ProductImagesDContainer');
    if (!container) return;

    addImageBase({
        containerElement: container,
        indexRef: 'imageIndex',
        indexName: 'image',
        namePrefix: 'ProductImages',
        previewFn: 'previewImage',
        updatePrimaryFn: 'updatePrimaryImage',
        removeFn: 'removeImageInput',
        radioName: 'PrimaryImageIndex',
        isPrimaryParam: isPrimary
    });
}

window.previewImage = function (input) {
    handleImagePreview({
        input,
        wrapperSelector: '[data-image-index]',
        previewSelector: 'img[id^="ProductImages_preview_"]',
        iconSelector: 'i[id^="ProductImages_icon_"]'
    });
};

window.removeImageInput = function (index) {
    const imageDiv = document.querySelector(`[data-image-index="${index}"]`);
    if (!imageDiv) return;

    // Verificar si es la imagen principal
    const isPrimaryRadio = imageDiv.querySelector('input[name="PrimaryImageIndex"]');
    const wasPrimary = isPrimaryRadio && isPrimaryRadio.checked;

    // Verificar si es la única imagen
    const allImages = document.querySelectorAll('#ProductImagesDContainer [data-image-index]');
    if (allImages.length <= 1) {
        showWarning('Debe existir al menos una imagen del producto');
        return;
    }

    // Animación de salida
    imageDiv.style.opacity = '0';
    imageDiv.style.transform = 'scale(0.95)';
    imageDiv.style.transition = 'all 0.3s ease';

    setTimeout(() => {
        imageDiv.remove();
        imageIndex--;
        updateNoImagesMessage();
        reindexImages();
        // Si era la imagen principal, seleccionar la primera disponible
        if (wasPrimary) {
            const firstRadio = document.querySelector('input[name="PrimaryImageIndex"]');
            if (firstRadio) {
                firstRadio.checked = true;
                updatePrimaryImage(firstRadio.value);
            }
        }

        showSuccess('Imagen eliminada correctamente');
    }, 300);
};

function reindexImages() {
    const container = document.getElementById('ProductImagesDContainer');
    const images = container.querySelectorAll('[data-image-index]');

    images.forEach((imageDiv, newIndex) => {

        // data-image-index
        imageDiv.setAttribute('data-image-index', newIndex);

        // File input
        const fileInput = imageDiv.querySelector('input[type="file"]');
        fileInput.name = `ProductImages[${newIndex}].ImageFile`;
        fileInput.setAttribute('onchange', `previewImage(this, ${newIndex})`);

        // SortOrder
        const sortInput = imageDiv.querySelector('input[name$=".SortOrder"]');
        sortInput.name = `ProductImages[${newIndex}].SortOrder`;
        sortInput.value = newIndex + 1;

        // Radio
        const radio = imageDiv.querySelector('input[type="radio"]');
        radio.value = newIndex;
        radio.setAttribute('onchange', `updatePrimaryImage(${newIndex})`);

        // Hidden IsPrimary
        const hidden = imageDiv.querySelector('input[type="hidden"]');
        hidden.name = `ProductImages[${newIndex}].IsPrimary`;
        hidden.id = `isPrimary_${newIndex}`;

        // Preview
        const img = imageDiv.querySelector('img');
        img.id = `preview_${newIndex}`;

        const icon = imageDiv.querySelector('i');
        icon.id = `icon_${newIndex}`;

        // Botón eliminar
        const removeBtn = imageDiv.querySelector('button');
        removeBtn.setAttribute('onclick', `removeImageInput(${newIndex})`);
    });

    // 🔥 Actualizar el contador global
    window.imageIndex = images.length;
}

window.updatePrimaryImage = function (index) {
    // Actualizar todos los hidden inputs de IsPrimary
    const allIsPrimaryInputs = document.querySelectorAll('[id^="isPrimary_"]');
    allIsPrimaryInputs.forEach(input => {
        input.value = 'false';
    });

    // Marcar el seleccionado como principal
    const selectedIsPrimary = document.getElementById(`isPrimary_${index}`);
    if (selectedIsPrimary) {
        selectedIsPrimary.value = 'true';
    }
};

window.updateNoImagesMessage = function () {
    const ProductImagesDContainer = document.getElementById('ProductImagesDContainer');
    const noProductImagesDMessage = document.getElementById('noProductImagesDMessage');

    if (!ProductImagesDContainer || !noProductImagesDMessage) return;

    const hasImages = ProductImagesDContainer.querySelectorAll('[data-image-index]').length > 0;
    noProductImagesDMessage.style.display = hasImages ? 'none' : 'block';
};

/* ===================================================== 
   GESTIÓN DE VARIANTES 
   ===================================================== */
function initVariants() {
    // Ajustar el índice basado en variantes existentes
    const existingVariants = document.querySelectorAll('.variant-item');
    if (existingVariants.length > 0) {
        variantIndex = Math.max(...Array.from(existingVariants).map(v =>
            parseInt(v.getAttribute('data-variant-index')) || 0
        )) + 1;
    }

    // Inicializar botones de agregar imágenes de variante
    initVariantImageButtons();

    // Actualizar números de variantes
    updateVariantNumbers();
}

function initVariantImageButtons() {
    // Usar delegación de eventos para manejar botones dinámicos
    document.addEventListener('click', function (e) {
        if (e.target && e.target.id === 'addVariantImage') {
            e.preventDefault();
            addVariantImage(e.target);
            updateNoVariantImagesMessage();
        }
    });
}

async function addVariant() {
    try {
        const response = await ObtenerProductVariantView({ index: variantIndex });

        if (!response.success || !response.data) {
            showError('Error al obtener la vista de la variante');
            return;
        }

        const container = document.getElementById('variantsContainer');
        if (!container) {
            showError('Contenedor de variantes no encontrado');
            return;
        }

        // Eliminar mensaje de "no hay variantes" si existe
        const emptyMessage = container.querySelector('.text-center.py-12');
        if (emptyMessage) {
            emptyMessage.remove();
        }

        // Crear elemento temporal para insertar el HTML
        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = response.data.trim();

        // Obtener el primer elemento hijo válido
        const newVariant = tempDiv.querySelector('.variant-item') || tempDiv.firstElementChild;

        if (!newVariant) {
            console.error('HTML recibido:', response.data);
            showError('Error al procesar la variante');
            return;
        }

        // Agregar al contenedor
        container.appendChild(newVariant);

        // Incrementar el índice
        variantIndex++;

        // Scroll suave hacia la nueva variante
        setTimeout(() => {
            newVariant.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        }, 100);

        // Actualizar números de variantes
        updateVariantNumbers();

        // Mostrar mensaje de éxito
        showSuccess('Variante agregada correctamente');

    } catch (error) {
        console.error('Error al agregar variante:', error);
        showError('Error al agregar la variante: ' + error.message);
    }
}
function removeVariant(event, index) {
    const container = document.getElementById('variantsContainer');
    const variants = container.querySelectorAll('.variant-item');

    // No permitir eliminar si solo hay una variante
    if (variants.length <= 1) {
        showWarning('Debe existir al menos una variante');
        return;
    }

    // Confirmar eliminación
    if (!confirm('¿Está seguro de eliminar esta variante?')) {
        return;
    }

    const variantElement = container.querySelector(`[data-variant-index="${index}"]`);
    if (!variantElement) return;

    // Animación de salida
    variantElement.style.opacity = '0';
    variantElement.style.transform = 'scale(0.95)';
    variantElement.style.transition = 'all 0.3s ease';

    setTimeout(() => {
        variantElement.remove();
        updateVariantNumbers();
        reindexVariant();
        showSuccess('Variante eliminada correctamente');
    }, 300);
}

function updateVariantNumbers() {
    const variants = document.querySelectorAll('.variant-item');
    variants.forEach((variant, index) => {
        const numberSpan = variant.querySelector('.variant-number');
        if (numberSpan) {
            numberSpan.textContent = index + 1;
        }
    });
}

function reindexVariant() {

    const container = document.getElementById('variantsContainer');
    const variants = container.querySelectorAll('[data-variant-index]');
    variants.forEach((variantDiv, newIndex) => {

        // Buscar todos los elementos que tengan name ProductVariants[n].*
        const fields = variantDiv.querySelectorAll('[name^="ProductVariants["]');

        fields.forEach(field => {

            field.name = field.name.replace(
                /ProductVariants\[\d+\]/,
                `ProductVariants[${newIndex}]`
            );

        });

    });
}

/* ===================================================== 
   GESTIÓN DE IMÁGENES DE VARIANTES
   ===================================================== */
function addVariantImage(event) {
    let wrapper;
    let indice = 0
    if (event) {
        wrapper = event.closest('.product-variant-images-wrapper');
        if (event.dataset) {
            indice = event.dataset.variantImageIndex;
        }

    } else {
        // fallback: primera sección disponible
        wrapper = document.querySelector('.product-variant-images-wrapper');
    }

    if (!wrapper) return;

    const container = wrapper.querySelector('#ProductVariantImagesDContainer');
    if (!container) return;

    addImageBase({
        containerElement: container,
        indexRef: 'variantImageIndex',
        indexName: 'variant-image',
        namePrefix: `ProductVariants[${indice}].ProductVariantImages`,
        previewFn: 'previewVariantImage',
        updatePrimaryFn: 'updatePrimaryVariantImage',
        removeFn: 'removeVariantImage',
        radioName: `PrimaryVariantImageIndex_${indice}`
    });
}

window.previewVariantImage = function (input) {
    handleImagePreview({
        input,
        wrapperSelector: '[data-variant-image-index]',
        fallbackWrapperSelector: '.flex.items-start',
        previewSelector: 'img',
        iconSelector: 'i.fa-image'
    });
};

window.removeVariantImage = function (event, index) {
    // contenedor de la variante
    const variantContainer = event.currentTarget.closest('[data-variant-index]');
    if (!variantContainer) return;

    const variantIndex = parseInt(variantContainer.dataset.variantIndex, 10);

    // buscar SOLO dentro de la variante
    const imageDiv = event.currentTarget.closest('[data-variant-image-index]');
    if (!imageDiv) return;

    const isPrimaryRadio = imageDiv.querySelector('input[name="PrimaryVariantImageIndex"]');
    const wasPrimary = isPrimaryRadio && isPrimaryRadio.checked;

    // Animación de salida
    imageDiv.style.opacity = '0';
    imageDiv.style.transform = 'scale(0.95)';
    imageDiv.style.transition = 'all 0.3s ease';

    setTimeout(() => {
        imageDiv.remove();
        console.log(variantImageIndex);
        variantImageIndex[variantIndex]--;
        updateNoVariantImagesMessage();
        reindexVariantImages(variantIndex);

        // Si era principal, seleccionar la primera disponible
        if (wasPrimary) {
            const firstRadio = document.querySelector('input[name="PrimaryVariantImageIndex"]');
            if (firstRadio) {
                firstRadio.checked = true;
                updatePrimaryVariantImage(firstRadio.value);
            }
        }

        showSuccess('Imagen de variante eliminada');
    }, 300);
};


function reindexVariantImages(indiceContenedor) {
    const el = document.querySelector(
        `[data-variant-index="${indiceContenedor}"]`
    );
    if (!el) return;

    const container = el.querySelector('#ProductVariantImagesDContainer');
    if (!container) return;
    const images = container.querySelectorAll('[data-variant-image-index]');

    images.forEach((imageDiv, newIndex) => {

        // data-image-index
        imageDiv.setAttribute('data-variant-image-index', newIndex);

        // File input
        const fileInput = imageDiv.querySelector('input[type="file"]');
        fileInput.name = `ProductVariants[${indiceContenedor}].ProductVariantImages[${newIndex}].ImageFile`;
        fileInput.setAttribute('onchange', `previewImage(this, ${newIndex})`);

        // SortOrder
        const sortInput = imageDiv.querySelector('input[name$=".SortOrder"]');
        sortInput.name = `ProductVariants[${indiceContenedor}].ProductVariantImages[${newIndex}].SortOrder`;
        sortInput.value = newIndex + 1;

        // Radio
        const radio = imageDiv.querySelector('input[type="radio"]');
        radio.value = newIndex;
        radio.setAttribute('onchange', `updatePrimaryImage(${newIndex})`);

        // Hidden IsPrimary
        const hidden = imageDiv.querySelector('input[type="hidden"]');
        hidden.name = `ProductVariants[${indiceContenedor}].ProductVariantImages[${newIndex}].IsPrimary`;
        hidden.id = `isPrimary_${newIndex}`;

        // Preview
        const img = imageDiv.querySelector('img');
        img.id = `preview_${newIndex}`;

        const icon = imageDiv.querySelector('i');
        icon.id = `icon_${newIndex}`;

        // Botón eliminar
        const removeBtn = imageDiv.querySelector('button');
        removeBtn.setAttribute('onclick', `removeImageInput(${newIndex})`);
    });

    // 🔥 Actualizar el contador global
    window.imageIndex = images.length;
}

window.updatePrimaryVariantImage = function (index) {
    document.querySelectorAll('[id^="variant_isPrimary_"]').forEach(input => {
        input.value = 'false';
    });

    const selected = document.getElementById(`variant_isPrimary_${index}`);
    if (selected) {
        selected.value = 'true';
    }
};

function updateNoVariantImagesMessage() {
    const container = document.getElementById('ProductVariantImagesDContainer');
    const message = document.getElementById('noProductVariantImagesDMessage');

    if (!container || !message) return;

    const hasImages = container.querySelectorAll('[data-variant-image-index]').length > 0;
    message.style.display = hasImages ? 'none' : 'block';
}

/* ===================================================== 
   SLUG AUTOMÁTICO 
   ===================================================== */
function setupSlugAutoGeneration() {
    const nameInput = document.querySelector('[name="Products.Name"]');
    const slugInput = document.querySelector('[name="Products.Slug"]');

    if (!nameInput || !slugInput) {
        console.warn('Inputs de nombre o slug no encontrados');
        return;
    }

    nameInput.addEventListener('input', e => {
        if (!slugInput.value || slugInput.dataset.autoGenerated === 'true') {
            slugInput.value = generateSlug(e.target.value);
            slugInput.dataset.autoGenerated = 'true';
        }
    });

    // Si el usuario edita el slug manualmente, desactivar auto-generación
    slugInput.addEventListener('input', () => {
        if (slugInput.dataset.autoGenerated === 'true') {
            delete slugInput.dataset.autoGenerated;
        }
    });
}

function generateSlug(text) {
    return text
        .toLowerCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '') // Eliminar acentos
        .replace(/[^a-z0-9\s-]/g, '') // Solo letras, números, espacios y guiones
        .trim()
        .replace(/\s+/g, '-') // Espacios a guiones
        .replace(/-+/g, '-'); // Múltiples guiones a uno solo
}
function generateSKU(value) {
    const words = value
        .normalize("NFD")
        .replace(/[\u0300-\u036f]/g, "")
        .toUpperCase()
        .replace(/[^A-Z0-9 ]/g, "")
        .trim()
        .split(/\s+/);

    const result = [];

    for (let i = 0; i < words.length; i++) {
        const word = words[i];

        // si es KG y el elemento anterior es número → unir
        if (word === "KG" && result.length && /^\d+$/.test(result[result.length - 1])) {
            result[result.length - 1] += "KG";
            continue;
        }

        // si es solo número
        if (/^\d+$/.test(word)) {
            result.push(word);
            continue;
        }

        // palabras → 3 primeras letras
        result.push(word.substring(0, 3));
    }

    return result.join("-");
}

// tiempo real
document.addEventListener("input", function (e) {
    if (!e.target.classList.contains("variant-name")) return;

    const container = e.target.closest("[data-variant-index]");
    if (!container) return;

    const skuInput = container.querySelector(".variant-sku");
    if (!skuInput) return;

    skuInput.value = generateSKU(e.target.value);
});

/* ===================================================== 
   EXPONER FUNCIONES GLOBALMENTE 
   ===================================================== */
window.addVariant = addVariant;
window.removeVariant = removeVariant;
window.addVariantImage = addVariantImage;
window.removeVariantImage = removeVariantImage;
window.previewVariantImage = previewVariantImage;
window.updatePrimaryVariantImage = updatePrimaryVariantImage;
window.updateNoVariantImagesMessage = updateNoVariantImagesMessage;