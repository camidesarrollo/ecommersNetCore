import { loginUserSchema } from "/js/bundle/schema/login.user.shema.js";

import {
    initRealtimeValidation,
    validateForm,
    setButtonState
} from "/js/domain/utils/ui/input.validation.js";

document.addEventListener("DOMContentLoaded", () => {

    const form = document.getElementById("loginForm");
    const submitBtn = document.getElementById("loginSubmit");

    const fields = {
        email: document.getElementById("email"),
        password: document.getElementById("password")
    };

    // estado inicial
    setButtonState(submitBtn, false);

    // ✅ Validación reactiva estilo Vue
    initRealtimeValidation(fields, loginUserSchema, submitBtn);

    // ✅ Validación final antes de enviar
    form.addEventListener("submit", (e) => {
        const result = loginUserSchema.safeParse({
            email: fields.email.value,
            password: fields.password.value
        });

        setButtonState(submitBtn, result.success);

        if (!result.success) {
            e.preventDefault();
            validateForm(fields, loginUserSchema, submitBtn);
        }
    });

});
