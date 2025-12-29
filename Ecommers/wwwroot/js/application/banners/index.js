/* js/application/banners/index.js */

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
            className: "text-center",
            width: "60px"
        },
        {
            data: "seccion",
            title: "Sección"
        },
        {
            data: "titulo",
            title: "Título"
        },
        {
            data: "image",
            title: "Imagen",
            orderable: false,
            searchable: false,
            className: "text-center",
            render: function (data) {
                return data
                    ? `<img src="${data}" alt="Banner" class="w-14 h-14 object-cover rounded-lg mx-auto shadow-sm" />`
                    : `<div class="w-14 h-14 flex items-center justify-center bg-gray-100 rounded-lg mx-auto">
                         <i class="fas fa-image text-gray-400"></i>
                       </div>`;
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
            title: "Fecha creación",
            className: "text-center",
            render: function (data) {
                if (!data) {
                    return `<span class="text-gray-400">-</span>`;
                }

                const fecha = dayjs(data);
                return fecha.isValid()
                    ? `<div class="text-sm">
                         <div class="font-medium">${fecha.format("DD/MM/YYYY")}</div>
                         <div class="text-gray-500">${fecha.format("HH:mm")}</div>
                       </div>`
                    : `<span class="text-gray-400">-</span>`;
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
                    <a href="/Gestion/Banners/Detalle/${id}"
                       class="btn-navegacion inline-flex items-center justify-center w-9 h-9 rounded-lg bg-blue-100 text-blue-700 hover:bg-blue-200 transition-colors"
                       title="Ver detalles">
                        <i class="fas fa-eye"></i>
                    </a>

                    <a href="/Gestion/Banners/Editar/${id}"
                       class="btn-navegacion inline-flex items-center justify-center w-9 h-9 rounded-lg bg-amber-100 text-amber-700 hover:bg-amber-200 transition-colors"
                       title="Editar">
                        <i class="fas fa-edit"></i>
                    </a>

                    ${row.canDelete ? `
                    <a href="/Gestion/Banners/Eliminar/${id}"
                       class="btn-navegacion inline-flex items-center justify-center w-9 h-9 rounded-lg bg-red-100 text-red-700 hover:bg-red-200 transition-colors"
                       title="Eliminar">
                        <i class="fas fa-trash"></i>
                    </a>` : ""}
                </div>
                `;
            }
        }
    ];

    // ============================
    //   DEFINICIÓN DE COLUMNAS
    // ============================
    const columnDefs = [
        {
            targets: [0],
            width: "60px"
        },
        {
            targets: [3, 4, 5, 6],
            className: "text-center"
        }
    ];

    // ============================
    //   INICIALIZAR DATATABLE
    // ============================
    const dt = await initDataTable("#tabla-dinamica", {
        ajaxUrl: "/Gestion/Banners/ObtenerBannersDataTable",
        ajaxData: {},
        columnas: columnas,
        columnDefs: columnDefs,
        buttons: [],
        pageLength: 10
    });

    // ============================
    //   GUARDAR PÁGINA AL NAVEGAR
    // ============================
    document.addEventListener("click", function (e) {
        const btnNavegacion = e.target.closest(".btn-navegacion");
        if (btnNavegacion) {
            guardarPaginaYSalir(e, btnNavegacion);
        }
    });

});
