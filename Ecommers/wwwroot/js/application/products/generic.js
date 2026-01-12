// Importar todo lo necesario desde select2-init.js
import $, { onNewOptionCreated, addOption, getOptions }
    from '../utils/select2-init.js';  // <-- ruta relativa desde generic.js

import { ObtenerProductVariantView } from './products.api.js';

// Importar sistema de notificaciones Notyf
import { showSuccess, showError, showWarning, showInfo } from '../../bundle/notifications/notyf.config.js'

// ===============================
// CONTADORES GLOBALES
// ===============================
let imageIndex = 0;
let variantIndex = 0;

// ===============================
// INICIALIZACIÓN
// ===============================
document.addEventListener('DOMContentLoaded', function () {
    initializeImageManager();
    initVariants();
});

/* =====================================================
   GESTIÓN DE IMÁGENES
===================================================== */
function initializeImageManager() {
    const btnAddImage = document.getElementById('btnAddImage');

    if (!btnAddImage) {
        //console.error('Botón de agregar imagen no encontrado');
        return;
    }

    // Evento para agregar nueva imagen
    btnAddImage.addEventListener('click', function () {
        addImageInput();
        updateNoImagesMessage();
    });

    // Agregar una imagen por defecto al cargar
    if ($("#imagesContainer .border-olive-green-300").length == 0) {
        addImageInput(true);
        updateNoImagesMessage();
    }
}

