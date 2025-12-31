import { GetByName } from './servicioService.js';
import Swal from '../../bundle/vendors_sweetalert.js';

/* js\application\categorias\generic.js */
document.addEventListener('DOMContentLoaded', () => {
    setupLivePreview();
});


/* =====================================================
   PREVIEW EN TIEMPO REAL
===================================================== */
export function setupLivePreview() {
    const nameInput = document.querySelector('[name="Name"]');
    const DescInput = document.querySelector('[name="Description"]');
    const imageInput = document.getElementById('imageInput');
    const isActiveInput = document.getElementById('isActiveSwitch');

    const namePreview = document.getElementById('NamePreview');
    const DescPreview = document.getElementById('DescriptionPreview');
    const bgClassView = document.getElementById('BgClassView');
    const imagePreview = document.getElementById('ImageFilePreview');
    const bgClassLabelPreview = document.getElementById('BgClassLabelPreview');
    const statusIconPreview = document.getElementById('StatusIconPreview');
    const statusLabelPreview = document.getElementById('StatusLabelPreview');

    // Nombre
    if (nameInput && namePreview) {
        nameInput.addEventListener('input', () => {
            namePreview.textContent =
                nameInput.value.trim() || 'Nombre de Categoría';
        });
    }

    // Descripción corta
    if (DescInput && DescPreview) {
        DescInput.addEventListener('input', () => {
            DescPreview.textContent =
                DescInput.value.trim() || 'Descripción breve de la categoría';
        });
    }

    // Estado Activo/Inactivo
    if (isActiveInput && statusIconPreview && statusLabelPreview) {
        const updateStatusPreview = () => {
            if (isActiveInput.checked) {
                statusIconPreview.className = 'fas fa-circle text-mint-green-700';
                statusLabelPreview.textContent = 'Activa';
            } else {
                statusIconPreview.className = 'fas fa-circle text-gray-400';
                statusLabelPreview.textContent = 'Inactiva';
            }
        };

        // Actualizar al cargar y al cambiar
        updateStatusPreview();
        isActiveInput.addEventListener('change', updateStatusPreview);
    }

    // Imagen - No duplicar funcionalidad del image-handler.js
    // Solo actualizar el preview específico si es necesario
    if (imageInput && imagePreview) {
        imageInput.addEventListener('change', () => {
            const file = imageInput.files[0];

            if (!file) {
                hideImagePreview(imagePreview);
                return;
            }

            if (!validateImage(file, imageInput)) {
                hideImagePreview(imagePreview);
                return;
            }

            const reader = new FileReader();
            reader.onload = e => {
                imagePreview.src = e.target.result;
                imagePreview.style.display = 'block';
            };
            reader.readAsDataURL(file);
        });
    }
}

/* =====================================================
   HELPERS
===================================================== */
export function validateImage(file, input) {
    const validTypes = ['image/jpeg', 'image/png', 'image/webp', 'image/jpg'];
    const maxSize = 2 * 1024 * 1024; // 2MB

    if (!validTypes.includes(file.type)) {
        alert('Formato de imagen no válido. Use JPG, PNG o WEBP.');
        input.value = '';
        return false;
    }

    if (file.size > maxSize) {
        alert('La imagen no debe superar los 2MB');
        input.value = '';
        return false;
    }

    return true;
}

export function hideImagePreview(img) {
    img.src = '';
    img.style.display = 'none';
}


$('#Name').on('blur', function () {

    const nombre = $(this).val().trim();
    if (!nombre) return;

    const id = parseInt($('#Id').val()) || 0;

    GetByName({ id: id, name: nombre })
        .then(response => {

            if (response.result != null) {
                $('#Name').val('').focus();
                $("#Slug").val('').focus();
                Swal.fire({
                    icon: 'warning',
                    title: 'Servicio duplicado',
                    text: 'Ya existe una servicio con ese nombre',
                });
            }

        })
        .catch(error => {
            console.error(error);
        });
});
