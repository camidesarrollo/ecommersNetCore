import { $ } from "js/domain/vendors_datatables.js";

document.addEventListener("DOMContentLoaded", () => {

    console.log("Todo okay");
    // Inicializar DataTable
    $('#tableConfiguraciones').DataTable({
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json',
            search: "Buscar:",
            lengthMenu: "Mostrar _MENU_ registros por página",
            info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
            infoEmpty: "Mostrando 0 a 0 de 0 registros",
            infoFiltered: "(filtrado de _MAX_ registros totales)",
            zeroRecords: "No se encontraron registros coincidentes",
            emptyTable: "No hay datos disponibles en la tabla",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Último"
            }
        },
        responsive: true,
        pageLength: 10,
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "Todos"]],
        order: [[0, "desc"]],
        columnDefs: [
            {
                targets: -1,
                orderable: false,
                searchable: false
            },
            {
                targets: 3,
                orderable: false
            }
        ]
    });

    // Evento eliminar
    $(document).on("click", ".delete-btn", function (e) {
        e.preventDefault();

        const nombre = $(this).data("nombre");
        const form = $(this).closest("form");

        if (confirm(`¿Está seguro que desea eliminar la configuración "${nombre}"?\n\nEsta acción no se puede deshacer.`)) {
            $(this)
                .prop("disabled", true)
                .html('<i class="fas fa-spinner fa-spin"></i>');

            form.submit();
        }
    });
});
