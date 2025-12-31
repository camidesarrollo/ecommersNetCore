// ==============================
// 1. Validación total al hacer submit
// ==============================
const form = document.getElementById("formBanners");

form.addEventListener("submit", async function (e) {
    e.preventDefault();


    const confirm = await Swal.fire({
        title: "¿Eliminar categoría?",
        html: `Estás a punto de eliminar <b>${$("#Seccion").val()}</b>.`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
    });

    if (confirm.isConfirmed) {
        // Mostrar spinner si existe la función
        if (typeof showSpinner === 'function') {
            showSpinner('deleting');
        }

        form.submit();
    }


});