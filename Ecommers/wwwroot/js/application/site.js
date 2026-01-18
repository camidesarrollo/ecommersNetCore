// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// ==============================
// 2. Funciones auxiliares
// ==============================

function showError(input, message) {
    const span = getOrCreateValidationSpan(input);

    span.textContent = message;
    span.classList.remove('field-validation-valid');
    span.classList.add('field-validation-error');
}

function clearError(input) {
    const span = findValidationSpan(input);
    if (!span) return;

    span.textContent = '';
    span.classList.remove('field-validation-error');
    span.classList.add('field-validation-valid');
}

function emptyToNull(obj) {
    const cleaned = {};

    for (const key in obj) {
        cleaned[key] = obj[key] === '' ? null : obj[key];
    }

    return cleaned;
}

function findValidationSpan(input) {
    const name = input.getAttribute('name');
    if (!name) return null;

    return document.querySelector(
        `span[data-valmsg-for="${CSS.escape(name)}"]`
    );
}

function getOrCreateValidationSpan(input) {
    let span = findValidationSpan(input);
    if (span) return span;

    const name = input.getAttribute('name') || '';

    span = document.createElement('span');
    span.className =
        'text-red-700 text-sm mt-1 block field-validation-error';
    span.setAttribute('data-valmsg-for', name);
    span.setAttribute('data-valmsg-replace', 'true');

    input.insertAdjacentElement('afterend', span);

    return span;
}
function traducirMensaje(msg) {
    // URLs
    if (msg.includes("Invalid URL")) return "La URL no es válida.";

    // Email
    if (msg.includes("Invalid email") || msg.includes("email"))
        return "El correo electrónico no es válido.";

    // Números
    if (msg.includes("Expected number")) return "Debe ingresar un número.";
    if (msg.includes("Invalid number")) return "Número inválido.";

    // Booleanos
    if (msg.includes("Expected boolean")) return "Debe seleccionar una opción válida.";

    // Longitudes
    if (msg.includes("String must contain at most"))
        return "El texto excede la longitud permitida.";
    if (msg.includes("String must contain at least"))
        return "El texto es demasiado corto.";

    // Obligatorios
    if (msg.includes("Required")) return "Este campo es obligatorio.";

    // Datetime
    if (msg.includes("Invalid datetime")) return "La fecha y hora no tienen un formato válido.";

    // Strings vacíos
    if (msg.includes("Expected string")) return "Debe ingresar un texto.";

    // Fallback general
    return "El valor ingresado no es válido.";
}


function animateAndRemove(element, callback, delay = 300) {
    if (!element) return;

    element.style.opacity = '0';
    element.style.transform = 'scale(0.95)';
    element.style.transition = 'all 0.3s ease';

    setTimeout(() => {
        element.remove();
        if (typeof callback === 'function') {
            callback();
        }
    }, delay);
}