import { handleConfirmFormSubmit } from "../../bundle/utils/form-helpers.js";

const form = document.getElementById("formBanners");

handleConfirmFormSubmit({
    form,
    title: "�Eliminar el banners?",
    html: `Est�s a punto de eliminar <b>${$("#Seccion").val()}</b>.`,
    icon: "warning",
    confirmText: "S�, eliminar",
    cancelText: "Cancelar",
    spinnerAction: "deleting"
});