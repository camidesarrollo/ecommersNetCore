import Swal from '../../bundle/vendors_sweetalert.js';
import { GetByName } from './bannersService.js';
/* js\application\banners\generic.js */
document.addEventListener('DOMContentLoaded', () => {
    setupLivePreview();
});

/* =====================================================
   PREVIEW EN TIEMPO REAL  BANNERS
===================================================== */
export function setupLivePreview() {

    const seccionInput = document.querySelector('[name="Seccion"]');
    const tituloInput = document.querySelector('[name="Titulo"]');
    const subtituloInput = document.querySelector('[name="Subtitulo"]');
    const botonTextoInput = document.querySelector('[name="BotonTexto"]');
    const botonEnlaceInput = document.querySelector('[name="BotonEnlace"]');
    const altTextInput = document.querySelector('[name="AltText"]');

    const imageInput = document.getElementById('imageInput');
    const imageUrlHidden = document.getElementById('imageUrlHidden');
    const isActiveInput = document.getElementById('isActiveSwitch');

    // Preview elements
    const seccionPreview = document.getElementById('SeccionPreview');
    const tituloPreview = document.getElementById('TituloPreview');
    const subtituloPreview = document.getElementById('SubtituloPreview');
    const botonTextoPreview = document.getElementById('BotonTextoPreview');
    const imagePreview = document.getElementById('ImagePreview');
    const statusIconPreview = document.getElementById('StatusIconPreview');
    const statusLabelPreview = document.getElementById('StatusLabelPreview');

    /* ======================
       SECCIÓN
    ====================== */
    if (seccionInput && seccionPreview) {
        seccionInput.addEventListener('input', () => {
            seccionPreview.textContent =
                seccionInput.value.trim() || 'Sección del banner';
        });
    }

    /* ======================
       TITULO
    ====================== */
    if (tituloInput && tituloPreview) {
        tituloInput.addEventListener('input', () => {
            tituloPreview.textContent =
                tituloInput.value.trim() || 'Título del banner';
        });
    }

    /* ======================
       SUBTÍTULO
    ====================== */
    if (subtituloInput && subtituloPreview) {
        subtituloInput.addEventListener('input', () => {
            subtituloPreview.textContent =
                subtituloInput.value.trim() || 'Subtítulo del banner';
        });
    }

    /* ======================
       BOTÓN TEXTO
    ====================== */
    if (botonTextoInput && botonTextoPreview) {
        botonTextoInput.addEventListener('input', () => {
            botonTextoPreview.textContent =
                botonTextoInput.value.trim() || 'Ver más';
        });
    }

    /* ======================
       ESTADO ACTIVO / INACTIVO
    ====================== */
    if (isActiveInput && statusIconPreview && statusLabelPreview) {

        const updateStatusPreview = () => {
            if (isActiveInput.checked) {
                statusIconPreview.className = 'fas fa-circle text-mint-green-700';
                statusLabelPreview.textContent = 'Activo';
            } else {
                statusIconPreview.className = 'fas fa-circle text-gray-400';
                statusLabelPreview.textContent = 'Inactivo';
            }
        };

        updateStatusPreview();
        isActiveInput.addEventListener('change', updateStatusPreview);
    }

    /* ======================
       IMAGEN
    ====================== */
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

            imageUrlHidden.value = file.name;

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
        Swal.fire({
            icon: 'error',
            title: 'Formato inválido',
            text: 'La imagen debe ser JPG, PNG o WEBP',
        });
        input.value = '';
        return false;
    }

    if (file.size > maxSize) {
        Swal.fire({
            icon: 'error',
            title: 'Imagen muy grande',
            text: 'La imagen no debe superar los 2MB',
        });
        input.value = '';
        return false;
    }

    return true;
}

export function hideImagePreview(img) {
    img.src = '';
    img.style.display = 'none';
}

$('#Titulo').on('blur', function () {

    const nombre = $(this).val().trim();
    if (!nombre) return;

    const id = parseInt($('#Id').val()) || 0;

    GetByName({ id: id, name: nombre })
        .then(response => {

            if (response.result != null) {
                $('#Titulo').val('').focus();
                Swal.fire({
                    icon: 'warning',
                    title: 'banner duplicado',
                    text: 'Ya existe una banner con ese nombre',
                });
            }

        })
        .catch(error => {
            console.error(error);
        });
});
