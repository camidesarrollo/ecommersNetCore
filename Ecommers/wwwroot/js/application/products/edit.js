import { CambiarEstadoVariante } from './products.api.JS';
// ============================
//   ACTIVAR / DESACTIVAR
// ============================
document.addEventListener("click", (e) => {
    handleConfirmAction({
        event: e,
        selector: ".btn-toggle-variante",

        getData: (btn) => ({
            id: btn.dataset.id
        }),

        action:  CambiarEstadoVariante,

        confirmText: (() => {
            const btn = e.target.closest(".btn-toggle-variante");
            const isActive = btn?.dataset.isactive === "true";
            const textActivo = isActive ? "desactivar" : "activar";
            const titulo = btn?.dataset.titulo;

            return {
                title: `¿Deseas ${textActivo} la variante?`,
                html: `Estás a punto de ${textActivo} <b>${titulo}</b>.`,
                confirmButton: `Sí, ${textActivo}`
            };
        })(),

        reloadTable: true,
        dataTable: dt 
    });
});