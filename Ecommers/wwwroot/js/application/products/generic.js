// Importar todo lo necesario desde select2-init.js
import $, { onNewOptionCreated, addOption, getOptions } from '../utils/select2-init.js';
import { ObtenerProductVariantView } from './products.api.js';
import { showSuccess, showError, showWarning, showInfo } from '../../bundle/notifications/notyf.config.js';

// =============================== 
// CONTADORES GLOBALES 
// =============================== 
window.imageIndex = 0;
window.variantIndex = 0;
window.variantImageIndex = 0;

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

    // Evento para agregar nueva imagen
    btnAddImage.addEventListener('click', function () {
        addImageInput();
        updateNoImagesMessage();
    });

    // Verificar si hay imágenes existentes
    const existingImages = document.querySelectorAll('#ProductImagesDContainer .border-2.border-olive-green-300');
    if (existingImages.length === 0) {
        addImageInput(true); // Agregar primera imagen como principal
    }

    updateNoImagesMessage();
}

function addImageInput(isPrimary = false) {
    addImageBase({
        containerId: 'ProductImagesDContainer',
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

window.previewImage = function (input, index) {
    const preview = document.getElementById(`preview_${index}`);
    const icon = document.getElementById(`icon_${index}`);

    if (!input.files || !input.files[0]) return;

    const file = input.files[0];

    // Validar tamaño (5MB)
    const maxSize = 5 * 1024 * 1024;
    if (file.size > maxSize) {
        showError('El archivo es demasiado grande. El tamaño máximo es 5MB.');
        input.value = '';
        return;
    }

    // Validar tipo
    const allowedTypes = ['image/jpeg', 'image/png', 'image/webp', 'image/jpg'];
    if (!allowedTypes.includes(file.type)) {
        showError('Tipo de archivo no permitido. Solo se aceptan JPG, PNG y WebP.');
        input.value = '';
        return;
    }

    const reader = new FileReader();
    reader.onload = function (e) {
        preview.src = e.target.result;
        preview.classList.remove('hidden');
        icon.classList.add('hidden');
    };
    reader.readAsDataURL(file);
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
        updateNoImagesMessage();

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
        if (e.target && e.target.id === 'btnAddVariantImages') {
            e.preventDefault();
            addVariantImage();
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
function removeVariant(index) {
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

/* ===================================================== 
   GESTIÓN DE IMÁGENES DE VARIANTES
   ===================================================== */
function addVariantImage() {
    addImageBase({
        containerId: 'ProductVariantImagesDContainer',
        indexRef: 'variantImageIndex',
        indexName: 'variant-image',
        namePrefix: 'ProductVariantImages',
        previewFn: 'previewVariantImage',
        updatePrimaryFn: 'updatePrimaryVariantImage',
        removeFn: 'removeVariantImage',
        radioName: 'PrimaryVariantImageIndex'
    });
}

window.previewVariantImage = function (input, index) {
    const preview = document.getElementById(`variant_preview_${index}`);
    const icon = document.getElementById(`variant_icon_${index}`);

    if (!input.files || !input.files[0]) return;

    const file = input.files[0];
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
    reader.readAsDataURL(file);
};

window.removeVariantImage = function (index) {
    const imageDiv = document.querySelector(`[data-variant-image-index="${index}"]`);
    if (!imageDiv) return;

    const isPrimaryRadio = imageDiv.querySelector('input[name="PrimaryVariantImageIndex"]');
    const wasPrimary = isPrimaryRadio && isPrimaryRadio.checked;

    // Animación de salida
    imageDiv.style.opacity = '0';
    imageDiv.style.transform = 'scale(0.95)';
    imageDiv.style.transition = 'all 0.3s ease';

    setTimeout(() => {
        imageDiv.remove();
        updateNoVariantImagesMessage();

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