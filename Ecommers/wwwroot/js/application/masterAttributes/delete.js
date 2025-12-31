import { handleConfirmFormSubmit } from "../../domain/utils/form-helpers.js";

const form = document.getElementById("formMasterAttributes");

handleConfirmFormSubmit({
    form,
    title: "¿Eliminar el maestro de atributos?",
    html: `Estás a punto de eliminar <b>${$("#nombre").val()}</b>.`,
    icon: "warning",
    confirmText: "Sí, eliminar",
    cancelText: "Cancelar",
    spinnerAction: "deleting"
});
