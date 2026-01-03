import { initDataTable, guardarPaginaYSalir } from "../../application/utils/datatable-generic.js";
import dayjs from "../../bundle/vendors_dayjs.js";

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
            data: "slug",
            title: "Slug",
            className: "text-gray-500 text-sm"
        },
        {
            data: "dataType",
            title: "Tipo dato",
            className: "text-center",
            render: data => `
                <span class="px-2 py-1 rounded-md text-xs font-medium bg-indigo-100 text-indigo-700">
                    ${data}
                </span>`
        },
        {
            data: "inputType",
            title: "Input",
            className: "text-center",
            render: data => `
                <span class="px-2 py-1 rounded-md text-xs font-medium bg-blue-100 text-blue-700">
                    ${data}
                </span>`
        },
        {
            data: "category",
            title: "Categoría",
            render: data => data
                ? `<span class="px-2 py-1 rounded-md text-xs bg-gray-100 text-gray-700">${data}</span>`
                : `<span class="text-gray-400">-</span>`
        },
        {
            data: "isRequired",
            title: "Req.",
            className: "text-center",
            render: data => data
                ? `<span class="inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-semibold bg-green-100 text-green-800">
                        <i class="fas fa-check"></i>Sí
                   </span>`
                : `<span class="inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-semibold bg-gray-100 text-gray-600">
                        <i class="fas fa-minus"></i>No
                   </span>`
        },
        {
            data: "isFilterable",
            title: "Filtro",
            className: "text-center",
            render: data => data
                ? `<span class="inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-semibold bg-sky-100 text-sky-800">
                        <i class="fas fa-filter"></i>Sí
                   </span>`
                : `<span class="text-gray-400">-</span>`
        },
        {
            data: "isVariant",
            title: "Variante",
            className: "text-center",
            render: data => data
                ? `<span class="inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-semibold bg-purple-100 text-purple-800">
                        <i class="fas fa-layer-group"></i>Sí
                   </span>`
                : `<span class="text-gray-400">-</span>`
        },
        {
            data: "isActive",
            title: "Estado",
            className: "text-center",
            render: data => data
                ? `<span class="inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-semibold bg-green-100 text-green-800">
                        <i class="fas fa-check-circle"></i>Activo
                   </span>`
                : `<span class="inline-flex items-center gap-1 px-3 py-1 rounded-full text-xs font-semibold bg-red-100 text-red-800">
                        <i class="fas fa-times-circle"></i>Inactivo
                   </span>`
        },
        {
            data: "createdAt",
            title: "Creación",
            className: "text-center",
            render: data => {
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
                    <a href="/Gestion/MasterAttributes/Detalle/${id}"
                       class="btn-navegacion w-9 h-9 flex items-center justify-center rounded-lg bg-blue-100 text-blue-700 hover:bg-blue-200"
                       title="Ver">
                        <i class="fas fa-eye"></i>
                    </a>

                    <a href="/Gestion/MasterAttributes/Editar/${id}"
                       class="btn-navegacion w-9 h-9 flex items-center justify-center rounded-lg bg-amber-100 text-amber-700 hover:bg-amber-200"
                       title="Editar">
                        <i class="fas fa-edit"></i>
                    </a>

                    <button data-id="${id}"
                            class="btn-toggle w-9 h-9 flex items-center justify-center rounded-lg bg-gray-100 text-gray-700 hover:bg-gray-200"
                            title="Activar / Desactivar">
                        <i class="fas fa-toggle-on"></i>
                    </button>
                    ${row.canDelete
                            ? `
                    <a href="/Gestion/MasterAttributes/Eliminar/${id}"
                    class="btn-navegacion inline-flex items-center justify-center w-9 h-9 rounded-lg bg-red-100 text-red-700 hover:bg-red-200 transition-colors"
                    title="Eliminar">
                        <i class="fas fa-trash"></i>
                    </a>`
                            : ""
                    }
                </div>`;
            }
        }
    ];

    // ============================
    //   DEFINICIONES DE COLUMNAS
    // ============================
    const columnDefs = [
        { targets: [0], width: "60px" },
        { targets: [6, 7, 8, 9, 10, 11], className: "text-center" }
    ];

    // ============================
    //   INICIALIZAR DATATABLE
    // ============================
    const dt = await initDataTable("#tabla-dinamica", {
        ajaxUrl: "/Gestion/MasterAttributes/ObtenerMasterAttributesDataTable",
        ajaxData: {},
        columnas,
        buttons: [],
        columnDefs,
        pageLength: 10
    });

    // ============================
    //   GUARDAR PAGINA AL NAVEGAR
    // ============================
    document.addEventListener("click", e => {
        const btn = e.target.closest(".btn-navegacion");
        if (btn) guardarPaginaYSalir(e, btn);
    });

    // ============================
    //   ACTIVAR / DESACTIVAR
    // ============================
    document.addEventListener("click", async e => {
        const btn = e.target.closest(".btn-toggle");
        if (!btn) return;

        const id = btn.dataset.id;

        await fetch(`/Gestion/MasterAttributes/ToggleEstado/${id}`, {
            method: "POST",
            headers: {
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]')?.value
            }
        });

        dt.ajax.reload(null, false);
    });
});
