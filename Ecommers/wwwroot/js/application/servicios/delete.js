import { handleConfirmFormSubmit } from "../../domain/utils/form-helpers.js";

const form = document.getElementById("formServicios");

handleConfirmFormSubmit({
    form,
    title: "¿Eliminar el servicio?",
    html: `Estás a punto de eliminar <b>${$("#nombre").val()}</b>.`,
    icon: "warning",
    confirmText: "Sí, eliminar",
    cancelText: "Cancelar",
    spinnerAction: "deleting"
});