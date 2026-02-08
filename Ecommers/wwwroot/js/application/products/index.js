import { CambiarEstadoProducto } from './products.api.JS';
import { initDataTable, guardarPaginaYSalir, handleConfirmAction } from "../../application/utils/datatable-generic.js";
import dayjs from "../../bundle/vendors_dayjs.js";

document.addEventListener("DOMContentLoaded", async () => {

    // ============================
    //   COLUMNAS DEL DATATABLE
    // ============================
    const columnas = [
        // Imagen
        {
            data: "mainImageUrl",
            title: "Imagen",
            className: "text-center",
            orderable: false,
            render: function (data, type, row) {
                if (!data) {
                    return `<div class="flex items-center justify-center">
                              <div class="w-12 h-12 bg-gray-200 rounded-lg flex items-center justify-center">
                                <i class="fas fa-image text-gray-400"></i>
                              </div>
                            </div>`;
                }
                return `<div class="flex items-center justify-center">
                          <img src="${data}" 
                               alt="${row.productName}" 
                               class="w-12 h-12 object-cover rounded-lg shadow-sm"
                               onerror="this.onerror=null; this.src='/img/no-image.png';">
                        </div>`;
            }
        },
        // Nombre
        {
            data: "productName",
            title: "Nombre",
            render: function (data, type, row) {
                return `<div class="flex flex-col">
                          <span class="font-semibold text-gray-900">${data}</span>
                          <span class="text-xs text-gray-500">${row.shortDescription || ''}</span>
                        </div>`;
            }
        },
        // Categoría
        {
            data: "categoryName",
            title: "Categoría",
            className: "text-center",
            render: function (data) {
                return `<span class="inline-flex items-center px-2.5 py-1 rounded-full text-xs font-medium bg-purple-100 text-purple-800">
                          ${data}
                        </span>`;
            }
        },
        // Marca
        {
            data: "brand",
            title: "Marca",
            className: "text-center",
            render: function (data) {
                return data
                    ? `<span class="text-sm text-gray-700">${data}</span>`
                    : '<span class="text-gray-400">-</span>';
            }
        },
        // SKU
        {
            data: "defaultSKU",
            title: "SKU",
            className: "text-center",
            render: function (data, type, row) {
                if (!data && !row.variantSKUs) return '<span class="text-gray-400">-</span>';

                const sku = data || row.variantSKUs.split(',')[0].trim();
                const hasMultiple = row.totalVariants > 1;

                return `<div class="flex flex-col items-center">
                          <span class="font-mono text-sm bg-gray-100 px-2 py-1 rounded">${sku}</span>
                          ${hasMultiple ? `<span class="text-xs text-gray-500 mt-1">+${row.totalVariants - 1} más</span>` : ''}
                        </div>`;
            }
        },
        // Precio
        {
            data: "minVariantPrice",
            title: "Precio",
            className: "text-center",
            render: function (data, type, row) {
                if (!data) return '<span class="text-gray-400">-</span>';

                const min = parseFloat(data);
                const max = parseFloat(row.maxVariantPrice);

                if (min === max) {
                    return `<span class="font-semibold text-green-700">$${min.toLocaleString('es-CL')}</span>`;
                }

                return `<div class="text-sm">
                          <div class="text-gray-600">Desde</div>
                          <div class="font-semibold text-green-700">$${min.toLocaleString('es-CL')}</div>
                          <div class="text-xs text-gray-500">hasta $${max.toLocaleString('es-CL')}</div>
                        </div>`;
            }
        },
        // Stock
        {
            data: "totalStock",
            title: "Stock",
            className: "text-center",
            render: function (data, type, row) {
                let badgeClass = 'bg-green-100 text-green-800';
                let icon = 'fa-check-circle';

                if (data === 0) {
                    badgeClass = 'bg-red-100 text-red-800';
                    icon = 'fa-times-circle';
                } else if (data <= 10) {
                    badgeClass = 'bg-yellow-100 text-yellow-800';
                    icon = 'fa-exclamation-circle';
                }

                return `<div class="flex flex-col items-center gap-1">
                          <span class="font-semibold text-lg text-gray-900">${data}</span>
                          <span class="inline-flex items-center gap-1 px-2 py-0.5 rounded-full text-xs font-medium ${badgeClass}">
                            <i class="fas ${icon}"></i>${row.stockStatus}
                          </span>
                        </div>`;
            }
        },
        // Variantes
        {
            data: "totalVariants",
            title: "Variantes",
            className: "text-center",
            render: function (data) {
                if (!data || data === 0) {
                    return '<span class="text-gray-400">0</span>';
                }
                return `<span class="inline-flex items-center justify-center w-8 h-8 rounded-full bg-blue-100 text-blue-800 font-semibold text-sm">
                          ${data}
                        </span>`;
            }
        },
        // Estado
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
        // Fecha Creación
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
        // Acciones
        {
            data: "productId",
            title: "Acciones",
            orderable: false,
            searchable: false,
            className: "text-center",
            render: function (productId, type, row) {
                return `
                    <div class="flex items-center justify-center gap-2">
                        <a href="/Gestion/Products/Detalle/${productId}"
                           class="btn-navegacion inline-flex items-center justify-center w-9 h-9 rounded-lg bg-blue-100 text-blue-700 hover:bg-blue-200 transition-colors"
                           title="Ver detalles">
                            <i class="fas fa-eye"></i>
                        </a>

                        <a href="/Gestion/Products/Editar/${productId}"
                           class="btn-navegacion inline-flex items-center justify-center w-9 h-9 rounded-lg bg-amber-100 text-amber-700 hover:bg-amber-200 transition-colors"
                           title="Editar">
                            <i class="fas fa-edit"></i>
                        </a>

                        <button data-id="${productId}" 
                                data-isActive="${row.isActive}" 
                                data-titulo="${row.productName}"
                                class="btn-toggle w-9 h-9 flex items-center justify-center rounded-lg ${row.isActive ? 'bg-green-100 text-green-700 hover:bg-green-200' : 'bg-gray-100 text-gray-700 hover:bg-gray-200'}"
                                title="${row.isActive ? 'Desactivar' : 'Activar'}">
                            <i class="fas ${row.isActive ? 'fa-toggle-on' : 'fa-toggle-off'}"></i>
                        </button>

                         ${row.hasSales == 0
                        ? `
                            <a href="/Gestion/Products/Eliminar/${productId}"
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
            targets: [0], // Imagen
            width: "80px"
        },
        {
            targets: [1], // Nombre
            width: "250px"
        },
        {
            targets: [2, 3, 4, 5, 6, 7, 8, 9, 10], // Resto de columnas centradas
            className: "text-center"
        }
    ];

    // ============================
    //   CARGAR DATATABLE GENÉRICO
    // ============================
    const dt = await initDataTable("#tabla-dinamica", {
        ajaxUrl: "/Gestion/Products/ObtenerProductosDataTable",
        ajaxData: {},
        columnas: columnas,
        buttons: [
            {
                extend: 'excel',
                text: '<i class="fas fa-file-excel mr-2"></i>Excel',
                className: 'btn-dt-excel'
            },
            {
                extend: 'pdf',
                text: '<i class="fas fa-file-pdf mr-2"></i>PDF',
                className: 'btn-dt-pdf'
            }
        ],
        columnDefs: columnDefs,
        pageLength: 10,
        order: [[9, 'desc']] // Ordenar por fecha de creación descendente
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
    //   ACTIVAR / DESACTIVAR
    // ============================
    document.addEventListener("click", (e) => {
       handleConfirmAction({
           event: e,
           selector: ".btn-toggle",

           getData: (btn) => ({
               ProductoId: btn.dataset.id
           }),

           action:  CambiarEstadoProducto,

           confirmText: (() => {
               const btn = e.target.closest(".btn-toggle");
               const isActive = btn?.dataset.isactive === "true";
               const textActivo = isActive ? "desactivar" : "activar";
               const titulo = btn?.dataset.titulo;

               return {
                   title: `¿Deseas ${textActivo} el producto?`,
                   html: `Estás a punto de ${textActivo} <b>${titulo}</b>.`,
                   confirmButton: `Sí, ${textActivo}`
               };
           })(),

           reloadTable: true,
           dataTable: dt 
       });
    });
});