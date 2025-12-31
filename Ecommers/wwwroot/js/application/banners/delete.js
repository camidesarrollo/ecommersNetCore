import { handleConfirmFormSubmit } from "../../domain/utils/form-helpers.js";

const form = document.getElementById("formBanners");

handleConfirmFormSubmit({
    form,
    title: "¿Eliminar el banners?",
    html: `Estás a punto de eliminar <b>${$("#Seccion").val()}</b>.`,
    icon: "warning",
    confirmText: "Sí, eliminar",
    cancelText: "Cancelar",
    spinnerAction: "deleting"
});