function addImageInput(isPrimary = false) {
    const imagesContainer = document.getElementById('imagesContainer');
    const currentIndex = imageIndex++;

    const imageDiv = document.createElement('div');
    imageDiv.className = 'border-2 border-olive-green-300 rounded-lg p-5 bg-white hover:border-olive-green-500 transition-all duration-300 shadow-sm hover:shadow-md';
    imageDiv.setAttribute('data-image-index', currentIndex);

    imageDiv.innerHTML = `
                <div class="flex items-start gap-4">
                    <!-- Preview de la imagen -->
                    <div class="flex-shrink-0">
                        <div class="w-28 h-28 border-2 border-gray-300 rounded-lg overflow-hidden bg-gray-50 flex items-center justify-center">
                            <img id="preview_${currentIndex}"
                                 src=""
                                 alt="Preview"
                                 class="w-full h-full object-cover hidden" />
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
                                   class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg focus:border-olive-green-500 focus:ring-olive-green-500/20 outline-none transition-all duration-300 file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:bg-olive-green-500 file:text-white file:font-semibold hover:file:bg-olive-green-600 file:cursor-pointer cursor-pointer" />
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
                                       name="ProductImages[${currentIndex}].Order"
                                       value="${currentIndex + 1}"
                                       min="0"
                                       required
                                       class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg focus:border-olive-green-500 focus:ring-olive-green-500 outline-none transition" />
                            </div>

                            <div class="flex items-end">
                                <label class="flex items-center gap-2 cursor-pointer p-3 rounded-lg hover:bg-olive-green-50 transition-colors">
                                    <input type="radio"
                                           name="PrimaryImageIndex"
                                           value="${currentIndex}"
                                           ${isPrimary ? 'checked' : ''}
                                           onchange="updatePrimaryImage(${currentIndex})"
                                           class="w-5 h-5 text-olive-green-600 focus:ring-olive-green-500 cursor-pointer" />
                                    <span class="text-sm font-semibold text-dark-chocolate">Imagen principal</span>
                                </label>
                            </div>
                        </div>

                        <!-- Hidden para IsPrimary -->
                        <input type="hidden"
                               name="ProductImages[${currentIndex}].IsPrimary"
                               id="isPrimary_${currentIndex}"
                               value="${isPrimary}" />
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

    imagesContainer.appendChild(imageDiv);
}

window.previewImage = function (input, index) {
    const preview = document.getElementById(`preview_${index}`);
    const icon = document.getElementById(`icon_${index}`);

    if (input.files && input.files[0]) {
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
    }
}

window.removeImageInput = function (index) {
    const imageDiv = document.querySelector(`[data-image-index="${index}"]`);
    if (imageDiv) {
        // Verificar si es la imagen principal
        const isPrimaryRadio = imageDiv.querySelector('input[name="PrimaryImageIndex"]');
        const wasPrimary = isPrimaryRadio && isPrimaryRadio.checked;

        imageDiv.remove();
        updateNoImagesMessage();

        // Si era la imagen principal, seleccionar la primera disponible
        if (wasPrimary) {
            const firstRadio = document.querySelector('input[name="PrimaryImageIndex"]');
            if (firstRadio) {
                firstRadio.checked = true;
                const firstIndex = firstRadio.value;
                updatePrimaryImage(firstIndex);
            }
        }

        showSuccess('Imagen eliminada correctamente');
    }
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
}

window.updateNoImagesMessage = function () {
    const imagesContainer = document.getElementById('imagesContainer');
    const noImagesMessage = document.getElementById('noImagesMessage');
    const hasImages = imagesContainer.children.length > 0;

    if (noImagesMessage) {
        noImagesMessage.style.display = hasImages ? 'none' : 'block';
    }
}

/* =====================================================
   GESTIÓN DE VARIANTES
===================================================== */
function initVariants() {
    // Si hay variantes existentes, ajustar el índice
    const existingVariants = document.querySelectorAll('.variant-item');
    if (existingVariants.length > 0) {
        variantIndex = existingVariants.length;
    }
}

async function addVariant() {
    try {
        const response = await ObtenerProductVariantView({ index: variantIndex });

        if (response.success && response.data) {
            const container = document.getElementById('variantsContainer');

            if (!container) {
                showError('Contenedor de variantes no encontrado');
                return;
            }

            // Verificar si hay mensaje de "no hay variantes" y eliminarlo
            const emptyMessage = container.querySelector('.text-center.py-12');
            if (emptyMessage) {
                emptyMessage.remove();
            }

            // Crear un elemento temporal para insertar el HTML
            const tempDiv = document.createElement('div');
            tempDiv.innerHTML = response.data.trim();

            // Buscar el primer elemento hijo que sea un elemento válido
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
                newVariant.scrollIntoView({
                    behavior: 'smooth',
                    block: 'nearest'
                });
            }, 100);

            // Actualizar números de variantes
            updateVariantNumbers();

            // Mostrar mensaje de éxito
            showSuccess('Variante agregada correctamente');
        } else {
            showError('Error al agregar la variante');
        }
    } catch (error) {
        console.error('Error al agregar variante:', error);
        console.error('Detalles:', error.stack);
        showError('Error al agregar la variante');
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
    if (confirm('¿Está seguro de eliminar esta variante?')) {
        const variantElement = container.querySelector(`[data-variant-index="${index}"]`);

        if (variantElement) {
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
    }
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

function addVariantImage(variantIndex) {
    const container = document.getElementById(`variantImages_${variantIndex}`);

    if (!container) {
        showError('Contenedor de imágenes no encontrado');
        return;
    }

    const imageCount = container.querySelectorAll('.variant-image-item').length;

    const imageHtml = `
        <div class="variant-image-item border border-gray-200 rounded-lg p-3 bg-gray-50">
            <div class="flex items-center gap-3">
                <div class="flex-1">
                    <input type="file" 
                           name="ProductVariants[${variantIndex}].Images[${imageCount}].File"
                           accept="image/*"
                           class="w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:text-sm file:font-semibold file:bg-olive-green-100 file:text-olive-green-700 hover:file:bg-olive-green-200"
                           onchange="previewVariantImage(this, ${variantIndex}, ${imageCount})" />
                </div>
                <button type="button" 
                        onclick="removeVariantImage(this)"
                        class="text-red-500 hover:text-red-700 transition">
                    <i class="fas fa-times"></i>
                </button>
            </div>
            
            <div class="mt-3 flex items-center gap-4 text-sm">
                <label class="flex items-center gap-2 cursor-pointer">
                    <input type="checkbox" 
                           name="ProductVariants[${variantIndex}].Images[${imageCount}].IsPrimary"
                           class="w-4 h-4 text-olive-green-600 border-gray-300 rounded" />
                    <span><i class="fas fa-star text-amber-500"></i> Principal</span>
                </label>
                
                <label class="flex items-center gap-2 cursor-pointer">
                    <input type="checkbox" 
                           name="ProductVariants[${variantIndex}].Images[${imageCount}].IsActive"
                           class="w-4 h-4 text-olive-green-600 border-gray-300 rounded"
                           checked />
                    <span><i class="fas fa-eye text-green-600"></i> Visible</span>
                </label>
            </div>
            
            <div id="preview_${variantIndex}_${imageCount}" class="mt-3 hidden">
                <img src="" class="w-full rounded-lg shadow-sm max-h-48 object-cover" alt="Preview" />
            </div>
            
            <input type="hidden" name="ProductVariants[${variantIndex}].Images[${imageCount}].Order" value="${imageCount}" />
        </div>
    `;

    container.insertAdjacentHTML('beforeend', imageHtml);
    showSuccess('Imagen agregada a la variante');
}

function removeVariantImage(button) {
    const imageItem = button.closest('.variant-image-item');
    if (imageItem) {
        imageItem.style.opacity = '0';
        imageItem.style.transform = 'scale(0.95)';
        imageItem.style.transition = 'all 0.3s ease';

        setTimeout(() => {
            imageItem.remove();
            showSuccess('Imagen de variante eliminada');
        }, 300);
    }
}

function previewVariantImage(input, variantIndex, imageIndex) {
    if (input.files && input.files[0]) {
        const file = input.files[0];

        // Validar tamaño (5MB)
        const maxSize = 5 * 1024 * 1024;
        if (file.size > maxSize) {
            showError('El archivo es demasiado grande. Máximo 5MB.');
            input.value = '';
            return;
        }

        // Validar tipo
        const allowedTypes = ['image/jpeg', 'image/png', 'image/webp', 'image/jpg'];
        if (!allowedTypes.includes(file.type)) {
            showError('Formato no permitido. Use JPG, PNG o WebP.');
            input.value = '';
            return;
        }

        const reader = new FileReader();
        reader.onload = function (e) {
            const previewContainer = document.getElementById(`preview_${variantIndex}_${imageIndex}`);
            const img = previewContainer?.querySelector('img');

            if (img && previewContainer) {
                img.src = e.target.result;
                previewContainer.classList.remove('hidden');
            }
        };

        reader.readAsDataURL(file);
    }
}

/* =====================================================
   SLUG AUTOMÁTICO
===================================================== */
export function setupSlugAutoGeneration() {
    const nameInput = document.querySelector('[name="Products.Name"]');
    const slugInput = document.querySelector('[name="Products.Slug"]');

    if (!nameInput || !slugInput) return;

    nameInput.addEventListener('input', e => {
        if (!slugInput.value || slugInput.dataset.autoGenerated) {
            slugInput.value = generateSlug(e.target.value);
            slugInput.dataset.autoGenerated = 'true';
        }
    });

    // Si el usuario edita el slug manualmente
    slugInput.addEventListener('input', () => {
        delete slugInput.dataset.autoGenerated;
    });
}

export function generateSlug(text) {
    return text
        .toLowerCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '')
        .replace(/[^a-z0-9\s-]/g, '')
        .trim()
        .replace(/\s+/g, '-')
        .replace(/-+/g, '-');
}

/* =====================================================
   EXPONER FUNCIONES GLOBALMENTE
===================================================== */
window.addVariant = addVariant;
window.removeVariant = removeVariant;
window.addVariantImage = addVariantImage;
window.removeVariantImage = removeVariantImage;
window.previewVariantImage = previewVariantImage;