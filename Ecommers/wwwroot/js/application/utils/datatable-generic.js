import Swal from '../../bundle/notifications/vendors_sweetalert.js';
import { $ } from "../../bundle/vendors_datatables.js";
import { initGlobalUI,showSpinner, hideSpinner, showResultModal } from "../../bundle/components/spinner.js";

document.addEventListener("DOMContentLoaded", () => {
    initGlobalUI();
});
// =============================
//  CONFIGURACIÓN GLOBAL
// =============================
const modulo = window.location.pathname.split("/")[1];
const cookiePagina = `${modulo}_paginaDataTable`;
const cookieBuscador = `${modulo}_buscadorDataTable`;


// ===========================================================
// FUNCIONES: COOKIES
// ===========================================================
function setCookie(nombre, valor, dias = 1) {
    const exp = new Date();
    exp.setTime(exp.getTime() + dias * 24 * 60 * 60 * 1000);
    document.cookie = `${nombre}=${encodeURIComponent(valor)}; path=/; expires=${exp.toUTCString()}`;
}

function getCookie(nombre) {
    const cookies = document.cookie.split("; ");
    for (const c of cookies) {
        const [key, value] = c.split("=");
        if (key === nombre) return decodeURIComponent(value);
    }
    return null;
}

function deleteOtherModuleCookies(baseName) {
    document.cookie.split(";").forEach(cookie => {
        const name = cookie.split("=")[0].trim();
        if (name.includes(baseName) && !name.includes(modulo)) {
            document.cookie = `${name}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC;`;
        }
    });
}

function getLocationData() {
    return {
        origin: window.location.origin,
        protocol: window.location.protocol,
        host: window.location.host,
        hostname: window.location.hostname,
        port: window.location.port,
        pathname: window.location.pathname,
        search: window.location.search,
        hash: window.location.hash
    };
}



// ===========================================================
//  RESTAURAR BUSCADOR
// ===========================================================
function restoreSearchValue(dt) {
    const valor = getCookie(cookieBuscador) || "";
    const buscador = $(".tabla-buscador");

    if (buscador.length) {
        buscador.val(valor);
        dt.search(valor).draw(false);
    }
}


// ===========================================================
//  INITIALIZER GENÉRICO DE DATATABLES
// ===========================================================
async function initDataTable(selector, opciones = {}) {

    deleteOtherModuleCookies("paginaDataTable");
    deleteOtherModuleCookies("buscadorDataTable");

    const {
        ajaxUrl,
        ajaxData = {},
        columnas = [],
        buttons = [],
        columnDefs = [],
        pageLength = 5
    } = opciones;

    let dt = $(selector).removeClass('display').removeClass('d-block')
        .addClass('table table-striped')
        .DataTable({
            stateSave: false,
            processing: true,
            serverSide: true,
            filter: true,
            ajax: {
                url: ajaxUrl,
                type: "POST", // Cambiar a POST para enviar draw correctamente
                searching: true,
                data: function (d) {
                    // Asegurar que draw siempre sea un número válido
                    d.draw = d.draw || 1;

                    // Agregar datos personalizados
                    return $.extend({}, d, ajaxData);
                },
                dataSrc: function (json) {
                    // Validar respuesta
                    if (!json || typeof json.draw === 'undefined') {
                        console.error('Respuesta inválida de DataTables:', json);
                        return [];
                    }
                    return json.data || [];
                },
                error: function (xhr, error, thrown) {
                    console.error('Error en DataTables:', error, thrown);
                    hideLoader();
                }
            },
            language: {
                url: getLocationData().origin + '/lib/datatables/plug-ins/2.3.6/i18n/es-ES.json',
                processing: '<div class="dt-loader-custom"><span>Procesando...</span><div class="dt-loader-dots"><div></div><div></div><div></div><div></div></div></div>'
            },
            columns: columnas,
            buttons: buttons,
            columnDefs: columnDefs,
            //dom: '<"btn-group float-end filter-dataTable"lfB>t<"contenedor-tabla-footer mt-3 "ip>',
            lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
            pageLength: pageLength,

            initComplete: function () {
                // Restaurar buscador
                restoreSearchValue(dt);

                // Guardar al blur o Enter
                $(document).on('blur keyup', '.tabla-buscador', function (e) {
                    if (e.type === 'blur' || e.key === 'Enter') {
                        setCookie(cookieBuscador, $(this).val());
                    }
                });

                // Forzar ocultar loader al completar
                hideLoader();
            },

            // Eventos para controlar el loader
            preDrawCallback: function () {
                showLoader();
            },

            drawCallback: function () {
                hideLoader();
            }
        });


    // Restaurar página guardada
    dt.on("init", function () {
        const pagina = parseInt(getCookie(cookiePagina) || "1") - 1;
        const paginas = dt.page.info().pages;

        if (!isNaN(pagina) && pagina >= 0 && pagina < paginas) {
            dt.page(pagina).draw(false);
        } else {
            dt.page(0).draw(false);
        }
    });

    return dt;
}


// ===========================================================
//  CONTROL MANUAL DEL LOADER
// ===========================================================
function showLoader() {
    const loader = $('.dataTables_processing, .dt-processing, #tableCategorias_processing');
    if (loader.length) {
        loader.css('display', 'block');
    }
}

function hideLoader() {
    const loader = $('.dataTables_processing, .dt-processing, #tableCategorias_processing');
    if (loader.length) {
        loader.css('display', 'none');
    }
}


// ===========================================================
//  GUARDAR PÁGINA ANTES DE SALIR
// ===========================================================
async function guardarPaginaYSalir(event, boton) {
    event.preventDefault();

    const dt = $("#tabla-dinamica").DataTable();
    const paginaActual = dt.page() + 1;

    setCookie(cookiePagina, paginaActual);

    //await showModalCargando();
    setTimeout(() => window.location.href = boton.href, 800);
}

async function handleConfirmAction({
    event,
    selector,
    getData,
    action,
    confirmText,
    successTitle = "¡Éxito!",
    errorTitle = "Error",
    reloadTable = false,
    dataTable = null   // 👈 NUEVO
}) {
    const btn = event.target.closest(selector);
    if (!btn) return;

    const data = getData(btn);

    const confirm = await Swal.fire({
        title: confirmText.title,
        html: confirmText.html,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: confirmText.confirmButton,
        cancelButtonText: "Cancelar"
    });

    if (!confirm.isConfirmed) return;

    if (typeof showSpinner === "function") {
        showSpinner("editing");
    }

    try {
        const response = await action(data);

        setTimeout(() => {
            hideSpinner();

            if (response.success) {
                showResultModal(
                    "success",
                    successTitle,
                    response.message || "Operación completada correctamente"
                );

                // ✅ RELOAD CORRECTO
                if (reloadTable && dataTable) {
                    dataTable.ajax.reload(null, false);
                }

            } else {
                showResultModal(
                    "error",
                    errorTitle,
                    response.message || "Ocurrió un error en la operación"
                );
            }
        }, 600);

    } catch (error) {
        hideSpinner();
        console.error(error);

        showResultModal(
            "error",
            errorTitle,
            "Ocurrió un error inesperado"
        );
    }
}

// =============================
//   EXPORTS
// =============================
export { initDataTable, guardarPaginaYSalir, showLoader, hideLoader, handleConfirmAction };