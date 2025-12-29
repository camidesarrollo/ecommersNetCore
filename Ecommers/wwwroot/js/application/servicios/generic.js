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

            if (response.message == 'Servicio obtenida correctamente.') {
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
