// Importar todo lo necesario desde select2-init.js
import $, { onNewOptionCreated, addOption, getOptions }
    from '../utils/select2-init.js';  // <-- ruta relativa desde generic.js

// Contador para índices únicos de imágenes
let imageIndex = 0;

// Inicializar al cargar el documento
document.addEventListener('DOMContentLoaded', function () {
    initializeImageManager();
});

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
    if($("#imagesContainer .border-olive-green-300").length == 0){
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
            alert('El archivo es demasiado grande. El tamaño máximo es 5MB.');
            input.value = '';
            return;
        }

        // Validar tipo
        const allowedTypes = ['image/jpeg', 'image/png', 'image/webp', 'image/jpg'];
        if (!allowedTypes.includes(file.type)) {
            alert('Tipo de archivo no permitido. Solo se aceptan JPG, PNG y WebP.');
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
