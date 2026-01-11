import { handleConfirmFormSubmit } from "../../bundle/utils/form-helpers.js";

const form = document.getElementById("formProducto");

if (!form) {
    console.error("No se encontró el formulario #formProducto");
} else {
    handleConfirmFormSubmit({
        form,
        title: "¿Eliminar el producto?",
        html: `Estás a punto de eliminar <b>${$("#nombre").val()}</b>.`,
        icon: "warning",
        confirmText: "Sí, eliminar",
        cancelText: "Cancelar",
        spinnerAction: "deleting"
    });
}