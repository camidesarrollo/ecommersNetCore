/* js\application\categorias\index.js */
/* js\application\categorias\index.js */
import { initDataTable, guardarPaginaYSalir } from "../../domain/utils/datatable-generic.js";
import { dayjs } from "../../bundle/vendors_dayjs.js";

document.addEventListener("DOMContentLoaded", async () => {

    // ============================
    //   COLUMNAS DEL DATATABLE
    // ============================
    const columnas = [
        {
            data: "id",
            title: "ID",
            className: "text-center"
        },
        {
            data: "name",
            title: "Nombre"
        },
        {
            data: "shortDescription",
            title: "Descripción corta"
        },
        {
            data: "description",
            title: "Descripción",
            visible: false // Oculta por defecto, muy largo
        },
        {
            data: "image",
            title: "Ícono",
            orderable: false,
            searchable: false,
            className: "text-center",
            render: function (data) {
                return data
                    ? `<img src="${data}" alt="Categoría" class="w-12 h-12 object-cover rounded-lg mx-auto shadow-sm" />`
                    : `<div class="w-12 h-12 flex items-center justify-center bg-gray-100 rounded-lg mx-auto">
                         <i class="fas fa-image text-gray-400"></i>
                       </div>`;
            }
        },
        {
            data: "cantidadProductos",
            title: "Cantidad productos",
            className: "text-center",
            render: function (data) {
                const cantidad = data || 0;
                return `<span class="inline-flex items-center gap-1 px-2 py-1 rounded-md text-sm font-medium bg-gray-100 text-gray-700">
                          <i class="fas fa-box text-xs"></i>${cantidad}
                        </span>`;
            }
        },
        {
            data: "isActive",
            title: "Estado",
            className: "text-center",
            render: function (data) {
                return data
                    ? `<span class="inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-semibold bg-green-100 text-green-800">
                         <i class="fas fa-check-circle"></i>Activo
                       </span>`
                    : `<span class="inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-semibold bg-red-100 text-red-800">
                         <i class="fas fa-times-circle"></i>Inactivo
                       </span>`;
            }
        },
        {
            data: "createdAt",
            title: "Fecha Creación",
            className: "text-center",
            render: function (data) {
                if (!data) return '<span class="text-gray-400">-</span>';
                const fecha = dayjs(data);
                return fecha.isValid()
                    ? `<div class="text-sm">
                         <div class="font-medium">${fecha.format("DD/MM/YYYY")}</div>
                         <div class="text-gray-500">${fecha.format("HH:mm")}</div>
                       </div>`
                    : '<span class="text-gray-400">-</span>';
            }
        },
        {
            data: "id",
            title: "Acciones",
            orderable: false,
            searchable: false,
            className: "text-center",
            render: function (id, type, row) {
                return `
            <div class="flex items-center justify-center gap-2">
                <a href="/Gestion/Categorias/Detalle/${id}"
                   class="btn-navegacion inline-flex items-center justify-center w-9 h-9 rounded-lg bg-blue-100 text-blue-700 hover:bg-blue-200 transition-colors"
                   title="Ver detalles">
                    <i class="fas fa-eye"></i>
                </a>

                <a href="/Gestion/Categorias/Editar/${id}"
                   class="btn-navegacion inline-flex items-center justify-center w-9 h-9 rounded-lg bg-amber-100 text-amber-700 hover:bg-amber-200 transition-colors"
                   title="Editar">
                    <i class="fas fa-edit"></i>
                </a>

                ${row.canDelete
                        ? `
                <a href="/Gestion/Categorias/Eliminar/${id}"
                   class="btn-navegacion inline-flex items-center justify-center w-9 h-9 rounded-lg bg-red-100 text-red-700 hover:bg-red-200 transition-colors"
                   title="Eliminar">
                    <i class="fas fa-trash"></i>
                </a>`
                        : ""
                    }
            </div>
        `;
            }
        }
    ];

    // ============================
    //   DEFINICIONES DE COLUMNAS
    // ============================
    const columnDefs = [
        {
            targets: [3], // Descripción completa
            visible: false
        },
        {
            targets: [0], // ID
            width: "60px"
        },
        {
            targets: [4, 5, 6, 7, 8], // Ícono, Cantidad, Estado, Fecha, Acciones
            className: "text-center"
        }
    ];

    // ============================
    //   CARGAR DATATABLE GENÉRICO
    // ============================
    const dt = await initDataTable("#tabla-dinamica", {
        ajaxUrl: "/Gestion/Categorias/ObtenerCategoriasDataTable",
        ajaxData: {},
        columnas: columnas,
        buttons: [],
        columnDefs: columnDefs,
        pageLength: 10
    });

    // ============================
    //   EVENTO PARA GUARDAR PÁGINA AL NAVEGAR
    // ============================
    document.addEventListener("click", function (e) {
        const btnNavegacion = e.target.closest(".btn-navegacion");
        if (btnNavegacion) {
            guardarPaginaYSalir(e, btnNavegacion);
        }
    });

    // ============================
    //   EVENTO PARA ELIMINAR
    // ============================
    document.addEventListener("click", async function (e) {
        const btnDelete = e.target.closest(".btn-delete");
        if (!btnDelete) return;

        const id = btnDelete.dataset.id;
        const nombre = btnDelete.dataset.nombre;

        // Confirmación
        const confirmar = confirm(`¿Está seguro de eliminar la categoría "${nombre}"?`);
        if (!confirmar) return;

        try {
            // Mostrar loader si existe
            if (typeof showSpinner === 'function') {
                showSpinner('deleting');
            }

            // Realizar petición DELETE
            const response = await fetch(`/Categorias/Eliminar/${id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                }
            });

            if (response.ok) {
                // Recargar tabla
                dt.ajax.reload(null, false); // false = mantener página actual

                // Mostrar mensaje de éxito si tienes sistema de notificaciones
                alert('Categoría eliminada correctamente');
            } else {
                const error = await response.text();
                console.error('Error al eliminar:', error);
                alert('Error al eliminar la categoría');
            }

        } catch (error) {
            console.error('Error en la petición:', error);
            alert('Error al eliminar la categoría');
        } finally {
            // Ocultar loader
            if (typeof hideSpinner === 'function') {
                hideSpinner();
            }
        }
    });
});


    // ============================
    //   ELIMINAR CATEGORÍA (MODAL)
    // ============================
//    document.addEventListener("click", async function (e) {
//        if (e.target.closest(".delete-btn")) {
//            const btn = e.target.closest(".delete-btn");
//            const id = btn.dataset.id;
//            const nombre = btn.dataset.nombre;

//            const confirm = await Swal.fire({
//                title: "¿Eliminar categoría?",
//                html: `Estás a punto de eliminar <b>${nombre}</b>.`,
//                icon: "warning",
//                showCancelButton: true,
//                confirmButtonText: "Sí, eliminar",
//                cancelButtonText: "Cancelar"
//            });

//            if (confirm.isConfirmed) {
//                await fetch(`/Categorias/Eliminar/${id}`, { method: "POST" });
//                dt.ajax.reload(null, false); // recargar sin perder la página
//            }
//        }
//    });


