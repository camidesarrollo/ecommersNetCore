console.log('input.validation.js cargado');

document.addEventListener("DOMContentLoaded", () => {

    document.querySelectorAll(".input-wrapper input").forEach(input => {

        const wrapper = input.closest(".input-wrapper");
        const errorMsg = wrapper.querySelector(".ui-error");
        const counter = wrapper.querySelector(".ui-counter");

        const update = () => {

            // FLOAT LABEL
            wrapper.classList.toggle("has-value", input.value.trim() !== "");

            // COUNTER
            if (counter) {
                counter.innerText = input.value.length + "/" + input.maxLength;
            }

            // VALIDATION
            if (!input.checkValidity()) {
                input.setAttribute("aria-invalid", "true");
                wrapper.classList.add("error");
            } else {
                input.setAttribute("aria-invalid", "false");
                wrapper.classList.remove("error");
                if (input.value.trim() !== "") {
                    wrapper.classList.add("success");
                }
            }
        };

        input.addEventListener("input", update);
        input.addEventListener("blur", update);
        update();
    });
});

function UiInput_TogglePassword(id) {
    const input = document.getElementById(id);
    const icon = document.getElementById(id + "-toggle");

    if (!input || !icon) return;

    if (input.type === "password") {
        input.type = "text";
        icon.classList.replace("fa-eye", "fa-eye-slash");
    } else {
        input.type = "password";
        icon.classList.replace("fa-eye-slash", "fa-eye");
    }
}


/* ========================================
UI INPUT VALIDATION (GENÉRICO)
======================================== */

export function initRealtimeValidation(fields, schema, submitBtn) {
    let typingTimer;
    const delay = 400;

    Object.keys(fields).forEach(fieldId => {
        const input = fields[fieldId];

        input.addEventListener("input", () => {
            debounceValidation();
        });

        input.addEventListener("blur", () => {
            validateSingleField(fieldId, fields, schema);
        });
    });

    function debounceValidation() {
        clearTimeout(typingTimer);

        typingTimer = setTimeout(() => {
            validateForm(fields, schema, submitBtn);
        }, delay);
    }
}

export function validateForm(fields, schema, submitBtn) {
    clearAllErrors(fields);

    const data = collectData(fields);

    const result = schema.safeParse(data);

    setButtonState(submitBtn, result.success);

    if (!result.success) {
        showFormErrors(fields, result.error.format());
    }
}

export function validateSingleField(fieldId, fields, schema) {
    const value = fields[fieldId].value;

    const partial = schema.pick({ [fieldId]: true });
    const result = partial.safeParse({ [fieldId]: value });

    clearFieldError(fields[fieldId]);

    if (!result.success) {
        const error = result.error.format()[fieldId]?._errors[0];
        if (error) showFieldError(fields[fieldId], error);
    }
}

export function collectData(fields) {
    const data = {};
    Object.keys(fields).forEach(k => data[k] = fields[k].value);
    return data;
}

export function setButtonState(button, isValid) {
    if (!button) return;

    button.disabled = !isValid;
    button.setAttribute("aria-disabled", String(!isValid));
    button.classList.toggle("opacity-50", !isValid);
    button.classList.toggle("cursor-not-allowed", !isValid);
}

/* ============================
   ERROR VISUAL
============================ */

export function showFormErrors(fields, errors) {
    let first = null;

    Object.keys(fields).forEach(id => {
        const msg = errors[id]?._errors[0];
        if (msg) {
            showFieldError(fields[id], msg);
            first ??= fields[id];
        }
    });

    first?.focus();
}

export function showFieldError(input, message) {
    const wrapper = input.closest(".input-wrapper");
    const container = wrapper.querySelector(".relative");

    wrapper.classList.add("has-error");

    wrapper.querySelector(".inline-error")?.remove();
    container?.querySelector(".error-icon")?.remove();

    input.setAttribute("aria-invalid", "true");

    const msg = document.createElement("p");
    msg.className = "inline-error shake";
    msg.innerHTML = `<i class="fas fa-exclamation-circle"></i> ${message}`;
    wrapper.appendChild(msg);

    const icon = document.createElement("div");
    icon.className = "absolute right-10 top-1/2 -translate-y-1/2 error-icon";
    icon.innerHTML = `<i class="fas fa-times-circle"></i>`;
    container.appendChild(icon);
}

export function clearAllErrors(fields) {
    Object.values(fields).forEach(input => clearFieldError(input));
}

export function clearFieldError(input) {
    const wrapper = input.closest(".input-wrapper");
    const container = wrapper.querySelector(".relative");

    wrapper.classList.remove("has-error");
    wrapper.querySelector(".inline-error")?.remove();
    container?.querySelector(".error-icon")?.remove();
    input.removeAttribute("aria-invalid");
}


export function initBlurValidation({
    form,
    schema,
    ignoreTypes = ['file']
}) {
    const inputs = form.querySelectorAll('input, textarea, select');

    inputs.forEach(input => {
        input.addEventListener('blur', async () => {
            const field = input.name;
            if (!field) return;

            if (ignoreTypes.includes(input.type)) {
                clearError(input);
                return;
            }

            if (!schema.shape[field]) return;

            let value = input.value;

            // ✅ BOOLEAN
            if (input.type === 'checkbox') {
                value = input.checked;
            }

            // ✅ SELECT boolean
            if (input.tagName === 'SELECT') {
                if (value === 'true') value = true;
                if (value === 'false') value = false;
            }

            // ✅ String vacío → null
            if (value === '') value = null;

            try {
                await schema.shape[field].parseAsync(value);
                clearError(input);
            } catch (err) {
                const message = traducirMensaje(
                    err?.errors?.[0]?.message || ''
                );
                showError(input, message);
            }
        });
    });
}