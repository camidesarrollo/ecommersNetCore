import { handleConfirmFormSubmit } from "../../bundle/utils/form-helpers.js";

const form = document.getElementById("formCategoria");

handleConfirmFormSubmit({
    form,
    title: "¿Eliminar el categoria?",
    html: `Estás a punto de eliminar <b>${$("#nombre").val()}</b>.`,
    icon: "warning",
    confirmText: "Sí, eliminar",
    cancelText: "Cancelar",
    spinnerAction: "deleting"
});