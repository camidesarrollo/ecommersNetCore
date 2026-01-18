// Importar todo lo necesario desde select2-init.js
import $, { onNewOptionCreated, addOption, getOptions } from '../utils/select2-init.js';
import { ObtenerProductVariantView } from './products.api.js';
import { showSuccess, showError, showWarning, showInfo } from '../../bundle/notifications/notyf.config.js';

// =============================== 
// CONTADORES GLOBALES 
// =============================== 
let imageIndex = 0;
let variantIndex = 0;
let variantImageIndex = 0;

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
    const ProductImagesDContainer = document.getElementById('ProductImagesDContainer');
    if (!ProductImagesDContainer) {
        console.error('Contenedor de imágenes no encontrado');
        return;
    }

    const currentIndex = imageIndex++;
    const isFirstImage = ProductImagesDContainer.querySelectorAll('.border-2.border-olive-green-300').length === 0;
    const shouldBePrimary = isPrimary || isFirstImage;

    const imageDiv = document.createElement('div');
    imageDiv.className = 'border-2 border-olive-green-300 rounded-lg p-5 bg-white hover:border-olive-green-500 transition-all duration-300 shadow-sm hover:shadow-md';
    imageDiv.setAttribute('data-image-index', currentIndex);

    imageDiv.innerHTML = `
        <div class="flex items-start gap-4">
            <!-- Preview de la imagen -->
            <div class="flex-shrink-0">
                <div class="w-28 h-28 border-2 border-gray-300 rounded-lg overflow-hidden bg-gray-50 flex items-center justify-center">
                    <img id="preview_${currentIndex}" src="" alt="Preview" class="w-full h-full object-cover hidden">
                    <i id="icon_${currentIndex}" class="fas fa-image text-4xl text-gray-400"></i>
                </div>
            </div>

            <!-- Controles de la imagen -->
            <div class="flex-1 space-y-4">
                <!-- Input de archivo -->
                <div>
                    <label class="block font-semibold mb-2">
                        Archivo de imagen <span class="text-red-700">*</span>
                    </label>
                    <input type="file" 
                           name="ProductImages[${currentIndex}].ImageFile" 
                           accept="image/*" 
                           data-max-size="5" 
                           data-allowed-types="image/jpeg,image/png,image/webp" 
                           required 
                           onchange="previewImage(this, ${currentIndex})" 
                           class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg focus:border-olive-green-500 focus:ring-olive-green-500/20 outline-none transition-all duration-300 file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:bg-olive-green-500 file:text-white file:font-semibold hover:file:bg-olive-green-600 file:cursor-pointer cursor-pointer">
                    <p class="text-xs text-gray-500 mt-2 flex items-center gap-1">
                        <i class="fas fa-info-circle"></i>
                        Formatos: JPG, PNG, WebP (Máx: 5MB)
                    </p>
                </div>

                <!-- Orden y Principal -->
                <div class="grid grid-cols-2 gap-4">
                    <div>
                        <label class="block font-semibold mb-2 text-sm">
                            Orden <span class="text-red-700">*</span>
                        </label>
                        <input type="number" 
                               name="ProductImages[${currentIndex}].SortOrder" 
                               value="${currentIndex + 1}" 
                               min="0" 
                               required 
                               class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg focus:border-olive-green-500 focus:ring-olive-green-500 outline-none transition">
                    </div>

                    <div class="flex items-end">
                        <label class="flex items-center gap-2 cursor-pointer p-3 rounded-lg hover:bg-olive-green-50 transition-colors">
                            <input type="radio" 
                                   name="PrimaryImageIndex" 
                                   value="${currentIndex}" 
                                   ${shouldBePrimary ? 'checked' : ''} 
                                   onchange="updatePrimaryImage(${currentIndex})" 
                                   class="w-5 h-5 text-olive-green-600 focus:ring-olive-green-500 cursor-pointer">
                            <span class="text-sm font-semibold text-dark-chocolate">Imagen principal</span>
                        </label>
                    </div>
                </div>

                <!-- Hidden para IsPrimary -->
                <input type="hidden" 
                       name="ProductImages[${currentIndex}].IsPrimary" 
                       id="isPrimary_${currentIndex}" 
                       value="${shouldBePrimary ? 'true' : 'false'}">
            </div>

            <!-- Botón eliminar -->
            <button type="button" 
                    onclick="removeImageInput(${currentIndex})" 
                    class="flex-shrink-0 text-red-600 hover:text-white hover:bg-red-600 transition-all duration-200 p-3 rounded-lg" 
                    title="Eliminar imagen">
                <i class="fas fa-trash-alt text-lg"></i>
            </button>
        </div>
    `;

    ProductImagesDContainer.appendChild(imageDiv);

    // Scroll suave hacia la nueva imagen
    setTimeout(() => {
        imageDiv.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }, 100);
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
    const container = document.getElementById('ProductVariantImagesDContainer');
    if (!container) {
        console.error('Contenedor de imágenes de variante no encontrado');
        return;
    }

    const currentIndex = variantImageIndex++;
    const isFirstImage = container.querySelectorAll('[data-variant-image-index]').length === 0;

    const imageDiv = document.createElement('div');
    imageDiv.className = 'border-2 border-olive-green-300 rounded-lg p-5 bg-white hover:border-olive-green-500 transition-all duration-300 shadow-sm hover:shadow-md';
    imageDiv.setAttribute('data-variant-image-index', currentIndex);

    imageDiv.innerHTML = `
        <div class="flex items-start gap-4">
            <!-- Preview -->
            <div class="flex-shrink-0">
                <div class="w-28 h-28 border-2 border-gray-300 rounded-lg overflow-hidden bg-gray-50 flex items-center justify-center">
                    <img id="variant_preview_${currentIndex}" src="" alt="Preview" class="w-full h-full object-cover hidden">
                    <i id="variant_icon_${currentIndex}" class="fas fa-image text-4xl text-gray-400"></i>
                </div>
            </div>

            <!-- Controles -->
            <div class="flex-1 space-y-4">
                <div>
                    <label class="block font-semibold mb-2">
                        Archivo de imagen <span class="text-red-700">*</span>
                    </label>
                    <input type="file" 
                           name="ProductVariantImages[${currentIndex}].ImageFile" 
                           accept="image/*" 
                           required 
                           onchange="previewVariantImage(this, ${currentIndex})" 
                           class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg focus:border-olive-green-500 outline-none transition file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:bg-olive-green-500 file:text-white file:font-semibold hover:file:bg-olive-green-600 file:cursor-pointer cursor-pointer">
                    <p class="text-xs text-gray-500 mt-2">
                        <i class="fas fa-info-circle"></i> Formatos: JPG, PNG, WebP (Máx: 5MB)
                    </p>
                </div>

                <div class="grid grid-cols-2 gap-4">
                    <div>
                        <label class="block font-semibold mb-2 text-sm">
                            Orden <span class="text-red-700">*</span>
                        </label>
                        <input type="number" 
                               name="ProductVariantImages[${currentIndex}].SortOrder" 
                               value="${currentIndex + 1}" 
                               min="0" 
                               required 
                               class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg focus:border-olive-green-500 outline-none transition">
                    </div>

                    <div class="flex items-end">
                        <label class="flex items-center gap-2 cursor-pointer p-3 rounded-lg hover:bg-olive-green-50 transition-colors">
                            <input type="radio" 
                                   name="PrimaryVariantImageIndex" 
                                   value="${currentIndex}" 
                                   ${isFirstImage ? 'checked' : ''} 
                                   onchange="updatePrimaryVariantImage(${currentIndex})" 
                                   class="w-5 h-5 text-olive-green-600 focus:ring-olive-green-500 cursor-pointer">
                            <span class="text-sm font-semibold text-dark-chocolate">Imagen principal</span>
                        </label>
                    </div>
                </div>

                <input type="hidden" 
                       name="ProductVariantImages[${currentIndex}].IsPrimary" 
                       id="variant_isPrimary_${currentIndex}" 
                       value="${isFirstImage ? 'true' : 'false'}">
            </div>

            <button type="button" 
                    onclick="removeVariantImage(${currentIndex})" 
                    class="flex-shrink-0 text-red-600 hover:text-white hover:bg-red-600 transition-all duration-200 p-3 rounded-lg" 
                    title="Eliminar imagen">
                <i class="fas fa-trash-alt text-lg"></i>
            </button>
        </div>
    `;

    container.appendChild(imageDiv);

    // Scroll suave
    setTimeout(() => {
        imageDiv.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }, 100);
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